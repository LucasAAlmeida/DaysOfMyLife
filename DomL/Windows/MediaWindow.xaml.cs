
using DomL.Business.Entities;
using DomL.Business.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for MediaWindow.xaml
    /// Convenient way to edit multiple Media entries.
    ///     Get them from Database,
    ///     Open that in Excel,
    ///     Make your changes,
    ///     Save to Database.
    /// </summary>
    public partial class MediaWindow : Window
    {
        const string MEDIA_DIR_PATH = "C:\\Users\\Lyucs\\OneDrive\\Área de Trabalho\\DomL\\Media\\";

        public MediaWindow()
        {
            InitializeComponent();
        }

        private void SaveAllMediaFromDatabaseToFileButton_Click(object sender, RoutedEventArgs e)
        {
            this.SaveBooksFromDatabaseToFileButton_Click(sender, e);
            this.SaveComicsFromDatabaseToFileButton_Click(sender, e);
            this.SaveGamesFromDatabaseToFileButton_Click(sender, e);
            this.SaveMoviesFromDatabaseToFileButton_Click(sender, e);
            this.SaveShowsFromDatabaseToFileButton_Click(sender, e);
        }

        private void SaveBooksFromDatabaseToFileButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.SaveMediaFromDatabaseToFile(MEDIA_DIR_PATH, Category.BOOK_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void SaveComicsFromDatabaseToFileButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.SaveMediaFromDatabaseToFile(MEDIA_DIR_PATH, Category.COMIC_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void SaveGamesFromDatabaseToFileButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.SaveMediaFromDatabaseToFile(MEDIA_DIR_PATH, Category.GAME_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }


        private void SaveMoviesFromDatabaseToFileButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.SaveMediaFromDatabaseToFile(MEDIA_DIR_PATH, Category.MOVIE_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void SaveShowsFromDatabaseToFileButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.SaveMediaFromDatabaseToFile(MEDIA_DIR_PATH, Category.SHOW_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        //===================================================================================
        //===================================================================================
        //===================================================================================

        private void SaveAllMediaToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            this.SaveBooksFromFileToDatabaseButton_Click(sender, e);
            this.SaveComicsFromFileToDatabaseButton_Click(sender, e);
            this.SaveGamesFromFileToDatabaseButton_Click(sender, e);
            this.SaveMoviesFromFileToDatabaseButton_Click(sender, e);
            this.SaveShowsFromFileToDatabaseButton_Click(sender, e);
        }

        private void SaveBooksFromFileToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.SaveMediaFromFileToDatabase(MEDIA_DIR_PATH, Category.BOOK_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void SaveComicsFromFileToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.SaveMediaFromFileToDatabase(MEDIA_DIR_PATH, Category.COMIC_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void SaveGamesFromFileToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.SaveMediaFromFileToDatabase(MEDIA_DIR_PATH, Category.GAME_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        
        private void SaveMoviesFromFileToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.SaveMediaFromFileToDatabase(MEDIA_DIR_PATH, Category.MOVIE_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }
        
        private void SaveShowsFromFileToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.SaveMediaFromFileToDatabase(MEDIA_DIR_PATH, Category.SHOW_ID);
            MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }
    }
}
