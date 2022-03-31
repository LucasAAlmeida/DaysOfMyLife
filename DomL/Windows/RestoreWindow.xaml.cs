﻿
using DomL.Business.Entities;
using DomL.Business.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for RestoreFullWindow.xaml
    /// Write on database from files
    /// </summary>
    public partial class RestoreWindow : Window
    {
        const string BACKUP_DIR_PATH = "C:\\Users\\Lyucs\\OneDrive\\Área de Trabalho\\DomL\\Backup\\";

        public RestoreWindow()
        {
            InitializeComponent();
        }

        private void AllRestoreButton_Click(object sender, RoutedEventArgs e)
        {

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.AUTO_ID);
            Console.WriteLine("AUTO");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.BOOK_ID);
            Console.WriteLine("BOOK");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.COMIC_ID);
            Console.WriteLine("COMIC");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.COURSE_ID);
            Console.WriteLine("COURSE");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.DOOM_ID);
            Console.WriteLine("DOOM");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.EVENT_ID);
            Console.WriteLine("EVENT");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.GAME_ID);
            Console.WriteLine("GAME");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.GIFT_ID);
            Console.WriteLine("GIFT");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.HEALTH_ID);
            Console.WriteLine("HEALTH");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.MEET_ID);
            Console.WriteLine("MEET");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.MOVIE_ID);
            Console.WriteLine("MOVIE");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.PET_ID);
            Console.WriteLine("PET");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.PLAY_ID);
            Console.WriteLine("PLAY");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.PURCHASE_ID);
            Console.WriteLine("PURCHASE");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.SHOW_ID);
            Console.WriteLine("SHOW");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.TRAVEL_ID);
            Console.WriteLine("TRAVEL");

            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.WORK_ID);
            Console.WriteLine("WORK");
        }

        private void AutoRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.AUTO_ID);
                MessageBox.Show(((Button)sender).Content + " Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void BookRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.BOOK_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void ComicRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.COMIC_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void CourseRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.COURSE_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void DoomRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.DOOM_ID);
                MessageBox.Show(((Button)sender).Content + " Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void EventRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.EVENT_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void GameRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.GAME_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void GiftRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.GIFT_ID);
                MessageBox.Show(((Button)sender).Content + " Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void HealthRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.HEALTH_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void MeetRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.MEET_ID);
                MessageBox.Show(((Button)sender).Content + " Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void MovieRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.MOVIE_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }
        
        private void PetRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.PET_ID);
                MessageBox.Show(((Button)sender).Content + " Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PlayRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.PLAY_ID);
                MessageBox.Show(((Button)sender).Content + " Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void PurchaseRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.PURCHASE_ID);
                MessageBox.Show(((Button)sender).Content + " Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void ShowRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.SHOW_ID);
                MessageBox.Show(((Button)sender).Content + " Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void TravelRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.TRAVEL_ID);
                MessageBox.Show(((Button)sender).Content + " Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }

        private void WorkRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.WORK_ID);
                MessageBox.Show(((Button)sender).Content + " Funcionou!");
            } catch (Exception exception) {
                MessageLabel.Content = exception.Message;
                Console.WriteLine(exception);
            }
        }
    }
}
