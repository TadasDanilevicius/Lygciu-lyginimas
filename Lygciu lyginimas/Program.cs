Console.WriteLine("Parasykite reakcijos lygti, kuria norite islyginti\n");
while (true)
{
    string reakcija = Console.ReadLine();
    if (reakcija == "clc") { Console.Clear(); Console.WriteLine("Parasykite reakcijos lygti, kuria norite islyginti"); }
    else
    {
        try
        {
            int rodykle = reakcija.IndexOf("=>");
            if (rodykle == -1)
                throw new Exception("Praleidote reakcijos zenkla '=>'");
            if(reakcija.IndexOf("=>",rodykle+1) != -1)
                throw new Exception("Zenklas '=>' parasytas net kelis kartus");
            string[] reakcijosPuses = reakcija.Split("=>");
            string[] reagentai = reakcijosPuses[0].Split("+");
            string[] produktai = reakcijosPuses[1].Split("+");
            int reagentuSkaicius = reagentai.Length;
            string[] junginiai = new string[reagentai.Length + produktai.Length];
            for (int i = 0; i < reagentai.Length; i++)
            { junginiai[i] = reagentai[i]; }
            for(int i=reagentai.Length; i<junginiai.Length; i++)
            { junginiai[i] = produktai[i - reagentai.Length]; }
            List<string> elementai = new List<string>();
            //istiriame junginiuose esancius elementus ir sudedame i List'a elementai
            for(int j=0; j<junginiai.Length; j++)
            {
                junginiai[j] = junginiai[j].Trim();
                if (junginiai[j].IndexOf(" ") != -1)
                    throw new Exception("junginius rasykite be tarpo");
                if (junginiai[j].IndexOf(" ") != -1)
                    throw new Exception("junginius rasykite be tarpo");
                char[] isskaidytas = junginiai[j].ToCharArray();
                for (int i = 0; i < isskaidytas.Length; i++)
                {
                    char a = isskaidytas[i];
                    if (a >= 'A' && a <= 'Z')
                    {
                        string simbolis = a.ToString();
                        if (i + 1 < isskaidytas.Length)
                        {
                            if (i <= isskaidytas.Length - 1 && isskaidytas[i + 1] >= 'a' && isskaidytas[i + 1] <= 'z')
                            {
                                i++;
                                simbolis += isskaidytas[i].ToString();
                            }
                        }
                        if (!elementai.Contains(simbolis))
                        {
                            elementai.Add(simbolis);
                        }
                    }
                }
            }
            int[,] matrica = new int[elementai.Count, junginiai.Length];
            int[] koeficientai = new int[junginiai.Length];
            // koefientai yra dalikliai prie reakcijos koeficientu ABC kanonineje israiskoje => A/k1 = B/k2 = C/k3
            for (int x = 0; x < elementai.Count; x++)
            {
                for (int y = 0; y < junginiai.Length; y++)
                {
                    matrica[x, y] = ElementuSkaicius(junginiai[y], elementai[x]);
                    if (y >= reagentuSkaicius)
                        matrica[x, y] *= -1;
                }
            }
            if (matrica.GetLength(1) > matrica.GetLength(0) + 1)
                throw new Exception("Reakcijos lygtis turi daugiau nei viena sprendini");
            int rez = solveGauss(ref matrica,koeficientai);
            if (rez == -1)
                throw new Exception("Reakcijos lygtis yra neteisinga");
            else if (rez == -2)
                throw new Exception("Reakcijos lygtis turi daugiau nei viena sprendini");
            string rezultatas = "";
            for(int i=0; i<junginiai.Length; i++)
            {
                if (i == reagentuSkaicius)
                    rezultatas += " => ";
                else if (i != 0)
                    rezultatas += " + ";
                if (koeficientai[i] != 1)
                    rezultatas += koeficientai[i];
                rezultatas += junginiai[i];
            }
            Console.WriteLine("\n\tIslyginta reakcijos lygtis:");
            Console.WriteLine("\t"+rezultatas+"\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
//skaiciuoja elemento atomu skaiciu junginyje
int ElementuSkaicius(string junginys, string simbolis)
{
    int elementoKiekis = 0;
    junginys = junginys.Replace("[", "(");
    junginys = junginys.Replace("]", ")");
    int skaicius;
    while(true)
    {
        int vieta = junginys.IndexOf(simbolis);
        int skliaustai = junginys.IndexOf("(");
        int zvaigzdute = junginys.IndexOf("*");
        if(vieta == -1)
        { break; }
        // patikrina kas greiciau junginyje prasideda skliaustai ar elementas
        if((skliaustai > vieta || skliaustai == -1) && (zvaigzdute > vieta || zvaigzdute == -1))
        {
            junginys = junginys.Substring(vieta + simbolis.Length);
            skaicius = GautiSkaiciu(junginys);
            elementoKiekis += skaicius;
        }
        else if ((skliaustai < zvaigzdute && zvaigzdute == -1) || skliaustai != -1)
        {
            junginys = junginys.Substring(skliaustai + 1);
            int galas = uzdaromiSkliaustai(junginys);
            string skliaustuVidus = junginys.Substring(0, galas-1);
            int skaiciusV = ElementuSkaicius(skliaustuVidus, simbolis);
            junginys = junginys.Substring(galas);
            skaicius = GautiSkaiciu(junginys);
            elementoKiekis += skaiciusV * skaicius;
        }
        else
        {
            junginys = junginys.Substring(zvaigzdute + 1);
            skaicius = GautiSkaiciu(junginys);
            elementoKiekis = skaicius * ElementuSkaicius(junginys, simbolis);
            break;
        }
    }
    return elementoKiekis;
}
//is teksto nufiltruoja ten esanti skaiciu (koeficienta prie elemento ar skliaustu chemineje formuleje)
int GautiSkaiciu(string tekstas, int vieta=0)
{
    int i = 0;
    if(vieta == tekstas.Length)
    { return 1; }
    while (char.IsDigit(tekstas, i + vieta))
    {
        if (vieta+i+1 >= tekstas.Length)
        { i++; break; }
        i++;
    }
    if (i == 0)
    { return 1; }
    return Int32.Parse(tekstas.Substring(vieta, i));
}
// suranda skliaustu gala, kurie uzdaro atidarytus skliaustus
int uzdaromiSkliaustai(string tekstas, int vieta=0)
{
    int skliaustai = 1;
    char[] charai = tekstas.ToCharArray();
    do
    {
        vieta++;
        if (charai[vieta] == ')')
        { skliaustai--; }
        else if (charai[vieta] == '(')
        { skliaustai++; }
    }
    while (skliaustai > 0 || vieta++ >= tekstas.Length);
    if(skliaustai != 0)
    { throw new Exception("ss"); }
    return vieta;
}
int solveGauss(ref int[,] matrica, in int[] koeficientai, bool pirmasKartas=true)
{
    //perkilnojame eilutes
    int i = 0;
    int pask = matrica.GetLength(0)-1;
    while(i < pask)
    {
        if(matrica[i,0] == 0)
        { SukeistiEilutes(ref matrica, i, pask); pask--; }
        i++;
    }
    //suvienodiname koeficientus
    int bendrasKartotinis = 1;
    for(int j = 0; j < matrica.GetLength(0); j++)
    {
        if(matrica[j,0] != 0)
        { bendrasKartotinis *= Math.Abs(matrica[j,0]); }
    }
    for (int j = 0; j < matrica.GetLength(0); j++)
    {
        if (matrica[j, 0] != 0)
        {
            int daugiklis = bendrasKartotinis/Math.Abs(matrica[j, 0]);
            if((j != 0 && matrica[j,0] > 0) || (j == 0 && matrica[j, 0] < 0)) { daugiklis *= -1; }
                padaugintiEilute(ref matrica,j,daugiklis);
        }
    }
    //sudedame eilutes
    for(int j=1; j< matrica.GetLength(0);j++)
    {
        if (matrica[j, 0] != 0)
        {
            SudetiEilutes(ref matrica, 0, j);
        }
    }
    suprastintimatrica(ref matrica);
    if (matrica.GetLength(1) == 2)
    {
        if (matrica[0, 0]*matrica[0, 1] >= 0)
            return -1;
        if(matrica.GetLength(1) > 1)
        {
            for(int j=1; j<matrica.GetLength(0); j++)
            {
                for(int k=0; k<matrica.GetLength(1); k++)
                {
                    if (matrica[j, k] != 0)
                        return -1;
                }
            }
        }
        koeficientai[koeficientai.Length - 1] = Math.Abs(matrica[0, 0]);
        koeficientai[koeficientai.Length-2] = Math.Abs(matrica[0, 1]);
    }
    else
    {
        int[,] naujaMatrica = new int[matrica.GetLength(0) - 1, matrica.GetLength(1) - 1];
        for (int x = 1; x < matrica.GetLength(0); x++)
        {
            for (int y = 1; y < matrica.GetLength(1); y++)
            {
                naujaMatrica[x-1, y-1] = matrica[x, y];
            }
        }
        if (solveGauss(ref naujaMatrica, koeficientai, false) == -1)
            return -1;
        //perkeliame matricos vidu i senja matrica
        for(int x=1; x< matrica.GetLength(0); x++)
        {
            for (int y = 1; y < matrica.GetLength(1); y++)
            {
                matrica[x, y] = naujaMatrica[x-1,y-1];
            }
        }
        //pridedame viena koeficienta naudodami komplikuota matematika
        int koef=0;
        for (int y = 1; y < matrica.GetLength(1); y++)
        {
            koef += matrica[0, y]*koeficientai[koeficientai.Length - matrica.GetLength(1) + y];
            koeficientai[koeficientai.Length - matrica.GetLength(1) + y] *= matrica[0,0];
        }
        koeficientai[koeficientai.Length - matrica.GetLength(1)] = Math.Abs(koef);
        // koef skaiciavimuose butinai turi gautis neigiamas, kitu atveju reakcijos lygtis neteisinga
        if (koef >= 0)
            return -1;
        int daliklis = DBD(koeficientai);
        if (daliklis != 1)
        {
            for (int j = 0; j < koeficientai.Length; j++)
            {
                koeficientai[j] /= daliklis;
            }
        }
    }
    if (pirmasKartas)
    {
        int mPlotis = matrica.GetLength(1); //junginiu skaicius
        if (mPlotis - 1 < matrica.GetLength(0))
        {
            if (matrica[mPlotis - 1, mPlotis - 1] != 0 && matrica[mPlotis - 1, mPlotis - 2] != 0)
                //reakcijos lygtis teisinga, sprendiniu yra daugiau nei vienas
                return -2;
        }
    }
    //reakcijos lygtis teisinga, sprendinys gautas
    return 0;
}
//prideda pirma eil prie eil2 matricos eilutes
void SudetiEilutes(ref int[,] matrica, int eil1, int eil2)
{
    for(int i=0; i<matrica.GetLength(1); i++)
    { matrica[eil2, i] += matrica[eil1, i]; }
}
void SukeistiEilutes(ref int[,] matrica, int eil1, int eil2)
{
    int laikinas;
    for(int i=0;i<matrica.GetLength(1);i++)
    {
        laikinas = matrica[eil1,i];
        matrica[eil1,i] = matrica[eil2,i];
        matrica[eil2, i] = laikinas;
    }
}
void padaugintiEilute(ref int[,] matrica, int eil, int daugiklis)
{
    for(int i=0; i<matrica.GetLength(1); i++)
    { matrica[eil, i] *= daugiklis; }
}
void suprastintimatrica(ref int[,] matrica)
{
    for (int i = 0; i<matrica.GetLength(0); i++)
    {
        int[] eilute = new int[matrica.GetLength(1)];
        for (int j = 0; j < matrica.GetLength(1); j++)
        {
            eilute[j] = matrica[i, j];
        }
        int daliklis = DBD(eilute);
        for (int j = 0; j < matrica.GetLength(1); j++)
        {
            matrica[i,j] /= daliklis;
        }
    }
}
int DBD(int[] vektorius)
{
    int minReiksme = Math.Abs(vektorius[0]);
    int daliklis = 1; ;
    //suranda maziausia nenuline reiksme
    for(int i=1; i<vektorius.Length; i++)
    {
        if ((minReiksme == 0 || Math.Abs(vektorius[i]) < minReiksme) && vektorius[i] != 0)
            minReiksme = Math.Abs(vektorius[i]);
    }
    //bando dalinti visa eilute
    for (int i = 2; i <= minReiksme; i++)
    {
        bool dalinasi = true;
        for (int j = 0; j < vektorius.Length; j++)
        {
            if(vektorius[j] % i != 0)
            { dalinasi = false; break; }
        }
        if (dalinasi == true)
        {
            daliklis = i;
        }
    
    }
    return daliklis;
}