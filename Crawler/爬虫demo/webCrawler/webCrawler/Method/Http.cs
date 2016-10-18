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
    
    
        /// <summary>
        /// 功能简介：1主要使用HttpWebRequest和HttpWebResponse提交和接收数据
        ///           2使用WebClient几种方法提交和接收返回数据
        /// 创建日期：2009-06-16 sk
        /// 修改记录：
        /// </summary>
        public class Http
        {
            /// <summary>
            /// 从URL获取数据，编码模式默认utf8
            /// </summary>
            /// <param name="url">网页地址形如： “http://news.163.com”</param>
            /// <returns></returns>
            public static string GetPage(string url)
            {
                string content = "";
                // Create a new HttpWebRequest object.Make sure that
                // a default proxy is set if you are behind a fure wall.
                //其中,HttpWebRequest实例不使用HttpWebRequest的构造函数来创建,二是使用WebRequest的Create方法来创建.
                HttpWebRequest myHttpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest1.UserAgent = "Google Spider";
                myHttpWebRequest1.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //不维持与服务器的请求状态
                myHttpWebRequest1.KeepAlive = false;

                //创建一个HttpWebRequest对象
                //Assign the response object of HttpWebRequest to a HttpWebResponse variable.\
                HttpWebResponse myHttpWebResponse1;
                try
                {
                    // 根据微软MSDN上所说:"决不要直接创建HttpWebResponse的实例,要使用HttpWebRequest的GetResponse()方法返回的实例."具体的原因我也不清楚,可能HttpWebResponse类的构造函数中没有实现HttpWebResponse实例的代码吧.
                    myHttpWebResponse1 = (HttpWebResponse)myHttpWebRequest1.GetResponse();
                    //设置页面的编码模式
                    System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
                    Stream streamResponse = myHttpWebResponse1.GetResponseStream();
                    StreamReader streamRead = new StreamReader(streamResponse, utf8);

                    Char[] readBuff = new Char[256];
                    //这里使用了StreamReader的Read()方法,参数意指从0开始读取256个char到readByff中.
                    //Read()方法返回值为指定的字符串数组,当达到文件或流的末尾使,方法返回0
                    int count = streamRead.Read(readBuff, 0, 256);
                    while (count > 0)
                    {
                        String outputData = new String(readBuff, 0, count);
                        content += outputData;
                        count = streamRead.Read(readBuff, 0, 256);
                    }
                    myHttpWebResponse1.Close();
                    return (content);
                }
                catch (WebException ex)
                {
                    content = "在请求URL为：" + url + "的页面时产生错误，错误信息为" + ex.ToString();
                    return (content);
                }
            }

            /// <summary>
            /// 获取Uri网页信息，默认编码utf8
            /// </summary>
            /// <param name="uri"></param>
            /// <returns></returns>
            public static string GetPage(Uri uri)
            {
                string content = "";
                // Create a new HttpWebRequest object.Make sure that
                // a default proxy is set if you are behind a fure wall.
                //其中,HttpWebRequest实例不使用HttpWebRequest的构造函数来创建,二是使用WebRequest的Create方法来创建.
                HttpWebRequest myHttpWebRequest1 = (HttpWebRequest)WebRequest.Create(uri);

                //不维持与服务器的请求状态
                myHttpWebRequest1.KeepAlive = false;
                //创建一个HttpWebRequest对象
                //Assign the response object of HttpWebRequest to a HttpWebResponse variable.\
                HttpWebResponse myHttpWebResponse1;
                try
                {
                    // 根据微软MSDN上所说:"决不要直接创建HttpWebResponse的实例,要使用HttpWebRequest的GetResponse()方法返回的实例."具体的原因我也不清楚,可能HttpWebResponse类的构造函数中没有实现HttpWebResponse实例的代码吧.
                    myHttpWebResponse1 = (HttpWebResponse)myHttpWebRequest1.GetResponse();
                    //设置页面的编码模式
                    System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
                    Stream streamResponse = myHttpWebResponse1.GetResponseStream();
                    StreamReader streamRead = new StreamReader(streamResponse, utf8);

                    Char[] readBuff = new Char[256];
                    //这里使用了StreamReader的Read()方法,参数意指从0开始读取256个char到readByff中.
                    //Read()方法返回值为指定的字符串数组,当达到文件或流的末尾使,方法返回0
                    int count = streamRead.Read(readBuff, 0, 256);
                    while (count > 0)
                    {
                        String outputData = new String(readBuff, 0, count);
                        content += outputData;
                        count = streamRead.Read(readBuff, 0, 256);
                    }
                    myHttpWebResponse1.Close();
                    return (content);
                }
                catch (WebException ex)
                {
                    content = "在请求URL为：" + uri.ToString() + "的页面时产生错误，错误信息为" + ex.ToString();
                    return (content);
                }
            }

            static CookieContainer m_cookCont = new CookieContainer();

            /// <summary>
            /// 从URL获取数据。httpUrl:GET请求网址autoRedirect:是否自动跳转. encoding：编码
            /// </summary>
            /// <param name="httpUrl"></param>
            /// <param name="autoRedirect"></param>
            /// <returns></returns>
            public static string GetPage(string httpUrl, bool autoRedirect, string encoding)
            {
                string data = "";
                try
                {
                    HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(httpUrl);
                    httpWReq.CookieContainer = m_cookCont;
                    httpWReq.Method = "GET";
                    httpWReq.AllowAutoRedirect = autoRedirect;

                    HttpWebResponse httpWResp = (HttpWebResponse)httpWReq.GetResponse();
                    httpWResp.Cookies = m_cookCont.GetCookies(httpWReq.RequestUri);
                    m_cookCont = httpWReq.CookieContainer;
                    using (Stream respStream = httpWResp.GetResponseStream())
                    {
                        using (StreamReader respStreamReader = new StreamReader(respStream, System.Text.Encoding.GetEncoding(encoding)))
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

            public static string GetImages(string httpUrl, string ua)
            {
                string data = "";
                try
                {
                    HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(httpUrl);
                    httpWReq.CookieContainer = m_cookCont;
                    httpWReq.Method = "GET";
                    httpWReq.UserAgent = ua;
                    httpWReq.AllowAutoRedirect = false;

                    httpWReq.Timeout = 60000;

                    HttpWebResponse httpWResp = (HttpWebResponse)httpWReq.GetResponse();
                    //				Stream respStream = httpWResp.GetResponseStream();
                    //				StreamReader respStreamReader = new StreamReader(respStream, System.Text.Encoding.UTF8);
                    //				data = respStreamReader.ReadToEnd();
                    //				respStreamReader.Close();
                    //				respStream.Close();
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


            /// <summary>
            /// 从URL获取数据
            /// </summary>
            /// <param name="httpUrl">url</param>
            /// <param name="ua">UserAgent信息</param>
            /// <returns></returns>
            public static string GetPage(string httpUrl, string ua)
            {
                string data = "";
                try
                {
                    HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(httpUrl);
                    httpWReq.CookieContainer = m_cookCont;
                    httpWReq.Method = "GET";
                    httpWReq.UserAgent = ua;
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

            /// <summary>
            /// postData:要发送的数据
            /// xhttpUrl:发送网址
            /// </summary>
            /// <param name="postData"></param>
            /// <param name="xhttpUrl"></param>
            /// <returns></returns>
            public static string Post(string postData, string xhttpUrl, bool autoRedirect, string encoding)
            {
                string cookieHeader = "";
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(xhttpUrl);
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                httpWReq.Method = "POST";
                httpWReq.AllowAutoRedirect = autoRedirect;

                httpWReq.CookieContainer = m_cookCont;
                httpWReq.CookieContainer.SetCookies(new Uri(xhttpUrl), cookieHeader);

                Stream reqStream = httpWReq.GetRequestStream();
                StreamWriter reqStreamWrite = new StreamWriter(reqStream);
                byte[] pdata = Encoding.Default.GetBytes(postData);
                char[] cpara = new char[pdata.Length];
                for (int i = 0; i < pdata.Length; i++)
                { cpara[i] = System.Convert.ToChar(pdata[i]); }
                reqStreamWrite.Write(cpara, 0, cpara.Length);

                reqStreamWrite.Close();
                reqStream.Close();

                HttpWebResponse httpWResp = (HttpWebResponse)httpWReq.GetResponse();

                httpWResp.Cookies = m_cookCont.GetCookies(httpWReq.RequestUri);
                m_cookCont = httpWReq.CookieContainer;

                Stream respStream = httpWResp.GetResponseStream();
                StreamReader respStreamReader = new StreamReader(respStream, System.Text.Encoding.GetEncoding(encoding));
                string data = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                respStream.Close();

                return data;
            }
            /// <summary>
            /// 向网址发送数据
            /// </summary>
            /// <param name="postData"></param>
            /// <param name="xhttpUrl"></param>
            /// <returns></returns>
            public static string Post(string postData, string xhttpUrl)
            {
                string cookieHeader = "";
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(xhttpUrl);
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                httpWReq.Method = "POST";
                httpWReq.AllowAutoRedirect = false;

                httpWReq.CookieContainer = m_cookCont;
                httpWReq.CookieContainer.SetCookies(new Uri(xhttpUrl), cookieHeader);

                Stream reqStream = httpWReq.GetRequestStream();
                StreamWriter reqStreamWrite = new StreamWriter(reqStream);
                byte[] pdata = Encoding.Default.GetBytes(postData);
                //char[] cpara = new ASCIIEncoding().GetChars(pdata);
                char[] cpara = new char[pdata.Length];
                for (int i = 0; i < pdata.Length; i++)
                { cpara[i] = System.Convert.ToChar(pdata[i]); }
                reqStreamWrite.Write(cpara, 0, cpara.Length);
                reqStreamWrite.Close();
                reqStream.Close();

                HttpWebResponse httpWResp = (HttpWebResponse)httpWReq.GetResponse();

                httpWResp.Cookies = m_cookCont.GetCookies(httpWReq.RequestUri);
                m_cookCont = httpWReq.CookieContainer;

                Stream respStream = httpWResp.GetResponseStream();
                StreamReader respStreamReader = new StreamReader(respStream, System.Text.Encoding.Default);
                string data = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                respStream.Close();

                return data;
            }
            public static string Post(string postData, string xhttpUrl, string encoding)
            {
                return Post(postData, xhttpUrl, false, encoding);
            }

            /// <summary>
            /// 获取页面编码
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            public static string GetEncoding(string url)
            {
                WebClient myWebClient = new WebClient();
                myWebClient.Credentials = CredentialCache.DefaultCredentials;

                byte[] myDataBuffer = myWebClient.DownloadData(url);
                string strWebData = Encoding.Default.GetString(myDataBuffer);

                //获取网页字符编码描述信息 
                Match charSetMatch = Regex.Match(strWebData, "<meta([^<]*)charset=([^<]*)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string webCharSet = charSetMatch.Groups[2].Value;
                return webCharSet;
            }

            /// <summary>
            /// UTF-8读URL
            /// </summary>
            /// <param name="httpUrl"></param>
            /// <returns></returns>
            public static string GetPageByWebClientUTF8(string httpUrl)
            {
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;

                //方法一：
                try
                {
                    //使用DownloadData方法 从资源下载数据并返回 Byte 数组。
                    Byte[] pageData = wc.DownloadData(httpUrl);
                    return Encoding.UTF8.GetString(pageData);
                }
                catch (Exception ex)
                {
                    string content = "在请求URL为：" + httpUrl + "的页面时产生错误，错误信息为" + ex.ToString();
                    return (content);
                }

            }
            /// <summary>
            /// Default读URL
            /// </summary>
            /// <param name="httpUrl"></param>
            /// <returns></returns>
            public static string GetPageByWebClientDefault(string httpUrl)
            {
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;

                //方法二： 
                // ***************代码开始*******  
                try
                {
                    //从资源以 Stream 的形式返回数据。
                    Stream resStream = wc.OpenRead(httpUrl);
                    StreamReader sr = new StreamReader(resStream, System.Text.Encoding.Default);
                    string text = sr.ReadToEnd();
                    resStream.Close();
                    return text;
                }
                catch (Exception ex)
                {
                    string content = "在请求URL为：" + httpUrl + "的页面时产生错误，错误信息为" + ex.ToString();
                    return (content);
                }
                // **************代码结束******** 
            }

            /// <summary>
            /// 将数据提交到URL，然后接受返回的数据
            /// </summary>
            /// <param name="postString">提交的字符串</param>
            /// <param name="httpUrl">URL</param>
            /// <returns></returns>
            public static string GetPageByWebClient(string postString, string httpUrl)
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                byte[] postData = Encoding.Default.GetBytes(postString);

                try
                {
                    //将字节数组发送到资源，并返回包含任何响应的 Byte 数组。
                    byte[] responseData = client.UploadData(httpUrl, "POST", postData);
                    Encoding _gbk = Encoding.Default;
                    return _gbk.GetString(responseData);
                }
                catch (Exception ex)
                {
                    string content = "在请求URL为：" + httpUrl + "的页面时产生错误，错误信息为" + ex.ToString();
                    return (content);
                }

            }

            /// <summary>
            /// 新增采集。
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            private string getHtmlSource(string url) //获取页面HTML源码
            {
                string userAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
                string pagehtml;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 10000;
                request.UserAgent = userAgent;
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    //视频原地址
                    Stream stream = response.GetResponseStream();
                    StreamReader sReader = new StreamReader(stream, System.Text.Encoding.UTF8); //默认编码为UTF-8
                    pagehtml = sReader.ReadToEnd();

                    if (isLuan(pagehtml))
                    {
                        sReader = new StreamReader(stream, System.Text.Encoding.Default); //新浪默认编码为UTF-8
                        pagehtml = sReader.ReadToEnd();
                    }
                }
                catch
                {
                    pagehtml = "";
                }
                return pagehtml;
            }
            //生成页面的htmlDocument文件
            
            public HtmlDocument getDoucument(string strUrl)
            {
                WebBrowser wb = new WebBrowser();
                
                //忽略加载错误
                wb.ScriptErrorsSuppressed = true;
                //写入网络源码
                string strHtml = getHtmlSource(strUrl);
                wb.Navigate("about:blank");
                wb.Document.Write(strHtml);
                HtmlDocument document = wb.Document;
                wb.Dispose();
                return document;
            }

            //判断编码
            bool isLuan(string txt)
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
