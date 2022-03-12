using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.TrainingSessionRepo
{
    public class TrainingSessionRepository : ITrainingSessionRepository
    {
        private readonly HeroDbContext _dbContext;
        public TrainingSessionRepository(HeroDbContext heroDbContext)
        {
            this._dbContext = heroDbContext;
        }

        public async Task Add(TrainingSession session)
        {
            _dbContext.TrainingSessions.Add(session);
            await this._dbContext.SaveChangesAsync();
        }
    }
}
