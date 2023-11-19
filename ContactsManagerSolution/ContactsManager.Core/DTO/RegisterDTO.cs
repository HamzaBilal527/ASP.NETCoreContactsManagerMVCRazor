using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name Cant Be Blank")]

        public string PersonName { get; set; }

        [Required(ErrorMessage = "Email Cant Be Blank")]
        [EmailAddress(ErrorMessage = "Email should be in proper format")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already taken")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Cant Be Blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone should only contains digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password Cant Be Blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Cant Be Blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmPassword { get; set; }

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;

    }
}
