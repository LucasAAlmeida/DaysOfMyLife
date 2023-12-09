using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class GiftConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Gift;
        public string IsToOrFrom;
        public string Who;
        public string Description;

        public GiftConsolidatedDTO(Activity activity) : base(activity)
        {
            var giftActivity = activity.GiftActivity;

            Gift = Util.GetStringOrDash(giftActivity.Gift);
            IsToOrFrom = giftActivity.IsFrom ? "From" : "To";
            Who = Util.GetStringOrDash(giftActivity.Who);
            Description = Util.GetStringOrDash(giftActivity.Description);

            FillCommonInfo();
        }
        public GiftConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Gift = Util.GetStringOrDash(rawSegments[1]);
            IsToOrFrom = Util.GetStringOrDash(rawSegments[2]);
            Who = Util.GetStringOrDash(rawSegments[3]);
            Description = Util.GetStringOrDash(rawSegments.Length > 4 ? rawSegments[4] : "-");

            FillCommonInfo();
        }

        public GiftConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Gift = backupSegments[5];
            IsToOrFrom = backupSegments[6];
            Who = backupSegments[7];
            Description = backupSegments[8];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "GIFT";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
                + GetGiftActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + Gift + "\t" + IsToOrFrom + " " + Who + "\t" + Description;
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetGiftActivityInfo();
        }

        public string GetGiftActivityInfo()
        {
            return Gift + "\t" + IsToOrFrom + "\t" + Who + "\t" + Description;
        }
    }
}
