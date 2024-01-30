using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Business.Utils;
using DomL.DataAccess;
using DomL.Presentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class ComicService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            segments[0] = "";
            var comicWindow = new ComicWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                comicWindow.ShowDialog();
            }

            var consolidated = new ComicConsolidatedDTO(comicWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ComicConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static List<Comic> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.ComicRepo.GetAllComics();
        }

        private static void SaveFromConsolidated(ComicConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var comicVolume = GetOrUpdateOrCreateComicVolume(consolidated, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateComicActivity(activity, comicVolume, consolidated.Description, unitOfWork);
        }

        private static Comic GetOrUpdateOrCreateComicVolume(ComicConsolidatedDTO consolidated, UnitOfWork unitOfWork)
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
                instance = new Comic() {
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

        private static void CreateComicActivity(Activity activity, Comic comic, string description, UnitOfWork unitOfWork)
        {
            var comicActivity = new ComicActivity() {
                Activity = activity,
                Comic = comic,
                Description = Util.GetStringOrNull(description)
            };

            activity.ComicActivity = comicActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.ComicRepo.CreateComicActivity(comicActivity);
        }

        //TODO add year to search
        public static Comic GetByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.ComicRepo.GetComicByTitle(title);
        }

        //TODO add year to search
        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var comic = activity.ComicActivity.Comic;
            return previousStartingActivities.Where(u => 
                u.CategoryId == Category.COMIC_ID
                && u.ComicActivity.Comic.Title == comic.Title
            );
        }

        // Used to get information
        // from the database into a file
        // to be more easily manipulated
        public static void SaveMediaFromDatabaseToFile(string fileDir)
        {
            List<Comic> comics;
            using (var unitOfWork = new UnitOfWork(new DomLContext()))
            {
                comics = unitOfWork.ComicRepo.GetAllComics();
            }
            var filePath = fileDir + "COMICS.txt";
            using (var file = new StreamWriter(filePath))
            {
                foreach (var comic in comics)
                {
                    string comicString = comic.Id
                        + "\t" + comic.Title + "\t" + comic.Type
                        + "\t" + comic.Series + "\t" + comic.Number
                        + "\t" + comic.Person + "\t" + comic.Company
                        + "\t" + comic.Year + "\t" + comic.Score;
                    file.WriteLine(comicString);
                }
            }
        }

        // Used to save information
        // from a file into the database
        public static void SaveMediaFromFileToDatabase(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext()))
            {
                using (var reader = new StreamReader(fileDir + "COMICS.txt"))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        var comicInfo = Regex.Split(line, "\t");
                        int comicId = int.Parse(comicInfo[0]);
                        var comic = unitOfWork.ComicRepo.GetComicOfId(comicId);

                        if (comic == null) { continue; }

                        var correctId = comicInfo[9];
                        if (string.IsNullOrWhiteSpace(correctId))
                        {
                            comic.Title = comicInfo[1];
                            comic.Type = comicInfo[2];
                            comic.Series = comicInfo[3];
                            comic.Number = comicInfo[4];
                            comic.Person = comicInfo[5];
                            comic.Company = comicInfo[6];
                            comic.Year = comicInfo[7];
                            comic.Score = comicInfo[8];
                        }
                        else
                        {
                            // If this field is filled, that means that the info on this line is duplicated,
                            // and that we should update all records that point to this media to actually point to the given `correctId`
                            var comicActivityList = unitOfWork.ComicRepo.Find(b => b.ComicId == comicId);
                            var correctComicId = int.Parse(correctId);
                            foreach (var comicActivity in comicActivityList)
                            {
                                comicActivity.ComicId = correctComicId;
                            }
                            unitOfWork.ComicRepo.RemoveComic(comic);
                        }

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}
