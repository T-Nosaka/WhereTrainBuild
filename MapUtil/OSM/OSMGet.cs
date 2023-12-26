using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WhereTrainBuild.MapUtil.OSM
{
    /// <summary>
    /// OpenStreetMap画像取得
    /// </summary>
    public class OSMGet : MapGetIf
    {
        /// <summary>
        /// URLフォーマット
        /// </summary>
        protected string m_url_format = "https://tile.openstreetmap.org/{0}/{1}/{2}.png";

        /// <summary>
        /// URLフォーマットプロパティ
        /// </summary>
        public string URLFormat
        {
            get
            {
                return m_url_format;
            }
            set
            {
                m_url_format = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OSMGet()
        {
        }

        /// <summary>
        /// 要求リスト
        /// </summary>
        protected List<HttpWebRequest> m_request_list = new List<HttpWebRequest>();

        /// <summary>
        /// デストラクタ
        /// </summary>
        public void Dispose()
        {
            lock (m_request_list)
            {
                m_request_list.ForEach(req => req.Abort());
                m_request_list.Clear();
            }
        }

        /// <summary>
        /// 地図画像取得
        /// 
        /// </summary>
        /// <param name="iTileX"></param>
        /// <param name="iTileY"></param>
        public Image GetMap(int iTileX, int iTileY, int iZoom, int timeout)
        {
            try
            {
                string url = string.Format(URLFormat, iZoom, iTileX, iTileY);

                var image = new Bitmap(GetContent(url, timeout));
                image = image.Clone(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// コンテンツダウンロード
        /// </summary>
        /// <param name="url"></param>
        protected Stream GetContent(string requesturi, int timeout)
        {
            MemoryStream outstream = new MemoryStream();

            HttpWebRequest webReq = null;

            try
            {
                webReq = HttpWebRequest.Create(requesturi) as HttpWebRequest;
                webReq.Timeout = timeout;
                webReq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.38 Safari/533.4";
                webReq.Method = WebRequestMethods.File.DownloadFile;

                lock (m_request_list)
                {
                    m_request_list.Add(webReq);
                }

                // 結果取得（今回はヘッダとステータス）
                using (HttpWebResponse res = webReq.GetResponse() as HttpWebResponse)
                {
                    using (Stream inStrm = res.GetResponseStream())
                    {
                        byte[] block = new byte[4096];

                        while (true)
                        {
                            int iReadLength = inStrm.Read(block, 0, block.Length);
                            if (iReadLength <= 0)
                            {
                                outstream.Flush();
                                break;
                            }

                            outstream.Write(block, 0, iReadLength);
                        }
                    }
                }

                outstream.Position = 0;

                return outstream;
            }
            catch (Exception ex)
            {
                return new MemoryStream();
            }
            finally
            {
                lock (m_request_list)
                {
                    m_request_list.Remove(webReq);
                }
            }
        }
    }
}
