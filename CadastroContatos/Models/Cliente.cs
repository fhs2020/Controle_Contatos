using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroContatos.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public String UserId { get; set; }
        public String UserName { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Cell { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cidade { get; set; }

        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public IEnumerable<Produto> ListaProdutos { get; set; }

       
        public string Status { get; set; }
        public int StatusId { get; set; }
        public IEnumerable<Status> ListaStatus { get; set; }
    }
}
