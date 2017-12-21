using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroContatos.Models
{
    public class Conversa
    {
        public int Id { get; set; }
        public String UserId { get; set; }
        public string UsuarioNome { get; set; }
        public int ClienteId { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraConversa { get; set; }

        public string ClienteNome { get; set; }
        public IEnumerable<Cliente> ListaClientes { get; set; }
    }
}
