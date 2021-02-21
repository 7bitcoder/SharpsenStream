using System;

namespace SharpsenStreamBackend.Classes
{
    public class UserToken
    {
        public int TokenId { get; set; }
        public int OwnerId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
