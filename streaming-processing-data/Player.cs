using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace streamingprocessingdata
{

    public class RootObject
    {
        public Player[] Player { get; set; }
    }

    public class Player
    {
        [JsonProperty(PropertyName = "first_name")] //ngubah dari first_name ke Firstname tanpa ngubah file.json
        public string FirstName { get; set; }

        public int id { get; set; }
        public string points_per_game { get; set; }
        public string second_name { get; set; }
        public string team_name { get; set; }
    }
}
