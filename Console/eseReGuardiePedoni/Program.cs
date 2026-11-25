using eseReGuardiePedoni;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static System.Console;

namespace eseReGuardiePedoni
{
    internal class Program
    {
        static List<Guardia> listaGuardie = new List<Guardia>();
        static List<Pedone> listaPedoni = new List<Pedone>();
        static List<string> listaComandi = new List<string>();
        static List<string> registro = new List<string>();
        static Re re = null;
        static int indexComCorrente = 0;

        static void Main(string[] args)
        {
            string percorsoFileIn = "commands.txt";

            File.WriteAllText("output.txt", string.Empty);

            if(!File.Exists(percorsoFileIn))
            {
                WriteLine($"ERRORE: file '{percorsoFileIn}' non trovato");
                return;
            }

            CaricaFile(percorsoFileIn);


            int scelta;
            do
            {
                Clear();
                StampaInterfacciaConsole();

                WriteLine("\n Scegli un azione:");
                WriteLine("1. Esegui un passo");
                WriteLine("2. Esegui tutto");
                WriteLine("0. Esci");
                Write("Scelta: ");

                while(!int.TryParse(ReadLine(), out scelta) || (scelta < 0 && scelta > 2))
                {
                    WriteLine("ERRORE: scelta non valida");
                    Write("Scelta (0-2): ");
                }

                WriteLine("-- SISTEMA DI DIFESA ATTIVATO --");

                switch(scelta)
                {
                    case 1:
                        EseguiProssimoComando();
                        break;

                    case 2:
                        EseguiTutto();
                        break;
                }
            } while (scelta != 0);
        }

        static void CaricaFile(string percorso)
        {
            string[] linee = File.ReadAllLines(percorso);

            if (linee.Length > 0)
                re = new Re(linee[0].Trim());

            if (linee.Length > 1 && !string.IsNullOrWhiteSpace(linee[1]))
            {
                string[] nomiGuardie = linee[1].Split(new[] { ' ' });
                foreach (var nome in nomiGuardie)
                {
                    listaGuardie.Add(new Guardia(nome));
                }
            }

            if (linee.Length > 2 && !string.IsNullOrWhiteSpace(linee[2]))
            {
                string[] nomiPedoni = linee[2].Split(new[] { ' ' });
                foreach (var nome in nomiPedoni)
                {
                    listaPedoni.Add(new Pedone(nome));
                }
            }

            for (int i = 3; i < linee.Length; i++)
            {
                string cmd = linee[i].Trim();
                if (!string.IsNullOrWhiteSpace(cmd))
                {
                    listaComandi.Add(cmd);
                    if (cmd == "End")
                    {
                        break;
                    }
                }
            }
        }

        static void EseguiComando(string cmd)
        {
            registro.Add($"> {cmd}");

            if(cmd == "Attacca Re" && re != null)
            {
                registro.Add(re.Attacco());
                foreach(var g in listaGuardie)
                {
                    string ris = g.Attacco();
                    if(!string.IsNullOrWhiteSpace(ris))
                        registro.Add(ris);
                }
                foreach(var p in listaPedoni)
                {
                    string ris = p.Attacco();
                    if(!string.IsNullOrWhiteSpace(ris))
                        registro.Add(ris);
                }
            }
            else if (cmd.StartsWith("Cattura "))
            {
                string nomeCatturato = cmd.Substring(8).Trim();
                Guardia gRimossa = null;
                Pedone pRimosso = null;

                foreach(var guardia in listaGuardie)
                {
                    if(guardia.nome ==  nomeCatturato)
                    {
                        gRimossa = guardia;
                        break;
                    }
                }
                if(gRimossa != null)
                {
                    listaGuardie.Remove(gRimossa);
                } else
                {
                    foreach(var pedone in listaPedoni)
                    {
                        if(pedone.nome == nomeCatturato)
                        {
                            pRimosso = pedone;
                            break;
                        }
                    }

                    if(pRimosso != null)
                    {
                        listaPedoni.Remove(pRimosso);
                    }
                }
            }
            else if (cmd == "End")
            {
                registro.Add("Sistema di difesa in pausa");
            }
            else
            {
                registro.Add("Comando non valido");
            }
        }

        static void EseguiProssimoComando()
        {
            if(indexComCorrente < listaComandi.Count)
            {
                string cmd = listaComandi[indexComCorrente];
                indexComCorrente++;
                EseguiComando(cmd);

                if(cmd == "End")
                {
                    indexComCorrente = listaComandi.Count;
                }
            }
            else
            {
                registro.Add("Nessun altro comando da eseguire");
            }
        }

        static void EseguiTutto()
        {
            while(indexComCorrente < listaComandi.Count)
            {
                string cmd = listaComandi[indexComCorrente];
                bool fine = (cmd == "End");

                EseguiProssimoComando();

                Clear();
                StampaInterfacciaConsole();

                if (fine)
                    break;
            }

            WriteLine("\n Premi un tasto per continuare..");
            ReadKey();
        }

        static void StampaInterfacciaConsole()
        {
            WriteLine("========== REGNO DI ASTERON ==========");
            WriteLine($"Re: {re.nome}");

            Write("Guardie disponibili: ");
            foreach(var guardia in listaGuardie)
            {
                Write($"{guardia.nome}");
            }
            WriteLine();

            Write("Pedoni disponibili: ");
            foreach(var pedone in listaPedoni)
            {
                Write($"{pedone.nome}");
            }
            WriteLine();

            WriteLine("ELENCO COMANDI:");
            for(int i = 0; i < listaComandi.Count; i++)
            {
                if(i == indexComCorrente)
                {
                    WriteLine($"--> {listaComandi[i]}");
                } else
                {
                    WriteLine($" {listaComandi[i]}");
                }
            }

            WriteLine("\n -- REGISTRO --");
            foreach(var r in registro)
            {
                WriteLine(r);
            }
            WriteLine("==============================================");
        }
    }
}