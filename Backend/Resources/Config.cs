using SharpsenStreamBackend.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources
{
    public class Config
    {
        public readonly Configuration config;
        public Config()
        {
            using StreamReader file = File.OpenText(@"configuration.json");
            config = JsonSerializer.Deserialize<Configuration>(file.ReadToEnd());
        }
    }
}
