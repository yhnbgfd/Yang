using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using WindowsFormsApplication1.DataDef;

namespace WindowsFormsApplication1.Method
{
    class LoadData
    {
        AllDataInOne adio = new AllDataInOne();

        public AllDataInOne Load(string Path)
        {
            Logging savelog = new Logging();
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(Path);
                XmlTextReader reader = new XmlTextReader(Path);
                GenerateData gd = new GenerateData();
                int ic = 0;
                string[] pp = Path.Split(new string[] { "." }, StringSplitOptions.None);
                string[] p2 = pp[2].Split(new string[] { "q" }, StringSplitOptions.None);
                adio.m = int.Parse(p2[0]);
                adio.n = int.Parse(p2[1]);
                adio.totalData = gd.CountTotalData(adio.m, adio.n);
                adio.FilterStatistics = new int[adio.totalData];
                adio.SpecialMark = new int[adio.totalData];
                adio.DeleteMark = new int[adio.totalData];
                while (reader.Read())
                {
                    if (reader.Name == "list")
                    {
                        string[] dd = reader.ReadString().Split(new string[] { "," }, StringSplitOptions.None);
                        for (int i = 0; i < dd.Length - 1; i++)
                        {
                            adio.choiceDate.Add(int.Parse(dd[i]));
                        }
                    }
                    else if (reader.Name == "array")
                    {
                        int[] idata = new int[adio.n];
                        string[] kk = reader.ReadString().Split(new string[] { "," }, StringSplitOptions.None);
                        for (int j = 0; j < adio.n; j++)
                        {
                            idata[j] = int.Parse(kk[j]);
                        }
                        adio.l_totalDataBase.Add(idata);
                        adio.FilterStatistics[ic] = int.Parse(kk[adio.n]);
                        adio.SpecialMark[ic] = int.Parse(kk[adio.n+1]);
                        adio.DeleteMark[ic] = int.Parse(kk[adio.n + 2]);
                        ic++;
                    }
                }
                reader.Close();
                return adio;
            }
            catch (Exception ex)
            {
                savelog.errorlog(Path.Substring(0, Path.IndexOf(@"\")), ex.ToString());
                return null;
            }
        }
    }
}
