using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.DTOs.Trainer
{
    public class TrainerCreateDTO
    {  

        [JsonPropertyName("username")]
        [Required]
        public string Name { get; set; }

        [JsonPropertyName("password")]
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$",
         ErrorMessage = "The password needs to contain at least 8 characters, one capital letter," +
            " one number and one of the following nonalphanumeric characters: #$^+=!*()@%&")]
        public string Password { get; set; }


        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
} 