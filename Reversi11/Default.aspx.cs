﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Reversi11
{
    public partial class Default : System.Web.UI.Page
    {
        private bool koniecGry = false;
        ReversiSilnik silnik;

        private Color kolorPodpowiedzi(int numerGracza)
        { int r = (kolory[0].R + kolory[numerGracza].R) / 2;
            int g = (kolory[0].G + kolory[numerGracza].G) / 2;
            int b = (kolory[0].B + kolory[numerGracza].B) / 2;
            return Color.FromArgb(r, g, b);
        }

        private void zaznaczNajlepszyRuch()
        {
            if (koniecGry) return; 
            try
            {
                int poziomo, pionowo;
                silnik.ProponujNajlepszyRuch(out poziomo, out pionowo);
                Plansza1.UstawKolorPola(poziomo, pionowo, kolorPodpowiedzi(silnik.NumerGraczaWykonujacegoNastepnyRuch));
                Button2.Text = "Wykonaj najlepszy ruch (" + SymbolPola(poziomo, pionowo) + ")";

            }

            catch
            {
                if (silnik.IloscPolPustych > 1) WyswietlKomunikat("Gracz " +
                      nazwyGraczy[silnik.NumerGraczaWykonujacegoNastepnyRuch] + " nie może wykonać ruchu", true);
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            ReversiSilnik silnik = new ReversiSilnik(1);
            //silnik.PolozKamien(2,4);
            */
            if (Session["silnik"] == null)
            {
                silnik = new ReversiSilnik(1);
                Session.Add("silnik", silnik);
                WyswietlKomunikat("Gra rozpoczeta.", false);
            }
            else silnik = Session["silnik"] as ReversiSilnik;

            uzgodnijZawartoscPlanszy(silnik);
            Plansza1.KliknieciePolaPlanszy += Plansza1_KliknieciePolaPlanszy;
        }
        public static string SymbolPola(int poziomo, int pionowo)
        {
            if (poziomo > 25 || pionowo > 8) return "(" + poziomo.ToString() + "." + pionowo.ToString() + ")";
            return "" + "ABCDEFGHIJKLMNOPRSTUWXYZ"[poziomo] + "123456789"[pionowo];
        }

        void WyswietlKomunikat(string trescKomunikatu, bool dodaj)
        {
            if (!dodaj) Label3.Text = "";
            Label3.Text = DateTime.Now.ToString() + " - " + trescKomunikatu + "<br />" + Label3.Text;
        }
        void Plansza1_KliknieciePolaPlanszy(object sender, Plansza.PolePlanszyEventArgs e)
        {
            if (koniecGry) return;
            //wykonanie ruchu
            int zapamietanyNumerGracza = silnik.NumerGraczaWykonujacegoNastepnyRuch;
            if (silnik.PolozKamien(e.poziomo, e.pionowo))
            {
                uzgodnijZawartoscPlanszy(silnik);

                //listaruchow
                switch (zapamietanyNumerGracza)
                {
                    case 1: ListBox1.Items.Add(SymbolPola(e.poziomo, e.pionowo)); break;
                    case 2: ListBox2.Items.Add(SymbolPola(e.poziomo, e.pionowo)); break;
                }
                ListBox1.SelectedIndex = ListBox1.Items.Count - 1;
                ListBox2.SelectedIndex = ListBox2.Items.Count - 1;

                //sytuacje specjalne

                /*
                ReversiSilnik.SytuacjeNaPlanszy sytuacjaNaPlanszy = silnik.ZbadajSytuacjeNaPlanszy();
                switch (sytuacjaNaPlanszy)
                {
                    case ReversiSilnik.SytuacjeNaPlanszy.BiezacyGraczNieMozeWykonacRuchu:
                        WyswietlKomunikat("Gracz " + nazwyGraczy[silnik.NumerGraczaWykonujacegoNastepnyRuch] + " zmuszony jest do oddania ruchu", true);
                        silnik.Pasuj();
                        uzgodnijZawartoscPlanszy(silnik);
                        break;
                    case ReversiSilnik.SytuacjeNaPlanszy.ObajGraczeNieMogaWykonacRuchu:
                        WyswietlKomunikat("Obaj Gracze nie moga wykonac ruchu", true);
                        koniecGry = true;
                        break;
                    case ReversiSilnik.SytuacjeNaPlanszy.WszystkiePolaPlanszySaZajete:
                        koniecGry = true;
                        break;
                }
                */

                //wylonienie zwyciezcy
                if (koniecGry)
                {
                    for (int i = 0; i < ReversiSilnik.PlanszaSzer; i++)
                        for (int j = 0; j < ReversiSilnik.PlanszaWys; j++)
                            Plansza1.ZablokujPole(i, j);

                    int numerZwyciezcy = (silnik.IloscPolGracz1 > silnik.IloscPolGracz2) ? 1 : 2;
                    if (silnik.IloscPolGracz1 == silnik.IloscPolGracz2) numerZwyciezcy = 0;
                    if (numerZwyciezcy != 0) WyswietlKomunikat("Wygral gracz " + nazwyGraczy[numerZwyciezcy], true);
                    else WyswietlKomunikat("Remis", true);

                    Button1.Font.Bold = true;
                    Button2.Enabled = false;
                    CheckBox1.Enabled = false;
                }

                // WyswietlKomunikat("<font color=" + kolory[zapamietanyNumerGracza].Name + ">Gracz " + nazwyGraczy[zapamietanyNumerGracza] + " polozyl kamien na polu " + SymbolPola(e.poziomo, e.pionowo) + "</font>", true);
            }
            else WyswietlKomunikat("Polozenie kamienia na polu " + SymbolPola(e.poziomo, e.pionowo) + " przez gracza " + nazwyGraczy[silnik.NumerGraczaWykonujacegoNastepnyRuch] + " nie bylo mozliwe", true);
        }
        /*
           string s = "plansza: <br />";
           for (int pionowo = 0; pionowo < ReversiSilnik.PlanszaWys; pionowo++)
           {
               for (int poziomo = 0; poziomo < ReversiSilnik.PlanszaSzer; poziomo++)
               {
                   s += silnik.StanPola(poziomo, pionowo) + " ";
               }
               s += "<br />";
           }
           s += "<p>Ilosc pustych pol: " + silnik.IloscPolPustych + "<br />Ilosc pol gracza 1:" + silnik.IloscPolGracz1 + "<br />Ilosc pol gracza 2:" + silnik.IloscPolGracz2;

           Label l = new Label();
           l.Text = s;
           l.Font.Name = "Courier";
           form1.Controls.Add(l);

       }
                   */
        private string[] nazwyGraczy = { "", "zielony", "brazowy" };
        private Color[] kolory = { Color.Ivory, Color.Green, Color.Sienna };
        private void uzgodnijZawartoscPlanszy(ReversiSilnik silnik)
        {
            for (int i = 0; i < ReversiSilnik.PlanszaSzer; i++)
            {
                for (int j = 0; j < ReversiSilnik.PlanszaWys; j++)
                {
                    int stanPola = silnik.StanPola(i, j);
                    Plansza1.UstawKolorPola(i, j, kolory[stanPola]);
                    Plansza1.ZablokujPole(i, j, stanPola != 0);
                }


                if (CheckBox1.Checked) zaznaczNajlepszyRuch();
                Label1.Text = "<font color=" + kolory[1].Name + ">Gracz" + nazwyGraczy[1] + "<br/>Ilość pól: " + silnik.IloscPolGracz1.ToString() + "</font";
                Label2.Text = "<font color=" + kolory[2].Name + ">Gracz" + nazwyGraczy[2] + "<br/>Ilość pól: " + silnik.IloscPolGracz2.ToString() + "</font";
                Button1.BackColor = kolory[0];
                Button2.BackColor = kolory[0];
                Label4.Text = "<font color=" + kolory[silnik.NumerGraczaWykonujacegoNastepnyRuch].Name + ">" + nazwyGraczy[silnik.NumerGraczaWykonujacegoNastepnyRuch] + "</font>";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            silnik = new ReversiSilnik(1);
            Session["silnik"] = silnik;
            przygotowaniePlanszyDoNowejGry();
        }
        private void przygotowaniePlanszyDoNowejGry()
        {
            ListBox1.Items.Clear();
            ListBox2.Items.Clear();

            for (int i = 0; i < ReversiSilnik.PlanszaSzer; i++)
                for (int j = 0; j < ReversiSilnik.PlanszaWys; j++)
                    Plansza1.ZablokujPole(i, j, false); ;

            uzgodnijZawartoscPlanszy(silnik);

            Button1.Font.Bold = false;
            Button2.Enabled = true;
            CheckBox1.Enabled = true;

            WyswietlKomunikat("Gra rozpoczeta", false);
        }


    }
}