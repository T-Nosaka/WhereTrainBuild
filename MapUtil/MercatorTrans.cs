using System;
using System.Collections.Generic;
using System.Text;

namespace WhereTrainBuild.MapUtil
{
    /// <summary>
    /// メルカトル変換
    /// </summary>
    public class MercatorTrans
    {
        /// <summary>
        /// マップの画像幅
        /// </summary>
        protected static double m_view_size = 256.0d;

        /// <summary>
        /// マップの画像幅半分
        /// </summary>
        protected static double m_viewhalfsize = m_view_size / 2.0d;

        /// <summary>
        /// マップの画像幅
        /// </summary>
        public static double ViewSize
        {
            get
            {
                return m_view_size;
            }
        }

        /// <summary>
        /// R
        /// </summary>
        protected static double R = m_viewhalfsize / Math.PI;

        /// <summary>
        /// 変換
        /// </summary>
        /// <param name="dLat"></param>
        /// <param name="dLon"></param>
        public static void Trans(double dLat, double dLon, int iZoom, ref double dX, ref double dY)
        {
            //R

            //ピクセル座標
            var d2P = Math.Pow(2, iZoom);
            dX = (R * (dLon + Math.PI)) * d2P;
            var dLatSin = Math.Sin(dLat);
            dY = (R / 2.0d * Math.Log((1.0d + dLatSin) / (1.0d - dLatSin)) - m_viewhalfsize) * d2P;

            return;
        }

        /// <summary>
        /// 逆変換
        /// X座標をリミットオーバーしないようにすべし
        /// </summary>
        /// <param name="dX"></param>
        /// <param name="dY"></param>
        /// <param name="iZoom"></param>
        public static void Reverse(double dX, double dY, int iZoom, ref double dLat, ref double dLon)
        {
            //R

            var d2P = Math.Pow(2, iZoom);
            double dSrcX = dX / d2P;
            double dSrcY = dY / d2P;

            //ATAN(SINH((128-C29)/$C$3))*180/PI()
            dLat = Math.Atan(Math.Sinh((dSrcY + m_viewhalfsize) / R));
            //=(C28/$C$3-PI())*180/PI()
            dLon = (dSrcX / R - Math.PI);
        }

    }
}
