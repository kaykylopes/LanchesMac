using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanchesMac.Repositories;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class LancheController : Controller
    {
        private readonly ILancheRepository _lancherepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public LancheController(ILancheRepository lancherepository, ICategoriaRepository categoriaRepository)
        {
            _lancherepository = lancherepository;
            _categoriaRepository = categoriaRepository;
        }

        public IActionResult List()
        {
            ViewBag.Lanche = "Lanches";
            ViewData["Categoria"] = "Categoria";

            /*var lanches = _lancherepository.Lanches;
            return View(lanches); ;*/

            var lanchesListViewModel = new LancheListViewModel();
            lanchesListViewModel.Lanches = _lancherepository.Lanches;
            lanchesListViewModel.CategoriaAtual = "Categoria Atual";
            return View(lanchesListViewModel);
        }
    }
}