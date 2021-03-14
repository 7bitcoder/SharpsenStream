using SharpsenStreamBackend.Classes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Classes
{
    public class Configuration
    {
        public Database database { get; set; }
        public Jwt jwt { get; set; }
    }
}
