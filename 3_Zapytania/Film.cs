using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_Zapytania
{
    class Film
    {
        public string Tytul { get; set;}
        public string Gatunek { get; set; }
        public float Ocena { get; set; }

        private int _rok;
        public int Rok 
        { 
            get
            {
                Console.WriteLine($"Zwraca {_rok} i {Tytul}");
                return _rok;
            }
            set
            {
                _rok = value;
            }
        }
    }
}
