using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reversi11
{
    public partial class Plansza : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private int szerokosc;
        private int wysokosc;
        private int iloscPrzyciskowPoziomo;
        private int iloscPrzyciskowPionowo;
        class PolePlanszy : Button
        {
            public int poziomo;
            public int pionowo;
        }
        private Panel panel;
        private Button[,] polaPlanszy;
        public Plansza(int iloscPrzyciskowPoziomo, int iloscPrzyciskowPionowo)
        {
            this.iloscPrzyciskowPoziomo = iloscPrzyciskowPoziomo;
            this.iloscPrzyciskowPionowo = iloscPrzyciskowPionowo;

            panel = new Panel();
            this.Controls.Add(panel);

            //tworzenie przyciskow
            polaPlanszy = new PolePlanszy[iloscPrzyciskowPoziomo, iloscPrzyciskowPionowo];

            for (int i=0; i < iloscPrzyciskowPoziomo; i++)
            {
                for (int j = 0; j < iloscPrzyciskowPionowo; j++)
                {
                    PolePlanszy b = new PolePlanszy();
                    b.poziomo = i;
                    b.pionowo = j;
                    b.Click += new EventHandler(kliknieciePlanszy);
                    polaPlanszy[i, j] = b;
                    panel.Controls.Add(polaPlanszy[i, j]);
                }
            }
            UstalRozmiarPlanszy(320, 320);
        }
        public Plansza()
            : this(8, 8)
        {
        }
        public void UstalRozmiarPlanszy (int szerokosc, int wysokosc)
        {
            this.szerokosc = szerokosc;
            this.wysokosc = wysokosc;
            panel.Width = szerokosc;
            panel.Height = wysokosc;

            int przyciskSzer = szerokosc / iloscPrzyciskowPoziomo;
            int przyciskWys = wysokosc / iloscPrzyciskowPionowo;

            for (int i = 0; i < iloscPrzyciskowPoziomo; i++)
            {
                for (int j = 0; j < iloscPrzyciskowPionowo; j++)
                {
                    polaPlanszy[i, j].Width = przyciskSzer;
                    polaPlanszy[i, j].Height = przyciskWys;
                }
            }
        }
        public void UstawKolorPola(int poziomo, int pionowo, System.Drawing.Color kolor)
        {
            polaPlanszy[poziomo, pionowo].BackColor = kolor;
        }
        public void ZablokujPole(int poziomo, int pionowo, bool zablokowany = true)
        {
            polaPlanszy[poziomo, pionowo].Enabled = !zablokowany;
        }
        public class PolePlanszyEventArgs : EventArgs
        {
            public int poziomo;
            public int pionowo;
        }
        public delegate void KliknieciePolaPlanszyEventHandler(object sender, PolePlanszyEventArgs e);
        public event KliknieciePolaPlanszyEventHandler KliknieciePolaPlanszy;
        protected virtual void OnKliknieciePolaPlanszy(object sender, PolePlanszyEventArgs e)
        {
            if (KliknieciePolaPlanszy != null) KliknieciePolaPlanszy(sender, e);
        }
        void kliknieciePlanszy(object sender, EventArgs e)
        {
            /*
            //szukamy pola planszy odpowiadajacego kliknietemu przyciskowi
            int kliknietePoziomo = -1, kliknietePionowo = -1;
            for (int i = 0; i < iloscPrzyciskowPoziomo; i++)
                for (int j = 0; j < iloscPrzyciskowPionowo; j++)
                    if (sender == polaPlanszy[i,j])
                    {
                        kliknietePoziomo = i;
                        kliknietePionowo = j;
                        goto ZnalezionyPrzycisk;
                    }
            ZnalezionyPrzycisk:
            //jezeli nie znaleziony = zglaszanie bledu
            if (kliknietePoziomo == -1 || kliknietePionowo == -1)
                throw new Exception("Niezidentyfikowane pole planszy");
                */
            PolePlanszy pole = sender as PolePlanszy;
            OnKliknieciePolaPlanszy(
                this,
                new PolePlanszyEventArgs
                {
                    poziomo = pole.poziomo,
                    pionowo = pole.pionowo
                });
         
        }           
        
    }
}