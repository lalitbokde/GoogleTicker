using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CpaTicker.Models
{
    public class ContactModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "The e-mail address isn't in a correct format")]
        public string Email { get; set; }

        public string Subject { get; set; }

        [DisplayName("Your message")]
        public string Message { get; set; }

        [Required]
        //[StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        public string Phone { get; set; }
    }
}