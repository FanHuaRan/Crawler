using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Common
{
    public class HttpMethod
    {

        /************************************************************************************ 
         * Copyright (c) 2014 南京安讯科技 All Rights Reserved. 
         * 版本号：  V1.0.0.0 
         * 创建人：  王光辉
         * 创建时间：2009/06/16 14:02:00 
         * 修改人  ：  
         * 修改时间：2009/06/16 14:02:00 
         * 描述    ：http请求方法
         */

        public static string SendGetForGBK(string Url)
        {
            #region 发送HTTP GET 数据

            HttpWebRequest httpWebRequest = WebRequest.Create(Url) as HttpWebRequest;

            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Timeout = 60000;

            //byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonString);

            //using (Stream stream = httpWebRequest.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();

            string result = string.Empty;
            using (StreamReader responseReader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("gb2312")))
            {
                result = responseReader.ReadToEnd();
            }
            return result;
            #endregion
        }

        public static string SendGetForUTF8(string Url)
        {
            #region 发送HTTP GET 数据

            HttpWebRequest httpWebRequest = WebRequest.Create(Url) as HttpWebRequest;

            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Timeout = 60000;

            //byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonString);

            //using (Stream stream = httpWebRequest.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();

            string result = string.Empty;
            using (StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8))
            {
                result = responseReader.ReadToEnd();
            }
            return result;
            #endregion
        }

        public static string SendPost(string Url, string jsonString, string contentType)
        {
            #region 发送HTTP POST 数据

            HttpWebRequest httpWebRequest = WebRequest.Create(Url) as HttpWebRequest;

            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = contentType;
            httpWebRequest.Timeout = 60000;

            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonString);

            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();

            string result = string.Empty;
            using (StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8))
            {
                result = responseReader.ReadToEnd();
            }
            return result;
            #endregion
        }

        public static string SendPut(string Url, string jsonString)
        {
            #region 发送HTTP PUT 数据

            HttpWebRequest httpWebRequest = WebRequest.Create(Url) as HttpWebRequest;

            httpWebRequest.Method = "PUT";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Timeout = 60000;

            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonString);

            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();

            string result = string.Empty;
            using (StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8))
            {
                result = responseReader.ReadToEnd();
            }
            return result;
            #endregion
        }

        public static string gb2312_utf8(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string utf8_gb2312(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }

    }
}
