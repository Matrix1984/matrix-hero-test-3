using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.TrainingSessionRepo
{
    public interface ITrainingSessionRepository
    {
        Task Add(TrainingSession session);
    }
}
