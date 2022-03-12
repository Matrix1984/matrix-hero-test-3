using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.HeroRepo
{
    public interface IHeroRepository
    {
        Task Add(Hero her); 
        Task Update(Hero her); 
        Task<Hero> GetById(int id); 
        IEnumerable<Hero> ListHeroes(); 
        Task GenerateRandomHeroesForTrainer(string trainerId);
    }
}
