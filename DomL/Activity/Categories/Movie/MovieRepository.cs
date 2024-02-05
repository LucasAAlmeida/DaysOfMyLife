using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;
using System.Collections.Generic;

namespace DomL.DataAccess
{
    public class MovieRepository : DomLRepository<MovieActivity>
    {
        public MovieRepository(DomLContext context) : base(context) { }
        
        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public Movie GetMovieByTitle(string title)
        {
            var cleanTitle = Util.CleanString(title);
            return DomLContext.Movie
                .SingleOrDefault(u =>
                    u.Title.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanTitle
                );
        }

        public void CreateMovieActivity(MovieActivity movieActivity)
        {
            DomLContext.MovieActivity.Add(movieActivity);
        }

        public void CreateMovie(Movie movie)
        {
            DomLContext.Movie.Add(movie);
        }

        public List<Movie> GetAllMovies()
        {
            return DomLContext.Movie.ToList();
        }

        public Movie GetMovieOfId(int movieId)
        {
            return DomLContext.Movie.SingleOrDefault(e => e.Id == movieId);
        }

        public void RemoveMovieOfId(int movieId)
        {
            var movie = DomLContext.Movie.Single(g => g.Id == movieId);
            DomLContext.Movie.Remove(movie);
        }
    }
}
