using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace CpaTicker.Areas.admin.Classes
{
    public class Domain
    {
        public int Id { get; set; }

        [Required]
        [RemoteWithServerSideAttribute("CheckDomainExists", "Validation", "admin", ErrorMessage = "This domain is already taken.")] 
        public string DomainName { get; set; }
    }

    
}