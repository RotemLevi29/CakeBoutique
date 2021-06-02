using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace jewelry.Models
 
{
    public class User
    {

        public enum gender
        {
            male,
            female
        }
        public enum UserType
        {
            Client,
            Editor,
            Admin
        }


        [Required]
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Please enter the user name")]
        [StringLength(maximumLength: 20)]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = "";


        [Required(ErrorMessage = "Please enter your first name")]
        [StringLength(maximumLength: 20)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";


        [Required(ErrorMessage = "Please enter your last name")]
        [StringLength(maximumLength: 20)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";


        [Required(ErrorMessage ="Enter minimum lower case letter, upper case letter and a number" )]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Please enter a valid email adrress")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Required]
        public gender Gender { get; set; } = 0;//0 = male, 1 = female


        [Required]
        public UserType Type { get; set; } = UserType.Client;

        public int CartId { get; set; }

        public Address Address { get; set; }

        public List<Order> Orders { get; set; } = null; //List of history orders
    }
}
