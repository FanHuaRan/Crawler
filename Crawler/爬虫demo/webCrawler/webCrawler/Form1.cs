using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using HtmlAgilityPack;

namespace webCrawler
{
    public partial class Form1 : Form
    {
        CrawlDemo.GatherMethod.ApMethod apmethod = new CrawlDemo.GatherMethod.ApMethod();

        public Form1()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = texturl.Text;
                //WebClient mywebclient = new WebClient();
                //mywebclient.Credentials = CredentialCache.DefaultCredentials;
                //Byte[] pagedata = mywebclient.DownloadData("http://" + url);
                //string pagehtml = Encoding.UTF8.GetString(pagedata);
                //pagehtml = GetTitleContent(pagehtml,"a");
                //textBox2.Text = pagehtml;

                string htmlstr = apmethod.GetUrltoHtml(url);
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(htmlstr);
                if (htmlDoc == null)
                {
                    return;
                }
                HtmlNodeCollection elementCollectiondiv = htmlDoc.DocumentNode.SelectNodes(@"//" + textBox1.Text.Trim());
                textBox2.Text = "";
                foreach (var elementdiv in elementCollectiondiv)
                {
                    if (!string.IsNullOrEmpty(textBox3.Text.Trim()))
                    {
                        textBox2.Text += elementdiv.GetAttributeValue(textBox3.Text.Trim(), "") + "\r\n";
                    }
                    else
                    {
                        textBox2.Text += elementdiv.InnerText + "\r\n";
                    }
                }
            }
            catch{
                MessageBox.Show("返回错误 可能是url不正确，不需要加http://");
            }
        }

        private void texturl_TextChanged(object sender, EventArgs e)
        {
            
        }

        /// <summary>  
        /// 获取字符中指定标签的值  
        /// </summary>  
        /// <param name="str">字符串</param>  
        /// <param name="title">标签</param>  
        /// <returns>值</returns>  
        public static string GetTitleContent(string str, string title)
        {
            string tmpStr = string.Format("<{0}[^>]*?>(?<Text>[^<]*)</{1}>", title, title); //获取<title>之间内容  

            Match TitleMatch = Regex.Match(str, tmpStr, RegexOptions.IgnoreCase);

            string result = TitleMatch.Groups["Text"].Value;
            return result;
        }
        
    }


}
