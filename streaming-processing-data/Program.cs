using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace streamingprocessingdata
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine("direktori sekarang : "+currentDirectory);
            Console.WriteLine();

            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var files = directory.GetFiles();
            Console.WriteLine("Isi direktori tersebut :");
            foreach (var f in files)
            {
                Console.WriteLine("\t"+f.Name);
            }
            Console.WriteLine();

            //var fileName = Path.Combine(directory.FullName, "SoccerGameResults.csv");
            //Console.WriteLine("file path = " + fileName);

            // READING FILE OLD-WAY
            //Console.WriteLine("Isi file " + fileName + " = ");
            //var file = new FileInfo(fileName);
            //if (file.Exists)
            //{
            //    // using for opening and closing file automatically
            //    using (var reader = new StreamReader(file.FullName))
            //    {
            //        Console.SetIn(reader);
            //        Console.WriteLine(Console.ReadLine());
            //        reader.Close();
            //    }
            //}

            // READING FILE NEW-WAY
            //var fileContents = ReadFile(fileName);
            //Console.WriteLine(fileContents);
            //Console.WriteLine();

            // Spliting file from commas or enter
            //Console.WriteLine("-------------- Separating data with something --------------");
            //string[] fileLines = fileContents.Split('\r','\n');
            //foreach (var line in fileLines)
            //    Console.WriteLine(line);
            //Console.WriteLine("Panjang array = " + fileLines.Length);

            //Parsing data in files in CSV files
            //var fileContents = ReadFootbalResults(fileName);
            //foreach (var line in fileContents)
            //    Console.WriteLine("nama = " + line.TeamName + ", goals = " + line.Goals + ", possession = " + line.PossessionPercent + "%, ");
            //Console.WriteLine();

            // Parsing data in JSON files
            //var fileName = Path.Combine(currentDirectory, "players.json");
            //Console.WriteLine("Isi file " + fileName + " = ");
            //var fileContents = DeserializedPlayer(fileName);
            //foreach (var line in fileContents)
            //Console.WriteLine(line.FirstName);

            // From WEB API to get source page
            //Console.WriteLine("Isi WEB :");
            //Console.WriteLine(GetGoogleHomePage());

            // AZURE API search engine query
            Console.WriteLine("Search Engine AZURE");
            //Console.WriteLine(GetNewsFromPlayer("Diego valeri")); //output berupa json dan fungsi getsnews from player masih string
            var user = GetNewsFromPlayer("Wahyu Trisvianto");
            foreach (var name in user)
            {
                Console.WriteLine("Tanggal : {0}", name.datePublished);
                Console.WriteLine("Kategori : {0}", name.category);
                Console.WriteLine("Headline : {0}", name.description);
            }
		}

        //Nambahin fungsi di class utama
        public static string ReadFile (string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }    
        }

        // Untuk file csv
        public static List<GameResult> ReadFootbalResults(string fileName)
        {
            var soccerResults = new List<GameResult>();
            using (var reader = new StreamReader(fileName))
            {
                var line = "";
                reader.ReadLine(); //biar skip baris pertama
                while((line = reader.ReadLine()) != null) //baca per baris
                {
                    string[] values = line.Split(',');

                    // Class declaration
                    GameResult gr = new GameResult();
                    DateTime gameDate;
                    HomeOrAway homeOrAway;

                    //ngubah dari string ke DateTime
                    if (DateTime.TryParse(values[0], out gameDate))
                        gr.GameDate = gameDate;
                    
                    gr.TeamName = values[1];

                    //ngubah dari string ke homeoraway data type
                    if (Enum.TryParse(values[2], out homeOrAway))
                        gr.HomeOrAway = homeOrAway;

					//from string to int
                    int parseInt;
					if (int.TryParse(values[3], out parseInt))
						gr.Goals = parseInt;
					
					if (int.TryParse(values[4], out parseInt))
						gr.GoalAttempts = parseInt;
					
					if (int.TryParse(values[5], out parseInt))
						gr.ShotsOnGoal = parseInt;
					
					if (int.TryParse(values[6], out parseInt))
						gr.ShotsOffGoal = parseInt;

                    //convert from string to double
					Double PossessionPercent;
					if (Double.TryParse(values[6], out PossessionPercent))
					{
						gr.PossessionPercent = PossessionPercent;
					}

                    // output object game result
                    soccerResults.Add(gr);
                }
            }
            return soccerResults;
        }

        // Untuk file json
		public static List<Player> DeserializedPlayer(string fileName)
		{
			var players = new List<Player>();
			var serializer = new JsonSerializer();

            using (var reader = new StreamReader(fileName))
            using (var jsonreader = new JsonTextReader(reader))
            {
                players = serializer.Deserialize<List<Player>>(jsonreader);    
            }
			return players;
		}

        // Untuk WEB API (get html, js, etc)
        public static string GetGoogleHomePage() 
        {
            var webClient = new WebClient();
            byte[] googleHome = webClient.DownloadData("http://www.berkshirehathaway.com");

            using (var stream = new MemoryStream(googleHome))
	            using (var reader = new StreamReader(stream))
	            {
	                return reader.ReadToEnd();
	            }
        }

		// Dari azure API
		public static List<NewsResult> GetNewsFromPlayer(string playerName)
		{
			var webClient = new WebClient();
            webClient.Headers.Add("Ocp-Apim-Subscription-Key", "5142e63248004153bd550fa4e7366470");
            byte[] playerNews = webClient.DownloadData(string.Format("https://api.cognitive.microsoft.com/bing/v5.0/news/search?q={0}&mkt=en-us", playerName));
            var serializer = new JsonSerializer();


			// for initiating a variable
			var results = new List<NewsResult>();

			using (var stream = new MemoryStream(playerNews))
			using (var reader = new StreamReader(stream))
            using (var jsonreader = new JsonTextReader(reader))
			{
				results = serializer.Deserialize<NewsSearch>(jsonreader).NewsResults;
			}

            return results;
		}
    }
}
