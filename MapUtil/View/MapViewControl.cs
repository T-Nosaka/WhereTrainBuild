using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace WhereTrainBuild.MapUtil.View
{
    /// <summary>
    /// 地図描画コントロール
    /// </summary>
    public partial class MapViewControl : UserControl
    {
        /// <summary>
        /// 地図タイル管理
        /// </summary>
        protected MapTitleManager m_maptilemanager = new MapTitleManager();

        /// <summary>
        /// 描画エリア
        /// </summary>
        protected ViewArea m_viewarea = new ViewArea();

        /// <summary>
        /// スケール
        /// 20レベルを100倍したスケール
        /// </summary>
        protected int m_scale = 0;

        /// <summary>
        /// スケールプロパティ
        /// 0-1999
        /// </summary>
        public int MapScale
        {
            get
            {
                return m_scale;
            }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 1999)
                    value = 1999;

                int oldvalue = m_scale;

                m_scale = value;

                if (m_scale != oldvalue)
                {
                    CalcScale();
                }
            }
        }

        /// <summary>
        /// 地図タイル管理プロパティ
        /// </summary>
        public MapTitleManager TileManager
        {
            get
            {
                return m_maptilemanager;
            }
        }

        /// <summary>
        /// 描画エリアプロパティ
        /// </summary>
        public ViewArea ViewArea
        {
            get
            {
                return m_viewarea;
            }
        }

        /// <summary>
        /// タイムアウトプロパティ
        /// </summary>
        public int Timeout
        {
            get
            {
                return m_maptilemanager.Timeout;
            }
            set
            {
                m_maptilemanager.Timeout = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapViewControl()
        {
            InitializeComponent();

            if (DesignMode == true)
                return;

            m_maptilemanager.NotifyCompleted += new MapTitleManager.NotifyCompletedMapImageDelegate(OnNotifyCompletedMapImage);


            this.Disposed += (sender, args) =>
            {
                TileManager.Dispose();
            };
        }

        /// <summary>
        /// 地図描画取得完了
        /// </summary>
        /// <param name="maptilemanager"></param>
        /// <param name="maptile"></param>
        protected void OnNotifyCompletedMapImage(MapTitleManager maptilemanager, MapTile maptile, bool result )
        {
            //再描画
            //描画に必要かを判断
            if (maptilemanager.IsContainForRedraw(maptile) == false)
                return;

            try
            {
                if (InvokeRequired == true)
                {
                    Invoke(new MapTitleManager.NotifyCompletedMapImageDelegate(OnNotifyCompletedMapImageImpl), new object[] { maptilemanager, maptile, result });
                }
                else
                {
                    OnNotifyCompletedMapImageImpl(maptilemanager, maptile, result);
                }
            }
            catch { } //終了処理対策
        }

        /// <summary>
        /// 地図描画取得完了実装
        /// </summary>
        /// <param name="maptilemanager"></param>
        /// <param name="maptile"></param>
        protected virtual void OnNotifyCompletedMapImageImpl(MapTitleManager maptilemanager, MapTile maptile, bool result)
        {
            if( result == true )
                RefreshImage();
        }

        /// <summary>
        /// 中心座標自動設定
        /// </summary>
        public void SetCenter()
        {
            m_viewarea.SetCenter(Width, Height);
        }

        /// <summary>
        /// ズームレベル変更でキャッシュリセット
        /// </summary>
        protected bool m_changezoom_resetcache = true;

        /// <summary>
        /// ズームレベル変更でキャッシュリセットプロパティ
        /// </summary>
        public bool ChangeZoomResetCache
        {
            get
            {
                return m_changezoom_resetcache;
            }
            set
            {
                m_changezoom_resetcache = value;
            }
        }

        /// <summary>
        /// スケール変更による再計算
        /// ズームレベルと画像縮尺
        /// </summary>
        protected void CalcScale()
        {
            int oldzoomlevel = m_viewarea.ViewInfo.ZoomLevel;

            //ズームレベル計算
            int iZoom = MapScale / 100 + 1;
            m_viewarea.ViewInfo.ZoomLevel = iZoom;

            //ズームレベル内の画像縮尺( 50-99% )
            int iScale = (MapScale % 100) + 100;
            m_viewarea.ViewInfo.Scale = (double)((iScale >> 1) / 100.0d);

            if (oldzoomlevel != iZoom)
            {
                if (m_changezoom_resetcache == true)
                {
                    //ズームレベルチェンジで、メモリキャッシュ破棄とする
                    TileManager.ResetMemoryCache();
                }
            }
        }

        /// <summary>
        /// ドラッグポイント
        /// </summary>
        protected Point m_dragpoint = new Point(0, 0);

        /// <summary>
        /// ドラッグポイント
        /// メルカトル座標
        /// </summary>
        protected Point m_dragmercatorpoint = new Point(0, 0);

        /// <summary>
        /// ドラッグ状態
        /// </summary>
        protected bool m_dragenable = false;

        /// <summary>
        /// マウスダウン通知イベント型
        /// </summary>
        /// <param name="dLat"></param>
        /// <param name="dLng"></param>
        public delegate void OnMercatorMouseDownDelegate(Point viewpoint, double dLat, double dLng, MouseEventArgs e);

        /// <summary>
        /// マウスダウン通知イベント
        /// </summary>
        public event OnMercatorMouseDownDelegate OnMercatorDownPoint;

        /// <summary>
        /// マウスダウンイベント発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapViewControl_MouseDown(object sender, MouseEventArgs e)
        {
            m_dragpoint = PointToClient(Cursor.Position);
            m_dragmercatorpoint = m_viewarea.ViewInfo.Center;

            m_dragenable = true;

            if (OnMercatorDownPoint != null)
            {
                Point viewpoint = m_dragpoint;

                var diffX = (viewpoint.X - Width * 0.5) / m_viewarea.ViewInfo.Scale;
                var diffY = (viewpoint.Y - Height * 0.5) / m_viewarea.ViewInfo.Scale;

                double x = m_viewarea.ViewInfo.Center.X + diffX;
                double y = m_viewarea.ViewInfo.Center.Y - diffY;

                double dLat = 0.0, dLng = 0.0;

                MercatorTrans.Reverse(x, y, m_viewarea.ViewInfo.ZoomLevel, ref dLat, ref dLng);

                OnMercatorDownPoint(viewpoint, latlontool.ToAngle(dLat), latlontool.ToAngle(dLng), e);
            }

/*
            foreach (var pnt in m_viewarea.GetInsidePoint(m_dragpoint.X, m_dragpoint.Y))
            {
            }
*/
        }

        /// <summary>
        /// 白ブラシ
        /// </summary>
        protected SolidBrush m_whitebrush = new SolidBrush(Color.White);

        /// <summary>
        /// ラジアン定数
        /// </summary>
        protected static double m_rad_const = Math.PI / 180.0d;

        /// <summary>
        /// バックイメージ取得
        /// </summary>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        protected void DrawBackImage(Graphics gr, int iWidth, int iHeight)
        {
            double dLat = 0.0;
            double dLon = 0.0;
            m_viewarea.ViewInfo.GetCenterLatLon(ref dLat, ref dLon);

            gr.FillRectangle(m_whitebrush, 0, 0, iWidth, iHeight);

            m_maptilemanager.DrawImage(gr, dLat * m_rad_const, dLon * m_rad_const, iWidth, iHeight, m_viewarea.ViewInfo.ZoomLevel, m_viewarea.ViewInfo.Scale);
        }

        /// <summary>
        /// 画像描画
        /// </summary>
        /// <returns></returns>
        protected void DrawImage(Graphics gr)
        {
            DrawBackImage(gr, Width, Height);

            m_viewarea.DrawImage(gr, new Size(Width, Height));
        }

        /// <summary>
        /// マウス位置通知イベント型
        /// </summary>
        /// <param name="dLat"></param>
        /// <param name="dLng"></param>
        public delegate void OnMercatorMousePointDelegate(Point viewpoint, double dLat, double dLng, MouseEventArgs e);

        /// <summary>
        /// マウス位置通知イベント
        /// </summary>
        public event OnMercatorMousePointDelegate OnMercatorMousePoint;

        /// <summary>
        /// マウスムーブイベント発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapViewControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point viewpoint = PointToClient(Cursor.Position);

            if (m_dragenable == true )
            {
                //移動量計算
                int iDeltaX = m_dragpoint.X - viewpoint.X;
                int iDeltaY = m_dragpoint.Y - viewpoint.Y;

                m_viewarea.MoveCenter(iDeltaX, iDeltaY, m_dragmercatorpoint);

                RefreshImage();
            }

            if (OnMercatorMousePoint != null)
            {
                var gP = ReverseTrans(viewpoint);

                OnMercatorMousePoint(viewpoint, gP.lat, gP.lng, e);
            }
        }

        /// <summary>
        /// 逆変換
        /// </summary>
        /// <param name="viewpoint"></param>
        /// <returns></returns>
        public latlontool.latlng ReverseTrans(PointF viewpoint)
        {
            var diffX = (viewpoint.X - Width * 0.5) / m_viewarea.ViewInfo.Scale;
            var diffY = (viewpoint.Y - Height * 0.5) / m_viewarea.ViewInfo.Scale;

            double x = m_viewarea.ViewInfo.Center.X + diffX;
            double y = m_viewarea.ViewInfo.Center.Y - diffY;

            double dLat = 0.0, dLng = 0.0;

            MercatorTrans.Reverse(x, y, m_viewarea.ViewInfo.ZoomLevel, ref dLat, ref dLng);

            latlontool.latlng result = new latlontool.latlng(latlontool.ToAngle(dLat), latlontool.ToAngle(dLng));
            return result;
        }

        /// <summary>
        /// 描画更新
        /// </summary>
        public virtual void RefreshImage()
        {
            Invalidate();
        }

        /// <summary>
        /// マウスアップ通知イベント型
        /// </summary>
        /// <param name="dLat"></param>
        /// <param name="dLng"></param>
        public delegate void OnMercatorMouseUpDelegate(Point viewpoint, double dLat, double dLng, MouseEventArgs e);

        /// <summary>
        /// マウスアップ通知イベント
        /// </summary>
        public event OnMercatorMouseUpDelegate OnMercatorUpPoint;

        /// <summary>
        /// マウスアップイベント発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapViewControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_dragenable == true)
            {
                m_dragenable = false;
            }

            if (OnMercatorUpPoint != null)
            {
                Point viewpoint = PointToClient(Cursor.Position);

                var diffX = (viewpoint.X - Width * 0.5) / m_viewarea.ViewInfo.Scale;
                var diffY = (viewpoint.Y - Height * 0.5) / m_viewarea.ViewInfo.Scale;

                double x = m_viewarea.ViewInfo.Center.X + diffX;
                double y = m_viewarea.ViewInfo.Center.Y - diffY;

                double dLat = 0.0, dLng = 0.0;

                MercatorTrans.Reverse(x, y, m_viewarea.ViewInfo.ZoomLevel, ref dLat, ref dLng);

                OnMercatorUpPoint(viewpoint, latlontool.ToAngle(dLat), latlontool.ToAngle(dLng), e);
            }
        }

        /// <summary>
        /// マウスリーブイベント発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapViewControl_MouseLeave(object sender, EventArgs e)
        {
            if (m_dragenable == true)
            {
                m_dragenable = false;
            }
        }

        /// <summary>
        /// ベイントイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void MapViewControl_Paint(object sender, PaintEventArgs e)
        {
            if (DesignMode == false)
            {
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                DrawImage(e.Graphics);
            }
        }

        /// <summary>
        /// リサイズイベント発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapViewControl_Resize(object sender, EventArgs e)
        {
            if (DesignMode == true)
                return;

            RefreshImage();
        }
    }
}
