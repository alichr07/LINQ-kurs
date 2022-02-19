using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Samochody
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SamochodDB>());
            WstawDane();
            ZapytanieDane();
        }

        private static void WstawDane()
        {
            var samochody = WczytywanieSamochodu("paliwo.csv");
            var db = new SamochodDB();
            db.Database.Log = Console.WriteLine;

            if (!db.Samochody.Any())
            {
                foreach (var samochod in samochody)
                {
                    db.Samochody.Add(samochod);
                }
                db.SaveChanges();
            }
        }

        private static void ZapytanieDane()
        {
            var db = new SamochodDB();
            db.Database.Log = Console.WriteLine;

            var zapytanie = from samochod in db.Samochody
                            orderby samochod.SpalanieAutostrada descending, samochod.Model ascending
                            select samochod;

            var zapytanie2 = db.Samochody.Where(s => s.Model == "Audi")
                                         .OrderByDescending(s => s.SpalanieAutostrada).ThenBy(s => s.Model).Take(10);

            foreach (var samochod in zapytanie2)
            {
                Console.WriteLine($"{samochod.Model} : {samochod.SpalanieAutostrada}");
            }
        }

        private static void ZapytanieXML()
        {
            XNamespace ns = "namespace";
            XNamespace ex = "namespace/ex";

            var dokument = XDocument.Load("paliwo.xml");
            var zapytanie = from element in dokument.Element(ns + "Samochody")?.Elements(ex + "Samochod") ?? Enumerable.Empty<XElement>()
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
            XNamespace ns = "namespace";
            XNamespace ex = "namespace/ex";

            var rekordy = WczytywanieSamochodu("paliwo.csv");

            var dokument = new XDocument();
            var samochody = new XElement(ns + "Samochody", from rekord in rekordy
                                                      select new XElement(ex + "Samochod",
                                                                            new XAttribute("Rok", rekord.Rok),
                                                                            new XAttribute("Producent", rekord.Producent),
                                                                            new XAttribute("Model", rekord.Model),
                                                                            new XAttribute("SpalanieAutostrada", rekord.SpalanieAutostrada),
                                                                            new XAttribute("SpalanieMiasto", rekord.SpalanieMiasto),
                                                                            new XAttribute("SpalanieMieszane", rekord.SpalanieMieszane)));
            samochody.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));

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

