using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScoreTracker.Models
{
    class MatchClass
    {
        public string HomeTeam { get; set; }
        public string HomeScore { get; set; }
        public string AwayTeam { get; set; }
        public string AwayScore { get; set; }

        public MatchClass() { }

        public MatchClass(string ht, string hs, string awt, string aws)
        {
            HomeTeam = ht;
            HomeScore = hs;
            AwayTeam = awt;
            AwayScore = aws;
        }

        public static void SaveSoccerDataToFile(List<MatchClass> list)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filename = Path.Combine(path, "SavedGames.txt");
            StringBuilder sb = new StringBuilder();

            if (File.Exists(filename))
            {
                //read any json text already on file
                using (var reader = new StreamReader(filename, false))
                {
                    //read all in file and add , if  json content exists in file
                    sb.Append(reader.ReadToEnd());
                    if (sb.ToString().Equals(""))
                    {
                        
                    }
                    else {
                        sb.Append(',');
                    }
                }
                //write to file using a StreamWriter
                using (var writer = new StreamWriter(filename, false))
                {
                    //serialize list to json object
                    string jsonText = JsonConvert.SerializeObject(list, Formatting.Indented);

                    //add jsontext to stringbuilder and replace all [] brackets (for reformatting when multible entries added)
                    sb.Append(jsonText);
                    sb.Replace('[', ' ');
                    sb.Replace(']', ' ');

                    //reformat json, by appending [ ] to start and end respectively to string builder
                    sb.Insert(0, "[");
                    sb.Append(']');

                    //write string builder contents to file
                    writer.WriteLine(sb);
                }
            }
            //if file doesn't exist
            else
            {
                // Will create new file when doesn't exist
                using (var writer = new StreamWriter(filename, false))
                {
                    //serialize list to json format
                    string jsonText = JsonConvert.SerializeObject(list, Formatting.Indented);
                    //write jsontext to file
                    writer.WriteLine(jsonText);
                }

            }
        }
    }
}
