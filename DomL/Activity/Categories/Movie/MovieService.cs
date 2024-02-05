using DomL.Business.Entities;
using DomL.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;
using DomL.DataAccess;
using DomL.Business.DTOs;
using DomL.Business.Utils;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class MovieService
    {
        /// <summary>
        /// The primary way of getting DoML information
        /// From One Note lines -> through Media Window -> to the Database
        /// </summary>
        /// <param name="rawSegments"></param>
        /// <param name="activity"></param>
        /// <param name="unitOfWork"></param>
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            // MOVIE; Title; Type (Genre); Series; Number; Person (Director); Company (Production Company); Score; Description
            rawSegments[0] = "";
            var movieWindow = new MovieWindow(rawSegments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                movieWindow.ShowDialog();
            }

            var consolidated = new MovieConsolidatedDTO(movieWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static List<Movie> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.MovieRepo.GetAllMovies();
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new MovieConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(MovieConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var movie = GetOrUpdateOrCreateMovie(consolidated, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateMovieActivity(activity, movie, consolidated.Description, unitOfWork);
        }

        private static void CreateMovieActivity(Activity activity, Movie movie, string description, UnitOfWork unitOfWork)
        {
            var movieActivity = new MovieActivity() {
                Activity = activity,
                Movie = movie,
                Description = Util.GetStringOrNull(description)
            };

            activity.MovieActivity = movieActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.MovieRepo.CreateMovieActivity(movieActivity);
        }

        private static Movie GetOrUpdateOrCreateMovie(MovieConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var instance = GetByTitle(consolidated.Title, unitOfWork);

            var title = Util.GetStringOrNull(consolidated.Title);
            var type = Util.GetStringOrNull(consolidated.Type);
            var series = Util.GetStringOrNull(consolidated.Series);
            var number = Util.GetStringOrNull(consolidated.Number);
            var person = Util.GetStringOrNull(consolidated.Person);
            var company = Util.GetStringOrNull(consolidated.Company);
            var year = Util.GetStringOrNull(consolidated.Year);
            var score = Util.GetStringOrNull(consolidated.Score);

            if (instance == null) {
                instance = new Movie() {
                    Title = title,
                    Type = type,
                    Series = series,
                    Number = number,
                    Person = person,
                    Company = company,
                    Year = year,
                    Score = score,
                };
            } else {
                instance.Type = type ?? instance.Type;
                instance.Series = series ?? instance.Series;
                instance.Number = number ?? instance.Number;
                instance.Person = person ?? instance.Person;
                instance.Company = company ?? instance.Company;
                instance.Year = year ?? instance.Year;
                instance.Score = score ?? instance.Score;
            }

            return instance;
        }

        public static Movie GetByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.MovieRepo.GetMovieByTitle(title);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var movie = activity.MovieActivity.Movie;
            return previousStartingActivities.Where(u =>
                u.CategoryId == Category.MOVIE_ID
                && u.MovieActivity.Movie.Title == movie.Title
            );
        }

        // Used to get information
        // from the database into a file
        // to be more easily manipulated
        public static void SaveMediaFromDatabaseToFile(string fileDir)
        {
            List<Movie> movies;
            using (var unitOfWork = new UnitOfWork(new DomLContext()))
            {
                movies = unitOfWork.MovieRepo.GetAllMovies();
            }
            var filePath = fileDir + "MOVIES.txt";
            using (var file = new StreamWriter(filePath))
            {
                foreach (var movie in movies)
                {
                    string movieString = movie.Id
                        + "\t" + movie.Title + "\t" + movie.Type
                        + "\t" + movie.Series + "\t" + movie.Number
                        + "\t" + movie.Person + "\t" + movie.Company
                        + "\t" + movie.Year + "\t" + movie.Score;
                    file.WriteLine(movieString);
                }
            }
        }

        // Used to save information
        // from a file into the database
        public static void SaveMediaFromFileToDatabase(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext()))
            {
                using (var reader = new StreamReader(fileDir + "MOVIES.txt"))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        var movieInfo = Regex.Split(line, "\t");
                        var movieId = 0;
                        var movieExists = int.TryParse(movieInfo[0], out movieId);

                        var correctId = movieInfo[9];
                        if (!string.IsNullOrWhiteSpace(correctId))
                        {
                            FixDuplicatedMedia(unitOfWork, movieId, correctId);
                        }
                        else if (!movieExists)
                        {
                            CreateMedia(unitOfWork, movieInfo);
                        }
                        else
                        {
                            UpdateExistingMedia(unitOfWork, movieId, movieInfo);
                        }
                    }

                    unitOfWork.Complete();
                }
            }
        }

        /// <summary>
        /// The movie record of id `movieId` is a duplicate of the movie record of id `correctId`
        /// we should update all records that point to `movieId` to actually point to `correctId`
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="movieId"></param>
        /// <param name="correctId"></param>
        private static void FixDuplicatedMedia(UnitOfWork unitOfWork, int movieId, string correctId)
        {
            var movieActivityList = unitOfWork.MovieRepo.Find(b => b.MovieId == movieId);
            var correctMovieId = int.Parse(correctId);
            foreach (var movieActivity in movieActivityList)
            {
                movieActivity.MovieId = correctMovieId;
            }
            unitOfWork.MovieRepo.RemoveMovieOfId(movieId);
        }

        private static void CreateMedia(UnitOfWork unitOfWork, string[] movieInfo)
        {
            var movie = new Movie();
            FillMovieData(movie, movieInfo);
            unitOfWork.MovieRepo.CreateMovie(movie);
        }

        private static void UpdateExistingMedia(UnitOfWork unitOfWork, int movieId, string[] movieInfo)
        {
            var movie = unitOfWork.MovieRepo.GetMovieOfId(movieId);
            FillMovieData(movie, movieInfo);
        }

        private static void FillMovieData(Movie movie, string[] movieInfo)
        {
            movie.Title = movieInfo[1];
            movie.Type = movieInfo[2];
            movie.Series = movieInfo[3];
            movie.Number = movieInfo[4];
            movie.Person = movieInfo[5];
            movie.Company = movieInfo[6];
            movie.Year = movieInfo[7];
            movie.Score = movieInfo[8];
        }
    }
}
