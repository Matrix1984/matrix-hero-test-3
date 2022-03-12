using AutoMapper;
using Infrastructure;
using Infrastructure.Repositories.HeroRepo;
using Infrastructure.Repositories.TrainingSessionRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Hero;
using Models.Entities;
using System.Security.Claims;

namespace hero_trainer_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroRepository _heroRepository;

        private readonly ITrainingSessionRepository _trainingSessionRepository;

        private readonly HeroDbContext _dbContext;

        public readonly IMapper _mapper;
        public HeroesController(IHeroRepository heroRepository,
            ITrainingSessionRepository trainingSessionRepository,
            HeroDbContext heroDbContext,
            IMapper mapper)
        {
            this._heroRepository = heroRepository;
            this._trainingSessionRepository = trainingSessionRepository;
            this._mapper = mapper;
            this._dbContext = heroDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> TrainHero(HeroTrainDTO heroTrainDTO)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            Claim someClaim = claimsIdentity.FindFirst("uid");
            string trainerId = someClaim.Value;
            Hero hero = await this._heroRepository.GetById(heroTrainDTO.HeroId);

            if (hero == null)
                return NotFound();


            if (hero.Id != trainerId)
                return BadRequest();

            DateTime beginningOfDay = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);


            List<TrainingSession> trainingSessions = (from n in this._dbContext.TrainingSessions
                                                      where n.HeroId == heroTrainDTO.HeroId && hero.HeroTrainingDate > beginningOfDay
                                                      && n.Id == trainerId
                                                      select n).ToList();

            if (trainingSessions.Count >= 5)
                return BadRequest("A hero cant be trained more than 5 times a day.");

            Random rnd = new Random();

            int randomPowerBoost = rnd.Next(0, 10);

            decimal powerBoostIncreasePercentage = Convert.ToDecimal(randomPowerBoost / 100M);

            decimal powerBoostDecreaseTotal = hero.CurrentPower * powerBoostIncreasePercentage;

            if (hero.CurrentPower == 0)
                hero.CurrentPower = 1;
            else
                hero.CurrentPower  += powerBoostDecreaseTotal;

            hero.HeroTrainingDate = DateTime.UtcNow;

            await this._heroRepository.Update(hero);

            TrainingSession session = new();

            session.TrainingSessionStart = DateTime.UtcNow;

            session.HeroId = hero.HeroId;

            session.Id = trainerId;

            await this._trainingSessionRepository.Add(session);

            return NoContent();
        }

        [HttpGet]
        public IEnumerable<HeroSelectDTO> ListHeroes()
          => _mapper.Map<IEnumerable<HeroSelectDTO>>(this._heroRepository.ListHeroes());
    }
}
