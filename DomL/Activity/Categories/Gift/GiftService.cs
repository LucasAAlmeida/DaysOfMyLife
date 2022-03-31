﻿using DomL.Business.DTOs;
using DomL.Business.Entities;
using DomL.Business.Utils;
using DomL.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class GiftService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            var consolidated = new GiftConsolidatedDTO(rawSegments, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new GiftConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(GiftConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var isFrom = consolidated.IsToOrFrom.ToLower() == "from";

            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateGiftActivity(activity, consolidated.Gift, isFrom, consolidated.Who, consolidated.Description, unitOfWork);
        }

        private static void CreateGiftActivity(Activity activity, string gift, bool isFrom, string who, string description, UnitOfWork unitOfWork)
        {
            var giftActivity = new GiftActivity() {
                Activity = activity,
                Gift = Util.GetStringOrNull(gift),
                IsFrom = isFrom,
                Who = Util.GetStringOrNull(who),
                Description = Util.GetStringOrNull(description)
            };

            activity.GiftActivity = giftActivity;

            unitOfWork.GiftRepo.CreateGiftActivity(giftActivity);
        }
    }
}
