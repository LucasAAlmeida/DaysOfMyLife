﻿using DomL.Business.Entities;
using DomL.Business.Services;
using DomL.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for MovieWindow.xaml
    /// </summary>
    public partial class MovieWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        enum NamedIndices
        {
            title = 0,
            series = 1,
            number = 2,
            person = 3,
            company = 4,
            year = 5,
            score = 6,
            description = 7
        }

        public MovieWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            InitializeComponent();

            UnitOfWork = unitOfWork;

            InfoMessage.Content = activity.GetInfoMessage();
            Util.FillSegmentosStack(segments, SegmentosStack);

            var movies = MovieService.GetAll(unitOfWork);

            var titleList = movies.Select(u => u.Title).Distinct().ToList();
            var seriesList = movies.Where(u => u.Series != null).Select(u => u.Series).Distinct().ToList();
            var numberList = Util.GetDefaultNumberList();
            var personList = movies.Where(u => u.Person != null).Select(u => u.Person).Distinct().ToList();
            var companyList = movies.Where(u => u.Company != null).Select(u => u.Company).Distinct().ToList();
            var yearList = Util.GetDefaultYearList();
            var scoreList = Util.GetDefaultScoreList();

            segments[0] = "";
            var remainingSegments = segments;
            var orderedSegments = new string[Enum.GetValues(typeof(NamedIndices)).Length];
            var indexesToAvoid = new int[] { (int)NamedIndices.number, (int)NamedIndices.year, (int)NamedIndices.score };

            Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.title, remainingSegments[1], indexesToAvoid);
            Util.SetComboBox(TitleCB, titleList, orderedSegments[(int)NamedIndices.title]);
            TitleCB_LostFocus(null, null);

            // MOVIE; Title; (Director Name); (Series Name); (Number In Series); (Score); (Description)
            while (remainingSegments.Length > 2 && orderedSegments.Any(u => u == null)) {
                var searched = remainingSegments[2];
                if (int.TryParse(searched, out int number)) {
                    searched = number.ToString("00");
                }

                if (Util.ListContainsText(seriesList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.series, searched, indexesToAvoid);
                } else if (Util.ListContainsText(numberList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.number, searched, indexesToAvoid);
                } else if (Util.ListContainsText(personList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.person, searched, indexesToAvoid);
                } else if (Util.ListContainsText(companyList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.company, searched, indexesToAvoid);
                } else if (Util.ListContainsText(yearList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.year, searched, indexesToAvoid);
                } else if (Util.ListContainsText(scoreList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.score, searched, indexesToAvoid);
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[2]).ToArray();
            }

            Util.SetComboBox(SeriesCB, seriesList, orderedSegments[(int)NamedIndices.series]);
            Util.SetComboBox(NumberCB, numberList, orderedSegments[(int)NamedIndices.number]);
            Util.SetComboBox(PersonCB, personList, orderedSegments[(int)NamedIndices.person]);
            Util.SetComboBox(CompanyCB, companyList, orderedSegments[(int)NamedIndices.company]);
            Util.SetComboBox(YearCB, yearList, orderedSegments[(int)NamedIndices.year]);
            Util.SetComboBox(ScoreCB, scoreList, orderedSegments[(int)NamedIndices.score]);
            Util.SetComboBox(DescriptionCB, new List<string>(), orderedSegments[(int)NamedIndices.description]);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void TitleCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TitleCB.IsKeyboardFocusWithin) {
                return;
            }

            var title = TitleCB.Text;
            var movie = MovieService.GetByTitle(title, UnitOfWork);
            Util.ChangeInfoLabel(title, movie, TitleInfoLb);

            if (movie != null) {
                UpdateOptionalComboBoxes(movie);
            }
        }

        private void UpdateOptionalComboBoxes(Movie movie)
        {
            SeriesCB.Text = movie.Series ?? SeriesCB.Text;
            NumberCB.Text = movie.Number ?? NumberCB.Text;
            PersonCB.Text = movie.Person ?? PersonCB.Text;
            CompanyCB.Text = movie.Company ?? CompanyCB.Text;
            YearCB.Text = movie.Year ?? YearCB.Text;
            ScoreCB.Text = movie.Score ?? ScoreCB.Text;
        }
    }
}
