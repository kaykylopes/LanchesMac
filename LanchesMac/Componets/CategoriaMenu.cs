using LanchesMac.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanchesMac.Componets
{
    public class CategoriaMenu : ViewComponent
    {
        private readonly ICategoriaRepository _categoriaReposity;

        public CategoriaMenu(ICategoriaRepository categoriaRepository)
        {
            _categoriaReposity = categoriaRepository;
        }

        public IViewComponentResult Invoke()
        {
            var categorias = _categoriaReposity.Categorias.OrderBy(c =>
                c.CategoriaNome);

            return View(categorias);
        }
    }
}
