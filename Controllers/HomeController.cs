using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoEscalada.Models;

namespace ProyectoEscalada.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //Método que lleva a la vista muestra
    public IActionResult Muestra(){
        




        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
