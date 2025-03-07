using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GStore.Models;
using GStore.Data;
using Microsoft.EntityFrameworkCore;
using GStore.ViewModels;

namespace GStore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _db;

    public HomeController(ILogger<HomeController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        List<Produto> produtos = _db.Produtos
            .Where(p => p.Destaque)
            .Include(p => p.Fotos)
            .ToList();
        return View(produtos);
    }

    public IActionResult Produto(int id)
    {
        Produto produto = _db.Produtos
            .Where(p => p.Id == id)
            .Include(p => p.Categoria)
            .Include(p => p.Fotos)
            .SingleOrDefault();

        ProdutoVM produtoVM = new()
        {
            Produto = produto
        };
        produtoVM.Produtos = _db.Produtos
            .Where(p => p.CategoriaId == produto.CategoriaId && p.Id != produto.Id
            ).Take(4).Include(p => p.Fotos).ToList();
        return View(produtoVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
