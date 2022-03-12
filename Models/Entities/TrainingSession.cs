using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class TrainingSession
    { 
        [Key]
        public int TrainingSessionId { get; set; }

        public DateTime TrainingSessionStart { get; set; }

        public int HeroId { get; set; }

        public Hero Hero { get; set; }

        [ForeignKey("Trainer")]
        public string Id { get; set; }

        public Trainer Trainer { get; set; }
    }
}
