using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamagotchi.Model
{
    public class Cuidador
    {
        public string Nome { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
        public List<Mascote> mascotes { get; set; } = new List<Mascote>();

        public Cuidador() { }
    }
}
