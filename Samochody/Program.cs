using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Samochody
{
    class Program
    {
        static void Main(string[] args)
        {
            TworzenieXML();
            ZapytanieXML();
        }

        private static void ZapytanieXML()
        {
            var dokument = XDocument.Load("paliwo.xml");
            var zapytanie = from element in dokument.Element("Samochody").Elements("Samochod")
                            where element.Attribute("Producent2")?.Value == "Ferrari"
                            select new
                            {
                                model = element.Attribute("Model").Value,
                                producent = element.Attribute("Producent").Value
                            };

            foreach (var samochod in zapytanie)
            {
                Console.WriteLine(samochod.producent + " " + samochod.model);
            }
        }

        private static void TworzenieXML()
        {
            var rekordy = WczytywanieSamochodu("paliwo.csv");

            var dokument = new XDocument();
            var samochody = new XElement("Samochody", from rekord in rekordy
                                                      select new XElement("Samochod",
                                                                            new XAttribute("Rok", rekord.Rok),
                                                                            new XAttribute("Producent", rekord.Producent),
                                                                            new XAttribute("Model", rekord.Model),
                                                                            new XAttribute("SpalanieAutostrada", rekord.SpalanieAutostrada),
                                                                            new XAttribute("SpalanieMiasto", rekord.SpalanieMiasto),
                                                                            new XAttribute("SpalanieMieszane", rekord.SpalanieMieszane)));

            dokument.Add(samochody);
            dokument.Save("paliwo.xml");
        }

        static List<Samochod> WczytywanieSamochodu(string sciezka)
            {
                var zapytanie = File.ReadAllLines(sciezka)
                                    .Skip(1)
                                    .Where(l => l.Length > 1)
                                    .WSamochod();

                return zapytanie.ToList();
            }
            static List<Producent> WczytywanieProducenci(string sciezka)
            {
                var zapytanie = File.ReadAllLines(sciezka)
                                    .Where(l => l.Length > 1)
                                    .Select(l =>
                                    {
                                        var kolumny = l.Split(',');
                                        return new Producent
                                        {
                                            Nazwa = kolumny[0],
                                            Siedziba = kolumny[1],
                                            Rok = int.Parse(kolumny[2])
                                        };
                                    });
                return zapytanie.ToList();
            }
        
        
    }
}

