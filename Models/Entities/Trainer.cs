using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Trainer : IdentityUser
    {
        public ICollection<Hero> Heroes { get; set; } 
        public ICollection<TrainingSession> TrainingSessions { get; set; }

    }
}
