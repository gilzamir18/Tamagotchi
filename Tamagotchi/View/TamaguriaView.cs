using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamaguria.Model;
using Tamaguria.Service;

namespace Tamaguria
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

        public static Cuidador ObterInformacoesDoCuidador()
        {
            ExibirCabecalhoDaOpcao("Protetor dos Mascotes, oh mestre supremo, apresente-se: ");
            Cuidador cuidador = new Cuidador();
            Console.WriteLine("\n\n");
            Console.Write("Apelido: ");
            var apelido = Console.ReadLine()!;
            cuidador.Apelido = apelido;
            Console.Write("Nome Completo: ");
            cuidador.Nome = Console.ReadLine()!;
            ExibirRodapeDaOpcao(false);
            return cuidador;
        }


        public static void AdotarMascote(Cuidador cuidador, DAO dao)
        {
            ExibirCabecalhoDaOpcao("Adote um mascote antes que a promoção acabe. Corre!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.Write("Nome do bichinho que você quer adotar? ");
            string bichinho = Console.ReadLine()!;
            Console.WriteLine();
            try
            {
                Mascote? mascote = dao!.GetMascote(bichinho);
                if (mascote != null)
                {
                    cuidador.mascotes.Add(mascote);
                    var marcas = ExibirTituloDaOpcao($"\nQue Legal! Veja algumas habilidades e outras informações sobre seu novo mascote {bichinho}: ");
                    Console.WriteLine($"Altura: {mascote.Altura}");
                    Console.WriteLine($"Largura: {mascote.Largura}");
                    Console.WriteLine($"Peso: {mascote.Peso}");
                    Console.WriteLine("\nHabilidades: ");
                    foreach (Habilidade h in mascote.Habilidades)
                    {
                        Console.WriteLine(": " + h.Nome);
                    }
                    Console.WriteLine(marcas);
                }
                else
                {
                    Console.WriteLine($"Não encontramos o bichinho {bichinho}. Tente outro!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Detectamos um problema em sua aplicação!
                    Verifique sua conexão de internet e tente novamente. 
                    Se o problema persistir, entre em contato com o desenvolvedor!
                ");
                Console.Write("Você deseja visualizar um relatório de erro mais detalhado? [s/n] ");
                char r = Console.ReadKey().KeyChar;
                while (r != 's' && r != 'n')
                {
                    Console.Write("\rOpção Inválida, tente novamente!");
                    Console.Write(" Você deseja visualizar um relatório de erro mais detalhado? [s/n] ");
                    r = Console.ReadKey().KeyChar;
                }
                Console.WriteLine();
                if (r == 's')
                {
                    Console.WriteLine(ex.Message);
                }
            }

            ExibirRodapeDaOpcao();
        }

        public static void PesquisarMascotes(DAO dao)
        {
            ExibirCabecalhoDaOpcao("Procure pelos mascotes mais lidinhos do universo!!!!");
            try
            {
                List<MascoteLocation> locations = dao!.GetMascoteLocationList(100);
                foreach (var location in locations)
                {
                    Console.WriteLine($"{location.Nome}: {location.URL}");
                }
            } catch(Exception ex)
            {
                Console.WriteLine(@"Detectamos um problema em sua aplicação!
                    Verifique sua conexão de internet e tente novamente. 
                    Se o problema persistir, entre em contato com o desenvolvedor!
                ");
                Console.Write("Você deseja visualizar um relatório de erro mais detalhado? [s/n] ");
                char r = Console.ReadKey().KeyChar;
                while (r != 's' && r != 'n')
                {
                    Console.Write("\rOpção Inválida, tente novamente!");
                    Console.Write(" Você deseja visualizar um relatório de erro mais detalhado? [s/n] ");
                    r = Console.ReadKey().KeyChar;
                }
                Console.WriteLine();
                if (r == 's')
                {
                    Console.WriteLine(ex.Message);
                }
            }
            ExibirRodapeDaOpcao();
        }

        public static void AlimentarMascote(Cuidador cuidador)
        {
            ExibirCabecalhoDaOpcao("Alimente o seu mascote, mascotes nutridos são mascotes felizes!");
            Console.Write("Informe o nome do mascote que você quer alimentar? ");
            var nomeMascote = Console.ReadLine();
            try
            {
                var mascote = cuidador.mascotes.Find(m => m.Nome == nomeMascote);
                if (mascote != null)
                {
                    char opt = ' ';
                    while (opt != 'q')
                    {
                        Console.Clear();
                        ExibirCabecalhoDaOpcao("Alimente o seu mascote, mascotes nutridos são mascotes felizes!");
                        mascote.Alimentar();
                        MostrarNutricao(mascote);
                        Console.WriteLine($"Nutrição do Mascote: {mascote!.Alimentacao}");
                        Console.WriteLine("Pressione 'q' para parar de alimentar o mascote ou outra tecla para continuar!");
                        opt = Console.ReadKey().KeyChar;
                    }
                }
                else
                {
                    Console.WriteLine($"Você não tem nenhum mascote nomeado {nomeMascote}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Detectamos um problema em sua aplicação!
                    Verifique sua conexão de internet e tente novamente. 
                    Se o problema persistir, entre em contato com o desenvolvedor!
                ");
                Console.Write("Você deseja visualizar um relatório de erro mais detalhado? [s/n] ");
                char r = Console.ReadKey().KeyChar;
                while (r != 's' && r != 'n')
                {
                    Console.Write("\rOpção Inválida, tente novamente!");
                    Console.Write(" Você deseja visualizar um relatório de erro mais detalhado? [s/n] ");
                    r = Console.ReadKey().KeyChar;
                }
                Console.WriteLine();
                if (r == 's')
                {
                    Console.WriteLine(ex.Message);
                }
            }
            ExibirRodapeDaOpcao();
        }

        public  static void BrincarComMascote(Cuidador cuidador)
        {
            if (cuidador!.mascotes.Count <= 0)
            {
                Console.WriteLine($"{cuidador.Apelido}, atualmente você não tem nenhum mascote!");
            }
            else
            {
                ExibirCabecalhoDaOpcao("Brinque com o seu Bichinho para torná-lo mais feliz!");
                Console.Write("Com qual mascote você quer brincar? ");
                var nomeMascote = Console.ReadLine();
                var mascote = cuidador.mascotes.Find(m => m.Nome == nomeMascote);
                if (mascote != null)
                {
                    char opt = ' ';
                    while (opt != 'q')
                    {
                        Console.Clear();
                        ExibirCabecalhoDaOpcao("Brinque com o seu Bichinho para torná-lo mais feliz!");
                        mascote.Brincar();
                        MostrarHumor(mascote);
                        Console.WriteLine($"Humor: {mascote!.Humor}");
                        Console.WriteLine("Pressione q para parar de brincar ou qualquer outra tecla para continuar!");
                        opt = Console.ReadKey().KeyChar;
                    }
                }
                else
                {
                    Console.WriteLine($"Você não tem nenhum mascote nomeado {nomeMascote}");
                }
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
                    MostrarHumor(mascote);
                    Console.WriteLine("Habilidades: ");
                    for (int i = 0; i < mascote.QtdHabilidades; i++)
                    {
                        Habilidade? hab = mascote.GetHabilidade(i);
                        Console.WriteLine($"\t{hab.Nome}");
                        if (mascote.Alimentacao > 5)
                        {
                            Console.WriteLine("Mascote alimentado."); 
                        }
                        else
                        {
                            Console.WriteLine("Mascote faminto.");
                        }
                    }
                }
                Console.WriteLine("\n\n");
            }
            ExibirRodapeDaOpcao();
        }


        private static void MostrarHumor(Mascote mascote)
        {
            if (mascote.Humor > 7)
            {
                Console.WriteLine(@"________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶__________ 
______¶¶¶¶¶¶_____________¶¶¶¶¶¶________ 
_____¶¶¶¶¶_________________¶¶¶¶¶¶______ 
____¶¶¶¶_____________________¶¶¶¶¶_____ 
___¶¶¶¶_______________________¶¶¶¶¶____ 
__¶¶¶¶_____¶¶¶¶_______¶¶¶¶______¶¶¶____ 
__¶¶¶_____¶¶¶¶¶¶_____¶¶¶¶¶¶_____¶¶¶¶___ 
_¶¶¶¶_____¶¶¶¶¶¶_____¶¶¶¶¶¶______¶¶¶___ 
_¶¶¶_______¶¶¶¶_______¶¶¶¶_______¶¶¶¶__ 
_¶¶¶______________________________¶¶¶__ 
_¶¶¶______________________________¶¶¶__ 
_¶¶¶____¶¶_________________¶¶_____¶¶¶__ 
_¶¶¶____¶¶¶¶_____________¶¶¶¶____¶¶¶¶__ 
_¶¶¶¶____¶¶¶¶¶_________¶¶¶¶¶_____¶¶¶___ 
__¶¶¶_______¶¶¶¶¶¶¶¶¶¶¶¶¶_______¶¶¶¶___ 
__¶¶¶¶__________¶¶¶¶¶__________¶¶¶¶____ 
___¶¶¶¶_______________________¶¶¶¶_____ 
____¶¶¶¶____________________¶¶¶¶¶______ 
_____¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_______ 
_______¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_________ 
");
            }
            else if (mascote.Humor > 5)
            {
                Console.WriteLine(@"________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶__________ 
______¶¶¶¶¶¶_____________¶¶¶¶¶¶________ 
_____¶¶¶¶¶_________________¶¶¶¶¶¶______ 
____¶¶¶¶_____________________¶¶¶¶¶_____ 
___¶¶¶¶_______________________¶¶¶¶¶____ 
__¶¶¶¶_____¶¶¶¶_______¶¶¶¶______¶¶¶____ 
__¶¶¶_____¶¶¶¶¶¶_____¶¶¶¶¶¶_____¶¶¶¶___ 
_¶¶¶¶_____¶¶¶¶¶¶_____¶¶¶¶¶¶______¶¶¶___ 
_¶¶¶_______¶¶¶¶_______¶¶¶¶_______¶¶¶¶__ 
_¶¶¶______________________________¶¶¶__ 
_¶¶¶______________________________¶¶¶__ 
_¶¶¶______________________________¶¶¶__ 
_¶¶¶_____________________________¶¶¶¶__ 
_¶¶¶¶____________________________¶¶¶___ 
__¶¶¶______¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶______¶¶¶¶___ 
__¶¶¶¶_________________________¶¶¶¶____ 
___¶¶¶¶_______________________¶¶¶¶_____ 
____¶¶¶¶____________________¶¶¶¶¶______ 
_____¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_______ 
_______¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_________ 
""");
            }
            else
            {
                Console.WriteLine(@"________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶__________ 
______¶¶¶¶¶¶_____________¶¶¶¶¶¶________ 
_____¶¶¶¶¶_________________¶¶¶¶¶¶______ 
____¶¶¶¶_____________________¶¶¶¶¶_____ 
___¶¶¶¶_______________________¶¶¶¶¶____ 
__¶¶¶¶_____¶¶¶¶_______¶¶¶¶______¶¶¶____ 
__¶¶¶_____¶¶¶¶¶¶_____¶¶¶¶¶¶_____¶¶¶¶___ 
_¶¶¶¶_____¶¶¶¶¶¶_____¶¶¶¶¶¶______¶¶¶___ 
_¶¶¶_______¶¶¶¶_______¶¶¶¶_______¶¶¶¶__ 
_¶¶¶______________________________¶¶¶__ 
_¶¶¶______________________________¶¶¶__ 
_¶¶¶______________________________¶¶¶__ 
_¶¶¶____________¶¶¶¶¶____________¶¶¶¶__ 
_¶¶¶¶________¶¶¶¶¶¶¶¶¶¶¶_________¶¶¶___ 
__¶¶¶______¶¶¶¶¶_____¶¶¶¶¶______¶¶¶¶___ 
__¶¶¶¶____¶¶¶___________¶¶¶____¶¶¶¶____ 
___¶¶¶¶___¶¶_____________¶¶___¶¶¶¶_____ 
____¶¶¶¶____________________¶¶¶¶¶______ 
_____¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_______ 
_______¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_________ 
");
            }
        }



        private static void MostrarNutricao(Mascote mascote)
        {
            int quantidadeDeLetras = mascote.Alimentacao;
            string fill = string.Empty.PadLeft(quantidadeDeLetras, '¶');
            Console.WriteLine("Nível: " + fill); 
        }
    }
}
