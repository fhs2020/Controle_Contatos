using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CadastroContatos.Data;
using CadastroContatos.Models;
using Microsoft.AspNetCore.Identity;

namespace CadastroContatos.Controllers
{
    public class ConversasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConversasController(ApplicationDbContext context)
        {
            _context = context;
        }

        //// GET: Conversas
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Conversa.ToListAsync());
        //}


        // GET: Conversas
        public IActionResult Index(int clienteId)
        {
            var conversa = _context.Conversa.Where(x => x.ClienteId == clienteId).ToList();

            var usuarioNome = User.Identity.Name;

            ViewBag.Usuario = usuarioNome;

            var cliente = _context.Clientes.Find(clienteId);

            ViewBag.ClienteNome = cliente.Nome;

            ViewBag.ClienteId = clienteId;

            return View(conversa);
        }


        // GET: Conversas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversa = await _context.Conversa
                .SingleOrDefaultAsync(m => m.Id == id);
            if (conversa == null)
            {
                return NotFound();
            }

            return View(conversa);
        }

        // GET: Conversas/Create
        public IActionResult Create(int? clienteId)
        {

            var id = RouteData.Values["id"];

            var routClienteID = Convert.ToInt32(id);

            var model = new ConversaViewModels();

            var cliente = new Cliente();

            if (clienteId != null)
            {
                cliente = _context.Clientes.Find(clienteId.Value);
            }
            else if (routClienteID > 0)
            {
                cliente = _context.Clientes.Find(routClienteID);
            }
           

            model.ClienteId = cliente.Id;
            model.ClienteNome = cliente.Nome + " " + cliente.Sobrenome;

            return View(model);
        }

        // POST: Conversas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Conversa conversa)
        {
            if (ModelState.IsValid)
            {
                conversa.Id = 0;

                var cliente = _context.Clientes.Find(conversa.ClienteId);

                conversa.ClienteNome = cliente.Nome;

                var usuarioNome = User.Identity.Name;

                conversa.UsuarioNome = usuarioNome;

                conversa.DataHoraConversa = DateTime.Now;

                _context.Add(conversa);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Conversas", new { clienteId = conversa.ClienteId });
            }


            return View(conversa);
        }

        // GET: Conversas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversa = await _context.Conversa.SingleOrDefaultAsync(m => m.Id == id);
            if (conversa == null)
            {
                return NotFound();
            }
            return View(conversa);
        }

        // POST: Conversas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(Conversa conversa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conversa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConversaExists(conversa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Conversas", new { clienteId = conversa.ClienteId });
            }
            return View(conversa);
        }

        // GET: Conversas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversa = await _context.Conversa
                .SingleOrDefaultAsync(m => m.Id == id);
            if (conversa == null)
            {
                return NotFound();
            }

            return View(conversa);
        }

        // POST: Conversas/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conversa = await _context.Conversa.SingleOrDefaultAsync(m => m.Id == id);
            _context.Conversa.Remove(conversa);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", "Conversas", new { clienteId = conversa.ClienteId });
        }

        private bool ConversaExists(int id)
        {
            return _context.Conversa.Any(e => e.Id == id);
        }
    }
}
