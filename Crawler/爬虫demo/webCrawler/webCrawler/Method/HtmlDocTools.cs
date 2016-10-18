using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace CrawlDemo.GatherMethod
{
    class HtmlDocTools
    {
        //生成页面的htmlDocument文件
        public HtmlDocument getDoucument(string strUrl)
        {
            WebBrowser wb = new WebBrowser();
            HtmlDocument document = null;
            try
            {
                wb.ScriptErrorsSuppressed = true;
                //写入网络源码
                CrawlDemo.Common.HttpItem item = new CrawlDemo.Common.HttpItem();
                item.URL = strUrl;
                CrawlDemo.Common.HttpHelper helper = new CrawlDemo.Common.HttpHelper();
                string strHtml = helper.GetHtml(item).Html;

                wb.Navigate("about:blank");
                wb.Document.Write(strHtml);
                document = wb.Document;
                wb.Dispose();
            }
            catch
            {
                //异常待处理。
                return null;
            }
            return document;
        }

        //生成页面的htmlDocument文件
        public string getHtml(string strUrl)
        {
            WebBrowser wb = new WebBrowser();
            try
            {
                wb.ScriptErrorsSuppressed = true;
                //写入网络源码
                CrawlDemo.Common.HttpItem item = new CrawlDemo.Common.HttpItem();
                item.URL = strUrl;
                CrawlDemo.Common.HttpHelper helper = new CrawlDemo.Common.HttpHelper();
                string strHtml = helper.GetHtml(item).Html;
                wb.Dispose();
                return strHtml;
            }
            catch
            {
                //异常待处理。
                return null;
            }
        }

        public HtmlDocument html2Doc(string html)
        {
            WebBrowser wb = new WebBrowser();
            HtmlDocument document = null;
            try
            {
                wb.ScriptErrorsSuppressed = true;
                //写入网络源码
                wb.Navigate("about:blank");
                wb.Document.Write(html);
                document = wb.Document;
                wb.Dispose();
            }
            catch
            {
                //异常待处理。
            }
            return document;
        }

        //获取网页源码
        private string GetHttp(string sUrl)
        {
            WebClient WC = new WebClient();
            WC.Headers.Add("user-agent", "Opera/9.80 (Windows NT 5.1; U; Edition IBIS; zh-cn) Presto/2.5.22 Version/10.50");
            WC.Credentials = CredentialCache.DefaultCredentials;
            Byte[] PageData = WC.DownloadData(sUrl);
            string HtmlCode = Encoding.UTF8.GetString(PageData);
            WC.Dispose();
            // Session.Abandon();
            return HtmlCode;
        }


        //格式化链接地址
        private string FormatUrl(string BaseUrl, string theUrl)
        {
            int pathLevel = 0;
            string hostName;
            // theUrl = theUrl.ToLower();
            hostName = BaseUrl.Substring(0, BaseUrl.IndexOf("/", 8));
            BaseUrl = BaseUrl.Substring(0, BaseUrl.LastIndexOf("/"));
            if (theUrl.StartsWith("./"))
            {
                theUrl = theUrl.Remove(0, 1);
                theUrl = BaseUrl + theUrl;
            }
            else if (theUrl.StartsWith("/"))
            {
                theUrl = hostName + theUrl;
            }
            else if (theUrl.StartsWith("../"))
            {
                while (theUrl.StartsWith("../"))
                {
                    pathLevel = ++pathLevel;
                    theUrl = theUrl.Remove(0, 3);
                }
                for (int i = 0; i < pathLevel; i++)
                {
                    BaseUrl = BaseUrl.Substring(0, BaseUrl.LastIndexOf("/", BaseUrl.Length - 2));
                }
                theUrl = BaseUrl + "/" + theUrl;
            }
            if (!theUrl.StartsWith("http://") && !theUrl.StartsWith("https://"))
            {
                theUrl = BaseUrl + "/" + theUrl;
            }
            return theUrl;
        }

        //正则匹配内容
        public static string[] CutStr(string sStr, string Patrn)
        {
            string[] RsltAry;
            Regex tmpreg = new Regex(Patrn, RegexOptions.Compiled);
            MatchCollection sMC = tmpreg.Matches(sStr);
            if (sMC.Count != 0)
            {
                RsltAry = new string[sMC.Count];
                for (int i = 0; i < sMC.Count; i++)
                {
                    RsltAry[i] = sMC[i].Groups[1].Value;
                }
            }
            else
            {
                RsltAry = new string[1];
                RsltAry[0] = "";
            }
            return RsltAry;
        }

        private string GetPage(string httpUrl)
        {
            CookieContainer m_cookCont = new CookieContainer();
            string data = "";
            try
            {
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(httpUrl);
                httpWReq.CookieContainer = m_cookCont;
                httpWReq.Method = "GET";
                httpWReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                httpWReq.AllowAutoRedirect = false;

                httpWReq.Timeout = 60000;

                HttpWebResponse httpWResp = (HttpWebResponse)httpWReq.GetResponse();
                httpWResp.Cookies = m_cookCont.GetCookies(httpWReq.RequestUri);
                m_cookCont = httpWReq.CookieContainer;
                using (Stream respStream = httpWResp.GetResponseStream())
                {
                    using (StreamReader respStreamReader = new StreamReader(respStream, System.Text.Encoding.UTF8))
                    {
                        data = respStreamReader.ReadToEnd();
                    }
                }
            }
            catch
            {

            }
            return data;
        }
    }
}
