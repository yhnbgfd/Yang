using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindowsFormsApplication1.Method
{
    class Logging
    {
        public void errorlog(string path, string log)
        {
            FileStream fs = new FileStream(path + "error.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write("\n"+log+"\n");
            //清空缓冲区  
            sw.Flush();
            //关闭流  
            sw.Close();
            fs.Close();  
        }
    }
}
