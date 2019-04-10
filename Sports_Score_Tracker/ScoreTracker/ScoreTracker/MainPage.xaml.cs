using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScoreTracker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            SetupDefaults();
        }

        //Method to setup default values needed for page setup
        private void SetupDefaults()
        {
            //turn off navigation bar at top of application - referenced - https://stackoverflow.com/questions/36656895/remove-navigation-bar-on-xamarin-forms-app-with-caliburn-micro 
            NavigationPage.SetHasNavigationBar(this, false);

            var assembly = typeof(MainPage);

            // Choose between platform/build options for each device
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    //change header font and text colour for android
                    Header.FontSize = 25;
                    Header.TextColor = Color.White;
                    //set background image source for android
                    string androidBackground = "ScoreTracker.Assets.Images.grass.jpg";
                    ImgBackground.Source = ImageSource.FromResource(androidBackground, assembly);
                    break;
                case Device.UWP:
                    //set background image source for UWP
                    string uwpBackground = "ScoreTracker.Assets.Images.uwpback.jpg";
                    ImgBackground.Source = ImageSource.FromResource(uwpBackground, assembly);
                    break;
                default:
                    break;
            }

            //Setup paths for image sources
            string soccerImgPath = "ScoreTracker.Assets.Images.soccer.jpg";
            ImgSoccer.Source = ImageSource.FromResource(soccerImgPath, assembly);

            string basketballImgPath = "ScoreTracker.Assets.Images.basketball.jpg";
            ImgBasketball.Source = ImageSource.FromResource(basketballImgPath, assembly);

            string tennisImgPath = "ScoreTracker.Assets.Images.tennisball.png";
            ImgTennis.Source = ImageSource.FromResource(tennisImgPath, assembly);

            string hockeyImgPath = "ScoreTracker.Assets.Images.icehockey.PNG";
            ImgHockey.Source = ImageSource.FromResource(hockeyImgPath, assembly);

            string gaaImgPath = "ScoreTracker.Assets.Images.gaa.jpg";
            ImgGaa.Source = ImageSource.FromResource(gaaImgPath, assembly);

            string rugbyImgPath = "ScoreTracker.Assets.Images.rugby.jpg";
            ImgRugby.Source = ImageSource.FromResource(rugbyImgPath, assembly);

        }

        private async void ImgSoccer_Tapped(object sender, EventArgs e)
        {
            //Navigate to the Soccer.xaml page
            await Navigation.PushAsync(new Soccer());
        }

        private async void Matches_btn_Clicked(object sender, EventArgs e)
        {
            //Navigate to the LoadGames.xaml page
            await Navigation.PushAsync(new LoadGames());
        }

        private async void ImgHockey_Tapped(object sender, EventArgs e)
        {
            //Navigate to the IceHockey.xaml page
            await Navigation.PushAsync(new IceHockey());
        }

        private async void ImgBasketball_Tapped(object sender, EventArgs e)
        {
            //Navigate to the Basketball.xaml page
            await Navigation.PushAsync(new Basketball());
        }
        private async void ImgTennis_Tapped(object sender, EventArgs e)
        {
            //Navigate to the Tennis.xaml page
            await Navigation.PushAsync(new Tennis());
        }

        private async void ImgGAA_Tapped(object sender, EventArgs e)
        {
            //Navigate to the GAA.xaml page
            await Navigation.PushAsync(new GAA());
        }
        private async void ImgRugby_Tapped(object sender, EventArgs e)
        {
            //Navigate to the Rugby.xaml page
            await Navigation.PushAsync(new Rugby());
        }
    }
}
