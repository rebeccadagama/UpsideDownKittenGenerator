using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UpsideDownKittenGenerator.Authentication
{
    public class UserName
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
    }
}
