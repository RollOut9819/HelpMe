using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpMi.EL.HelpMiDb.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Relazione inversa: un utente può creare più ticket
        public ICollection<Ticket> Tickets { get; set; }
    }

}
