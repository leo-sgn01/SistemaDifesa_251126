using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace eseReGuardiePedoni
{
    public class Pedone
    {
        public string nome {  get; set; }

        public Pedone(string n)
        {
            this.nome = n;
        }

        public string Attacco()
        {
            return $"Il Pedone {nome} si sta preparando\n";
        }
    }
}