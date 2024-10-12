using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpMi.EL.HelpMiDb.Entities
{
    public class Status
    {
        public int StatusId { get; set; }
        public string Name { get; set; }  // Es. "Aperto", "In corso", "Risolto", "Chiuso"

        // Relazione inversa: uno stato può essere assegnato a più ticket
        public ICollection<Ticket> Tickets { get; set; }
    }

}
