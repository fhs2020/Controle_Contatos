using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CadastroContatos.Data;
using CadastroContatos.Models;
using Microsoft.AspNetCore.Authorization;

namespace CadastroContatos.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cliente
        public IActionResult Index(string search)
        {
            ViewBag.CurrentPage = 1;

            ViewBag.LastPage = Math.Ceiling(Convert.ToDouble(_context.Clientes.ToList().Count()) / 5);

            if (!String.IsNullOrEmpty(search))
            {
                var searchWord = search.ToLower();

                var listaClientes = _context.Clientes.ToList();

                var clienteSearched = listaClientes.Where(s => s.Nome.ToLower().Contains(searchWord) ||
                                       s.Email.ToLower().Contains(searchWord) ||
                                       s.Telefone.ToLower().Contains(searchWord)).ToList();

                if (clienteSearched != null)
                {
                    return View(clienteSearched);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View(_context.Clientes.Take(5));
            }
        }

        [HttpPost]
        public IActionResult Index(int CurrentPage, int LastPage, string search)
        {
            ViewBag.CurrentPage = CurrentPage;

            ViewBag.LastPage = LastPage;

            if (!String.IsNullOrEmpty(search))
            {
                var searchWord = search.ToLower();

                var listaClientes = _context.Clientes.ToList();

                var clienteSearched = listaClientes.Where(s => s.Nome.ToLower().Contains(searchWord) || s.Sobrenome.ToLower().Contains(searchWord)).ToList();

                if (clienteSearched != null)
                {
                    return View(clienteSearched);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                var quantidadePaginas = 0;

                if (CurrentPage >= 1)
                {
                    quantidadePaginas = (CurrentPage - 1);

                    ViewBag.CurrentPage = CurrentPage;
                }
                else
                {
                    quantidadePaginas = 1;

                    ViewBag.CurrentPage = 2;
                }

                var clientes = _context.Clientes.OrderBy(x => x.Id).Skip(quantidadePaginas * 5).Take(5);

                return View(clientes);
            }
        }


        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            var status = _context.Status.ToList();

            ViewBag.Status = new SelectList(status, "Id", "Descrição");

            var interesse = _context.Produto.ToList();

            ViewBag.Interesse = new SelectList(interesse, "Id", "Descricao");

            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var status = _context.Status.Find(cliente.StatusId);

                cliente.Status = status.Descrição;

                var interesse = _context.Produto.Find(cliente.ProdutoId);

                cliente.ProdutoNome = interesse.Descricao;

                var usuarioNome = User.Identity.Name;

                cliente.UserName = usuarioNome;


                _context.Add(cliente);
                await _context.SaveChangesAsync();

                return RedirectToAction("Create", "Conversas", new { clienteId = cliente.Id });
            }

            var statusLista = _context.Status.ToList();

            ViewBag.Status = new SelectList(statusLista, "Id", "Descrição");

            var interesseLista = _context.Produto.ToList();

            ViewBag.Interesse = new SelectList(interesseLista, "Id", "Descricao");

            return View();
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.SingleOrDefaultAsync(m => m.Id == id);

            var status = _context.Status.ToList();

            var statusDefault = new Status();

            if (cliente.StatusId > 0)
            {
                statusDefault = _context.Status.Find(cliente.StatusId);
            }

            ViewBag.Status = new SelectList(status, "Id", "Descrição", statusDefault);

            var interesse = _context.Produto.ToList();

            var interesseDefault = new Produto();

            if (cliente.ProdutoId > 0)
            {
                interesseDefault = _context.Produto.Find(cliente.ProdutoId);
            }

            ViewBag.Interesse = new SelectList(interesse, "Id", "Descricao", interesse);

            var listaConveras = _context.Conversa.Where(x => x.ClienteId == cliente.Id).ToList();

            ViewBag.ListaConversas = listaConveras;

            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Create", "Conversas", new { clienteId = cliente.Id });
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.SingleOrDefaultAsync(m => m.Id == id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
