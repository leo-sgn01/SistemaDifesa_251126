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
        static void Main(string[] args)
        {
            List<Guardia> listaGuardie = new List<Guardia>();
            List<Pedone> listaPedoni = new List<Pedone>();
            List<string> listaComandi = new List<string>();
            Re re = null;
            string percorsoFileIn = "commands.txt";

            File.WriteAllText("output.txt", string.Empty);
            
            if(File.Exists(percorsoFileIn))
            {
                string[] linee = File.ReadAllLines(percorsoFileIn);

                if(linee.Length > 0)
                {
                    string nomeRe = linee[0].Trim();
                    re = new Re(nomeRe);
                    WriteLine($"Caricato re: {nomeRe}");
                }

                if(linee.Length > 1 && !string.IsNullOrWhiteSpace(linee[1]))
                {
                    string[] nomiGuardie = linee[1].Split(new[] { ' ' });
                    foreach(var nome in nomiGuardie)
                    {
                        listaGuardie.Add(new Guardia(nome));
                    }
                    WriteLine($"Caricate {listaGuardie.Count} guardie");
                }

                if(linee.Length > 2 && !string.IsNullOrWhiteSpace(linee[2]))
                {
                    string[] nomiPedoni = linee[2].Split(new[] {' '});
                    foreach(var nome in nomiPedoni)
                    {
                        listaPedoni.Add(new Pedone(nome));
                    }
                    WriteLine($"Caricati {listaPedoni.Count} pedoni");
                }

                for(int i = 3; i < linee.Length; i++)
                {
                    string comando = linee[i].Trim();
                    if(!string.IsNullOrWhiteSpace(comando))
                    {
                        listaComandi.Add(comando);
                        if (comando == "End")
                        {
                            break;
                        }
                    }
                }
                WriteLine($"Caricati {listaComandi.Count} comandi");

                WriteLine();    
                WriteLine($"--- ATTIVAZIONE SISTEMA DI DIFESA ---");

                foreach (var com in listaComandi)
                {
                    WriteLine($"> {com}");

                    if (com == "Attacca Re" && re != null)
                    {
                        Write(re.Attacco());

                        foreach (var guardia in listaGuardie)
                        {
                            string risulG = guardia.Attacco();
                            if (!string.IsNullOrWhiteSpace(risulG))
                            {
                                Write(risulG);
                            }
                        }
                        foreach (var ped in listaPedoni)
                        {
                            string risulP = ped.Attacco();
                            if (!string.IsNullOrWhiteSpace(risulP))
                            {
                                Write(risulP);
                            }
                        }

                        WriteLine();
                    }
                    else if (com.StartsWith("Cattura "))
                    {
                        WriteLine();

                        string nomeCatturato = com.Substring(8).Trim();
                        Guardia guardiaRimossa = null;
                        Pedone pedoneRimosso = null;

                        foreach (var g in listaGuardie)
                        {
                            if(g.nome == nomeCatturato)
                            {
                                guardiaRimossa = g;
                                break;
                            }
                        }

                        if (guardiaRimossa != null)
                        {
                            listaGuardie.Remove(guardiaRimossa);
                        }
                        else
                        {
                            foreach (var p in listaPedoni)
                            {
                                if (p.nome == nomeCatturato)
                                {
                                    pedoneRimosso = p;
                                    break;
                                }
                            }

                            if(pedoneRimosso != null)
                            {
                                listaPedoni.Remove(pedoneRimosso);
                            }
                        }
                    } else if(com == "End")
                    {
                        WriteLine("Attacchi terminati: sistema di difesa in pausa");
                        break;
                    } else
                    {
                        WriteLine("ERRORE: Comando non valido");
                    }
                }

                WriteLine("-- SISTEMA DI DIFESA DISATTIVATO --");
            }
            else
            {
                WriteLine($"ERRORE: Il file '{percorsoFileIn}' non è stato trovato");
            }

            WriteLine("\nPremi un tasto per terminare..");
            ReadKey();
        }
    }
}