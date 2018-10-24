using System;
using System.ComponentModel.DataAnnotations;

namespace Cyclr.LaunchExample.Models
{
    public class OAuthToken
    {
        [Key]
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }
}