using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    [Table("UserProfilePic")]
    public class UserProfilePic
    {
        [Key]
        public int ID { get; set; }
        public int UserId { get; set; }
        public byte[] ProfilePic { get; set; }
    }
}