using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WhereTrainBuild.MapUtil
{
    /// <summary>
    /// 地図タイル
    /// </summary>
    public class MapTile
    {
        /// <summary>
        /// 中心緯度
        /// </summary>
        protected double m_dLatitude = 0.0d;

        /// <summary>
        /// 中心経度
        /// </summary>
        protected double m_dLongitude = 0.0d;

        /// <summary>
        /// タイルX
        /// </summary>
        protected int m_tile_x = 0;

        /// <summary>
        /// タイルY
        /// </summary>
        protected int m_tile_y = 0;

        /// <summary>
        /// タイル幅
        /// 画素数
        /// </summary>
        protected int m_width = 256;

        /// <summary>
        /// タイル高
        /// 画素数
        /// </summary>
        protected int m_height = 256;

        /// <summary>
        /// ズームレベル
        /// </summary>
        protected int m_zoom = 0;

        /// <summary>
        /// 地図画像
        /// </summary>
        protected Image m_image = null;

        /// <summary>
        /// 地図画像取得
        /// </summary>
        /// <returns></returns>
        public Image GetImage()
        {
            return m_image;
        }

        /// <summary>
        /// 必須だ
        /// </summary>
        protected bool m_nativemethod = true;

        /// <summary>
        /// タイル位置
        /// </summary>
        public Point TilePosition
        {
            get
            {
                return new Point(m_tile_x, m_tile_y);
            }
        }

        /// <summary>
        /// ズームレベル
        /// </summary>
        public int Zoom
        {
            get
            {
                return m_zoom;
            }
        }

        /// <summary>
        /// タイル取得フラグ
        /// </summary>
        public bool NativeMathod
        {
            get
            {
                return m_nativemethod;
            }
            set
            {
                m_nativemethod = value;
            }
        }

        /// <summary>
        /// 地図取得
        /// </summary>
        protected MapGetIf m_mapgetter = new OSM.OSMGet();

        /// <summary>
        /// 地図取得プロパティ
        /// </summary>
        public MapGetIf MapGetter
        {
            get
            {
                return m_mapgetter;
            }
            set
            {
                m_mapgetter = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapTile( int tile_x, int tile_y, int zoom)
        {
            //中心緯度・経度算出
            double dX = tile_x * MercatorTrans.ViewSize + MercatorTrans.ViewSize / 2.0d;
            double dY = -tile_y * MercatorTrans.ViewSize - MercatorTrans.ViewSize / 2.0d;
            MercatorTrans.Reverse(dX, dY, zoom, ref m_dLatitude, ref m_dLongitude);

            m_zoom = zoom;
            m_tile_x = tile_x;
            m_tile_y = tile_y;
        }

        /// <summary>
        /// 地図取得強制終了
        /// </summary>
        public void CancelGetMap()
        {
            MapGetter.Dispose();
        }

        /// <summary>
        /// タイルキー
        /// </summary>
        /// <param name="tile_x"></param>
        /// <param name="tile_y"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public static string MakeKey( int tile_x, int tile_y, int zoom )
        {
            return string.Format("{0},{1},{2}", tile_x, tile_y, zoom);
        }

        /// <summary>
        /// タイルキー
        /// </summary>
        public string Key
        {
            get
            {
                return MakeKey(m_tile_x, m_tile_y, m_zoom);
            }
        }

        /// <summary>
        /// 隅型
        /// </summary>
        public enum CornerType
        {
            LeftTop,
            RightTop,
            LeftBottom,
            RightBottom
        }

        /// <summary>
        /// 隅取得
        /// </summary>
        /// <returns></returns>
        public virtual Point GetCorner( CornerType corner )
        {
            //中心取得
            double dX = m_tile_x * MercatorTrans.ViewSize + MercatorTrans.ViewSize / 2.0d;
            double dY = m_tile_y * MercatorTrans.ViewSize + MercatorTrans.ViewSize / 2.0d;

            switch (corner)
            {
                case CornerType.LeftTop:
                    {
                        dX -= (m_width / 2);
                        dY -= (m_height / 2);
                    }
                    break;

                case CornerType.RightTop:
                    {
                        dX += (m_width / 2);
                        dY -= (m_height / 2);
                    }
                    break;

                case CornerType.LeftBottom:
                    {
                        dX -= (m_width / 2);
                        dY += (m_height / 2);
                    }
                    break;

                case CornerType.RightBottom:
                    {
                        dX += (m_width / 2);
                        dY += (m_height / 2);
                    }
                    break;
            }

            return new Point((int)dX, (int)dY);
        }

        /// <summary>
        /// 縮小サイズ取得
        /// </summary>
        /// <returns></returns>
        public List<MapTile> GetSmallTile()
        {
            List<MapTile> tilelist = new List<MapTile>();

            MapTile lefttop = new VDetailMapTile( VDetailMapTile.CornerType.LeftTop, this, m_tile_x * 2, m_tile_y * 2, m_zoom + 1);
            MapTile righttop = new VDetailMapTile( VDetailMapTile.CornerType.RightTop, this, m_tile_x * 2 + 1, m_tile_y * 2, m_zoom + 1);
            MapTile leftbottom = new VDetailMapTile( VDetailMapTile.CornerType.LeftBottom, this, m_tile_x * 2, m_tile_y * 2 + 1, m_zoom + 1);
            MapTile rightbottom = new VDetailMapTile( VDetailMapTile.CornerType.RightBottom, this, m_tile_x * 2 + 1, m_tile_y * 2 + 1, m_zoom + 1);

            tilelist.Add(lefttop);
            tilelist.Add(righttop);
            tilelist.Add(leftbottom);
            tilelist.Add(rightbottom);

            return tilelist;
        }

        /// <summary>
        /// 拡大サイズ取得
        /// </summary>
        /// <returns></returns>
        public virtual MapTile GetBigTile()
        {
            return new VBigMapTile(this, m_tile_x / 2, m_tile_y / 2, m_zoom - 1);
        }

        /// <summary>
        /// 非同期画像取得排他
        /// </summary>
        protected AutoResetEvent m_exclusive = new AutoResetEvent(true);

        /// <summary>
        /// 非同期画像取得コールバック型
        /// </summary>
        /// <param name="maptile"></param>
        public delegate void CompletedGetImageDelegate(MapTile maptile, bool result );

        /// <summary>
        /// 並立画像取得最大数
        /// </summary>
        protected static Semaphore m_getmap_control = new Semaphore(10, 10);

        /// <summary>
        /// 並立画像取得最大数設定
        /// </summary>
        /// <param name="max"></param>
        public static void SetParallelism(int max)
        {
            m_getmap_control = new Semaphore(max, max);
        }

        /// <summary>
        /// 非同期画像取得
        /// </summary>
        /// <param name="basefolder"></param>
        /// <param name="callback"></param>
        public bool GetImageAsync(string basefolder, int timeout, CompletedGetImageDelegate callback)
        {
            if (m_exclusive.WaitOne(0, false) == false)
                return false;

            new Thread(() =>
            {
                try
                {
                    //処理空き待ち
                    if (m_getmap_control.WaitOne(timeout) == false)
                    {
                        callback(this, false);
                        return;
                    }
                    try
                    {
                        if (GetImage(basefolder, timeout) != null)
                            callback(this, true );
                        else
                            callback(this, false );
                    }
                    finally
                    {
                        m_getmap_control.Release();
                    }
                }
                finally
                {
                    m_exclusive.Set();
                }
            }).Start();

            return true;
        }

        /// <summary>
        /// 画像取得
        /// </summary>
        /// <returns></returns>
        public virtual Image GetImage(string basefolder, int timeout )
        {
            if (m_image == null)
            {
                if (Load(basefolder) == true)
                    return m_image;
                else
                {
                    if( NativeMathod == true )
                        m_image = m_mapgetter.GetMap(m_tile_x, m_tile_y, m_zoom, timeout);
                    if( m_image != null )
                        Write(basefolder, m_image);

                    return m_image;
                }
            }

            return m_image;
        }

        /// <summary>
        /// ファイル名作成
        /// </summary>
        /// <returns></returns>
        protected string FileName()
        {
            return string.Format("{0}_{1}.jpg", m_tile_x, m_tile_y);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="basefolder"></param>
        protected void Write(string basefolder, Image image )
        {
            if (basefolder == string.Empty)
                return;

            //ベースフォルダ
            try
            {
                string folder = Path.Combine(basefolder, string.Format("{0}", m_zoom));
                if (Directory.Exists(folder) == false)
                    Directory.CreateDirectory(folder);

                string fullpath = Path.Combine(folder, FileName());

                image.Save(fullpath);
            }
            catch { }
        }

        /// <summary>
        /// ロード
        /// </summary>
        /// <param name="basefolder"></param>
        protected bool Load(string basefolder)
        {
            if (basefolder == string.Empty)
                return false;

            //ベースフォルダ
            string folder = Path.Combine(basefolder, string.Format("{0}", m_zoom));
            if (Directory.Exists(folder) == false)
            {
                return false;
            }

            string fullpath = Path.Combine(folder, FileName());
            if (File.Exists(fullpath) == false)
                return false;

            try
            {
                using (FileStream fs = new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var image = new Bitmap(fs);
                    m_image = image.Clone(new Rectangle(0,0,image.Width,image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                }

                return true;
            }
            catch
            {
                m_image = null;
                return false;
            }
        }

        /// <summary>
        /// キャッシュ中
        /// </summary>
        /// <returns></returns>
        public virtual bool IsCashed(string basefolder)
        {
            if (m_exclusive.WaitOne(0, false) == false)
                return false;
            try
            {
                if (m_image != null)
                    return true;
            }
            finally
            {
                m_exclusive.Set();
            }

            if (basefolder == string.Empty)
                return false;

            //ベースフォルダ
            string folder = Path.Combine(basefolder, string.Format("{0}", m_zoom));
            string fullpath = Path.Combine(folder, FileName());

            return File.Exists(fullpath);
        }
    }
}
