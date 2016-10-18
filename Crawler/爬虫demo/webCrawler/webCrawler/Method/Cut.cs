using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;

namespace CrawlDemo.GatherMethod
{
    public class Cut
    {
        //正则匹配内容
        public string[] CutStr(string sStr, string Patrn)
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

        //正则无外框
        public string CutStrOp(string sStr, string Patrn)
        {
            Regex regex = new Regex(Patrn, RegexOptions.IgnoreCase);
            Match m = regex.Match(sStr);
            return m.Value.ToString();
        }
    }
}
