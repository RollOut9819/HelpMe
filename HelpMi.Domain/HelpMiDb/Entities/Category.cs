using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpMi.EL.HelpMiDb.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }  // Es. "Tecnico", "Amministrativo", "Generale"

        // Relazione inversa: una categoria può avere più ticket associati
        public ICollection<Ticket> Tickets { get; set; }
    }

}
