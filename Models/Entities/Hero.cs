using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Hero
    {
        [Key]
        public int HeroId { get; set; } 
        public string GuidId { get; set; } 
        public DateTime HeroTrainingDate { get; set; } 
        public string Name { get; set; }
        public string Colors { get; set; }
        public decimal StartPower { get; set; } 
        public decimal CurrentPower { get; set; }
 
        [ForeignKey("Trainer")]
        public string Id { get; set; }
        public Trainer Trainer { get; set; }  
        public ICollection<TrainingSession> TrainingSessions { get; set; }
    }
}
