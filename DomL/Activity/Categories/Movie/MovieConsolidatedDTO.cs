using DomL.Business.Entities;
using DomL.Business.Utils;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class MovieConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Title;
        public string Series;
        public string Number;
        public string Person;
        public string Company;
        public string Year;
        public string Score;
        public string Description;

        public MovieConsolidatedDTO(Activity activity) : base(activity)
        {
            var movieActivity = activity.MovieActivity;
            var movie = movieActivity.Movie;
            
            Title = Util.GetStringOrDash(movie.Title);
            Series = Util.GetStringOrDash(movie.Series);
            Number = Util.GetStringOrDash(movie.Number);
            Person = Util.GetStringOrDash(movie.Person);
            Company = Util.GetStringOrDash(movie.Company);
            Year = Util.GetStringOrDash(movie.Year);
            Score = Util.GetStringOrDash(movie.Score);
            Description = Util.GetStringOrDash(movieActivity.Description);

            FillCommonInfo();
        }

        public MovieConsolidatedDTO(MovieWindow movieWindow, Activity activity) : base(activity)
        {
            Title = Util.GetStringOrDash(movieWindow.TitleCB.Text);
            Series = Util.GetStringOrDash(movieWindow.SeriesCB.Text);
            Number = Util.GetStringOrDash(movieWindow.NumberCB.Text);
            Person = Util.GetStringOrDash(movieWindow.PersonCB.Text);
            Company = Util.GetStringOrDash(movieWindow.CompanyCB.Text);
            Year = Util.GetStringOrDash(movieWindow.YearCB.Text);
            Score = Util.GetStringOrDash(movieWindow.ScoreCB.Text);
            Description = Util.GetStringOrDash(movieWindow.DescriptionCB.Text);

            FillCommonInfo();
        }

        public MovieConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Title = backupSegments[5];
            Series = backupSegments[6];
            Number = backupSegments[7];
            Person = backupSegments[8];
            Company = backupSegments[9];
            Year = backupSegments[10];
            Score = backupSegments[11];
            Description = backupSegments[12];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "MOVIE";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
                + GetMovieActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + Title;
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetMovieActivityInfo();
        }

        public string GetMovieActivityInfo()
        {
            return Title + "\t" + Person
                + "\t" + Series + "\t" + Number
                + "\t" + Company + "\t" + Year
                + "\t" + Score + "\t" + Description;
        }
    }
}
