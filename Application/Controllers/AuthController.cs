using Infrastructure.Repositories;
using Infrastructure.Repositories.HeroRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs.Auth;
using Models.DTOs.Response;
using Models.DTOs.Trainer;
using Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace hero_trainer_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ITrainerRepository _trainerRepository;
        private IHeroRepository _heroRepository;
        private readonly UserManager<Trainer> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(ITrainerRepository trainerRepository,
            IHeroRepository heroRepository,
            UserManager<Trainer> userManager,
            IConfiguration configuration)
        {
            this._trainerRepository = trainerRepository;
            this._heroRepository = heroRepository;
            this._userManager = userManager;
            this._configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(TrainerCreateDTO trainerCreateDTO)
        { 
            var userExists = await _userManager.FindByNameAsync(trainerCreateDTO.Name);

            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO() { Status = "Error",
                    Message = "User already exists!" });

            Trainer user = new()
            {
                Email = trainerCreateDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = trainerCreateDTO.Name
            };

            var result = await _userManager.CreateAsync(user, trainerCreateDTO.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", 
                    Message = "User creation failed! Please check user details and try again." });
      
            Trainer trainer = new(); 

            await this._trainerRepository.Add(trainer);

            await this._heroRepository.GenerateRandomHeroesForTrainer(trainer.Id);

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {

            var user = await _userManager.FindByNameAsync(loginDTO.LoginUserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                Trainer trainer = this._trainerRepository.GetTrainerByName(loginDTO.LoginUserName);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginDTO.LoginUserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("uid", trainer.Id)
                };

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
