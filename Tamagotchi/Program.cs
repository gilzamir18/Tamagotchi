using System.Reflection;
using Tamaguria.Model;
using Tamaguria.Service;

var dao = RestAPIDAO.GetInstance();
Cuidador? cuidador = null;

Console.Clear();
Tamaguria.Controller.Iniciar(dao, cuidador!);
Tamaguria.Controller.GerenciarMascotes();


