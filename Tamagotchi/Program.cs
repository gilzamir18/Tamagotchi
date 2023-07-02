using System.Reflection;
using Tamagotchi;

var dao = RestAPIDAO.GetInstance();
Cuidador? cuidador = null;

string ExibirTituloDaOpcao(string titulo, char marker='*', bool showTitle=true)
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
void ExibirLogo()
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

void ExibirCabecalhoDaOpcao(string tituloOpcao)
{
    Console.Clear();
    ExibirLogo();
    ExibirTituloDaOpcao(tituloOpcao);
    Console.WriteLine("\n\n");
}

void ExibirRodapeDaOpcao(bool callMainMenu=true)
{
    Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal.");
    Console.ReadKey();
    Console.Clear();
    if (callMainMenu)
    {
        MenuPrincipal();
    }
}

void ObterInformacoesDoCuidador()
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

void MenuPrincipal()
{
    ExibirLogo();
    if (cuidador == null)
    {
        ObterInformacoesDoCuidador();
    }
    ExibirTituloDaOpcao($"Olá {cuidador!.Apelido}, aqui estão tuas opções:");
    Console.WriteLine("\n\n");
    Console.WriteLine("Pressione 'a' para adotar um bicinho como mascote.");
    Console.WriteLine("Pressione 's' para pesquisar sobre algum bichinho do mundo poke.");
    Console.WriteLine("Pressione 'd' para  mostrar os seus bicinhos virtuais.");
    Console.WriteLine("Pressione 'q' para sair!");
    var key = Console.ReadKey();
    switch(key.KeyChar)
    {
        case 'a': AdotarMascote();
            break;
        case 's': PesquisarMascotes();
            break;
        case 'd': MostrarMeusMascotes();
            break;
        case 'q':
            Console.WriteLine($"{cuidador.Apelido}, volte sempre!");
            Thread.Sleep(2000);
            break;
        default:
            Console.WriteLine($"Opção inválida {key.KeyChar}! Tente outra vez!");
            MenuPrincipal();
            Thread.Sleep(2000);
            break;
    }
}


void AdotarMascote()
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

void PesquisarMascotes()
{
    ExibirCabecalhoDaOpcao("Procure pelos mascotes mais lidinhos do universo!!!!");
    List<TipoDeMascote> tipos = dao!.GetTiposDeMascote(100);
    foreach (var tipo in tipos)
    {
        Console.WriteLine($"{tipo.Nome}: {tipo.URL}");
    }
    ExibirRodapeDaOpcao();
}

void MostrarMeusMascotes()
{
    if (cuidador!.mascotes.Count <= 0)
    {
        Console.WriteLine($"{cuidador.Apelido}, atualmente você não tem nenhum mascote!");
    }
    else
    {
        string marcas = ExibirTituloDaOpcao("Meus mascotes virtuais:");
        Console.WriteLine("Quantidade: " + cuidador.mascotes.Count);
        foreach(Mascote mascote in cuidador.mascotes)
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
Console.Clear();
MenuPrincipal();


