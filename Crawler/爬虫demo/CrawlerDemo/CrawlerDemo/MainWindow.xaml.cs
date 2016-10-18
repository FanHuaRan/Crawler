using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CrawlerDemo.Helper;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
namespace CrawlerDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //等待采集链接 队列
        private List<string> NconnUrls = new List<string>();
        //等待采集的图片链接 队列
        private List<string> LoadingUrls = new List<string>();
        /// 已采集过图片链接  队列
        private List<string> UsedUrls = new List<string>();
        //已采集过链接链接    队列
        private List<string> UsedConnUrls = new List<string>();
        //已使用过的图片       队列
        private List<string> UsedImgUrls = new List<string>();

        //等待下载图片的最大个数
        private readonly int _maxtask = 200;

        //等待下载图片的个数
        private int _downingPicCount = 0;
        //已经下载的图片的个数
        private int _downLoadPicCount = 0;

        Thread Tstarting = null;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //  CheckForIllegalCrossThreadCalls = false;
            Tstarting = new Thread(DownLoading);
        }

        private void serch_btt_Click(object sender, RoutedEventArgs e)
        {

            if (Choice_floder())
            {
                LoadingUrls.Add(txt_url.Text);
                NconnUrls.Add(txt_url.Text);
                Global.WebUrl = StringHelper.GetPureUrl(txt_url.Text);
                if (Tstarting.ThreadState == ThreadState.Suspended)
                    Tstarting.Resume();
                else
                    Tstarting.Start();
                serch_btt.IsEnabled = false;
            }
        }

        private void about_btt_Click(object sender, RoutedEventArgs e)
        {

        }
 
        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        /// <summary>
        /// 选择图片存储文件夹
        /// </summary>
        private bool Choice_floder()
        {
            FolderBrowserDialog fbd_url = new FolderBrowserDialog();
            fbd_url.ShowDialog();
            string path = fbd_url.SelectedPath;
            
            if (!string.IsNullOrEmpty(path))
            {
                Global.FloderUrl = fbd_url.SelectedPath + "\\";
            }
            else
            {
                Global.FloderUrl = Environment.CurrentDirectory + Global.FloderMoUrl;
            }
            return !string.IsNullOrEmpty(path);
        }
        /// <summary>
        /// 收集图片
        /// </summary>
        private void DownLoading()
        {
            if (LoadingUrls.Count < 1)
            {
                TextResultChange("-----------下载完毕,正在重新收集链接 \r\n");
                CollectionUrls();
            }
            else
            {
                //继续下载
                //Tdown.Start();
                StartDownLoad();
            }
        }
        /// <summary>
        /// 下载中
        /// </summary>
        private void StartDownLoad()
        {
            //是否超过最大的队列
            if (_downingPicCount < _maxtask)
            {
                List<string> imgurls = HttpHelper.GetHtmlImageUrlList(LoadingUrls.FirstOrDefault());
                UsedUrls.Add(LoadingUrls.FirstOrDefault());
                LoadingUrls.RemoveAt(0);
                foreach (string url in imgurls)
                {
                    if (!UsedImgUrls.Contains(url))
                    {
                        //创建异步下载
                        DownloadHelper helper = new DownloadHelper();
                        StopTimeHandler stop = new StopTimeHandler(helper.DowloadImg);
                        AsyncCallback callback = new AsyncCallback(onDownLoadFinish);
                        IAsyncResult asyncResult = stop.BeginInvoke(url, callback, "--下载完成 \r\n");
                        //链接载入已使用URL
                        UsedImgUrls.Add(url);
                        TipStartDownLoad();
                    }

                }
                imgurls.Clear();
                TextUrlChange("");
            }
            else
            {
                Thread.Sleep(5000);
                TextResultChange("-----------任务过多，搜集程序休眠5秒 \r\n");
            }
            DownLoading();
        }
        private void TextResultChange(string text)
        {
            txt_usedurl.Dispatcher.Invoke(() =>
            txt_result.AppendText(text));
        }
        private void TextUrlChange(string text)
        {
            txt_usedurl.Dispatcher.Invoke(() =>
                {
                    txt_usedurl.Text = "";
                    txt_usedurl.AppendText(string.Join("\r\n ", UsedUrls));
                    txt_usedurl.ScrollToEnd();
                });
        }
        /// <summary>
        /// 收集URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private void CollectionUrls()
        {
            if (NconnUrls.Count > 0)
            {
                try
                {
                    string url = NconnUrls.FirstOrDefault();
                    //加入链接已采集存档
                    UsedConnUrls.Add(url);
                    //获取该链接的所有URL
                    List<string> urlList = HttpHelper.GetLinks(url);

                    foreach (string url1 in urlList)
                    {
                        //如果没采集过链接
                        if (!UsedConnUrls.Contains(url1))
                        {
                            NconnUrls.Add(url1);
                        }
                        //如果没采集过图片
                        if (!UsedUrls.Contains(url1))
                        {
                            LoadingUrls.Add(url1);
                        }
                    }
                    //删除已使用
                    NconnUrls.RemoveAt(0);
                }
                catch (Exception)
                {
                }
                DownLoading();
            }
            else
            {
                TextResultChange("-----------任务结束，全部爬行完毕 \r\n");
            }
        }
        
      
        /// <summary>
        /// 下载成功
        /// </summary>
        /// <param name="asyncresult"></param>
        private void onDownLoadFinish(IAsyncResult asyncresult)
        {
            AsyncResult result = (AsyncResult)asyncresult;
            StopTimeHandler del = (StopTimeHandler)result.AsyncDelegate;
            string data = (string)result.AsyncState;
            string name = del.EndInvoke(result);
            TextResultChange(name + data);
            _downLoadPicCount++;
            _downingPicCount--;
            TipDownLoad();
        }
        
        private async void TipStartDownLoad()
        {
            await Task.Run(() =>
                {
                    _downingPicCount++;
                }
            );
        }
        private async void TipDownLoad()
        {
            await Task.Run(() =>
                {
                    lbl_downingcount.Dispatcher.Invoke(() => lbl_downingcount.Content = string.Format("{0}张图片等待下载",_downingPicCount.ToString()));
                    lbl_piccount.Dispatcher.Invoke(()=>lbl_piccount.Content = string.Format("已下载{0}张图片", _downLoadPicCount));
                });
        }

        private async void start_btt_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
                {
                    if (Tstarting.ThreadState == ThreadState.Running)
                        Tstarting.Suspend();

                        serch_btt.Dispatcher.Invoke(() =>
                            serch_btt.IsEnabled = true);
                });
        }
  
    }
}
