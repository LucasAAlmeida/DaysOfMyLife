using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class ActivityConsolidatedDTO
    {
        public string Date;
        public string DayOrder;
        public string Block;
        public string ConsolidatedLine;

        public string StatusName;
        public string CategoryName;

        public string DatesStartAndFinish;

        public ActivityConsolidatedDTO(Activity activity)
        {
            Date = Util.GetFormatedDate(activity.Date);
            DayOrder = activity.DayOrder.ToString();
            Block = Util.GetStringOrDash(activity.Block);
            ConsolidatedLine = activity.ConsolidatedLine;

            var pairedDate = (activity.PairedActivity != null) ? Util.GetFormatedDate(activity.PairedActivity.Date) : "????-??-??";
            switch (activity.Status.Id) {
                case Status.SINGLE:
                    StatusName = "Single";
                    DatesStartAndFinish = Date;
                    break;
                case Status.START:
                    StatusName = "Start";
                    DatesStartAndFinish = Date + "\t" + pairedDate;
                    break;
                case Status.FINISH:
                    StatusName = "Finish";
                    DatesStartAndFinish = pairedDate + "\t" + Date;
                    break;
            }
        }

        // This is to restore data to the database from a previous backup
        // This should be in the same order than the GetInfoForBackup() method
        public ActivityConsolidatedDTO(string[] segments)
        {
            Date = segments[0];
            DayOrder = segments[1];
            CategoryName = segments[2];
            StatusName = segments[3];
            Block = segments[4];
        }

        protected string GetInfoForConsolidatedLine()
        {
            string statusName = (StatusName != "-" && StatusName != "Single") ? " " + StatusName : "";
            string blockName = Block != "-" ? " " + Block : "";

            return CategoryName + statusName + blockName;
        }

        public string GetInfoForMonthRecap()
        {
            return Date + "\t" + ConsolidatedLine;
        }

        protected string GetInfoForYearRecap()
        {
            return DatesStartAndFinish;
        }

        protected string GetInfoForBackup()
        {
            //      0               1                   2                   3                   4
            return Date + "\t" + DayOrder + "\t" + CategoryName + "\t" + StatusName + "\t" + Block;
        }
    }
}