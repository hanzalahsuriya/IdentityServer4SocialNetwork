using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.OAuthId.Quickstart.Account
{
    public class RegisterModel
    {
        public string Email
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        public string Password
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword
        {
            get;
            set;
        }
    }
}
