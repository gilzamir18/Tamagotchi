﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tamaguria.Model;
using Tamaguria.Service;
using Tamaguria;

namespace Tamaguria
{
    public sealed class Controller
    {

        private static Cuidador? cuidador;
        private static DAO? dao;

        private Controller() { }

        public static void Iniciar(DAO dao, Cuidador cuidador)
        {
            Controller.dao = dao;
            cuidador = View.ObterInformacoesDoCuidador();
            Controller.cuidador = cuidador;
        }

        public static void GerenciarMascotes()
        {
            View.ExibirLogo();
            View.ExibirTituloDaOpcao($"Olá {cuidador!.Apelido}, aqui estão tuas opções:");
            Console.WriteLine("\n\n");
            Console.WriteLine("Pressione 'a' para adotar um Pokemon como mascote.");
            Console.WriteLine("Pressione 'e' para brincar um mascote!");
            Console.WriteLine("Pressione 'n' para alimentar um mascote!");
            Console.WriteLine("Pressione 's' para pesquisar sobre algum bichinho do mundo poke.");
            Console.WriteLine("Pressione 'd' para  mostrar os seus bicinhos virtuais.");
            Console.WriteLine("Pressione 'q' para sair!");
            var key = Console.ReadKey();
            Console.WriteLine();
            switch (key.KeyChar)
            {
                case 'a':
                    View.AdotarMascote(cuidador, dao!);
                    break;
                case 'e': View.BrincarComMascote(cuidador);
                    break;
                case 'n': View.AlimentarMascote(cuidador);
                    break;
                case 's':
                    View.PesquisarMascotes(dao!);
                    break;
                case 'd':
                    View.MostrarMeusMascotes(cuidador);
                    break;
                case 'q':
                    Console.WriteLine($"{cuidador.Apelido}, volte sempre!");
                    Thread.Sleep(2000);
                    break;
                default:
                    Console.WriteLine($"Opção inválida {key.KeyChar}! Tente outra vez!");
                    View.ExibirRodapeDaOpcao(false);
                    GerenciarMascotes();
                    break;
            }
        }
    }
}
