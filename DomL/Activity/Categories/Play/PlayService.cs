﻿using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Business.Utils;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class PlayService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new PlayConsolidatedDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new PlayConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(PlayConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreatePlayActivity(activity, consolidated.Who, consolidated.Description, unitOfWork);
        }

        private static void CreatePlayActivity(Activity activity, string person, string description, UnitOfWork unitOfWork)
        {
            var playActivity = new PlayActivity() {
                Activity = activity,
                Who = Util.GetStringOrNull(person),
                Description = Util.GetStringOrNull(description)
            };

            activity.PlayActivity = playActivity;

            unitOfWork.PlayRepo.CreatePlayActivity(playActivity);
        }
    }
}
