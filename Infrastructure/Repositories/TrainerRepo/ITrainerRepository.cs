using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ITrainerRepository
    {
        Task Add(Trainer trainer);

        Trainer GetTrainerByName(string name);
    }
}
