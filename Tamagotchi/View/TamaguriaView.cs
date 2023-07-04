using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamagotchi.Model;
using Tamagotchi.Service;

namespace Tamagotchi
{
    public sealed class View
    {
        private View() { }
        public static string ExibirTituloDaOpcao(string titulo, char marker = '*', bool showTitle = true)
        {
            int quantidadeDeLetras = titulo.Length;
            string asteriscos = string.Empty.PadLeft(quantidadeDeLetras, marker);
            Console.WriteLine(asteriscos);
            if (showTitle)
            {
                Console.WriteLine(titulo);
                Console.WriteLine(asteriscos);
            }
            Console.WriteLine();
            return asteriscos;
        }
        public static void ExibirLogo()
        {
            Console.WriteLine(@"
████████╗░█████╗░███╗░░░███╗░█████╗░░██████╗░██╗░░░██╗██████╗░██╗░█████╗░
╚══██╔══╝██╔══██╗████╗░████║██╔══██╗██╔════╝░██║░░░██║██╔══██╗██║██╔══██╗
░░░██║░░░███████║██╔████╔██║███████║██║░░██╗░██║░░░██║██████╔╝██║███████║
░░░██║░░░██╔══██║██║╚██╔╝██║██╔══██║██║░░╚██╗██║░░░██║██╔══██╗██║██╔══██║
░░░██║░░░██║░░██║██║░╚═╝░██║██║░░██║╚██████╔╝╚██████╔╝██║░░██║██║██║░░██║
░░░╚═╝░░░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚═╝░░╚═╝░╚═════╝░░╚═════╝░╚═╝░░╚═╝╚═╝╚═╝░░╚═╝");
            ExibirTituloDaOpcao("Seja bem-vindo ao Tamaguria! O mais amplo buscador de mascotes virtuais do mundo!");
            Console.WriteLine("\n\n");
        }

        public static void ExibirCabecalhoDaOpcao(string tituloOpcao)
        {
            Console.Clear();
            ExibirLogo();
            ExibirTituloDaOpcao(tituloOpcao);
            Console.WriteLine("\n\n");
        }

        public static void ExibirRodapeDaOpcao(bool callMainMenu = true)
        {
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal.");
            Console.ReadKey();
            Console.Clear();
            if (callMainMenu)
            {
                Controller.GerenciarMascotes();
            }
        }

        public static void ObterInformacoesDoCuidador(Cuidador cuidador)
        {
            ExibirCabecalhoDaOpcao("Protetor dos Mascotes, oh mestre supremo, apresente-se: ");
            cuidador = new Cuidador();
            Console.WriteLine("\n\n");
            Console.Write("Apelido: ");
            var apelido = Console.ReadLine()!;
            cuidador.Apelido = apelido;
            Console.Write("Nome Completo: ");
            cuidador.Nome = Console.ReadLine()!;
            ExibirRodapeDaOpcao(false);
        }


        public static void AdotarMascote(Cuidador cuidador, DAO dao)
        {
            ExibirCabecalhoDaOpcao("Adote um mascote antes que a promoção acabe. Corre!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.Write("Nome do bichinho que você quer adotar? ");
            string bichinho = Console.ReadLine()!;
            Console.WriteLine();

            Mascote? mascote = dao!.GetMascote(bichinho);
            if (mascote != null)
            {
                mascote.Nome = bichinho;
                cuidador.mascotes.Add(mascote);
                var marcas = ExibirTituloDaOpcao($"\nQue Legal! Veja algumas habilidades e outras informações sobre seu novo mascote {bichinho}: ");
                Console.WriteLine($"Altura: {mascote.Altura}");
                Console.WriteLine($"Largura: {mascote.Largura}");
                Console.WriteLine($"Peso: {mascote.Peso}");
                Console.WriteLine("\nHabilidades: ");
                foreach (Habilidade hab in mascote.Habilidades!)
                {
                    Console.WriteLine(hab.HabilidadeInfo?.Nome);
                }
                Console.WriteLine(marcas);
            }
            else
            {
                Console.WriteLine($"Não encontramos o bichinho {bichinho}. Tente outro!");
            }
            ExibirRodapeDaOpcao();
        }

        public static void PesquisarMascotes(DAO dao)
        {
            ExibirCabecalhoDaOpcao("Procure pelos mascotes mais lidinhos do universo!!!!");
            List<TipoDeMascote> tipos = dao!.GetTiposDeMascote(100);
            foreach (var tipo in tipos)
            {
                Console.WriteLine($"{tipo.Nome}: {tipo.URL}");
            }
            ExibirRodapeDaOpcao();
        }

        public static void MostrarMeusMascotes(Cuidador cuidador)
        {
            if (cuidador!.mascotes.Count <= 0)
            {
                Console.WriteLine($"{cuidador.Apelido}, atualmente você não tem nenhum mascote!");
            }
            else
            {
                string marcas = ExibirTituloDaOpcao("Meus mascotes virtuais:");
                Console.WriteLine("Quantidade: " + cuidador.mascotes.Count);
                foreach (Mascote mascote in cuidador.mascotes)
                {
                    ExibirTituloDaOpcao("----------------------------------------", '-', false);
                    Console.WriteLine($"Nome: {mascote.Nome}");
                    Console.WriteLine("Habilidades: ");
                    foreach (Habilidade hab in mascote.Habilidades!)
                    {
                        Console.WriteLine($"\t{hab.HabilidadeInfo?.Nome}");
                    }
                }
                Console.WriteLine("\n\n");
            }
            ExibirRodapeDaOpcao();
        }
    }
}
