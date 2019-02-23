using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sports_Score_Tracker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            SetupDefaults();
        }

        private void SetupDefaults()
        {
            var assembly = typeof(MainPage);
            //string backgroundImgPath = "Sports_Score_Tracker.Assets.Images.background.jpg";
            //backGrdImg.Source = ImageSource.FromResource(backgroundImgPath, assembly);

            string soccerImgPath = "Sports_Score_Tracker.Assets.Images.soccer.jpg";
            imgSoccer.Source = ImageSource.FromResource(soccerImgPath, assembly);

            string basketballImgPath = "Sports_Score_Tracker.Assets.Images.basketball.jpg";
            imgBasketball.Source = ImageSource.FromResource(basketballImgPath, assembly);

            string tennisImgPath = "Sports_Score_Tracker.Assets.Images.tennisball.png";
            imgTennis.Source = ImageSource.FromResource(tennisImgPath, assembly);

            string hockeyImgPath = "Sports_Score_Tracker.Assets.Images.icehockey.PNG";
            imgHockey.Source = ImageSource.FromResource(hockeyImgPath, assembly);

            string gaaImgPath = "Sports_Score_Tracker.Assets.Images.gaa.jpg";
            imgGaa.Source = ImageSource.FromResource(gaaImgPath, assembly);

            string rugbyImgPath = "Sports_Score_Tracker.Assets.Images.rugby.jpg";
            imgRugby.Source = ImageSource.FromResource(rugbyImgPath, assembly);
        }
    }
}
