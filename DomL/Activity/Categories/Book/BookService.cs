﻿using DomL.Business.Entities;
using DomL.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using DomL.DataAccess;
using DomL.Business.DTOs;
using System.IO;
using System.Text.RegularExpressions;
using DomL.Business.Utils;

namespace DomL.Business.Services
{
    public class BookService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            var bookWindow = new BookWindow(segments, activity, unitOfWork);

            if (ConfigurationManager.AppSettings["ShowCategoryWindows"] == "true") {
                bookWindow.ShowDialog();
            }

            var consolidated = new BookConsolidatedDTO(bookWindow, activity);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        public static void SaveFromBackupSegments(string[] backupSegments, UnitOfWork unitOfWork)
        {
            var consolidated = new BookConsolidatedDTO(backupSegments);
            SaveFromConsolidated(consolidated, unitOfWork);
        }

        private static void SaveFromConsolidated(BookConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var book = GetOrUpdateOrCreateBook(consolidated, unitOfWork);
            var activity = ActivityService.Create(consolidated, unitOfWork);

            CreateBookActivity(activity, book, consolidated.Description, unitOfWork);
        }

        private static void CreateBookActivity(Activity activity, Book book, string description, UnitOfWork unitOfWork)
        {
            var bookActivity = new BookActivity() {
                Activity = activity,
                Book = book,
                Description = Util.GetStringOrNull(description)
            };

            activity.BookActivity = bookActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.BookRepo.CreateBookActivity(bookActivity);
        }

        public static List<Book> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.BookRepo.GetAllBooks();
        }

        public static Book GetOrUpdateOrCreateBook(BookConsolidatedDTO consolidated, UnitOfWork unitOfWork)
        {
            var instance = GetByTitle(consolidated.Title, unitOfWork);

            var title = Util.GetStringOrNull(consolidated.Title);
            var series = Util.GetStringOrNull(consolidated.Series);
            var number = Util.GetStringOrNull(consolidated.Number);
            var person = Util.GetStringOrNull(consolidated.Person);
            var company = Util.GetStringOrNull(consolidated.Company);
            var year = Util.GetStringOrNull(consolidated.Year);
            var score = Util.GetStringOrNull(consolidated.Score);

            if (instance == null) {
                instance = new Book() {
                    Title = title,
                    Series = series,
                    Number = number,
                    Person = person,
                    Company = company,
                    Year = year,
                    Score = score,
                };
            } else {
                instance.Series = series ?? instance.Series;
                instance.Number = number ?? instance.Number;
                instance.Person = person ?? instance.Person;
                instance.Company = company ?? instance.Company;
                instance.Year = year ?? instance.Year;
                instance.Score = score ?? instance.Score;
            }

            return instance;
        }

        //TODO (add year to search)
        public static Book GetByTitle(string title, UnitOfWork unitOfWork)
        {
            if (Util.IsStringEmpty(title)) {
                return null;
            }
            return unitOfWork.BookRepo.GetBookByTitle(title);
        }

        //TODO (add year to search)
        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var book = activity.BookActivity.Book;
            return previousStartingActivities.Where(u =>
                u.CategoryId == Category.BOOK_ID
                && u.BookActivity.Book.Title == book.Title
            );
        }

        // Used to get information
        // from the database into a file
        // to be more easily manipulated
        public static void SaveMediaFromDatabaseToFile(string fileDir)
        {
            List<Book> books;
            using (var unitOfWork = new UnitOfWork(new DomLContext()))
            {
                books = unitOfWork.BookRepo.GetAllBooks();
            }
            var filePath = fileDir + "BOOKS.txt";
            using (var file = new StreamWriter(filePath))
            {
                foreach (var book in books)
                {
                    string bookString = book.Id + "\t" + book.Title
                        + "\t" + book.Series + "\t" + book.Number
                        + "\t" + book.Person + "\t" + book.Company
                        + "\t" + book.Year + "\t" + book.Score;
                    file.WriteLine(bookString);
                }
            }
        }

        // Used to save information
        // from a file into the database
        public static void SaveMediaFromFileToDatabase(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext()))
            {
                using (var reader = new StreamReader(fileDir + "BOOKS.txt"))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        var bookInfo = Regex.Split(line, "\t");
                        int bookId = int.Parse(bookInfo[0]);
                        var book = unitOfWork.BookRepo.GetBookOfId(bookId);

                        if (book == null)
                        {
                            book = new Book();
                            book.Id = bookId;
                        }

                        var correctId = bookInfo[8];
                        if (string.IsNullOrWhiteSpace(correctId))
                        {
                            book.Title = bookInfo[1];
                            book.Series = bookInfo[2];
                            book.Number = bookInfo[3];
                            book.Person = bookInfo[4];
                            book.Company = bookInfo[5];
                            book.Year = bookInfo[6];
                            book.Score = bookInfo[7];
                        }
                        else
                        {
                            // If this field is filled, that means that the info on this line is duplicated,
                            // and that we should update all records that point to this media to actually point to the given `correctId`
                            var bookActivityList = unitOfWork.BookRepo.Find(b => b.BookId == bookId);
                            var correctBookId = int.Parse(correctId);
                            foreach (var bookActivity in bookActivityList)
                            {
                                bookActivity.BookId = correctBookId;
                            }
                            unitOfWork.BookRepo.RemoveBook(book);
                        }

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}
