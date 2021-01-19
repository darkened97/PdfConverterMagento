using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Globalization;

namespace PdfConverter_WF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            System.Configuration.SettingsProperty property = new System.Configuration.SettingsProperty("CustomSetting");
            InitializeComponent();
            String path = Settings1.Default.CalePdf;
            String path2 = Settings1.Default.CaleXml;
            MonitorDirectory(path);
            MonitorDirectory2(path2);



        }

        async static void GetRequest(string Uri)
        {

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage resp = await client.GetAsync(Uri))
                {
                    using (HttpContent content = resp.Content)
                    {
                        string cont = await content.ReadAsStringAsync();
                        Console.WriteLine(cont);
                    }
                }
            }
        }

        async static void PostRequest(string Uri, string jsonObject)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                var byteArray = Encoding.ASCII.GetBytes("apiUsr:Zaq14wsx!@");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage resp = await client.PostAsync(Uri, content))
                {
                    //using (HttpContent conte = resp.Content)
                    //{
                    //    string mycontent = await conte.ReadAsStringAsync();

                    //}
                }
            }
        }

        public void MonitorDirectory(string path)
        {

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

            fileSystemWatcher.Path = path;

            fileSystemWatcher.Created += FileSystemWatcher_Created;

            fileSystemWatcher.EnableRaisingEvents = true;

        }
        public void MonitorDirectory2(string path2)
        {

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

            fileSystemWatcher.Path = path2;

            fileSystemWatcher.Created += FileSystemWatcher_Created2;

            fileSystemWatcher.EnableRaisingEvents = true;

        }

        class Global
        {
            public static bool paused = false;
        }
        public void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Conversie();
        }
        public void FileSystemWatcher_Created2(object sender, FileSystemEventArgs e)
        {
            CitireXml_UseClass();//CitireXml();
        }
        public void Conversie()
        {
            Global.paused = true;
            string cale = Settings1.Default.CalePdf;
            string calexml = Settings1.Default.CaleXml;
            string caleexe = Settings1.Default.CaleExec;

            try
            {
                DirectoryInfo d = new DirectoryInfo(cale);
                FileInfo[] infos = d.GetFiles();
                foreach (FileInfo f in infos)
                {
                    File.Move(f.FullName, f.FullName.ToString().Replace(" ", "_"));
                }
                string[] fisiere = Directory.GetFiles(cale);
                foreach (string fisier in fisiere)
                {
                    FileInfo fis = new FileInfo(fisier);
                    string myDataTimeSTR = string.Format("{0:dd/MM/yyyy}", fis.LastWriteTime);

                    //int sw= Convert.ToInt32(Program.Run)
                    int sw = 0;
                    if (sw == 0)
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(caleexe + "pdftohtml.exe", "-xml " + fisier + " " + calexml + functieCale(fisier).Replace(".pdf", ""));
                        startInfo.RedirectStandardOutput = true;
                        startInfo.UseShellExecute = false;
                        startInfo.CreateNoWindow = true;
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo = startInfo;
                        process.Start();
                        Global.paused = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        String numeToLower = null;
        String functieCale(String path)
        {

            String nume = null;
            for (int i = path.Length - 1; i > 0; i--)
            {
                if (path[i] == '\\')
                    break;
                nume = nume + path[i];
            }
            numeToLower = nume.ToLower();
            char[] charArray = numeToLower.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);

        }

        public void CitireXml()
        {
            if (Global.paused == true)
            {
                Task.Delay(100).ContinueWith(t => CitireXml());
            }
            else
            {
                String cale = Settings1.Default.CaleXml;
                DirectoryInfo d = new DirectoryInfo(cale);
                FileInfo[] infos = d.GetFiles();
                foreach (FileInfo f in infos)
                {
                    XmlTextReader xtr = new XmlTextReader(Settings1.Default.CaleXml + f.Name);
                    XmlTextReader xtr2 = new XmlTextReader(Settings1.Default.CaleXml + f.Name);
                    XmlTextReader xtr3 = new XmlTextReader(Settings1.Default.CaleXml + f.Name);
                    string serviceUri = Settings1.Default.ApiUrl;
                    string s1 = "";
                    string s2 = "";
                    string s3 = "";
                    string existaScadenta = "";
                    decimal s2dec = 0;

                    while (xtr.Read())
                    {
                        if (xtr.IsStartElement())
                        {
                            if (xtr.IsEmptyElement)
                            {

                            }
                            else
                            {
                                xtr.Read();
                                if (xtr.Name == "b")
                                {

                                    string s0 = xtr.ReadElementContentAsString();
                                    if (s0 == "CUMPARATOR")
                                    {

                                        xtr.Read();
                                        xtr.Read();
                                        s1 = xtr.ReadElementContentAsString();
                                    }
                                    else if (s0 == "BILLING ADDRESS")
                                    {
                                        xtr.Read();
                                        xtr.Read();
                                        xtr.Read();
                                        xtr.Read();
                                        xtr.Read();
                                        xtr.Read();
                                        xtr.Read();
                                        xtr.Read();
                                        xtr.Read();
                                        s1 = xtr.ReadElementContentAsString();
                                    }
                                    else if (s0 == "GRAND TOTAL")
                                    {
                                        xtr2.Read();
                                        xtr2.Read();
                                        xtr2.Read();
                                        s3 = xtr2.ReadElementContentAsString();
                                        s3 = s3.Trim();
                                        s3 = s3.Trim(new char[] { 'R', 'O', 'N' });
                                    }
                                    else if (s0 == "Date:")
                                    {
                                        xtr3.Read();
                                        s3 = xtr3.ReadElementContentAsString();
                                        s3 = s3.Trim();
                                    }
                                    else if (s0 == "Data facturii:")
                                    {
                                        xtr3.Read();
                                        s3 = xtr3.ReadElementContentAsString();
                                        s3 = s3.Trim();

                                    }
                                    else if (s0 == "Data scadentei:")
                                    {
                                        xtr3.Read();
                                        existaScadenta = xtr3.ReadElementContentAsString();
                                    }
                                }
                                if (xtr.Name == "text")
                                {
                                    string s0 = xtr2.ReadElementContentAsString();

                                    if (s0 == "Total de plata")
                                    {
                                        xtr2.Read();
                                        s2 = xtr2.ReadElementContentAsString();
                                        s2 = s2.Trim();
                                        s2 = s2.Trim(new char[] { 'R', 'O', 'N' });
                                        s2 = s2.Replace(".", "");
                                        s2 = s2.Replace(",", ".");

                                    }
                                }


                            }

                        }
                    }
                    #region comment
                    //while (xtr2.Read())
                    //{
                    //    if (xtr2.IsStartElement())
                    //    {
                    //        if (xtr2.IsEmptyElement)
                    //        {

                    //        }
                    //        else
                    //        {
                    //            if (xtr2.Name == "text" && xtr2.GetAttribute("width").ToString() == "89" && xtr2.GetAttribute("height").ToString() == "17")
                    //            {
                    //                string s0 = xtr2.ReadElementContentAsString();

                    //                if (s0 == "Total de plata")  
                    //                {
                    //                    xtr2.Read();
                    //                    s2 = xtr2.ReadElementContentAsString();
                    //                    s2 = s2.Trim();
                    //                    s2 = s2.Trim(new char[] { 'R', 'O', 'N' });
                    //                    s2 = s2.Replace(".", "");
                    //                    s2 = s2.Replace(",", ".");

                    //                }
                    //            }
                    //            else if (xtr2.Name == "b")
                    //            {
                    //                string s0 = xtr2.ReadElementContentAsString();
                    //                if (s0 == "GRAND TOTAL")
                    //                {
                    //                    xtr2.Read();
                    //                    xtr2.Read();
                    //                    xtr2.Read();
                    //                    s3 = xtr2.ReadElementContentAsString();
                    //                    s3 = s3.Trim();
                    //                    s3 = s3.Trim(new char[] { 'R', 'O', 'N' });
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //while (xtr3.Read())
                    //{
                    //    if (xtr3.IsStartElement())
                    //    {
                    //        if (xtr3.IsEmptyElement)
                    //        {

                    //        }
                    //        else
                    //        {
                    //            if (xtr3.Name == "text")
                    //            {
                    //                string s0 = xtr3.ReadElementContentAsString();

                    //                if (s0 == "Data facturii:") 
                    //                {
                    //                    xtr3.Read();
                    //                    s3 = xtr3.ReadElementContentAsString();
                    //                    s3 = s3.Trim();

                    //                }
                    //                else if(s0 == "Date:")
                    //                {
                    //                    xtr3.Read();
                    //                    s3 = xtr3.ReadElementContentAsString();
                    //                    s3 = s3.Trim();
                    //                }
                    //                if(s0=="Data scadentei:")
                    //                {
                    //                    xtr3.Read();
                    //                    existaScadenta = xtr3.ReadElementContentAsString();
                    //                }

                    //            }
                    //        }
                    //    }
                    //}
                    //if (existaScadenta == "")
                    //{
                    //    existaScadenta = s3;
                    //}
                    #endregion comment

                    if (s1 != "" && s2 != "")
                    {
                        DOC_Import doc = new DOC_Import
                        {
                            StructuraCod = "SC", // codul ce indica punctul de lucru de unde s-a introdus documentul, se completeaza "SC"
                            TipCod = "FCTVA", //indica tipul de document ce va fi generat in sistem, se completeaza "FCTVA"
                            Numar = DateTime.Today.Date.Day.ToString()
                                    + "_" + DateTime.Now.Hour.ToString()
                                    + "_" + DateTime.Now.Minute.ToString()
                                    + "_" + DateTime.Now.Second.ToString(), // Numar document
                            Data = DateTime.Today, // Data documentului
                            PartenerCUI = "13264420",
                            PartenerNume = s1,
                            ValutaSimbol = "RON",
                            Curs = 1,// Daca valuta <> RON atunci, cursul <>1
                            TVA = 19, // cota de TVA a documentului, dar fiecare linie are de asemenea propria cota de TVA
                            Explicatie = "Autogenerat", //explicatia ce apare in partea de jos a facturii (ERA)

                        };
                        DOC_ImportDetalii impd = new DOC_ImportDetalii
                        {
                            ProdusCod = "6.8270.1215",
                            ProdusNume = "BURETE POLISH ALB 150 x 12 mm 4CR",
                            Custom = "Explicatia - apare pe factura, in dreptul produsului",
                            UnitateCod = "BUC",//se poate consulta nomenclatorul de unitati de masura disponibil la adresa ../PRO_Unitati
                            Cantitate = Convert.ToDecimal(5), // Numar de unitati
                            PretUnitar = s2dec, // Pret pentru o unitate
                            TVALinie = Convert.ToDecimal(19),//Cota TVA %
                            ValoareAdaosLinie = Convert.ToDecimal(-9.1),//Valoare Absoluta Discount
                            AdaosLinie = Convert.ToDecimal(-5), // Procent Adaos cu Minus = Discount 5%
                            ValoareLinie = Convert.ToDecimal(172.9),//Valoare cu Discount, (36.4 * 5) -(36.4 * 5) * 5% = 172.9
                            ValoareTVALinie = Convert.ToDecimal(32.85),//Valoare TVA, dupa aplicare discount                
                            GarantieLuni = Convert.ToInt16(12)// Numar de luni garantie produs, daca este cazul
                        };
                        doc.DOC_ImportDetalii.Add(impd);

                        string json = JsonConvert.SerializeObject(doc);
                        PostRequest(serviceUri, json);
                    }
                    GC.Collect();
                    xtr.Close();
                    xtr2.Close();

                }

                string pdfstart = Settings1.Default.CalePdf;
                string start = Settings1.Default.CaleXml;
                string destinatie = Settings1.Default.CaleExec + "Istoric";
                DirectoryInfo dirs = new DirectoryInfo(start);
                DirectoryInfo dird = new DirectoryInfo(destinatie);
                if (dirs.Exists == false)
                    Directory.CreateDirectory(start);
                if (dird.Exists == false)
                    Directory.CreateDirectory(destinatie);
                if (dirs.Exists == false)
                    Directory.CreateDirectory(pdfstart);
                List<String> XML = Directory.GetFiles(start, "*.*", SearchOption.TopDirectoryOnly).ToList();
                List<String> XMLpdf = Directory.GetFiles(pdfstart, "*.*", SearchOption.TopDirectoryOnly).ToList();
                foreach (string file in XML)
                {
                    FileInfo mFile = new FileInfo(file);
                    if (new FileInfo(dird + "\\" + mFile.Name).Exists == false)
                    {
                        mFile.MoveTo(dird + "\\" + mFile.Name);
                    }
                }
                foreach (string file in XMLpdf)
                {
                    FileInfo mFile = new FileInfo(file);
                    if (new FileInfo(dird + "\\" + mFile.Name).Exists == false)
                    {
                        mFile.MoveTo(dird + "\\" + mFile.Name);
                    }
                }
            }
        }
        /// <summary>
        /// adauga DOC_ImportDetalii la un document existent, prelucrand parametrii primiti
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="produsNume">numele produsului</param>
        /// <param name="valoriConcat">valori concatenate</param>
        private void DodImportDetaliiAppend(DOC_Import doc, string produsNume, string valoriConcat)
        {
            DOC_ImportDetalii impd = new DOC_ImportDetalii();
            #region valori implicite
            impd.Cantitate = 0;
            impd.PretUnitar = 0;
            impd.TVALinie = 0;
            impd.AdaosLinie = 0;
            impd.ValoareAdaosLinie = 0;
            impd.ValoareLinie = 0;
            impd.ValoareTVALinie = 0;
            impd.GarantieLuni = 12;
            #endregion valori implicite

            produsNume = produsNume.Trim();
            string tempNrCrt = produsNume.Substring(0, produsNume.IndexOf(' '));

            int rezNrCrt = 0;
            if (Int32.TryParse(tempNrCrt, out rezNrCrt) == true)//deci ce s-a extras este intreg
            {
                string tempProdusFaraNrCrt = produsNume.Substring(produsNume.IndexOf(' ') + 1, produsNume.Length - produsNume.IndexOf(' ') - 1);
                produsNume = tempProdusFaraNrCrt;
                impd.Custom = tempNrCrt;
            }
            impd.ProdusNume = produsNume;
            impd.ProdusCod = "";

            string[] vals = valoriConcat.Replace("  ", " ").Trim().Split(' ');//a)acolo unde apare 2 spatii se inlocuieste cu unul singur| b)separate prin spatiu
            if (vals.Length >= 10) //cheltuieli de transport are 10 substringuri
            {
                var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
                ci.NumberFormat.NumberDecimalSeparator = ","; //indica separatorul de zecimale

                for (int j = 0; j < vals.Length; j++)
                {
                    switch (j)
                    {
                        case 0:
                            impd.UnitateCod = vals[j];
                            break;
                        case 1:
                            impd.Cantitate = decimal.Parse(vals[j], ci);
                            break;
                        case 2://pret unitar
                            impd.PretUnitar = decimal.Parse(vals[j], ci);
                            break;
                        case 3:
                            //RON
                            break;
                        case 4: // Cota TVA
                            impd.TVALinie = decimal.Parse(vals[j].Replace("%", ""), ci);
                            break;
                        case 5://Valoare
                            impd.ValoareLinie = decimal.Parse(vals[j], ci);
                            break;
                        case 6://RON
                            break;
                        case 7://ValoareTVA
                            impd.ValoareTVALinie = decimal.Parse(vals[j], ci);
                            break;
                        case 8://RON
                            break;
                        case 9://Total                            
                            break;
                        case 10://RON
                            break;
                    }
                }
            }

            doc.DOC_ImportDetalii.Add(impd);
        }

        private void DodImportDetaliiAppendStorno(DOC_Import doc, string produsNume, string valoriConcat)
        {
            DOC_ImportDetalii impd = new DOC_ImportDetalii();
            #region valori implicite
            impd.Cantitate = 0;
            impd.PretUnitar = 0;
            impd.TVALinie = 0;
            impd.AdaosLinie = 0;
            impd.ValoareAdaosLinie = 0;
            impd.ValoareLinie = 0;
            impd.ValoareTVALinie = 0;
            impd.GarantieLuni = 12;
            #endregion valori implicite

            produsNume = produsNume.Trim();
            string tempNrCrt = produsNume.Substring(0, produsNume.IndexOf(' '));

            int rezNrCrt = 0;
            if (Int32.TryParse(tempNrCrt, out rezNrCrt) == true)//deci ce s-a extras este intreg
            {
                string tempProdusFaraNrCrt = produsNume.Substring(produsNume.IndexOf(' ') + 1, produsNume.Length - produsNume.IndexOf(' ') - 1);
                produsNume = tempProdusFaraNrCrt;
                impd.Custom = tempNrCrt;
            }
            impd.ProdusNume = produsNume;
            impd.ProdusCod = "";

            string[] vals = valoriConcat.Replace("  ", " ").Trim().Split(' ');//a)acolo unde apare 2 spatii se inlocuieste cu unul singur| b)separate prin spatiu
            if (vals.Length >= 7) //cheltuieli de transport are 10 substringuri
            {
                var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
                ci.NumberFormat.NumberDecimalSeparator = ","; //indica separatorul de zecimale

                for (int j = 0; j < vals.Length; j++)
                {
                    switch (j)
                    {
                        case 0://cod produs
                            impd.ProdusCod = vals[j];
                            break;
                        case 1://pret unitar
                            impd.PretUnitar = decimal.Parse(vals[j], ci);
                            break;
                        case 2://RON
                            break;
                        case 3://cantitate
                            impd.Cantitate = decimal.Parse(vals[j], ci);
                            break;
                        case 4:
                            //tva linie
                            impd.TVALinie = decimal.Parse(vals[j].Replace("%", ""), ci);
                            break;
                        case 5://RON
                            break;
                        case 6: //valoare linie
                            impd.ValoareLinie = decimal.Parse(vals[j], ci);
                            break;
                        case 7://ron
                            break;
                            //case 6://RON
                            //    break;
                            //case 7://ValoareTVA
                            //    impd.ValoareTVALinie = decimal.Parse(vals[j], ci);
                            //    break;
                            //case 8://RON
                            //    break;
                            //case 9://Total                            
                            //    break;
                            //case 10://RON
                            //    break;
                    }
                }
            }

            doc.DOC_ImportDetalii.Add(impd);
        }

        public void CitireXml_UseClass()
        {
            if (Global.paused == true)
            {
                Task.Delay(100).ContinueWith(t => CitireXml_UseClass());
            }
            else
            {

                String cale = Settings1.Default.CaleXml;
                DirectoryInfo d = new DirectoryInfo(cale);
                FileInfo[] infos = d.GetFiles();
                foreach (FileInfo f in infos)
                {
                    if (f.Name.Contains("creditmemo"))
                    {
                        try
                        {
                            System.Xml.Serialization.XmlSerializer serializerStorno = new System.Xml.Serialization.XmlSerializer(typeof(PdfConverter_WF_Storno.pdf2xml));
                            PdfConverter_WF_Storno.pdf2xml contentXmlFileStorno = new PdfConverter_WF_Storno.pdf2xml();

                            TextReader txtReaderStorno = new StringReader(File.ReadAllText(Settings1.Default.CaleXml + f.Name));
                            contentXmlFileStorno = (PdfConverter_WF_Storno.pdf2xml)serializerStorno.Deserialize(txtReaderStorno);

                            #region variabile locale
                            int linieTopVal = 0;//indica coordonata Top aferenta randului ce trebuie citit
                            int linieLeftVal_Ant = 0;//indica coordonata Left a randului citit anterior
                            string linieProdusNume = "";//formeaza numele produsului de pe linie, chiar daca e pe mai multe linii;                    
                            string linieValori = "";// va concatena informatiile cu privire la randul citit (UM, Cantitate, PretUnitar, CotaTVA, Valoare, ValoareTVA, Total)
                            bool finalCitireLinii = false;//indica daca s-au citit toate liniile

                            #endregion variabile locale

                            DOC_Import doc = new DOC_Import();
                            doc.Id = -1;
                            doc.StructuraCod = Settings1.Default.DocStructuraCod;
                            doc.TipCod = Settings1.Default.DocTipCod;
                            doc.ValutaSimbol = "RON";//de revizuit
                            doc.Curs = 1;
                            doc.TVA = 19;
                            doc.Explicatie = "Autogenerat - deserializare pdf-xml";

                            foreach (PdfConverter_WF_Storno.pdf2xmlPage pag in contentXmlFileStorno.Items)
                            {
                                linieTopVal = 0;//resetare pt fiecare pag
                                linieLeftVal_Ant = 0;//resetare pt fiecare pag
                                if (linieProdusNume.Length > 0 && linieValori.Length > 0) //cand se trece la pag urmatoare, trebuie salvat si ultima linie
                                    DodImportDetaliiAppendStorno(doc, linieProdusNume, linieValori);

                                linieProdusNume = "";
                                linieValori = "";

                                for (int i = 0; i < pag.text.Length; i++) //foreach (pdf2xmlPageText pgTxt in pag.text)
                                {
                                    if (finalCitireLinii == false)//citeste doar daca nu s-a intalnit ultimul sir care marcheaza final zona linii, sir ="Semnatura si"
                                    {
                                        try
                                        {
                                            PdfConverter_WF_Storno.pdf2xmlPageText pgTxt = pag.text[i] as PdfConverter_WF_Storno.pdf2xmlPageText;
                                            #region Antet document
                                            if (pgTxt.b != null && pgTxt.b.Contains("Creditmemo Number:"))
                                            {
                                                if (pgTxt.Value != null)
                                                    doc.Numar = pgTxt.Value;
                                                continue;
                                            }
                                            if (pgTxt.b != null && pgTxt.b.Contains("Date:"))
                                            {
                                                if (pgTxt.Value != null)
                                                    doc.Data = Convert.ToDateTime(pgTxt.Value.Replace(":", "").Trim());
                                                continue;
                                            }
                                            if (pgTxt.b != null && pgTxt.b.Contains("VAT Number:"))
                                            {
                                                if (pgTxt.Value != null)
                                                    doc.PartenerCUI = pgTxt.Value.TrimStart();
                                                continue;
                                            }
                                            //if (pgTxt.b != null && pgTxt.b.Contains("Data scadentei"))
                                            //{
                                            //    if (pgTxt.Value != null)
                                            //        doc.Scadenta = Convert.ToDateTime(pgTxt.Value.Replace(":", "").Trim());
                                            //    continue;
                                            //}
                                            if (pgTxt.b != null && pgTxt.b.Contains("BILLING ADDRESS"))
                                            {
                                                if (pag.text.Length > i + 6)//se verifica ca nu depaseste numarul de linii asteptat
                                                {
                                                    PdfConverter_WF_Storno.pdf2xmlPageText pgTxt_ParNume = pag.text[i + 1] as PdfConverter_WF_Storno.pdf2xmlPageText;
                                                    doc.PartenerNume = pgTxt_ParNume.Value;
                                                    continue;
                                                }
                                            }
                                            #endregion Antet document

                                            #region determinare valoare coordonata TOP - indica linia de citit
                                            if (pgTxt.b != null && pgTxt.b.Contains("SUBTOTAL"))
                                            {//se extrage coordonata Top aferenta primului rand din pagina
                                                if (pag.text.Length > i + 1)
                                                {
                                                    PdfConverter_WF_Storno.pdf2xmlPageText pgTxt_NextLinie = pag.text[i + 2] as PdfConverter_WF_Storno.pdf2xmlPageText;
                                                    linieTopVal = Convert.ToInt32(pgTxt_NextLinie.top);
                                                }
                                            }
                                            else
                                            {
                                                if (linieTopVal > 0 //indica ca s-a trecut de indicatorul "SUBTOTAL"
                                                    && Convert.ToInt32(pgTxt.top) > linieTopVal && Convert.ToInt32(pgTxt.left) < linieLeftVal_Ant)
                                                {
                                                    linieTopVal = Convert.ToInt32(pgTxt.top);// NOUA LINIE!!
                                                    DodImportDetaliiAppendStorno(doc, linieProdusNume, linieValori);
                                                    linieProdusNume = "";
                                                    linieValori = "";
                                                }
                                            }
                                            #endregion determinare valoare coordonata TOP - linie de citit

                                            #region citire LINIE
                                            if (Convert.ToInt32(pgTxt.top) == linieTopVal)//indica randul
                                            {
                                                if (Convert.ToInt32(pgTxt.left) < linieLeftVal_Ant) //sunt la inceput de rand, gasesc produs Nume
                                                {
                                                    linieProdusNume += pgTxt.Value;
                                                }
                                                else //NU sunt la inceput de rand, sigur alte informatii (UM, Cantitate, PretUnitar, CotaTVA, Valoare, ValoareTVA, Total)
                                                {
                                                    if (pgTxt.Value != null)
                                                        linieValori += " " + pgTxt.Value;
                                                }
                                            }
                                            else
                                            {
                                                if (pag.text.Length > i + 1)
                                                {
                                                    PdfConverter_WF_Storno.pdf2xmlPageText pgTxt_NextLinie = pag.text[i + 1] as PdfConverter_WF_Storno.pdf2xmlPageText;
                                                    //Daca urmatoarea pozitie are top = linieTopVal sau left = left curent => textul reprezinta continuare denumire produs
                                                    if (linieTopVal > 0
                                                        && (Convert.ToInt32(pgTxt_NextLinie.top) == linieTopVal || Convert.ToInt32(pgTxt_NextLinie.left) == Convert.ToInt32(pgTxt.left)))
                                                    {
                                                        if (pgTxt.Value != null)
                                                            linieProdusNume += " " + pgTxt.Value.Trim();
                                                    }
                                                }
                                            }
                                            #endregion citire LINIE

                                            ////
                                            linieLeftVal_Ant = Convert.ToInt32(pgTxt.left);

                                            if (pgTxt.Value != null && pgTxt.Value.Contains("Term:"))
                                            {
                                                finalCitireLinii = true;
                                                continue;
                                            }

                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                            }
                            #region expediere fisier catre API
                            string serviceUri = Settings1.Default.ApiUrl;
                            JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                            jsonSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                            jsonSettings.Culture = CultureInfo.InvariantCulture;
                            jsonSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                            string json = JsonConvert.SerializeObject(doc, jsonSettings);
                            PostRequest(serviceUri, json);
                            #endregion expediere fisier catre API

                            #region mutare fisiere in istoric
                            string pdfstart = Settings1.Default.CalePdf;
                            string start = Settings1.Default.CaleXml;
                            string destinatie = Settings1.Default.CaleExec + "Istoric";
                            DirectoryInfo dirs = new DirectoryInfo(start);
                            DirectoryInfo dird = new DirectoryInfo(destinatie);
                            if (dirs.Exists == false)
                                Directory.CreateDirectory(start);
                            if (dird.Exists == false)
                                Directory.CreateDirectory(destinatie);
                            if (dirs.Exists == false)
                                Directory.CreateDirectory(pdfstart);
                            List<String> XML = Directory.GetFiles(start, "*.*", SearchOption.TopDirectoryOnly).ToList();
                            List<String> XMLpdf = Directory.GetFiles(pdfstart, "*.*", SearchOption.TopDirectoryOnly).ToList();
                            foreach (string file in XML)
                            {
                                FileInfo mFile = new FileInfo(file);
                                string newFileName = mFile.Name.Replace(".xml", "") + (DateTime.Today.ToShortDateString()).Replace("/", "-")
                                                        + "_" + (DateTime.Now.ToLongTimeString()).Replace(":", "-")
                                                        + ".xml"; //daca exista in istoric fisierul cu acelasi nume se redenumeste si se muta

                                if (new FileInfo(dird + "\\" + newFileName).Exists == true)//daca destinatia contine deja fisierul cu noua denumire (ceea ce e putin probabil)
                                    mFile.Delete();
                                else
                                    mFile.MoveTo(dird + "\\" + newFileName);
                            }
                            foreach (string file in XMLpdf)
                            {
                                FileInfo mFile = new FileInfo(file);
                                string newFileName = mFile.Name.Replace(".pdf", "") + (DateTime.Today.ToShortDateString()).Replace("/", "-")
                                                        + "_" + (DateTime.Now.ToLongTimeString()).Replace(":", "-")
                                                        + ".pdf"; //daca exista in istoric fisierul cu acelasi nume se redenumeste si se muta

                                if (new FileInfo(dird + "\\" + newFileName).Exists == true)//daca destinatia contine deja fisierul cu noua denumire (ceea ce e putin probabil)
                                    mFile.Delete();
                                else
                                    mFile.MoveTo(dird + "\\" + newFileName);
                            }
                            #endregion mutare fisiere in istoric

                            string good = "Fisierul cu numele:" + f.Name + "a fost incarcat cu succes:";
                            string sMonth = DateTime.Now.ToString("yyyy_MM");
                            string numeLog = "log" + sMonth + ".txt";
                            string logpath = Settings1.Default.CaleExec + "//Log//" + numeLog;
                            //LOG
                            using (StreamWriter w = File.AppendText(logpath))
                            {
                                Log(good, w);
                            }

                        }
                        catch (Exception ex)
                        {
                            string fail = "Incarcarea a esuat:";
                            fail = fail + ex.Message;
                            string sMonth = DateTime.Now.ToString("yyyy_MM");
                            string numeLog = "log" + sMonth + ".txt";
                            string logpath = Settings1.Default.CaleExec + "//Log//" + numeLog;
                            //LOG
                            using (StreamWriter w = File.AppendText(logpath))
                            {
                                Log(fail, w);
                            }

                        }
                    }
                    else if (f.Name.Contains("factura"))
                    {

                        try
                        {
                            #region pentru fiecare fisier de tip xml din folder aferent
                            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(pdf2xml));
                            pdf2xml contentXmlFile = new pdf2xml();

                            TextReader txtReader = new StringReader(File.ReadAllText(Settings1.Default.CaleXml + f.Name));
                            contentXmlFile = (pdf2xml)serializer.Deserialize(txtReader);

                            #region variabile locale
                            int linieTopVal = 0;//indica coordonata Top aferenta randului ce trebuie citit
                            int linieLeftVal_Ant = 0;//indica coordonata Left a randului citit anterior
                            string linieProdusNume = "";//formeaza numele produsului de pe linie, chiar daca e pe mai multe linii;                    
                            string linieValori = "";// va concatena informatiile cu privire la randul citit (UM, Cantitate, PretUnitar, CotaTVA, Valoare, ValoareTVA, Total)
                            bool finalCitireLinii = false;//indica daca s-au citit toate liniile

                            #endregion variabile locale

                            DOC_Import doc = new DOC_Import();
                            doc.Id = -1;
                            doc.StructuraCod = Settings1.Default.DocStructuraCod;
                            doc.TipCod = Settings1.Default.DocTipCod;
                            doc.ValutaSimbol = "RON";//de revizuit
                            doc.Curs = 1;
                            doc.TVA = 19;
                            doc.Explicatie = "Autogenerat - deserializare pdf-xml";

                            foreach (pdf2xmlPage pag in contentXmlFile.Items)
                            {
                                linieTopVal = 0;//resetare pt fiecare pag
                                linieLeftVal_Ant = 0;//resetare pt fiecare pag
                                if (linieProdusNume.Length > 0 && linieValori.Length > 0) //cand se trece la pag urmatoare, trebuie salvat si ultima linie
                                    DodImportDetaliiAppend(doc, linieProdusNume, linieValori);

                                linieProdusNume = "";
                                linieValori = "";

                                for (int i = 0; i < pag.text.Length; i++) //foreach (pdf2xmlPageText pgTxt in pag.text)
                                {
                                    if (finalCitireLinii == false)//citeste doar daca nu s-a intalnit ultimul sir care marcheaza final zona linii, sir ="Semnatura si"
                                    {
                                        try
                                        {
                                            pdf2xmlPageText pgTxt = pag.text[i] as pdf2xmlPageText;
                                            #region Antet document
                                            if (pgTxt.b != null && pgTxt.b.Contains("Seria si nr."))
                                            {
                                                if (pgTxt.Value != null)
                                                    doc.Numar = pgTxt.Value;
                                                continue;
                                            }
                                            if (pgTxt.b != null && pgTxt.b.Contains("Data facturii"))
                                            {
                                                if (pgTxt.Value != null)
                                                    doc.Data = Convert.ToDateTime(pgTxt.Value.Replace(":", "").Trim());
                                                continue;
                                            }
                                            if (pgTxt.b != null && pgTxt.b.Contains("Data scadentei"))
                                            {
                                                if (pgTxt.Value != null)
                                                    doc.Scadenta = Convert.ToDateTime(pgTxt.Value.Replace(":", "").Trim());
                                                continue;
                                            }
                                            if (pgTxt.b != null && pgTxt.b.Contains("CUMPARATOR"))
                                            {
                                                if (pag.text.Length > i + 6)//se verifica ca nu depaseste numarul de linii asteptat
                                                {
                                                    pdf2xmlPageText pgTxt_ParNume = pag.text[i + 1] as pdf2xmlPageText;
                                                    doc.PartenerNume = pgTxt_ParNume.Value;
                                                    pdf2xmlPageText pgTxt_ParCUI = pag.text[i + 6] as pdf2xmlPageText;
                                                    doc.PartenerCUI = pgTxt_ParCUI.Value;
                                                    continue;
                                                }
                                            }
                                            #endregion Antet document

                                            #region determinare valoare coordonata TOP - indica linia de citit
                                            if (pgTxt.b != null && pgTxt.b.Contains("CU TVA"))
                                            {//se extrage coordonata Top aferenta primului rand din pagina
                                                if (pag.text.Length > i + 1)
                                                {
                                                    pdf2xmlPageText pgTxt_NextLinie = pag.text[i + 1] as pdf2xmlPageText;
                                                    linieTopVal = Convert.ToInt32(pgTxt_NextLinie.top);
                                                }
                                            }
                                            else
                                            {
                                                if (linieTopVal > 0 //indica ca s-a trecut de indicatorul "CU TVA"
                                                    && Convert.ToInt32(pgTxt.top) > linieTopVal && Convert.ToInt32(pgTxt.left) < linieLeftVal_Ant)
                                                {
                                                    linieTopVal = Convert.ToInt32(pgTxt.top);// NOUA LINIE!!
                                                    DodImportDetaliiAppend(doc, linieProdusNume, linieValori);
                                                    linieProdusNume = "";
                                                    linieValori = "";
                                                }
                                            }
                                            #endregion determinare valoare coordonata TOP - linie de citit

                                            #region citire LINIE
                                            if (Convert.ToInt32(pgTxt.top) == linieTopVal)//indica randul
                                            {
                                                if (Convert.ToInt32(pgTxt.left) < linieLeftVal_Ant) //sunt la inceput de rand, gasesc produs Nume
                                                {
                                                    linieProdusNume += pgTxt.Value;
                                                }
                                                else //NU sunt la inceput de rand, sigur alte informatii (UM, Cantitate, PretUnitar, CotaTVA, Valoare, ValoareTVA, Total)
                                                {
                                                    if (pgTxt.Value != null)
                                                        linieValori += " " + pgTxt.Value;
                                                }
                                            }
                                            else
                                            {
                                                if (pag.text.Length > i + 1)
                                                {
                                                    pdf2xmlPageText pgTxt_NextLinie = pag.text[i + 1] as pdf2xmlPageText;
                                                    //Daca urmatoarea pozitie are top = linieTopVal sau left = left curent => textul reprezinta continuare denumire produs
                                                    if (linieTopVal > 0
                                                        && (Convert.ToInt32(pgTxt_NextLinie.top) == linieTopVal || Convert.ToInt32(pgTxt_NextLinie.left) == Convert.ToInt32(pgTxt.left)))
                                                    {
                                                        if (pgTxt.Value != null)
                                                            linieProdusNume += " " + pgTxt.Value.Trim();
                                                    }
                                                }
                                            }
                                            #endregion citire LINIE

                                            ////
                                            linieLeftVal_Ant = Convert.ToInt32(pgTxt.left);

                                            if (pgTxt.Value != null && pgTxt.Value.Contains("Semnatura si"))
                                            {
                                                finalCitireLinii = true;
                                                continue;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //LOG
                                        }
                                    }
                                }
                            }

                            #endregion pentru fiecare fisier de tip xml din folder aferent


                            #region expediere fisier catre API
                            string serviceUri = Settings1.Default.ApiUrl;
                            JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                            jsonSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                            jsonSettings.Culture = CultureInfo.InvariantCulture;
                            jsonSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                            string json = JsonConvert.SerializeObject(doc, jsonSettings);
                            PostRequest(serviceUri, json);
                            #endregion expediere fisier catre API

                            #region mutare fisiere in istoric
                            string pdfstart = Settings1.Default.CalePdf;
                            string start = Settings1.Default.CaleXml;
                            string destinatie = Settings1.Default.CaleExec + "Istoric";
                            DirectoryInfo dirs = new DirectoryInfo(start);
                            DirectoryInfo dird = new DirectoryInfo(destinatie);
                            if (dirs.Exists == false)
                                Directory.CreateDirectory(start);
                            if (dird.Exists == false)
                                Directory.CreateDirectory(destinatie);
                            if (dirs.Exists == false)
                                Directory.CreateDirectory(pdfstart);
                            List<String> XML = Directory.GetFiles(start, "*.*", SearchOption.TopDirectoryOnly).ToList();
                            List<String> XMLpdf = Directory.GetFiles(pdfstart, "*.*", SearchOption.TopDirectoryOnly).ToList();
                            foreach (string file in XML)
                            {
                                FileInfo mFile = new FileInfo(file);
                                string newFileName = mFile.Name.Replace(".xml", "") + (DateTime.Today.ToShortDateString()).Replace("/", "-")
                                                        + "_" + (DateTime.Now.ToLongTimeString()).Replace(":", "-")
                                                        + ".xml"; //daca exista in istoric fisierul cu acelasi nume se redenumeste si se muta

                                if (new FileInfo(dird + "\\" + newFileName).Exists == true)//daca destinatia contine deja fisierul cu noua denumire (ceea ce e putin probabil)
                                    mFile.Delete();
                                else
                                    mFile.MoveTo(dird + "\\" + newFileName);
                            }
                            foreach (string file in XMLpdf)
                            {
                                FileInfo mFile = new FileInfo(file);
                                string newFileName = mFile.Name.Replace(".pdf", "") + (DateTime.Today.ToShortDateString()).Replace("/", "-")
                                                        + "_" + (DateTime.Now.ToLongTimeString()).Replace(":", "-")
                                                        + ".pdf"; //daca exista in istoric fisierul cu acelasi nume se redenumeste si se muta

                                if (new FileInfo(dird + "\\" + newFileName).Exists == true)//daca destinatia contine deja fisierul cu noua denumire (ceea ce e putin probabil)
                                    mFile.Delete();
                                else
                                    mFile.MoveTo(dird + "\\" + newFileName);
                            }
                            #endregion mutare fisiere in istoric

                            string good = "Fisierul cu numele: " + f.Name + " a fost incarcat cu succes:";
                           
                            string sMonth = DateTime.Now.ToString("yyyy_MM");
                            string numeLog = "log"+sMonth+".txt";
                            string logpath = Settings1.Default.CaleExec + "//Log//" + numeLog;
                            //LOG
                            using (StreamWriter w = File.AppendText(logpath))
                            {
                                Log(good, w);
                            }
                        }
                        catch (Exception ex)
                        {
                            string fail = "Incarcarea a esuat: ";
                            fail = fail + ex.Message;
                            string sMonth = DateTime.Now.ToString("yyyy_MM");
                            string numeLog = "log" + sMonth + ".txt";
                            string logpath = Settings1.Default.CaleExec + "//Log//" + numeLog;
                            //LOG
                            using (StreamWriter w = File.AppendText(logpath))
                            {
                                Log(fail, w);
                            }
                        }

                    }
                    else if (f.Name.Contains("uber"))
                    {
                        #region Dictionar de luni
                        Dictionary<string, string> month =
                                new Dictionary<string, string>();
                        month.Add("ianuarie", "01");
                        month.Add("februarie", "02");
                        month.Add("martie", "03");
                        month.Add("aprilie", "04");
                        month.Add("mai", "05");
                        month.Add("iunie", "06");
                        month.Add("iulie", "07");
                        month.Add("august", "08");
                        month.Add("septembrie", "09");
                        month.Add("octombrie", "10");
                        month.Add("noiembrie", "11");
                        month.Add("decembrie", "12");

                        month.Add("Ianuarie", "01");
                        month.Add("Februarie", "02");
                        month.Add("Martie", "03");
                        month.Add("Aprilie", "04");
                        month.Add("Mai", "05");
                        month.Add("Iunie", "06");
                        month.Add("Iulie", "07");
                        month.Add("August", "08");
                        month.Add("Septembrie", "09");
                        month.Add("Octombrie", "10");
                        month.Add("Noiembrie", "11");
                        month.Add("Decembrie", "12");

                        #endregion

                        try
                        {
                            #region pentru fiecare fisier de tip xml din folder aferent
                            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(pdf2xml));
                            pdf2xml contentXmlFile = new pdf2xml();

                            TextReader txtReader = new StringReader(File.ReadAllText(Settings1.Default.CaleXml + f.Name));
                            contentXmlFile = (pdf2xml)serializer.Deserialize(txtReader);

                            #region variabile locale
                            int linieTopVal = 0;//indica coordonata Top aferenta randului ce trebuie citit
                            int linieLeftVal_Ant = 0;//indica coordonata Left a randului citit anterior
                            string linieProdusNume = "";//formeaza numele produsului de pe linie, chiar daca e pe mai multe linii;                    
                            string linieValori = "";// va concatena informatiile cu privire la randul citit (UM, Cantitate, PretUnitar, CotaTVA, Valoare, ValoareTVA, Total)
                            bool finalCitireLinii = false;//indica daca s-au citit toate liniile

                            #endregion variabile locale

                            DOC_Import doc = new DOC_Import();
                            doc.Id = -1;
                            doc.StructuraCod = Settings1.Default.DocStructuraCod;
                            doc.TipCod = Settings1.Default.DocTipCod;
                            doc.ValutaSimbol = "RON";//de revizuit
                            doc.Curs = 1;
                            doc.TVA = 19;
                            doc.Explicatie = "Autogenerat - deserializare pdf-xml";

                            foreach (pdf2xmlPage pag in contentXmlFile.Items)
                            {
                                linieTopVal = 0;//resetare pt fiecare pag
                                linieLeftVal_Ant = 0;//resetare pt fiecare pag
                                if (linieProdusNume.Length > 0 && linieValori.Length > 0) //cand se trece la pag urmatoare, trebuie salvat si ultima linie
                                    DodImportDetaliiAppend(doc, linieProdusNume, linieValori);

                                linieProdusNume = "";
                                linieValori = "";

                                for (int i = 0; i < pag.text.Length; i++) //foreach (pdf2xmlPageText pgTxt in pag.text)
                                {
                                    if (finalCitireLinii == false)//citeste doar daca nu s-a intalnit ultimul sir care marcheaza final zona linii, sir ="Semnatura si"
                                    {
                                        try
                                        {
                                            pdf2xmlPageText pgTxt = pag.text[i] as pdf2xmlPageText;
                                            #region Antet document
                                            if (pgTxt.Value != null && pgTxt.Value.Contains("Numr Factur:"))
                                            {
                                                var numar = pgTxt.Value;
                                                if (pgTxt.Value != null)
                                                    doc.Numar = numar.Split(' ').LastOrDefault();
                                                continue;
                                            }
                                            if (pgTxt.Value != null && pgTxt.Value.Contains("Dat Factur:"))
                                            {
                                                DateTime time;
                                                string[] parts = pgTxt.Value.Split(' ');
                                                string lastWord = parts[parts.Length - 1];
                                                string secondlastword= parts[parts.Length - 2];
                                                string monthinnumber = month[secondlastword];
                                                string thirdlastword= parts[parts.Length - 3];
                                                string data = thirdlastword + "/" + monthinnumber+ "/" + lastWord;
                                                if (pgTxt.Value != null)
                                                {
                                                    if (DateTime.TryParse(data, out time))
                                                    {
                                                        doc.Data = time;
                                                    }
                                                }
                                                continue;
                                            }
                                            //if (pgTxt.b != null && pgTxt.b.Contains("Data scadentei"))
                                            //{
                                            //    if (pgTxt.Value != null)
                                            //        doc.Scadenta = Convert.ToDateTime(pgTxt.Value.Replace(":", "").Trim());
                                            //    continue;
                                            //}
                                            if (pgTxt.Value != null && pgTxt.Value.Contains("Uber"))//("Document fiscal emis de Uber")
                                            {
                                                if (pag.text.Length > i + 6)//se verifica ca nu depaseste numarul de linii asteptat
                                                {
                                                    pdf2xmlPageText pgTxt_ParNume = pag.text[i + 1] as pdf2xmlPageText;
                                                    doc.PartenerNume = pgTxt_ParNume.Value;
                                                    
                                                    continue;
                                                }
                                            }
                                            if (pgTxt.Value != null && pgTxt.Value.Contains("CIF:"))
                                            {
                                                string cif = pgTxt.Value;
                                                if (pgTxt.Value != null)
                                                    doc.PartenerCUI = cif.Split(' ').LastOrDefault();
                                                continue;

                                            }
                                            #endregion Antet document

                                            //    #region determinare valoare coordonata TOP - indica linia de citit
                                            //    if (pgTxt.b != null && pgTxt.b.Contains("CU TVA"))
                                            //{//se extrage coordonata Top aferenta primului rand din pagina
                                            //    if (pag.text.Length > i + 1)
                                            //    {
                                            //        pdf2xmlPageText pgTxt_NextLinie = pag.text[i + 1] as pdf2xmlPageText;
                                            //        linieTopVal = Convert.ToInt32(pgTxt_NextLinie.top);
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    if (linieTopVal > 0 //indica ca s-a trecut de indicatorul "CU TVA"
                                            //        && Convert.ToInt32(pgTxt.top) > linieTopVal && Convert.ToInt32(pgTxt.left) < linieLeftVal_Ant)
                                            //    {
                                            //        linieTopVal = Convert.ToInt32(pgTxt.top);// NOUA LINIE!!
                                            //        DodImportDetaliiAppend(doc, linieProdusNume, linieValori);
                                            //        linieProdusNume = "";
                                            //        linieValori = "";
                                            //    }
                                            //}
                                            //#endregion determinare valoare coordonata TOP - linie de citit

                                            #region citire LINIE
                                            //if (Convert.ToInt32(pgTxt.top) == linieTopVal)//indica randul
                                            //{
                                            //    if (Convert.ToInt32(pgTxt.left) < linieLeftVal_Ant) //sunt la inceput de rand, gasesc produs Nume
                                            //    {
                                            //        linieProdusNume += pgTxt.Value;
                                            //    }
                                            //    else //NU sunt la inceput de rand, sigur alte informatii (UM, Cantitate, PretUnitar, CotaTVA, Valoare, ValoareTVA, Total)
                                            //    {
                                            //        if (pgTxt.Value != null)
                                            //            linieValori += " " + pgTxt.Value;
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    if (pag.text.Length > i + 1)
                                            //    {
                                            //        pdf2xmlPageText pgTxt_NextLinie = pag.text[i + 1] as pdf2xmlPageText;
                                            //        //Daca urmatoarea pozitie are top = linieTopVal sau left = left curent => textul reprezinta continuare denumire produs
                                            //        if (linieTopVal > 0
                                            //            && (Convert.ToInt32(pgTxt_NextLinie.top) == linieTopVal || Convert.ToInt32(pgTxt_NextLinie.left) == Convert.ToInt32(pgTxt.left)))
                                            //        {
                                            //            if (pgTxt.Value != null)
                                            //                linieProdusNume += " " + pgTxt.Value.Trim();
                                            //        }
                                            //    }
                                            //}
                                            if (pgTxt.Value != null && pgTxt.Value.Contains("Valoarea Brut"))
                                            {

                                                if (pag.text.Length>i+1)//se verifica ca nu depaseste numarul de linii asteptat
                                                {
                                                    pdf2xmlPageText pgTxt_Total = pag.text[i + 1] as pdf2xmlPageText;
                                                    linieValori = pgTxt_Total.Value;
                                                    linieProdusNume = "Serviciu Transport";
                                                    DodImportDetaliiAppendStorno(doc, linieProdusNume, linieValori);
                                                    linieProdusNume = "";
                                                    linieValori = "";
                                                    continue;
                                                }
                                                

                                            }
                                            #endregion citire LINIE

                                            ////
                                            linieLeftVal_Ant = Convert.ToInt32(pgTxt.left);

                                            if (pgTxt.Value != null && pgTxt.Value.Contains("Regim special"))
                                            {
                                                finalCitireLinii = true;
                                                continue;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //LOG
                                        }
                                    }
                                }
                            }

                            #endregion pentru fiecare fisier de tip xml din folder aferent


                            #region expediere fisier catre API
                            string serviceUri = Settings1.Default.ApiUrl;
                            JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                            jsonSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                            jsonSettings.Culture = CultureInfo.InvariantCulture;
                            jsonSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                            string json = JsonConvert.SerializeObject(doc, jsonSettings);
                            PostRequest(serviceUri, json);
                            #endregion expediere fisier catre API

                            #region mutare fisiere in istoric
                            string pdfstart = Settings1.Default.CalePdf;
                            string start = Settings1.Default.CaleXml;
                            string destinatie = Settings1.Default.CaleExec + "Istoric";
                            DirectoryInfo dirs = new DirectoryInfo(start);
                            DirectoryInfo dird = new DirectoryInfo(destinatie);
                            if (dirs.Exists == false)
                                Directory.CreateDirectory(start);
                            if (dird.Exists == false)
                                Directory.CreateDirectory(destinatie);
                            if (dirs.Exists == false)
                                Directory.CreateDirectory(pdfstart);
                            List<String> XML = Directory.GetFiles(start, "*.*", SearchOption.TopDirectoryOnly).ToList();
                            List<String> XMLpdf = Directory.GetFiles(pdfstart, "*.*", SearchOption.TopDirectoryOnly).ToList();
                            foreach (string file in XML)
                            {
                                FileInfo mFile = new FileInfo(file);
                                string newFileName = mFile.Name.Replace(".xml", "") + (DateTime.Today.ToShortDateString()).Replace("/", "-")
                                                        + "_" + (DateTime.Now.ToLongTimeString()).Replace(":", "-")
                                                        + ".xml"; //daca exista in istoric fisierul cu acelasi nume se redenumeste si se muta

                                if (new FileInfo(dird + "\\" + newFileName).Exists == true)//daca destinatia contine deja fisierul cu noua denumire (ceea ce e putin probabil)
                                    mFile.Delete();
                                else
                                    mFile.MoveTo(dird + "\\" + newFileName);
                            }
                            foreach (string file in XMLpdf)
                            {
                                FileInfo mFile = new FileInfo(file);
                                string newFileName = mFile.Name.Replace(".pdf", "") + (DateTime.Today.ToShortDateString()).Replace("/", "-")
                                                        + "_" + (DateTime.Now.ToLongTimeString()).Replace(":", "-")
                                                        + ".pdf"; //daca exista in istoric fisierul cu acelasi nume se redenumeste si se muta

                                if (new FileInfo(dird + "\\" + newFileName).Exists == true)//daca destinatia contine deja fisierul cu noua denumire (ceea ce e putin probabil)
                                    mFile.Delete();
                                else
                                    mFile.MoveTo(dird + "\\" + newFileName);
                            }
                            #endregion mutare fisiere in istoric

                            string good = "Fisierul cu numele: " + f.Name + " a fost incarcat cu succes:";

                            string sMonth = DateTime.Now.ToString("yyyy_MM");
                            string numeLog = "log" + sMonth + ".txt";
                            string logpath = Settings1.Default.CaleExec + "//Log//" + numeLog;
                            //LOG
                            using (StreamWriter w = File.AppendText(logpath))
                            {
                                Log(good, w);
                            }
                        }
                        catch (Exception ex)
                        {
                            string fail = "Incarcarea a esuat: ";
                            fail = fail + ex.Message;
                            string sMonth = DateTime.Now.ToString("yyyy_MM");
                            string numeLog = "log" + sMonth + ".txt";
                            string logpath = Settings1.Default.CaleExec + "//Log//" + numeLog;
                            //LOG
                            using (StreamWriter w = File.AppendText(logpath))
                            {
                                Log(fail, w);
                            }
                        }

                    }
                    else
                    {

                        string good = "Fisierul cu numele: " + f.Name + " nu este de tipul acceptata si nu a fost incarcat";
                        string sMonth = DateTime.Now.ToString("yyyy_MM");
                        string numeLog = "log" + sMonth + ".txt";
                        string logpath = Settings1.Default.CaleExec + "//Log//" + numeLog;
                        //LOG
                        using (StreamWriter w = File.AppendText(logpath))
                        {
                            Log(good, w);
                        }
                    }
                }

               


            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            //if the form is minimized  
            //hide it from the task bar  
            //and show the system tray icon (represented by the NotifyIcon control)  
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("  :");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("-------------------------------");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PdfConverter_WF.Settings1.Default.CalePdf = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PdfConverter_WF.Settings1.Default.CaleXml = textBox2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PdfConverter_WF.Settings1.Default.CaleExec = textBox3.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PdfConverter_WF.Settings1.Default.DocTipCod = textBox4.Text;
        }
    }
}
