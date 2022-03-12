using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.DTOs.Hero
{
    public class HeroSelectDTO
    {
        [JsonPropertyName("heroId")]
        public int HeroId { get; set; }

        [JsonPropertyName("guidId")]
        public string GuidId { get; set; }

        [JsonPropertyName("heroTrainingDate")]
        public DateTime HeroTrainingDate { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("colors")]
        public string Colors { get; set; }

        [JsonPropertyName("startPower")]
        public decimal StartPower { get; set; }

        [JsonPropertyName("currentPower")]
        public decimal CurrentPower { get; set; }

        [JsonPropertyName("trainerId")]
        public string Id { get; set; }
    }
} 