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
    public class GameService
    {
        public static void SaveFromRawSegments(string[] rawSegments, Activity activity, UnitOfWork unitOfWork)
        {
            // GAME (Classification); Title; Platform Name; (Series Name); (Number In Series); (Director Name); (Publisher Name); (Score); (Description)
            rawSegments[0] = "";
            var gameWindow = new GameWindow(rawSegments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                gameWindow.ShowDialog();
            }

            var consolidated = new ConsolidatedGameDTO(gameWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new ConsolidatedGameDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(ConsolidatedGameDTO consolidated, UnitOfWork unitOfWork)
        {
            var game = GetOrUpdateOrCreateGame(consolidated, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);
            CreateGameActivity(activity, game, consolidated.Description, unitOfWork);
        }

        public static List<Game> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.GameRepo.GetAllGames();
        }

        private static Game GetOrUpdateOrCreateGame(ConsolidatedGameDTO consolidated, UnitOfWork unitOfWork)
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
                instance = new Game() {
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

        private static void CreateGameActivity(Activity activity, Game game, string description, UnitOfWork unitOfWork)
        {
            var gameActivity = new GameActivity() {
                Activity = activity,
                Game = game,
                Description = Util.GetStringOrNull(description)
            };

            activity.GameActivity = gameActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.GameRepo.CreateGameActivity(gameActivity);
        }

        //TODO add year to search
        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var game = activity.GameActivity.Game;
            return previousStartingActivities.Where(u => 
                u.CategoryId == Category.GAME_ID
                && u.GameActivity.Game.Title == game.Title
            );
        }

        public static List<string> GetDefaultTypeList()
        {
            var list = new List<string>();
            list.AddRange(new string[] {
                "Watch",
                "PC",
                "PS5", "PS4", "PS3", "PS2", "PSX",
                "Switch", "Wii U", "Wii", "NGC", "N64", "SNES", "NES",
                "3DS", "NDS", "GBA", "GBC", "NGB",
                "XBOX Series", "XBOX One", "XBOX360", "XBOX",
            });
            return list;
        }

        public static Game GetByTitle(string title, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(title)) {
                return null;
            }
            return unitOfWork.GameRepo.GetGameByTitle(title);
        }

        // Used to get information
        // from the database into a file
        // to be more easily manipulated
        public static void SaveMediaFromDatabaseToFile(string fileDir)
        {
            List<Game> games;
            using (var unitOfWork = new UnitOfWork(new DomLContext()))
            {
                games = unitOfWork.GameRepo.GetAllGames();
            }
            var filePath = fileDir + "GAMES.txt";
            using (var file = new StreamWriter(filePath))
            {
                foreach (var game in games)
                {
                    string gameString = game.Id
                        + "\t" + game.Title + "\t" + game.Type
                        + "\t" + game.Series + "\t" + game.Number
                        + "\t" + game.Person + "\t" + game.Company
                        + "\t" + game.Year + "\t" + game.Score;
                    file.WriteLine(gameString);
                }
            }
        }

        // Used to save information
        // from a file into the database
        public static void SaveMediaFromFileToDatabase(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext()))
            {
                using (var reader = new StreamReader(fileDir + "GAMES.txt"))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        var gameInfo = Regex.Split(line, "\t");
                        var gameId = 0;
                        var gameExists = int.TryParse(gameInfo[0], out gameId);

                        var correctId = gameInfo[9];
                        if (!string.IsNullOrWhiteSpace(correctId))
                        {
                            FixDuplicatedMedia(unitOfWork, gameId, correctId);
                            continue;
                        }

                        if (!gameExists)
                        {
                            CreateMedia(unitOfWork, gameInfo);
                            continue;
                        }

                        UpdateExistingMedia(unitOfWork, gameId, gameInfo);
                    }

                    unitOfWork.Complete();
                }
            }
        }

        /// <summary>
        /// The game record of id `gameId` is a duplicate of the game record of id `correctId`
        /// we should update all records that point to `gameId` to actually point to `correctId`
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="gameId"></param>
        /// <param name="correctId"></param>
        private static void FixDuplicatedMedia(UnitOfWork unitOfWork, int gameId, string correctId)
        {
            var gameActivityList = unitOfWork.GameRepo.Find(b => b.GameId == gameId);
            var correctGameId = int.Parse(correctId);
            foreach (var gameActivity in gameActivityList)
            {
                gameActivity.GameId = correctGameId;
            }
            unitOfWork.GameRepo.RemoveGameOfId(gameId);
        }

        private static void CreateMedia(UnitOfWork unitOfWork, string[] gameInfo)
        {
            var game = new Game();
            FillGameData(game, gameInfo);
            unitOfWork.GameRepo.CreateGame(game);
        }

        private static void UpdateExistingMedia(UnitOfWork unitOfWork, int gameId, string[] gameInfo)
        {
            var game = unitOfWork.GameRepo.GetGameOfId(gameId);
            FillGameData(game, gameInfo);
        }

        private static void FillGameData(Game game, string[] gameInfo)
        {
            game.Title = gameInfo[1];
            game.Type = gameInfo[2];
            game.Series = gameInfo[3];
            game.Number = gameInfo[4];
            game.Person = gameInfo[5];
            game.Company = gameInfo[6];
            game.Year = gameInfo[7];
            game.Score = gameInfo[8];
        }
    }
}
