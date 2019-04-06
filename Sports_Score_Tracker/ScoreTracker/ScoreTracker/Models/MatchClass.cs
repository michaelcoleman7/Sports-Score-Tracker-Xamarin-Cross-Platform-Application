using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace ScoreTracker.Models
{
    class MatchClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //setup strings for use with INotifyPropertyChanged
        private string _homeTeam;
        private string _awayTeam;
        private string _homeScore;
        private string _awayScore;

        //getters/setters for variables of MatchMlass
        public string GameType { get; set; }
        public string HomeTeam
        {
            get { return _homeTeam; }
            //set using INotifyPropertyChanged
            set
            {
                if (_homeTeam == value) return;
                _homeTeam = value;
                OnPropertyChanged(nameof(HomeTeam));
            }
        }
        public string HomeScore
        {
            get { return _homeScore; }
            //set using INotifyPropertyChanged
            set
            {
                if (_homeScore == value) return;
                _homeScore = value;
                OnPropertyChanged(nameof(HomeScore));
            }
        }
        public string AwayTeam
        {
            get { return _awayTeam; }
            //set using INotifyPropertyChanged
            set
            {
                if (_awayTeam == value) return;
                _awayTeam = value;
                OnPropertyChanged(nameof(AwayTeam));
            }
        }

        public string AwayScore
        {
            get { return _awayScore; }
            //set using INotifyPropertyChanged
            set
            {
                if (_awayScore == value) return;
                _awayScore = value;
                OnPropertyChanged(nameof(AwayScore));
            }
        }
        public string MatchName { get; set; }

        //Default Constructor
        public MatchClass() { }

        //Constructor with variables needed for a matchclass
        public MatchClass(string gt, string ht, string hs, string awt, string aws, string matName)
        {
            //assign MatchClass variables to values recieved
            GameType = gt;
            HomeTeam = ht;
            HomeScore = hs;
            AwayTeam = awt;
            AwayScore = aws;
            MatchName = matName;
        }

        //Method to save data to a file
        public static void SaveMatchDataToFile(List<MatchClass> list)
        {
            //Setup path for files
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //setup file name using path and filename
            string filename = Path.Combine(path, "SavedGames.txt");

            // Will create new file when doesn't exist
            using (var writer = new StreamWriter(filename, false))
            {
                //serialize list to json format
                string jsonText = JsonConvert.SerializeObject(list, Formatting.Indented);
                //write jsontext to file
                writer.WriteLine(jsonText);
            }

        }

        //Method to read the file and return it in a list of MatchClasses
        public static List<MatchClass> ReadList()
        {
            List<MatchClass> myList = new List<MatchClass>();
            string jsonText;

            //try Read localApplicationFolder
            try
            {
                string path = Environment.GetFolderPath(
                                Environment.SpecialFolder.LocalApplicationData);
                string filename = Path.Combine(path, "SavedGames.txt");
                using (var reader = new StreamReader(filename))
                {
                    //read text file contents into jsontext
                    jsonText = reader.ReadToEnd();
                }
            }
            // if unable to read localApplicationFolder, read the default file
            catch
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(
                                                typeof(MainPage)).Assembly;
                // Create stream
                Stream stream = assembly.GetManifestResourceStream(
                                    "ScoreTracker.DataFiles.SavedGames.txt");
                try
                {
                    using (var reader = new StreamReader(stream))
                    {
                        //read text file contents into jsontext
                        jsonText = reader.ReadToEnd();
                    }
                }
                //catch when trying to read file if it doesn't exist
                catch (Exception)
                {
                    //set jsontext to empty string so serializing is not being carried out on null string when file doesn't exist
                    jsonText = "";
                }
            }
            //deserialize json text into myList and return myList
            myList = JsonConvert.DeserializeObject<List<MatchClass>>(jsonText);
            return myList;
        }

        //Method to implement INotifyPropertyChanged
        private void OnPropertyChanged(string propertyName)
        {
            //if propertychanged ==null do nothing, otherwise invoke the propertychanged event handler with two arguments
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
