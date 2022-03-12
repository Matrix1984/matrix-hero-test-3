using AutoMapper;
using Infrastructure.Repositories.HeroRepo;
using Infrastructure.Repositories.TrainingSessionRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Hero;
using Models.Entities;

namespace hero_trainer_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroRepository _heroRepository;

        private readonly ITrainingSessionRepository _trainingSessionRepository;

        public readonly IMapper _mapper;
        public HeroesController(IHeroRepository heroRepository,
            ITrainingSessionRepository trainingSessionRepository,
            IMapper mapper)
        {
            this._heroRepository = heroRepository;
            this._trainingSessionRepository = trainingSessionRepository;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> TrainHero(HeroTrainDTO heroTrainDTO)
        {
            string trainerId = "";
            Hero hero = await this._heroRepository.GetById(heroTrainDTO.HeroId);

            if (hero == null)
                return NotFound();


            if (hero.Id != trainerId)
                return Unauthorized();

            DateTime beginningOfDay = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);

            List<TrainingSession> trainingSessions = (from n in hero.TrainingSessions
                                                      where n.HeroId == heroTrainDTO.HeroId && hero.HeroTrainingDate > beginningOfDay
                                                      select n).ToList();

            if (trainingSessions.Count > 5)
                return BadRequest("A hero cant be trained more than 5 times a day.");

            Random rnd = new Random();

            int randomPowerBoost = rnd.Next(0, 10);

            hero.CurrentPower += hero.CurrentPower * (randomPowerBoost / 1000);

            hero.HeroTrainingDate=DateTime.UtcNow;

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
          =>  _mapper.Map<IEnumerable<HeroSelectDTO>>(this._heroRepository.ListHeroes()); 
    }
}
