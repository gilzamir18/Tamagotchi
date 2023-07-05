using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamagotchi.Model
{
    public class MascoteLocation
    {
        public string Nome { get;}
        public string URL { get;}

        public MascoteLocation(string nome, string uRL)
        {
            Nome = nome;
            URL = uRL;
        }
    }
}
