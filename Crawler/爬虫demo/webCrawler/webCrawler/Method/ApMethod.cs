using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows.Forms;
using System.Reflection;

namespace CrawlDemo.GatherMethod
{
    public class ApMethod
    {
        /// <summary>
        /// 获取页面
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrltoHtml(string Url)
        {
            string result = "";
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                wReq.Timeout = 10000;
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.UTF8))  //Encoding.Default
                {
                    result = reader.ReadToEnd();
                }
                if (isLuan(result))
                {
                    System.Net.WebRequest wReq2 = System.Net.WebRequest.Create(Url);
                    wReq2.Timeout = 3000;
                    System.Net.WebResponse wResp2 = wReq2.GetResponse();
                    System.IO.Stream respStream2 = wResp2.GetResponseStream();
                    using (System.IO.StreamReader reader2 = new System.IO.StreamReader(respStream2, Encoding.Default))  //Encoding.Default
                    {
                        result = reader2.ReadToEnd();
                    }
                }
            }
            catch (System.Exception ex)
            {
                //errorMsg = ex.Message;
            }
            return result;
        }

        //判断编码
        public bool isLuan(string txt)
        {
            var bytes = Encoding.UTF8.GetBytes(txt);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }


    }
}
