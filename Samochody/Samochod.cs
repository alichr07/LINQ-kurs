using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Samochody
{
    public class Samochod
    {
        public int Rok { get; set; }
        public string Producent { get; set; }
        public string Model { get; set; }
        public double Pojemnosc { get; set; }
        public int IloscCylindrow { get; set; }
        public int SpalanieMiasto { get; set; }
        public int SpalanieAutostrada { get; set; }
        public int SpalanieMieszane { get; set; }
    }
}
