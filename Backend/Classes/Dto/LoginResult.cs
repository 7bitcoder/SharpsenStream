using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Classes.Dto
{
    public class LoginResult
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AvatarFilePath { get; set; }
        public int Color { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
