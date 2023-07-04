using System.Reflection;
using Tamagotchi.Model;
using Tamagotchi.Service;

var dao = RestAPIDAO.GetInstance();
Cuidador? cuidador = null;

Console.Clear();
Tamagotchi.Controller.Iniciar(dao, cuidador!);
Tamagotchi.Controller.GerenciarMascotes();


