using System.Reflection;
using Tamagotchi;

var dao = RestAPIDAO.GetInstance();

string ExibirTituloDaOpcao(string titulo)
{
    int quantidadeDeLetras = titulo.Length;
    string asteriscos = string.Empty.PadLeft(quantidadeDeLetras, '*');
    Console.WriteLine(asteriscos);
    Console.WriteLine(titulo);
    Console.WriteLine(asteriscos);
    Console.WriteLine();
    return asteriscos;
}

void ExibirLogo()
{
    Console.WriteLine(@"
████████╗░█████╗░███╗░░░███╗░█████╗░░██████╗░██╗░░░██╗██████╗░██╗░█████╗░
╚══██╔══╝██╔══██╗████╗░████║██╔══██╗██╔════╝░██║░░░██║██╔══██╗██║██╔══██╗
░░░██║░░░███████║██╔████╔██║███████║██║░░██╗░██║░░░██║██████╔╝██║███████║
░░░██║░░░██╔══██║██║╚██╔╝██║██╔══██║██║░░╚██╗██║░░░██║██╔══██╗██║██╔══██║
░░░██║░░░██║░░██║██║░╚═╝░██║██║░░██║╚██████╔╝╚██████╔╝██║░░██║██║██║░░██║
░░░╚═╝░░░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚═╝░░╚═╝░╚═════╝░░╚═════╝░╚═╝░░╚═╝╚═╝╚═╝░░╚═╝");
    ExibirTituloDaOpcao("Seja bem-vindo ao Tamaguria! O melhor caçador de bichinhos virtuais do mundo!");
    Console.WriteLine("\n\n");
}

void MenuPrincipal()
{
    ExibirLogo();
    ExibirTituloDaOpcao("Por enquanto temos apenas a opção de consulta!");
    Console.WriteLine("Pressione q para sair ou alguma outra tecla para consultar os bichinhos!");
    var key = Console.ReadKey();
    if (key.KeyChar == 'q')
    {
        Console.WriteLine("Tchau Tchau!!!");
        Thread.Sleep(1000);
    }
    else
    {
        SelecionarBichinho();
    }
}

void SelecionarBichinho()
{
    Console.Clear();
    ExibirLogo();
    ExibirTituloDaOpcao("Olá abiguinho! Vou exibir alguns nomes de bichinhos.");
    Thread.Sleep(1000);
    List<TipoDeMascote> tipos = dao.GetTiposDeMascote(10);
    foreach (var tipo in tipos)
    {
        Console.WriteLine($"{tipo.Nome}: {tipo.URL}");
    }
    Console.Write("Qual o nome do bichinho que você quer obter mais informações? ");
    string bichinho = Console.ReadLine()!;
    Console.WriteLine();
    try
    {
        Mascote mascote = dao.GetMascote(bichinho);
        var marcas = ExibirTituloDaOpcao($"\nQue Legal! Veja algumas habilidades do {bichinho}: ");
        foreach (Habilidade hab in mascote.Habilidades!)
        {
            Console.WriteLine(hab.HabilidadeInfo?.Nome);
        }
        Console.WriteLine(marcas);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Não encontramos o bichinho {bichinho}. Tente outro!");
    }
    Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal.");
    Console.ReadKey();
    Console.Clear();
    MenuPrincipal();
}

Console.Clear();
MenuPrincipal();


