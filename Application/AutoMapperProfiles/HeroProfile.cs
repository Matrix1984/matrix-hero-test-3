using AutoMapper;
using Models.DTOs.Hero;
using Models.Entities;

namespace hero_trainer_app.AutoMapperProfiles
{
    public class HeroProfile : Profile
    {
        public HeroProfile()
        { 
            CreateMap<Hero, HeroSelectDTO>();
        } 
    }
}
