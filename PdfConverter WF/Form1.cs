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
            CitireXml();
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
    }
}
