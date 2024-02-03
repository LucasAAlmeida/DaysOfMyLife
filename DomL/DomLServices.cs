﻿using DomL.Business.Entities;
using DomL.Business.Utils;

using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


/* IDEIAS
- Windows nao deixarem submeter a nao ser que todos os campos tenham sido preenchidos
- ao inves de escrever em label que deu certo, fazer um dialog box popar pra eu ter que clicar
- qnd clicar no ok da tela principal, procurar atividades naquele mes naquele ano. se tiver, aparecer popup perguntando se tem ctz
- se em algum segment tiver a string "com" (ex: "com o puggy") provavelmente eh descrição, pode dar sort pra lah
- receber os segments direto pras classes 'consolidated<categoria>dto.cs', e salvar no banco a partir de lá
- fazer o backup imprimindo cada atividade completa a partir do 'consolidated<categoria>dto.cs'
- fazer o restore do banco completo a partir do 'consolidated<categoria>dto.cs'
- show eh mais facil ter canal do que diretor (tem campo pra diretor, mas nao pra canal)
- jogo ou show eh melhor ter campo 'creator' do que director
- separar pessoas de creators/artists
- se nao cair numa tag conhecida qnd tah fazendo parse, TEM que dar erro
- se nao encontrar nenhuma serie correspondente repete o titulo (pra jogos)
- seriesseason e comicvolume nas janelas tb podem ter validação se jah existem ou não
- Movie só pode ser atualizado se tanto o titulo quanto o ano de lançamento (ou nome da série) for o mesmo
- campo pra resumo em movie, game, showseason, comicvolume, course, etc
- add manwha to comictypes
- add ova, animation, 3d animation, live action to movie type
- add reality show to show types
- se tiver mais de 3 palavras, provavelmente o segmento faz parte da descrição
- colocar restore em uma window e backups em outra window
- fazer uma pasta pra windows
*/
namespace DomL.Business.Services
{
    public class DomLServices
    {
        const string BASE_DIR = "C:\\Users\\Lyucs\\OneDrive\\Área de Trabalho\\DomL\\";
        const string RECAPS_DIR = BASE_DIR + "Recaps\\";

        public static void SaveFromRawMonthText(string rawMonthText, int month, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                string block = null;
                var date = new DateTime(year, month, 1);
                var dayOrder = 0;

                var activityRawLines = Regex.Split(rawMonthText, "\r\n");
                foreach(var rawLine in activityRawLines) {
                    try {
                        if (Util.IsLineBlank(rawLine)) {
                            continue;
                        }

                        if (Util.IsLineNewMonth(rawLine, out int mes))
                        {
                            month = mes;
                            continue;
                        }

                        if (Util.IsLineNewDay(rawLine, out int dia)) {
                            date = new DateTime(year, month, dia);
                            dayOrder = 0;

                            unitOfWork.ActivityRepo.DeleteAllFromDay(date);
                            unitOfWork.Complete();

                            continue;
                        }

                        if (Util.IsLineBlockTag(rawLine)) {
                            block = (rawLine != "<END>") ? rawLine.Substring(1, rawLine.Length - 2) : null;
                            continue;
                        }

                        dayOrder++;

                        var status = ActivityService.GetStatus(rawLine, unitOfWork);
                        var category = ActivityService.GetCategory(rawLine, unitOfWork);

                        var activity = new Activity() {
                            Date = date,
                            DayOrder = dayOrder,
                            Status = status,
                            Category = category,
                            Block = block,
                            ConsolidatedLine = rawLine
                        };

                        var rawSegments = Regex.Split(rawLine, "; ");
                        ActivityService.SaveFromRawSegments(rawSegments, activity, unitOfWork);

                        unitOfWork.Complete();
                    } catch (Exception e) {
                        var msg = "Deu ruim no dia " + date.Day + ", atividade: " + rawLine;
                        throw new ParseException(msg, e);
                    }
                }
            }
        }

        //===============================================================
        //===============================================================
        //===============================================================

        public static void RestoreFromFileToDatabase(string fileDir, int categoryId)
        {
            Category category;
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                category = unitOfWork.ActivityRepo.GetCategoryById(categoryId);

                unitOfWork.ActivityRepo.DeleteAllFromCategory(categoryId);
                unitOfWork.Complete();

                using (var reader = new StreamReader(fileDir + category.Name + ".txt")) {
                    string line = "";
                    while ((line = reader.ReadLine()) != null) {
                        if (string.IsNullOrWhiteSpace(line)) {
                            continue;
                        }

                        var backupSegments = Regex.Split(line, "\t");

                        ActivityService.SaveFromBackupSegments(backupSegments, category, unitOfWork);
                        unitOfWork.Complete();
                    }
                }
            }
        }

        public static void BackupFromDatabaseToFile(string fileDir, int categoryId)
        {
            List<Activity> activities;
            Category category;
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                activities = unitOfWork.ActivityRepo.GetAllInclusiveFromCategory(categoryId);
                category = unitOfWork.ActivityRepo.GetCategoryById(categoryId);
            }

            var filePath = fileDir + category.Name + ".txt";

            if (activities.Count == 0) {
                return;
            }

            using (var file = new StreamWriter(filePath)) {
                foreach (var activity in activities) {
                    string activityString = ActivityService.GetInfoForBackup(activity);
                    if (!string.IsNullOrWhiteSpace(activityString)) {
                        file.WriteLine(activityString);
                    }
                }
            }
        }

        //===============================================================

        // Saves Media Info that is in the Database into a File
        public static void SaveMediaFromDatabaseToFile(string fileDir, int categoryId)
        {
            switch (categoryId)
            {
                case Category.BOOK_ID: BookService.SaveMediaFromDatabaseToFile(fileDir); break;
                case Category.COMIC_ID: ComicService.SaveMediaFromDatabaseToFile(fileDir); break;
                case Category.GAME_ID: GameService.SaveMediaFromDatabaseToFile(fileDir); break;
                case Category.MOVIE_ID: MovieService.SaveFromDatabaseToFile(fileDir); break;
                case Category.SHOW_ID: ShowService.SaveFromDatabaseToFile(fileDir); break;
                default: break;
            }
        }

        // Saves Media Info that is in a File into the Database
        public static void SaveMediaFromFileToDatabase(string fileDir, int categoryId)
        {
            switch (categoryId)
            {
                case Category.BOOK_ID: BookService.SaveMediaFromFileToDatabase(fileDir); break;
                case Category.COMIC_ID: ComicService.SaveMediaFromFileToDatabase(fileDir); break;
                case Category.GAME_ID: GameService.SaveMediaFromFileToDatabase(fileDir); break;
                //case Category.MOVIE_ID: MovieService.SaveMediaFromFileToDatabase(fileDir); break;
                //case Category.SHOW_ID: ShowService.SaveMediaFromFileToDatabase(fileDir); break;
                default: break;
            }
        }

        //===============================================================
        //===============================================================
        //===============================================================

        /// <summary>
        /// Gets all activities for the specified month/year,
        /// then prints it to be put in the "Resumo do Mês" in the DomL, alongside the full raw month data from oneNote
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        public static void WriteMonthRecapFile(int month, int year)
        {
            string filePath = RECAPS_DIR + "Year" + year + "\\Months\\Month" + month.ToString("00") + ".txt";
            Util.CreateDirectory(filePath);

            List<Activity> monthActivities;
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                monthActivities = unitOfWork.ActivityRepo.GetAllInclusiveFromMonth(month, year);
            }
            
            using (var file = new StreamWriter(filePath)) {
                foreach (Activity activity in monthActivities) {
                    string activityString = ActivityService.GetInfoForMonthRecap(activity);
                    if (!string.IsNullOrWhiteSpace(activityString)) {
                        file.WriteLine(activityString);
                    }
                }
            }
        }

        /// <summary>
        /// Get all activities in the given `year`, then for each category,
        /// writes the information that will be put in the "Entretenimento", "Acontecimentos", and "Eventos" of the DomL
        /// </summary>
        /// <param name="year"></param>
        public static void WriteYearRecapFiles(int year)
        {
            string fileDir = RECAPS_DIR + "Year" + year + "\\Categories\\";
            Util.CreateDirectory(fileDir);

            List<Activity> yearActivities;
            List<Category> categories;
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                yearActivities = unitOfWork.ActivityRepo.GetAllInclusiveFromYear(year);
                categories = unitOfWork.ActivityRepo.GetAllCategories();
            }

            foreach(var category in categories) {
                var filePath = fileDir + category.Name + ".txt";
                var yearCategoryActivities = yearActivities.Where(u => u.CategoryId == category.Id).ToList();
                
                if (yearCategoryActivities.Count == 0) {
                    continue;
                }

                using (var file = new StreamWriter(filePath)) {
                    foreach (var yearActivity in yearCategoryActivities) {
                        string activityString = ActivityService.GetInfoForYearRecap(yearActivity);
                        if (!string.IsNullOrWhiteSpace(activityString)) {
                            file.WriteLine(activityString);
                        }
                    }
                }
            }
            WriteStatisticsFile(yearActivities, year);
        }

        private static void WriteStatisticsFile(List<Activity> activities, int year = 0)
        {
            string fileDir = RECAPS_DIR;
            if (year != 0) {
                fileDir += "Year" + year + "\\";
            }

            using (var file = new StreamWriter(fileDir + "Statistics.txt")) {
                file.WriteLine("Jogos começados:\t" + CountStarted(activities, Category.GAME_ID));
                file.WriteLine("Jogos terminados:\t" + CountFinished(activities, Category.GAME_ID));
                file.WriteLine("Temporadas de séries assistidas:\t" + CountFinished(activities, Category.SHOW_ID));
                file.WriteLine("Livros lidos:\t" + CountFinished(activities, Category.BOOK_ID));
                file.WriteLine("K Páginas de comics lidos:\t" + CountFinished(activities, Category.COMIC_ID));
                file.WriteLine("Filmes assistidos:\t" + CountFinished(activities, Category.MOVIE_ID));
                file.WriteLine("Viagens feitas:\t" + CountFinished(activities, Category.TRAVEL_ID));
                file.WriteLine("Pessoas novas conhecidas:\t" + CountFinished(activities, Category.MEET_ID));
                file.WriteLine("Compras notáveis:\t" + CountFinished(activities, Category.PURCHASE_ID));
            }
        }

        private static int CountStarted(List<Activity> activities, int categoryId)
        {
            return activities.Count(u => u.CategoryId == categoryId && u.StatusId != Status.FINISH);
        }

        private static int CountFinished(List<Activity> activities, int categoryId)
        {
            return activities.Count(u => u.CategoryId == categoryId && u.StatusId != Status.START);
        }
    }
}
