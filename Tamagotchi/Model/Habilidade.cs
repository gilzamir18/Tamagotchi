using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamaguria.Model
{
    public class Habilidade
    {
        public string Nome { get; }
        public bool Oculta { get; set; }
        public int Slot { get; set; }

        public string? URL { get; set; }

        public Habilidade(string nome)
        {
            Nome = nome;      
        }
    }
}
