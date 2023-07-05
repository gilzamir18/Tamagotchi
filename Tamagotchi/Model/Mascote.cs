using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tamagotchi.Model
{
    public class Mascote
    {
        public string Nome { get; }
        public int Peso { get; }
        public int Largura { get;  }
        public int Altura { get; }

        public int Alimentacao => alimentacao;
        public int Humor => humor;

        private List<Habilidade> habilidades;
        private int count = 0;
        private int alimentacao, humor;

        public int QtdHabilidades => count;

        private Random rnd;

        public Mascote(string nome, int peso, int altura, int largura)
        {
            rnd = new Random(42);

            Nome = nome;
            habilidades = new List<Habilidade>();
            Peso = peso;
            Altura = altura;
            Largura = largura;
            count = 0;
            alimentacao = rnd.Next(0, 10);
            humor = rnd.Next(0, 10);
        }


        public void Brincar()
        {
            humor = humor + 1 ;
            if (humor > 10)
            {
                humor = 10;
            }
            alimentacao -= 1;
            if (alimentacao <0 )
            {
                alimentacao = 0;
            }
        }

        public void Alimentar()
        {
            alimentacao = alimentacao + 1;
            if ( alimentacao > 10)
            {
                alimentacao = 10;
            }
        }

        public void AdicionarHabilidade(Habilidade hab)
        {
            habilidades.Add(hab);
            count++;
        }

        public Habilidade GetHabilidade(int i)
        {
            return habilidades[i];
        }
    }

}
