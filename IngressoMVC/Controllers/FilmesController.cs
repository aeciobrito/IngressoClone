using IngressoMVC.Data;
using IngressoMVC.Models;
using IngressoMVC.Models.ViewModels.RequestDTO;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace IngressoMVC.Controllers
{
    public class FilmesController : Controller
    {
        private IngressoDbContext _context;

        public FilmesController(IngressoDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Filmes);

        public IActionResult Detalhes(int id) => View(_context.Filmes.Find(id));

        public IActionResult Criar() => View();

        [HttpPost]
        public IActionResult Criar(PostFilmeDTO filmeDto)
        {
            var cinema = _context.Cinemas.FirstOrDefault(c => c.Nome == filmeDto.NomeCinema);
            if (cinema == null) return View();

            var produtor = _context.Produtores.FirstOrDefault(p => p.Nome == filmeDto.NomePodutor);
            if (produtor == null) return View();

            Filme filme = new Filme
                (
                    filmeDto.Titulo,
                    filmeDto.Descricao,
                    filmeDto.Preco,
                    filmeDto.ImageURL,
                    cinema.Id,
                    produtor.Id
                );

            _context.Add(filme);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult CriarFilmeComCategoriasAtores(PostFilmeDTO filmeDto)
        {
            var cinema = _context.Cinemas.FirstOrDefault(c => c.Nome == filmeDto.NomeCinema);
            if (cinema == null) return View();

            var produtor = _context.Produtores.FirstOrDefault(p => p.Nome == filmeDto.NomePodutor);
            if (produtor == null) return View();

            Filme filme = new Filme
                (
                    filmeDto.Titulo,
                    filmeDto.Descricao,
                    filmeDto.Preco,
                    filmeDto.ImageURL,
                    cinema.Id,
                    produtor.Id
                );

            _context.Add(filme);
            _context.SaveChanges();

            //Incluir Relacionamentos
            foreach (var categoriaId in filmeDto.CategoriasId)
            {
                var novaCategoria = new FilmeCategoria(filme.Id, categoriaId);
                _context.FilmesCategorias.Add(novaCategoria);
                _context.SaveChanges();
            }

            foreach (var atorId in filmeDto.AtoresId)
            {
                var novoAtor = new AtorFilme(atorId, filme.Id);
                _context.AtoresFilmes.Add(novoAtor);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Atualizar(int id)
        {
            //buscar o ator no banco
            //passar o ator na view
            return View();
        }

        public IActionResult Deletar(int id)
        {
            //buscar o ator no banco
            //passar o ator na view
            return View();
        }
    }
}
