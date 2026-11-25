using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eseReGuardiePedoni
{
    public class Guardia
    {
        public string nome {  get; set; }

        public Guardia(string n)
        {
            this.nome = n;
        }

        public string Attacco()
        {
            return $"La Guardia {nome} sta difendendo\n";
        }
    }
}