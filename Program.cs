using System;
using System.Collections.Generic;

namespace GestioneBancaria
{
    public class Cliente
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string CodiceFiscale { get; set; }
        public double Stipendio { get; set; }

        public Cliente(string nome, string cognome, string cf, double stipendio)
        {
            Nome = nome;
            Cognome = cognome;
            CodiceFiscale = cf;
            Stipendio = stipendio;
        }

        public override string ToString()
        {
            return $"{Nome} {Cognome} (CF: {CodiceFiscale}) - Stipendio: {Stipendio}€";
        }
    }

    public class Prestito
    {
        public double Ammontare { get; set; }
        public double Rata { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public Cliente Intestatario { get; set; }

        public Prestito(double ammontare, double rata, DateTime inizio, DateTime fine, Cliente intestatario)
        {
            Ammontare = ammontare;
            Rata = rata;
            DataInizio = inizio;
            DataFine = fine;
            Intestatario = intestatario;
        }

        public override string ToString()
        {
            return $"Prestito di {Ammontare}€ (Scadenza: {DataFine.ToShortDateString()})";
        }
    }

    public class Banca
    {
        public string Nome { get; set; }
        private List<Cliente> clienti = new List<Cliente>();
        private List<Prestito> prestiti = new List<Prestito>();

        public Banca(string nome) { Nome = nome; }

        public void AddCliente(Cliente c) => clienti.Add(c);

        public Cliente CercaCliente(string cf)
        {
            foreach (Cliente c in clienti)
            {
                if (c.CodiceFiscale == cf) return c;
            }
            return null;
        }

        // AGGIUNTA: Rimuovere un cliente (richiesto dal libro)
        public bool RimuoviCliente(string cf)
        {
            Cliente c = CercaCliente(cf);
            if (c != null)
            {
                clienti.Remove(c);
                return true;
            }
            return false;
        }

        public void AddPrestito(Prestito p) => prestiti.Add(p);

        public double TotalePrestiti(string cf)
        {
            double totale = 0;
            foreach (Prestito p in prestiti)
            {
                if (p.Intestatario.CodiceFiscale == cf)
                    totale += p.Ammontare;
            }
            return totale;
        }

        public List<Prestito> GetPrestitiCliente(string cf)
        {
            List<Prestito> trovati = new List<Prestito>();
            foreach (Prestito p in prestiti)
            {
                if (p.Intestatario.CodiceFiscale == cf)
                    trovati.Add(p);
            }
            return trovati;
        }
    } 

    class Program
    {
        static void Main(string[] args)
        {
            Banca b = new Banca("EuroBanca spa");

            Cliente x = new Cliente("Mario", "Rossi", "MRORSI78B14FN478F", 50000);
            b.AddCliente(x);

            b.AddPrestito(new Prestito(10000, 200, DateTime.Now, DateTime.Now.AddYears(5), x));
            b.AddPrestito(new Prestito(5000, 100, DateTime.Now, DateTime.Now.AddYears(2), x));

            Console.WriteLine($"Banca: {b.Nome}");
            Cliente trovato = b.CercaCliente("MRORSI78B14FN478F");

            if (trovato != null)
            {
                Console.WriteLine($"\nCliente: {trovato}");
                Console.WriteLine($"Totale prestiti: {b.TotalePrestiti(trovato.CodiceFiscale)}€");

                Console.WriteLine("Dettaglio prestiti:");
                foreach (var p in b.GetPrestitiCliente(trovato.CodiceFiscale))
                {
                    Console.WriteLine(" - " + p);
                }
            }

            Console.ReadKey();
        }
    }
}