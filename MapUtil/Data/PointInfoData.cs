using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

using WhereTrainBuild.MapUtil.View;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// 描画ポイント情報
    /// </summary>
    public class PointInfoData : ViewPointIF
    {
        /// <summary>
        /// 緯度
        /// </summary>
        protected double m_latitude = 0.0f;

        /// <summary>
        /// 経度
        /// </summary>
        protected double m_longitude = 0.0f;

        /// <summary>
        /// 描画距離
        /// </summary>
        protected int m_hitdistance = 10;

        /// <summary>
        /// 緯度プロパティ
        /// </summary>
        public double Latitude
        {
            get
            {
                return m_latitude;
            }
            set
            {
                m_latitude = value;
            }
        }

        /// <summary>
        /// 経度プロパティ
        /// </summary>
        public double Longitude
        {
            get
            {
                return m_longitude;
            }
            set
            {
                m_longitude = value;
            }
        }

        /// <summary>
        /// 描画距離プロパティ
        /// </summary>
        public int HitDistance
        {
            get
            {
                return m_hitdistance;
            }
            set
            {
                m_hitdistance = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PointInfoData()
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="viewreqinfo">描画情報</param>
        /// <returns>True..描画範囲内 False..範囲外</returns>
        public virtual bool Draw(ViewRequestInfo viewreqinfo)
        {
            //自身の座標を計算
            double dMyPositionX = 0;
            double dMyPositionY=0;

            //メルカトル座標に変換
            MercatorTrans.Trans(Latitude / 180.0d * Math.PI, Longitude / 180.0d * Math.PI, viewreqinfo.ZoomLevel, ref dMyPositionX, ref dMyPositionY);

            //描画座標クリップ領域
            Rectangle cliparea = viewreqinfo.ClipArea();

            //クリップ領域チェック
            if (cliparea.X > dMyPositionX || dMyPositionX > cliparea.Right ||
                cliparea.Y > dMyPositionY || dMyPositionY > cliparea.Bottom)
                return false;

            //描画
            OnDrawGraphics(dMyPositionX - cliparea.Left, cliparea.Bottom - dMyPositionY, viewreqinfo);

            return true;
        }

        /// <summary>
        /// グラフィクス描画
        /// ※引数座標は、メルカトル座標系
        /// </summary>
        /// <param name="dXPosition">水平座標</param>
        /// <param name="dYPosition">垂直座標</param>
        /// <param name="viewreqinfo">描画情報</param>
        protected virtual void OnDrawGraphics( double dXPosition, double dYPosition, ViewRequestInfo viewreqinfo )
        {
        }

        /// <summary>
        /// 当たり判定
        /// </summary>
        /// <param name="iX">画像X座標</param>
        /// <param name="iY">画像Y座標</param>
        /// <returns>True..範囲内 False..範囲外</returns>
        public virtual bool IsHit(int iX, int iY, ViewRequestInfo viewreqinfo)
        {
            //TODO: 描画チェックが要るかも

            //自身の座標を計算
            double dMyPositionX = 0;
            double dMyPositionY = 0;

            //メルカトル座標に変換
            MercatorTrans.Trans(Latitude / 180.0d * Math.PI, Longitude / 180.0d * Math.PI, viewreqinfo.ZoomLevel, ref dMyPositionX, ref dMyPositionY);

            //描画座標クリップ領域
            Rectangle cliparea = viewreqinfo.ClipArea();

            //クリップ領域チェック
            if (cliparea.X > dMyPositionX || dMyPositionX > cliparea.Right ||
                cliparea.Y > dMyPositionY || dMyPositionY > cliparea.Bottom)
                return false;

            //描画
            double dMyX = (dMyPositionX - cliparea.Left) * viewreqinfo.Scale;
            double dMyY = (cliparea.Bottom - dMyPositionY) * viewreqinfo.Scale;

            //距離算出
            double dDistance = Math.Sqrt((dMyX - iX) * (dMyX - iX) + (dMyY - iY) * (dMyY - iY));
            if (HitDistance >= dDistance)
                return true;

            return false;
        }
    }
}
