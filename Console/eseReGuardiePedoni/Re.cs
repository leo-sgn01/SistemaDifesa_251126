using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eseReGuardiePedoni
{
    public class Re
    {
        public string nome {  get; set; }

        public Re(string n)
        {
            this.nome = n;
        }

        public string Attacco()
        {
            return $"Il Re {nome} è sotto attacco\n";
        }
    }
}