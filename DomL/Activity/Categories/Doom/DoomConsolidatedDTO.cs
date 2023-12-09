using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class DoomConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Description;

        public DoomConsolidatedDTO(Activity activity) : base(activity)
        {
            var doomActivity = activity.DoomActivity;

            Description = Util.GetStringOrDash(doomActivity.Description);

            FillCommonInfo();
        }

        public DoomConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Description = Util.GetStringOrDash(rawSegments[1]);

            FillCommonInfo();
        }

        public DoomConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Description = backupSegments[5];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "DOOM";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
                + GetDoomActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t\t" + Description;
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetDoomActivityInfo();
        }

        public string GetDoomActivityInfo()
        {
            return Description;
        }
    }
}
