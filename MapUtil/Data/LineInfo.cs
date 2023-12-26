using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WhereTrainBuild.MapUtil.View;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// 線描画用
    /// </summary>
    public class LineInfo : ViewPointIF
    {
        /// <summary>
        /// 位置リスト
        /// </summary>
        protected List<latlontool.latlng> m_list = new List<latlontool.latlng>();

        /// <summary>
        /// 位置リスト
        /// </summary>
        /// <returns></returns>
        public latlontool.latlng[] ToList()
        {
            return m_list.ToArray();
        }

        /// <summary>
        /// 描画フラグ
        /// </summary>
        protected bool m_visible = false;

        /// <summary>
        /// 描画フラグプロパティ
        /// </summary>
        public bool Visible
        {
            get
            {
                return m_visible;
            }
            set
            {
                m_visible = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LineInfo()
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="viewreqinfo">描画情報</param>
        /// <returns>True..描画範囲内 False..範囲外</returns>
        public bool Draw(ViewRequestInfo viewreqinfo)
        {
            if (Visible == false)
                return true;

            if (m_list.Count <= 1)
                return true;

            //描画座標クリップ領域
            Rectangle cliparea = viewreqinfo.ClipArea();

            List<Point> lines = new List<Point>();
            foreach (var latlng in m_list)
            {
                //自身の座標を計算
                double dMyPositionX = 0;
                double dMyPositionY = 0;

                //メルカトル座標に変換
                MercatorTrans.Trans(latlng.lat / 180.0d * Math.PI, latlng.lng / 180.0d * Math.PI, viewreqinfo.ZoomLevel, ref dMyPositionX, ref dMyPositionY);

                //TODO: 描画クリップは、全体の一部に対して、内積(外積?)やったっけを取る必要があるな

                //描画ポイントセット
                lines.Add(new Point((int)((dMyPositionX - cliparea.Left) * viewreqinfo.Scale), (int)((cliparea.Bottom - dMyPositionY) * viewreqinfo.Scale)));
            }

            viewreqinfo.ViewGraphics.DrawLines(new Pen(Color.Black, 2), lines.ToArray());

            return true;
        }

        /// <summary>
        /// 当たり判定
        /// </summary>
        /// <param name="iX">画像X座標</param>
        /// <param name="iY">画像Y座標</param>
        /// <returns>True..範囲内 False..範囲外</returns>
        public bool IsHit(int iX, int iY, ViewRequestInfo viewreqinfo)
        {
            return false;
        }

        /// <summary>
        /// 位置追加
        /// </summary>
        /// <param name="pos"></param>
        public void AddPosition(latlontool.latlng pos)
        {
            m_list.Add(pos);
        }

        /// <summary>
        /// 位置初期化
        /// </summary>
        public void Clear()
        {
            m_list.Clear();
        }
    }
}
