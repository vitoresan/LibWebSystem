using Newtonsoft.Json;
using System;

namespace Infraestrutura.Domain.Models
{
    public class TipoUsuario
    {
        [Obsolete]
        public TipoUsuario()
        {

        }

        [JsonConstructor]
        public TipoUsuario(int id, string descr)
        {
            Id = id;
            Descr = descr;
        }
        public int Id { get; set; }
        public string Descr { get; set; }
    }
}
