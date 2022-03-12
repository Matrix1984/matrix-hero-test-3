using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly HeroDbContext _dbContext;
        public TrainerRepository(HeroDbContext heroDbContext)
        {
            this._dbContext = heroDbContext;
        }

        public async Task Add(Trainer trainer)
        {
            _dbContext.Trainers.Add(trainer);
             await  this._dbContext.SaveChangesAsync();
        }

        public Trainer GetTrainerByName(string name) => (from n in this._dbContext.Trainers
                                                         where n.UserName == name
                                                         select n).FirstOrDefault();
    }
}
