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
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void SetupDefaults()
        {
            //Setup paths for image sources
            var assembly = typeof(MainPage);

            string soccerImgPath = "ScoreTracker.Assets.Images.soccer.jpg";
            imgSoccer.Source = ImageSource.FromResource(soccerImgPath, assembly);

            string basketballImgPath = "ScoreTracker.Assets.Images.basketball.jpg";
            imgBasketball.Source = ImageSource.FromResource(basketballImgPath, assembly);

            string tennisImgPath = "ScoreTracker.Assets.Images.tennisball.png";
            imgTennis.Source = ImageSource.FromResource(tennisImgPath, assembly);

            string hockeyImgPath = "ScoreTracker.Assets.Images.icehockey.PNG";
            imgHockey.Source = ImageSource.FromResource(hockeyImgPath, assembly);

            string gaaImgPath = "ScoreTracker.Assets.Images.gaa.jpg";
            imgGaa.Source = ImageSource.FromResource(gaaImgPath, assembly);

            string rugbyImgPath = "ScoreTracker.Assets.Images.rugby.jpg";
            imgRugby.Source = ImageSource.FromResource(rugbyImgPath, assembly);

        }

        private async void ImgSoccer_Tapped(object sender, EventArgs e)
        {
            //naviage to the Soccer.xaml page
            await Navigation.PushAsync(new Soccer());
        }

        private async void Matches_btn_Clicked(object sender, EventArgs e)
        {
            //naviage to the LoadGames.xaml page
            await Navigation.PushAsync(new LoadGames());
        }

        private async void ImgHockey_Tapped(object sender, EventArgs e)
        {
            //naviage to the IceHockey.xaml page
            await Navigation.PushAsync(new IceHockey());
        }

        private async void ImgBasketball_Tapped(object sender, EventArgs e)
        {
            //naviage to the Basketball.xaml page
            await Navigation.PushAsync(new Basketball());
        }
        private async void ImgTennis_Tapped(object sender, EventArgs e)
        {
            //naviage to the Tennis.xaml page
            await Navigation.PushAsync(new Tennis());
        }
    }
}
