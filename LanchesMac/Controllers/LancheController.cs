using System;
using System.Collections.Generic;
using System.Linq;
using LanchesMac.Models;
using LanchesMac.Repositories;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class LancheController : Controller
    {
    private readonly ILancheRepository _lancheRepository;
    private ICategoriaRepository _categoriaRepository;

    public LancheController(ILancheRepository lancheRepository, 
                            ICategoriaRepository categoriaRepository)
        {
            // cria uma varíavel para guardar a instância
            _lancheRepository = lancheRepository;
            _categoriaRepository = categoriaRepository;
        }

        public IActionResult List(string categoria)
        {
            // existem duas formas de chamar a View
            // ViewBag.Lanche = "Lanches";
            // ViewData["Categoria"] = "Categoria";

            // var lanches = _lancheRepository.Lanches;
            // return View(lanches);

            string _categoria = categoria;
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if(string.IsNullOrEmpty(categoria))
            {
                lanches = _lancheRepository.Lanches.OrderBy(l => l.LancheId);
                categoria = "Todos os lanches";
            }
            else
            {
                if(string.Equals("Normal", _categoria, StringComparison.OrdinalIgnoreCase))
                {
                    lanches = _lancheRepository.Lanches.Where(l => 
                                l.Categoria.CategoriaNome.Equals("Normal")).OrderBy(l => l.Nome);
                }
                else if(string.Equals("Natural", _categoria, StringComparison.OrdinalIgnoreCase))
                {
                    lanches = _lancheRepository.Lanches.Where(l => 
                                l.Categoria.CategoriaNome.Equals("Natural")).OrderBy(l => l.Nome);
                }
                else
                {
                    lanches = Enumerable.Empty<Lanche>();
                    _categoria = "Esta categoria não existe";
                }

                categoriaAtual = _categoria;
            }
            
            var lanchesListViewModel = new LancheListViewModel
            {
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            };
        
            // var lanchesListViewModel = new LancheListViewModel();
            // lanchesListViewModel.Lanches = _lancheRepository.Lanches;
            // lanchesListViewModel.CategoriaAtual = "Categoria Atual";
            
            return View(lanchesListViewModel);
        }
        public ViewResult Details(int lancheId)
        {
            var lanche = _lancheRepository.Lanches.FirstOrDefault(d => d.LancheId == lancheId);
            if (lanche == null)
            {
                return View("~/Views/Error/Error.cshtml");
            }
            return View(lanche);
        }

        public IActionResult Search(string searchString)
        {
            string _searchString = searchString;
            IEnumerable<Lanche> lanches;
            string _categoriaAtual = string.Empty;

            if(string.IsNullOrEmpty(_searchString))
            {
                lanches = _lancheRepository.Lanches.OrderBy(l => l.LancheId);
            }
            else
            {
                lanches = _lancheRepository.Lanches.Where(l => l.Nome.ToLower().Contains(_searchString.ToLower()));
            }

            return View("~/Views/Lanche/List.cshtml", new LancheListViewModel { Lanches=lanches, CategoriaAtual="Todos os lanches" });
        }
    }
}