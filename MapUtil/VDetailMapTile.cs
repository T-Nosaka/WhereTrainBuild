using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WhereTrainBuild.MapUtil
{
    /// <summary>
    /// 詳細側仮想地図タイル
    /// ※対象地図画像が無い時用の詳細側地図タイル
    /// </summary>
    public class VDetailMapTile : MapTile
    {
        /// <summary>
        /// 元タイル
        /// </summary>
        protected MapTile m_srctile = null;

        /// <summary>
        /// 自身の位置
        /// </summary>
        protected CornerType m_corner = CornerType.LeftTop;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VDetailMapTile(CornerType corner, MapTile srctile, int tile_x, int tile_y, int zoom)
            : base(tile_x, tile_y, zoom)
        {
            m_srctile = srctile;
            m_corner = corner;
        }

        /// <summary>
        /// 隅取得
        /// </summary>
        /// <returns></returns>
        public override Point GetCorner(CornerType corner)
        {
            //元タイルの中心取得
            int dSrcTileCenterX = (int)(m_srctile.TilePosition.X * MercatorTrans.ViewSize + MercatorTrans.ViewSize / 2.0d);
            int dSrcTileCenterY = (int)(m_srctile.TilePosition.Y * MercatorTrans.ViewSize + MercatorTrans.ViewSize / 2.0d);

            //元タイル
            Point srcpoint = new Point(0, 0);
            switch (m_corner)
            {
                case CornerType.LeftTop:
                    srcpoint = m_srctile.GetCorner(CornerType.LeftTop);
                    break;
                case CornerType.RightTop:
                    srcpoint = new Point(dSrcTileCenterX, m_srctile.GetCorner(CornerType.LeftTop).Y);
                    break;
                case CornerType.LeftBottom:
                    srcpoint = new Point(m_srctile.GetCorner(CornerType.LeftTop).X, dSrcTileCenterY);
                    break;
                case CornerType.RightBottom:
                    srcpoint = new Point(dSrcTileCenterX,dSrcTileCenterY);
                    break;
            }

            switch (corner)
            {
                case CornerType.LeftTop:
                    {
                        return srcpoint;
                    }

                case CornerType.RightTop:
                    {
                        return new Point(srcpoint.X + m_width-1, srcpoint.Y);
                    }

                case CornerType.LeftBottom:
                    {
                        return new Point(srcpoint.X, srcpoint.Y + m_height-1);
                    }

                case CornerType.RightBottom:
                    {
                        return new Point(srcpoint.X + m_width-1, srcpoint.Y + m_height-1);
                    }
            }

            return srcpoint;
        }
    }
}
