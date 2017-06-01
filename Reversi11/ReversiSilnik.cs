using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reversi11
{
    public class ReversiSilnik
    {

        private struct MozliwyRuch: IComparable<MozliwyRuch>
        {
            public int poziomo;
            public int pionowo;
            public int priorytet; 

            public MozliwyRuch(int poziomo, int pionowo, int priorytet)
            {
                this.poziomo = poziomo;
                this.pionowo = pionowo;
                this.priorytet = priorytet;
            }

            public int CompareTo(MozliwyRuch innyRuch)
            {
                return innyRuch.priorytet - this.priorytet;
            }
        }

        public const int PlanszaSzer = 8;
        public const int PlanszaWys = 8;
        private int[,] plansza = new int[PlanszaSzer, PlanszaWys];
        private int numerGraczaWykonujacegoNastepnyRuch = 1;

        public int StanPola(int poziomo, int pionowo)
        {
            if (poziomo < 0 || poziomo >= PlanszaSzer || pionowo < 0 || pionowo >= PlanszaWys)
                throw new Exception("Nieprawidlowe wspolrzedne pola");
            return plansza[poziomo, pionowo];
        }

        public int NumerGraczaWykonujacegoNastepnyRuch
        {
            get
            {
                return numerGraczaWykonujacegoNastepnyRuch;
            }
        }
        public ReversiSilnik(int numerGraczaRozpoczynajacego)
        {
            if (numerGraczaRozpoczynajacego < 1 || numerGraczaRozpoczynajacego > 2)
                throw new Exception("Nieprawidlowy numer gracza rozpoczynajacego gre");

            for (int i = 1; i < PlanszaSzer; i++)
                for (int j = 0; j < PlanszaWys; j++)
                    plansza[i, j] = 0;

            int srodekSzer = PlanszaSzer / 2;
            int srodekWys = PlanszaWys / 2;
            plansza[srodekSzer - 1, srodekWys - 1] = plansza[srodekSzer, srodekWys] = 1;
            plansza[srodekSzer - 1, srodekWys] = plansza[srodekSzer, srodekWys - 1] = 2;

            numerGraczaWykonujacegoNastepnyRuch = numerGraczaRozpoczynajacego;
        }
        private void zmienBiezacegoGracza()
        {
            numerGraczaWykonujacegoNastepnyRuch = ((numerGraczaWykonujacegoNastepnyRuch == 1) ? 2 : 1);
        }
        protected int PolozKamien(int poziomo, int pionowo, bool tylkoTest)
        {
            //czy wspolrzedne sa prawidlowe?
            if (poziomo < 0 || poziomo >= PlanszaSzer || pionowo < 0 || pionowo >= PlanszaWys)
                throw new Exception("Nieprawidlowe wspolrzedne pola");

            //czy pole juz nie jest zajete?
            if (plansza[poziomo, pionowo] != 0) return -1;

            int ilePolPrzejetych = 0;

            //petla po 8 kierunkach
            for (int kierunekPoziomo = -1; kierunekPoziomo <= 1; kierunekPoziomo++)
                for (int kierunekPionowo = -1; kierunekPionowo <= 1; kierunekPionowo++)
                {
                    //wymuszenie przypadku, gdy obie zmienne sa rowne 0
                    if (kierunekPoziomo == 0 && kierunekPionowo == 0) continue;

                    //szukanie pionkow gracza w jednym z 8 kierunkow
                    int i = poziomo;
                    int j = pionowo;
                    bool znalezionyKamienPrzeciwnika = false;
                    bool znalezionyKamienGraczaWykonujacegoRuch = false;
                    bool znalezionePustePole = false;
                    bool osiagnietaKrawedzPlanszy = false;
                    do
                    {
                        i += kierunekPoziomo;
                        j += kierunekPionowo;
                        if (i < 0 || j < 0 || i >= PlanszaSzer || j >= PlanszaWys)
                            osiagnietaKrawedzPlanszy = true;
                        if (!osiagnietaKrawedzPlanszy)
                        {
                            if (plansza[i, j] == numerGraczaWykonujacegoNastepnyRuch)
                                znalezionyKamienGraczaWykonujacegoRuch = true;
                            if (plansza[i, j] == 0) znalezionePustePole = true;
                            if (plansza[i, j] == ((numerGraczaWykonujacegoNastepnyRuch == 1) ? 2 : 1))
                                znalezionyKamienPrzeciwnika = true;
                        }
                    }
                    while (!(osiagnietaKrawedzPlanszy || znalezionyKamienGraczaWykonujacegoRuch || znalezionePustePole));

                    //sprawdzenie warunku poprawnosci ruchu
                    bool mozliwePolozenieKamienia = (znalezionyKamienPrzeciwnika && znalezionyKamienGraczaWykonujacegoRuch && !znalezionePustePole);

                    //odwrocenie pionkow w przypadku spelnionego warunku
                    if (mozliwePolozenieKamienia)
                    {
                        int maks_indeks = Math.Max(Math.Abs(i - poziomo), Math.Abs(j - pionowo));
                        if (!tylkoTest)
                        {
                            for (int indeks = 0; indeks < maks_indeks; indeks++)
                                plansza[poziomo + indeks * kierunekPoziomo, pionowo + indeks * kierunekPionowo] = numerGraczaWykonujacegoNastepnyRuch;
                        }
                        ilePolPrzejetych += maks_indeks - 1;
                    }
                } //koniec petli po kierunkach
            //jezeli ruch zostal wykonany - zmiana gracza
            if (ilePolPrzejetych > 0 && !tylkoTest)
            {
                zmienBiezacegoGracza();
            }
            //ilePolPrzejetych nie uwzglednia dostawanego kamienia
            return ilePolPrzejetych;
        }
        public bool PolozKamien(int poziomo, int pionowo)
        {
            return PolozKamien(poziomo, pionowo, false) > 0;
        }
        private int[] ilosciPol = new int[3]; //puste,gracz1, gracz2

        private void obliczIloscPol()
        {
            for (int i = 0; i < ilosciPol.Length; ++i) ilosciPol[i] = 0;

            for (int i = 0; i < PlanszaSzer; ++i)
                for (int j = 0; j < PlanszaWys; ++j)
                    ilosciPol[plansza[i, j]]++;

        }
        public int IloscPolPustych { get { return ilosciPol[0]; } }
        public int IloscPolGracz1 { get { return ilosciPol[1]; } }
        public int IloscPolGracz2 { get { return ilosciPol[2]; } }
        private bool czyBiezacyGraczMozeWykonacRuch()
        {
            int iloscPolPoprawnych = 0;
            for (int i = 0; i < PlanszaSzer; ++i)
                for (int j = 0; j < PlanszaWys; ++j)
                    if (plansza[i, j] == 0)
                        if (PolozKamien(i, j, true) > 0)
                            iloscPolPoprawnych++;
            return iloscPolPoprawnych > 0;
        }
        public void Pasuj()
        {
            if (czyBiezacyGraczMozeWykonacRuch())
                throw new Exception("Gracz nie moze oddac ruchu, jezeli wykonanie ruchu jest mozliwe");
            zmienBiezacegoGracza();
        }
        public enum SytuacjeNaPlanszy
        {
            RuchJestMozliwy,
            BiezacyGraczNieMozeWykonacRuchu,
            ObajGraczeNieMogaWykonacRuchu,
            WszystkiePolaPlanszySaZajete
        };
        public SytuacjeNaPlanszy ZbadajSytuacjeNaPlanszy()
        {
            if (IloscPolPustych == 0) return SytuacjeNaPlanszy.WszystkiePolaPlanszySaZajete;

            //badanie mozliwosci ruchu biezacego gracza
            bool czyMozliwyRuch = czyBiezacyGraczMozeWykonacRuch();
            if (czyMozliwyRuch) return SytuacjeNaPlanszy.RuchJestMozliwy;
            else
            {
                //badanie mozliwosci ruchu przeciwnika
                zmienBiezacegoGracza();
                bool czyMozliwyRuchOponenta = czyBiezacyGraczMozeWykonacRuch();
                zmienBiezacegoGracza();
                if (czyMozliwyRuchOponenta) return SytuacjeNaPlanszy.BiezacyGraczNieMozeWykonacRuchu;
                else return SytuacjeNaPlanszy.ObajGraczeNieMogaWykonacRuchu;
                

            }
        }

    }
}