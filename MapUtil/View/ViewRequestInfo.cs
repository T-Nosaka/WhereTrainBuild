using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;


namespace WhereTrainBuild.MapUtil.View
{
    /// <summary>
    /// 描画情報
    /// </summary>
    public class ViewRequestInfo
    {
        /// <summary>
        /// 中心座標
        /// ※メルカトル グローバル座標
        /// </summary>
        protected Point m_center = new Point(0, 0);

        /// <summary>
        /// スケール
        /// </summary>
        protected double m_scale = 1.0f;

        /// <summary>
        /// ズームレベル
        /// </summary>
        protected int m_zoomlevel = 15;

        /// <summary>
        /// グラフィクス
        /// </summary>
        protected Graphics m_graphics;

        /// <summary>
        /// 中心座標プロパティ
        /// ※メルカトル グローバル座標
        /// </summary>
        public Point Center
        {
            get
            {
                return m_center;
            }
            set
            {
                m_center = value;
            }
        }

        /// <summary>
        /// スケールプロパティ
        /// </summary>
        public double Scale
        {
            get
            {
                return m_scale;
            }
            set
            {
                m_scale = value;
            }
         }

        /// <summary>
        /// ズームレベルプロパティ
        /// </summary>
        public int ZoomLevel
        {
            get
            {
                return m_zoomlevel;
            }
            set
            {
                //中心座標変換
                double dLat=0,dLon=0;
                GetCenterLatLon(ref dLat, ref dLon);

                m_zoomlevel = value;

                SetCenterLatLon(dLat, dLon);
            }
        }

        /// <summary>
        /// 描画オブジェクトプロパティ
        /// </summary>
        public Size ViewSize
        { get; set; }

        /// <summary>
        /// グラフィクスプロパティ
        /// </summary>
        public Graphics ViewGraphics
        {
            get
            {
                return m_graphics;
            }
            set
            {
                m_graphics = value;
            }
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewRequestInfo()
        {
        }

        /// <summary>
        /// 度数定数
        /// </summary>
        protected static double m_freq_const = 180.0d / Math.PI;

        /// <summary>
        /// 描画座標クリップ領域取得
        /// ※メルカトル座標系
        /// </summary>
        /// <returns></returns>
        public Rectangle ClipArea()
        {
            int iWidth = (int)(((double)ViewSize.Width / m_scale));
            int iHeight = (int)(((double)ViewSize.Height / m_scale));

            int iLeft = m_center.X - iWidth / 2;
            int iTop = m_center.Y - iHeight / 2;

            return new Rectangle(iLeft, iTop, iWidth, iHeight);
        }

        /// <summary>
        /// 中心緯度経度取得
        /// </summary>
        /// <param name="dLat"></param>
        /// <param name="dLon"></param>
        public void GetCenterLatLon(ref double dLat, ref double dLon)
        {
            MercatorTrans.Reverse(Center.X, Center.Y, ZoomLevel, ref dLat, ref dLon);

            dLat = dLat * m_freq_const;
            dLon = dLon * m_freq_const;
        }

        /// <summary>
        /// 中心緯度経度設定
        /// </summary>
        /// <param name="dLat"></param>
        /// <param name="dLon"></param>
        public void SetCenterLatLon(double dLat, double dLon)
        {
            dLat = dLat * Math.PI / 180.0d;
            dLon = dLon * Math.PI / 180.0d;

            double dX=0,dY=0;
            MercatorTrans.Trans(dLat, dLon, ZoomLevel, ref dX, ref dY);
            Center = new Point((int)dX, (int)dY);
        }

        /// <summary>
        /// 画面座標計算
        /// </summary>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <returns></returns>
        public PointF LatLongToViewPoint( double Latitude, double Longitude)
        {
            //自身の座標を計算
            double dMyPositionX = 0;
            double dMyPositionY = 0;

            //メルカトル座標に変換
            MercatorTrans.Trans(Latitude / 180.0d * Math.PI, Longitude / 180.0d * Math.PI, ZoomLevel, ref dMyPositionX, ref dMyPositionY);

            //描画座標クリップ領域
            Rectangle cliparea = ClipArea();

            //描画
            double dMyX = (dMyPositionX - cliparea.Left) * Scale;
            double dMyY = (cliparea.Bottom - dMyPositionY) * Scale;

            return new PointF((float)dMyX, (float)dMyY);
        }
    }
}
