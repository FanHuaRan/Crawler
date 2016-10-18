using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace CrawlDemo.Common
{
    public static class HttpRequestHelper
    {
        public static HtmlDocument GetDoucument(string strUrl, Encoding encoding = null)
        {
            HttpItem item = new HttpItem() { URL = strUrl };
            if (encoding != null)
            {
                item.Encoding = encoding;
            }
            HttpHelper helper = new HttpHelper();
            string htmlStr = helper.GetHtml(item).Html;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlStr);
            if (htmlDoc.DocumentNode != null)
            {
                // 清除script
                if (htmlDoc.DocumentNode.Descendants("script") != null)
                {
                    foreach (var script in htmlDoc.DocumentNode.Descendants("script").ToArray())
                        script.Remove();
                }

                // 清除style
                if (htmlDoc.DocumentNode.Descendants("style") != null)
                {
                    foreach (var style in htmlDoc.DocumentNode.Descendants("style").ToArray())
                        style.Remove();
                }
                // 清除注释
                if (htmlDoc.DocumentNode.SelectNodes("//comment()") != null)
                {
                    foreach (var comment in htmlDoc.DocumentNode.SelectNodes("//comment()").ToArray())
                        comment.Remove();
                }
            }

            return htmlDoc;
        }

        public static string GetHtmlSource(string strUrl, Encoding encoding = null)
        {
            HttpItem item = new HttpItem() { URL = strUrl };
            if (encoding != null)
            {
                item.Encoding = encoding;
            }
            HttpHelper helper = new HttpHelper();
            return helper.GetHtml(item).Html;
        }
    }
}
