using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.HeroRepo
{
    public class HeroRepository : IHeroRepository
    {
        private readonly HeroDbContext _dbContext;
        public HeroRepository(HeroDbContext heroDbContext)
        {
            this._dbContext = heroDbContext;
        }

        public async Task Add(Hero her)
        {
            _dbContext.Heroes.Add(her);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task Update(Hero her)
        {
            _dbContext.Entry<Hero>(her).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await this._dbContext.SaveChangesAsync();
        }

        public IEnumerable<Hero> ListHeroes()
        {
            return from n in  _dbContext.Heroes
                   orderby n.CurrentPower descending
                   select n;

        }

        public async Task<Hero> GetById(int id)
        {
           return await _dbContext.Heroes.FindAsync(id);
        }

        public async Task GenerateRandomHeroesForTrainer(string trainerId)
        {
            string[] colors = new string[] { "red", "blue", "black", "grey", "yellow","green" };

            string[] names = new string[] { 
                "Abdiel Holloway",
                "Odin Poole",
                "Iliana Fuentes",
                "Jaylin Burnett",
                "Ayana Weeks",
                "Aaden Aguilar",
                "Alfred Walls" ,
                "Maleah Pierce" ,
                "Leonel Finley" ,
                "Allan Sanders" ,
                "Maribel Liu" ,
                "Samir Jefferson" ,
                "Eli Romero" ,
                "Bryanna Salazar" ,
                "Erik Mcintosh" ,
                "Silas Velez" ,
                "Jamal Mcmillan"  };

            Random random = new Random();

            int randomIterator=random.Next(2,5); 

            for (int i = 0; i < randomIterator; i++)
            {

                int randomIndex = random.Next(5); 

                Hero hero = new();

                hero.GuidId = Guid.NewGuid().ToString();

                hero.HeroTrainingDate = DateTime.UtcNow;

                hero.Colors = colors[randomIndex];

                hero.Name = names[i];

                hero.StartPower = 0;

                hero.CurrentPower = 0;

                hero.Id = trainerId;

                await Add(hero);              }
        }
    }
} 