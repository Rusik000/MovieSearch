using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public string NameMovie { get; set; }
        public string ImagePath { get; set; }
        public int Year { get; set; }

        public string Writer { get; set; }
        public string Director { get; set; }

        public double Rating { get; set; }

        public string Minute { get; set; }

        public string Description { get; set; }
        public dynamic SingleData { get; set; }


        public dynamic Data { get; set; }

        HttpClient httpClient = new HttpClient();

        public int Count { get; set; } = 0;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
        }
        public void GetMovie()
        {
            var name = movieTextBox.Text;
            HttpResponseMessage responseMessage = new HttpResponseMessage();

            responseMessage = httpClient.GetAsync($@"http://www.omdbapi.com/?apikey=263bef28&s={name}&plot=full").Result;
            var str = responseMessage.Content.ReadAsStringAsync().Result;
            Data = JsonConvert.DeserializeObject(str);
            responseMessage = httpClient.GetAsync($@"http://www.omdbapi.com/?apikey=263bef28&t={Data.Search[Count].Title}&plot=full").Result;

            str = responseMessage.Content.ReadAsStringAsync().Result;
            SingleData = JsonConvert.DeserializeObject(str);

            MovieImage.Source = SingleData.Poster;
            MovieImage2.Source = SingleData.Poster;
            Minute = SingleData.Runtime;
            Description = SingleData.Genre;
            NameMovie = SingleData.Title;
            Writer = SingleData.Writer;
            Director = SingleData.Director;
            Rating = SingleData.imdbRating;
            Year = SingleData.Year;

            movieLabel.Content = NameMovie;
            lblTitle.Content = "Title :" + NameMovie;
            lblGenre.Content = "Genre :" + Description;
            lblYear.Content = "Year :" + Year.ToString();
            lblDirector.Content = "Director :" + Director;
            lblWriter.Content = "Writer :" + Writer;
            lblRating.Content = "Rating :" + Rating.ToString();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NextBtn.Visibility = Visibility;
            GetMovie();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            StckPanel.Visibility = Visibility.Visible;
            --Count;

            try
            {

                GetMovie();
                if (Count > 0)
                {
                    BackBtn.Visibility = Visibility.Visible;
                    NextBtn.Visibility = Visibility.Visible;
                }
                else if (Count == 0)
                {
                    BackBtn.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception)
            {
                BitmapImage yesImage = new BitmapImage(new Uri("/Images/Smile.png", UriKind.Relative));
                MovieImage2.Source = yesImage;


            }

        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            
            ++Count;
            try
            {
                GetMovie();
                if (Count > 0)
                {
                    BackBtn.Visibility = Visibility.Visible;
                    NextBtn.Visibility = Visibility.Visible;
                }
                else if (Count == 0)
                {
                    BackBtn.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception)
            {
                BitmapImage yesImage = new BitmapImage(new Uri("/Images/Smile.png", UriKind.Relative));
                MovieImage2.Source = yesImage;
                NextBtn.Visibility = Visibility.Hidden;
                movieLabel.Content = "No Movie";
                //lblTitle.Content = " ";
                //lblGenre.Content = " " ;
                //lblYear.Content = " ";
                //lblDirector.Content = " ";
                //lblWriter.Content = " " ;
                //lblRating.Content = " ";
                StckPanel.Visibility = Visibility.Hidden;
            }


        }

        private void movieTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieTextBox.Text == "Name of Movie")
            {
                movieTextBox.Text = String.Empty;
            }
            
        }

        private void movieTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieTextBox.Text == String.Empty)
            {
                movieTextBox.Text = "Name of Movie";
            }

        }
    }
}
