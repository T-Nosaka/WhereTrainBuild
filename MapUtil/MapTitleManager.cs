using System;
using System.Linq;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WhereTrainBuild.MapUtil
{
    /// <summary>
    /// 地図タイル管理
    /// </summary>
    public class MapTitleManager : IDisposable
    {
        /// <summary>
        /// 地図キャッシュフォルダ
        /// </summary>
        protected string m_basefolder = string.Empty;

        /// <summary>
        /// タイムアウト
        /// </summary>
        protected int m_timeout = 5000;

        /// <summary>
        /// タイムアウトプロパティ
        /// </summary>
        public int Timeout
        {
            get
            {
                return m_timeout;
            }
            set
            {
                m_timeout = value;
            }
        }

        /// <summary>
        /// メモリキャッシュ
        /// </summary>
        protected Dictionary<string, MapTile> m_cache = new Dictionary<string, MapTile>();

        /// <summary>
        /// メモリキャッシュされているタイルを取得する
        /// </summary>
        /// <param name="iX"></param>
        /// <param name="iY"></param>
        /// <returns></returns>
        public MapTile GetMapTile( int iX, int iY, int iZoom )
        {
            lock(m_cache)
            {
                var key = MapTile.MakeKey(iX, iY, iZoom);

                if (m_cache.ContainsKey(key) == true)
                    return m_cache[key];
            }

            return null;
        }

        /// <summary>
        /// メモリキャッシュされているタイルを全取得する
        /// </summary>
        /// <returns></returns>
        public MapTile[] GetMapTileArray()
        {
            lock (m_cache)
            {
                return m_cache.Values.ToArray();
            }
        }

        /// <summary>
        /// 地図キャッシュフォルダプロパティ
        /// </summary>
        public string BaseFolder
        {
            get
            {
                return m_basefolder;
            }
            set
            {
                m_basefolder = value;
            }
        }

        /// <summary>
        /// タイル取得フラグ
        /// </summary>
        protected bool m_nativemethod = true;

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
        /// コンストラクタ
        /// </summary>
        public MapTitleManager()
        {
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        public virtual void Dispose()
        {
            ResetMemoryCache();
        }

        /// <summary>
        /// 地図タイル構築型
        /// </summary>
        /// <param name="tile_x"></param>
        /// <param name="tile_y"></param>
        /// <param name="zoom"></param>
        public delegate MapTile BuildMapTileDelegate(int tile_x, int tile_y, int zoom);

        /// <summary>
        /// 地図タイル構築コールバック
        /// </summary>
        protected BuildMapTileDelegate m_buildmaptile = null;

        /// <summary>
        /// 地図タイル構築コールバック設定
        /// </summary>
        /// <param name="buildmaptile"></param>
        public void SetBuildMapTile(BuildMapTileDelegate buildmaptile)
        {
            m_buildmaptile = buildmaptile;
        }

        /// <summary>
        /// 地図タイル構築
        /// </summary>
        /// <param name="tile_x"></param>
        /// <param name="tile_y"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public MapTile BuildMapTile(int tile_x, int tile_y, int zoom)
        {
            if (m_buildmaptile != null)
                return m_buildmaptile(tile_x, tile_y, zoom);
            else
                return new MapTile(tile_x, tile_y, zoom);
        }

        /// <summary>
        /// タイル取得
        /// </summary>
        /// <param name="dLatitude"></param>
        /// <param name="dLongitude"></param>
        /// <returns></returns>
        public MapTile GetTile(double dLatitude, double dLongitude, int zoom)
        {
            //メルカトル変換
            double dX = 0, dY = 0;
            MercatorTrans.Trans(dLatitude, dLongitude, zoom, ref dX, ref dY);

            int iTileX = (int)(dX / MercatorTrans.ViewSize);
            int iTileY = (int)(-dY / MercatorTrans.ViewSize);

            return BuildMapTile(iTileX, iTileY, zoom);
        }

        /// <summary>
        /// 黒ブラシ
        /// </summary>
        protected SolidBrush m_blackbrush = new SolidBrush(Color.Black);

        /// <summary>
        /// 地図描画
        /// </summary>
        /// <returns></returns>
        public void DrawImage(Graphics gr, double dLatitude, double dLongitude, int iWidth, int iHeight, int zoom, double dScale)
        {
            //画像スケール
            int iSrcWidth = (int)((double)iWidth / dScale);
            int iSrcHeight = (int)((double)iHeight / dScale);

            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            gr.FillRectangle(m_blackbrush, new Rectangle(0, 0, iWidth, iHeight));
            gr.ScaleTransform((float)dScale, (float)dScale);

            DrawImage(gr, dLatitude, dLongitude, iSrcWidth, iSrcHeight, zoom);

            gr.ResetTransform();
        }

        /// <summary>
        /// メモリキャッシュ消去
        /// </summary>
        public void ResetMemoryCache()
        {
            List<MapTile> cachelist = null;
            lock (m_cache)
            {
                cachelist = new List<MapTile>(m_cache.Values);
                m_cache.Clear();
            }

            foreach (MapTile maptile in cachelist)
            {
                maptile.CancelGetMap();
            }
        }

        /// <summary>
        /// タイル描画必要判定
        /// </summary>
        /// <param name="dLatitude"></param>
        /// <param name="dLongitude"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <param name="zoom"></param>
        /// <param name="dScale"></param>
        /// <param name="maptile"></param>
        /// <returns></returns>
        public bool IsContainForRedraw(MapTile maptile )
        {
            lock (m_last_maptile_view)
            {
                foreach (MapTile currentview in m_last_maptile_view)
                {
                    if (currentview.TilePosition == maptile.TilePosition && currentview.Zoom == maptile.Zoom)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 最終描画対象
        /// </summary>
        protected List<MapTile> m_last_maptile_view = new List<MapTile>();

        /// <summary>
        /// 地図描画
        /// </summary>
        /// <returns></returns>
        protected virtual void DrawImage(Graphics mapgraphics, double dLatitude, double dLongitude, int width, int height, int zoom)
        {
            //左上座標算出

            //中心座標算出 メルカトル変換
            double dX = 0, dY = 0;
            MercatorTrans.Trans(dLatitude, dLongitude, zoom, ref dX, ref dY);

            //左上算出
            var iFindX = (int)(dX - width * 0.5);
            var iFindY = (int)(-dY - height * 0.5);

            int iTileX = (int)(iFindX / MercatorTrans.ViewSize);
            int iTileY = (int)(iFindY / MercatorTrans.ViewSize);

            int iTileX_Right = (int)((iFindX + width) / MercatorTrans.ViewSize);
            int iTileY_Bottom = (int)((iFindY + height) / MercatorTrans.ViewSize);

            //最終描画対象を再構築
            lock (m_last_maptile_view)
            {
                m_last_maptile_view.Clear();
                for (int iX = iTileX; iX <= iTileX_Right; iX++)
                {
                    for (int iY = iTileY; iY <= iTileY_Bottom; iY++)
                    {
                        //座標のみ
                        MapTile maptile = BuildMapTile(iX, iY, zoom);
                        m_last_maptile_view.Add(maptile);
                    }
                }
            }

            //描画に必要なタイル
            var needslist = new List<MapTile>();

            {
                for (int iX = iTileX; iX <= iTileX_Right; iX++)
                {
                    for (int iY = iTileY; iY <= iTileY_Bottom; iY++)
                    {
                        MapTile maptile = null;
                        string key = MapTile.MakeKey(iX, iY, zoom);

                        lock (m_cache)
                        {
                            //地図取得
                            if (m_cache.ContainsKey(key) == true)
                            {
                                maptile = m_cache[key];
                            }
                            else
                            {
                                maptile = BuildMapTile(iX, iY, zoom);
                                //メモリキャッシュ
                                m_cache[key] = maptile;
                            }
                        }

                        needslist.Add(maptile);

                        lock (maptile)
                        {
                            if (maptile.IsCashed(BaseFolder) == true)
                            {
                                //ロード済みの場合、描画
                                Image img = maptile.GetImage(BaseFolder, Timeout);
                                if (img == null)
                                {
                                    lock (m_cache)
                                    {
                                        //キャッシュからぬく
                                        m_cache.Remove(key);
                                        continue;
                                    }
                                }

                                var lefttop = maptile.GetCorner(MapTile.CornerType.LeftTop);
                                mapgraphics.DrawImage(img, (lefttop.X - iFindX), (lefttop.Y - iFindY), (float)MercatorTrans.ViewSize + 1, (float)MercatorTrans.ViewSize + 1);
                                continue;
                            }
                        }

                        //仮想タイル描画
                        DrawVirtualTile(new Point(iFindX, iFindY), maptile, mapgraphics);

                        //ロード予約
                        RequestGetMap(maptile);
                    }
                }
            }

            DoCancelRequest(needslist);
        }

        /// <summary>
        /// 要求リストから不必要なタイルを取り除く排他
        /// </summary>
        protected AutoResetEvent m_cancelrequest_ex = new AutoResetEvent(true);

        /// <summary>
        /// 要求リストから不必要なタイルを取り除く
        /// </summary>
        protected void DoCancelRequest(List<MapTile> needslist)
        {
            if (m_cancelrequest_ex.WaitOne(0, false) == false)
                return;

            new Thread(() => 
            {
                try
                {
                    //要求リストから不必要なタイルを取り除く
                    lock (m_request_list)
                    {
                        var ignorelist = new List<MapTile>();

                        foreach (var request in m_request_list)
                        {
                            if (needslist.Exists(needstile => needstile.Key == request.Key) == false)
                            {
                                request.CancelGetMap();

                                ignorelist.Add(request);
                            }
                        }

                        foreach (var ignore in ignorelist)
                        {
                            m_request_list.Remove(ignore);
                        }
                    }
                }
                finally
                {
                    m_cancelrequest_ex.Set();
                }
            }).Start();
        }

        /// <summary>
        /// 要求リスト
        /// </summary>
        protected List<MapTile> m_request_list = new List<MapTile>();

        /// <summary>
        /// 地図画像要求
        /// </summary>
        /// <param name="maptile"></param>
        protected virtual void RequestGetMap(MapTile maptile)
        {
            lock (m_request_list)
            {
                if (m_request_list.Find(target => target == maptile) != null)
                    return;
            }

            //ロード予約
            if (NotifyRequested != null)
                NotifyRequested(this, maptile);

            maptile.NativeMathod = NativeMathod;

            if (maptile.GetImageAsync(BaseFolder, Timeout, (pmaptile, result) =>
            {
                lock (m_request_list)
                {
                    m_request_list.Remove(pmaptile);
                }

                if (NotifyCompleted != null)
                    NotifyCompleted(this, maptile, result);

            }) == false)
            {
                lock (m_request_list)
                {
                    m_request_list.Remove(maptile);
                }

                if (NotifyCompleted != null)
                    NotifyCompleted(this, maptile, false);
            }
            else
            {
                lock (m_request_list)
                {
                    m_request_list.Add(maptile);
                }
            }
        }

        /// <summary>
        /// 仮想タイル描画
        /// </summary>
        protected void DrawVirtualTile(Point lefttop, MapTile targettile, Graphics mapgraphics)
        {
            //詳細タイル確認
            bool bExists = true;
            foreach (MapTile maptile in targettile.GetSmallTile())
            {
                MapTile vmaptile = maptile;
                lock (m_cache)
                {
                    if (m_cache.ContainsKey(maptile.Key) == true)
                    {
                        vmaptile = m_cache[maptile.Key];
                    }
                }
                if (vmaptile.IsCashed(BaseFolder) == true)
                {
                    Image img = maptile.GetImage(BaseFolder, Timeout);
                    if (img != null)
                    {
                        //描画
                        Point maplefttop = maptile.GetCorner(MapTile.CornerType.LeftTop);
                        mapgraphics.DrawImage(img,
                            new Rectangle((maplefttop.X - lefttop.X), (maplefttop.Y - lefttop.Y), img.Width/2+1, img.Height/2+1 ));
                    }
                }
                else
                {
                    bExists = false;
                    break;
                }
            }

            if (bExists == true)
                return;

            //無かった場合、大きい解像度で補う
            MapTile vbigmaptile = targettile;
            while (true)
            {
                vbigmaptile = vbigmaptile.GetBigTile();
                if (vbigmaptile == null)
                    return;
                lock (m_cache)
                {
                    if (m_cache.ContainsKey(vbigmaptile.Key) == true)
                    {
                        vbigmaptile = m_cache[vbigmaptile.Key];
                    }
                }
                if (vbigmaptile.IsCashed(BaseFolder) == true)
                {
                    Image img = vbigmaptile.GetImage(BaseFolder, Timeout);
                    if (img != null)
                    {
                        //描画
                        Point maplefttop = vbigmaptile.GetCorner(MapTile.CornerType.LeftTop);
                        mapgraphics.DrawImage(img, (maplefttop.X - lefttop.X), (maplefttop.Y - lefttop.Y), (float)MercatorTrans.ViewSize + 1, (float)MercatorTrans.ViewSize + 1);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 地図描画取得要求型
        /// </summary>
        /// <param name="maptilemanager"></param>
        /// <param name="maptile"></param>
        /// <returns></returns>
        public delegate void NotifyRequestMapImageDelegate(MapTitleManager maptilemanager, MapTile maptile);

        /// <summary>
        /// 地図描画取得要求イベント
        /// </summary>
        public event NotifyRequestMapImageDelegate NotifyRequested;

        /// <summary>
        /// 地図描画取得完了型
        /// </summary>
        /// <param name="maptilemanager"></param>
        /// <param name="maptile"></param>
        /// <returns></returns>
        public delegate void NotifyCompletedMapImageDelegate( MapTitleManager maptilemanager, MapTile maptile, bool result );

        /// <summary>
        /// 地図描画取得完了イベント
        /// </summary>
        public event NotifyCompletedMapImageDelegate NotifyCompleted;
    }
}
