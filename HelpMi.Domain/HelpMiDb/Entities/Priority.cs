using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpMi.EL.HelpMiDb.Entities
{
    public class Priority
    {
        public int PriorityId { get; set; }
        public string Name { get; set; }  // Es. "Alta", "Media", "Bassa"

        // Relazione inversa: una priorità può essere assegnata a più ticket
        public ICollection<Ticket> Tickets { get; set; }
    }

}
