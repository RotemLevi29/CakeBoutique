using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace jewelry.Models
 
{
    public class User
    {
        public enum UserType
        {
            Client,
            Editor,
            Admin
        }
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength:30,MinimumLength =6)]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Required]
        public Boolean Sex { get; set; }//True = male, False = female

        public UserType Type { get; set; } = UserType.Client;

        public List<Product> Products { get; set; }//List of history orders
        public Cart Cart { get; set; }
    }
}
