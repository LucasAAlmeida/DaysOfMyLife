﻿using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.DTOs
{
    public class MeetConsolidatedDTO : ActivityConsolidatedDTO
    {
        public string Person;
        public string Origin;
        public string Description;

        public MeetConsolidatedDTO(Activity activity) : base(activity)
        {
            var meetActivity = activity.MeetActivity;

            Person = Util.GetStringOrDash(meetActivity.Person);
            Origin = Util.GetStringOrDash(meetActivity.Origin);
            Description = Util.GetStringOrDash(meetActivity.Description);

            FillCommonInfo();
        }

        public MeetConsolidatedDTO(string[] rawSegments, Activity activity) : base(activity)
        {
            Person = Util.GetStringOrDash(rawSegments[1]);
            Origin = Util.GetStringOrDash(rawSegments[2]);
            Description = Util.GetStringOrDash(rawSegments.Length == 4 ? rawSegments[3] : "-");
            
            FillCommonInfo();
        }

        public MeetConsolidatedDTO(string[] backupSegments) : base(backupSegments)
        {
            Person = backupSegments[5];
            Origin = backupSegments[6];
            Description = backupSegments[7];

            FillCommonInfo();
        }

        private void FillCommonInfo()
        {
            CategoryName = "MEET";
            ConsolidatedLine = GetInfoForConsolidatedLine() + "; "
                + GetMeetActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + Person + "\t" + Origin + "\t" + Description;
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetMeetActivityInfo();
        }

        public string GetMeetActivityInfo()
        {
            return Person + "\t" + Origin + "\t" + Description;
        }
    }
}
