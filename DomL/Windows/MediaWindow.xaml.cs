
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

        private void GetAllMediaFromDatabaseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetBooksFromDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            DomLServices.RestoreFromFile(MEDIA_DIR_PATH, Category.BOOK_ID);
            //MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void GetComicsFromDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            //DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.COMIC_ID);
            //MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void GetGamesFromDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            //DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.GAME_ID);
            //MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }


        private void GetMoviesFromDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            //DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.MOVIE_ID);
            //MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void GetShowsFromDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
                //DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.SHOW_ID);
                //MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        //===================================================================================
        //===================================================================================
        //===================================================================================

        private void SaveAllMediaToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveBooksToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            //DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.BOOK_ID);
            //MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void SaveComicsToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            //DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.COMIC_ID);
            //MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        private void SaveGamesToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            //DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.GAME_ID);
            //MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }

        
        private void SaveMoviesToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            //DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.MOVIE_ID);
            //MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }
        
        private void SaveShowsToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
                //DomLServices.RestoreFromFile(BACKUP_DIR_PATH, Category.SHOW_ID);
                //MessageBox.Show(((Button)sender).Content + " Funcionou!");
        }
    }
}
