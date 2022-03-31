﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DomL.Business.Utils
{
    public class Util
    {
        public static bool IsEqualString(string string1, string string2)
        {
            string rExp = @"[^\w\d]";
            var string1Limpa = Regex.Replace(string1, rExp, "").ToLower().Replace("the", "");
            var string2Limpa = Regex.Replace(string2, rExp, "").ToLower().Replace("the", "");
            return string1Limpa == string2Limpa;
        }

        public static string CleanString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) {
                return null;
            }
            return value.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "");
        }

        public static bool ListContainsText(List<string> list, string searched)
        {
            var cleanSearched = CleanString(searched);
            return list.Any(u => CleanString(u) == cleanSearched);
        }

        public static void FillSegmentosStack(string[] segments, StackPanel segmentosStack)
        {
            for (int index = 1; index < segments.Length; index++) {
                var segmento = segments[index];
                var dynLabel = new TextBox {
                    Text = segmento,
                    IsReadOnly = true,
                    Margin = new Thickness(5)
                };

                segmentosStack.Children.Add(dynLabel);
            }
        }

        public static string GetFormatedDate(DateTime date)
        {
            return date.ToString("yyyy/MM/dd");
        }

        public static void CreateDirectory(string filePath)
        {
            var fi = new FileInfo(filePath);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
                Directory.CreateDirectory(fi.DirectoryName);
            }
        }

        public static List<string> GetDefaultNumberList()
        {
            var list = new List<string>();
            for (int number = 0; number <= 10; number++) {
                list.Add(number.ToString("00"));
            }
            return list;
        }

        public static List<string> GetDefaultSeasonsList()
        {
            var list = new List<string>();
            for (int i = 1; i < 20; i++) {
                list.Add("S" + i.ToString("00"));
            }
            return list;
        }

        public static List<string> GetDefaultChaptersList()
        {
            var list = new List<string>();
            for (int i = 1; i < 500; i += 50) {
                list.Add(i.ToString("000") + "~" + (i + 50).ToString("000"));
            }
            return list;
        }

        public static List<string> GetDefaultYearList()
        {
            var list = new List<string>();
            for (int year=2020; year>=1988; year--) {
                list.Add(year.ToString());
            }
            return list;
        }

        public static List<string> GetDefaultScoreList()
        {
            var list = new List<string>();
            for (int score=100; score>0; score-=5) {
                list.Add(score.ToString());
            }
            return list;
        }

        public static void SetComboBox(ComboBox comboBox, List<string> nameList, string chosenStr)
        {
            var possibleStrings = new List<string>();
            possibleStrings.AddRange(nameList);
            comboBox.ItemsSource = possibleStrings;
            comboBox.Text = !string.IsNullOrWhiteSpace(chosenStr) ? chosenStr : comboBox.Text;
        }

        public static bool IsStringEmpty(string text)
        {
            return string.IsNullOrEmpty(text) || text == "-";
        }

        public static string GetStringOrNull(string text)
        {
            return !IsStringEmpty(text) ? text : null;
        }

        public static int GetIntOrZero(string textNumber)
        {
            return !IsStringEmpty(textNumber) ? int.Parse(textNumber) : 0;
        }

        public static string GetStringOrDash(string text)
        {
            return text ?? "-";
        }

        public static void PlaceOrderedSegment(string[] orderedSegments, int index, string toPlace, int[] indexesToAvoid)
        {
            if (orderedSegments[index] != null) {
                var displaced = orderedSegments[index];
                PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, displaced);
            }
            orderedSegments[index] = toPlace;
        }

        public static void PlaceStringInFirstAvailablePosition(string[] orderedSegments, int[] indexesToAvoid, string searched)
        {
            var emptyIndex = GetFirstEmptyIndex(orderedSegments, indexesToAvoid);
            if (emptyIndex != -1) {
                orderedSegments[emptyIndex] = searched;
            }
        }

        public static int GetFirstEmptyIndex(string[] orderedSegments, int[] indexesToAvoid)
        {
            for (int i = 0; i < orderedSegments.Length; i++) {
                if (!indexesToAvoid.Contains(i) && orderedSegments[i] == null) {
                    return i;
                }
            }
            return -1;
        }

        public static void ChangeInfoLabel(string instanceName, object instance, Label infoLabel)
        {
            if (string.IsNullOrWhiteSpace(instanceName)) {
                infoLabel.Content = "";
                return;
            }

            if (instance == null) {
                infoLabel.Content = "New";
                infoLabel.Foreground = Brushes.DarkGreen;
                return;
            }

            infoLabel.Content = "Exists";
            infoLabel.Foreground = Brushes.Goldenrod;
        }

        public static bool IsLineBlockTag(string line)
        {
            return line.StartsWith("<");
        }

        public static bool IsLineBlank(string line)
        {
            return string.IsNullOrWhiteSpace(line) || line.StartsWith("---");
        }

        public static bool IsLineNewDay(string linha, out int dia)
        {
            int indexPrimeiroEspaco = linha.IndexOf(" ", StringComparison.Ordinal);
            string firstWord = (indexPrimeiroEspaco != -1) ? linha.Substring(0, indexPrimeiroEspaco) : linha;
            return int.TryParse(firstWord, out dia) && (linha.Contains(" - ") || linha.Contains(" – "));
        }
    }
}
