using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoEscalada.Models;
using ProyectoEscalada.Data;
using Microsoft.EntityFrameworkCore;

namespace ProyectoEscalada.Controllers;

public class HomeController : Controller{
    //Variable privada para guardar el contexto
    private readonly EscaladaContext _context;

    public HomeController(EscaladaContext context) => _context = context;


    [Route("/")]
    public IActionResult Index() => View();

    [Route("Privacy")]
    public IActionResult Privacy() => View();

    //Método que lleva a la vista muestra
    [Route("Muestra")]
    public async Task<IActionResult> Muestra(){
        
        var listaUsuarios = await _context.Usuarios.ToListAsync();
    
        return View(listaUsuarios);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("/Error")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
