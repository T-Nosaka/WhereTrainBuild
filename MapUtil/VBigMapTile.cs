using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WhereTrainBuild.MapUtil
{
    /// 大きい側仮想地図タイル
    /// ※対象地図画像が無い時用の大きい側地図タイル
    public class VBigMapTile : MapTile
    {
        /// <summary>
        /// 元タイル
        /// </summary>
        protected MapTile m_srctile = null;

        /// <summary>
        /// 親タイル
        /// </summary>
        protected VBigMapTile m_parent = null;

        /// <summary>
        /// 元の位置
        /// </summary>
        protected CornerType m_corner = CornerType.LeftTop;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VBigMapTile(MapTile srctile, int tile_x, int tile_y, int zoom)
            : base(tile_x, tile_y, zoom)
        {
            m_srctile = srctile;

            if (srctile.TilePosition.X % 2 == 1)
            {
                if (srctile.TilePosition.Y % 2 == 1)
                {
                    //右下
                    m_corner = CornerType.RightBottom;
                }
                else
                {
                    //右上
                    m_corner = CornerType.RightTop;
                }
            }
            else
            {
                if (srctile.TilePosition.Y % 2 == 1)
                {
                    //左下
                    m_corner = CornerType.LeftBottom;
                }
                else
                {
                    //左上
                    m_corner = CornerType.LeftTop;
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VBigMapTile(VBigMapTile srctile, int tile_x, int tile_y, int zoom)
            : base(tile_x, tile_y, zoom)
        {
            m_srctile = srctile.m_srctile;
            m_parent = srctile;

            if (srctile.TilePosition.X % 2 == 1)
            {
                if (srctile.TilePosition.Y % 2 == 1)
                {
                    //右下
                    m_corner = CornerType.RightBottom;
                }
                else
                {
                    //右上
                    m_corner = CornerType.RightTop;
                }
            }
            else
            {
                if (srctile.TilePosition.Y % 2 == 1)
                {
                    //左下
                    m_corner = CornerType.LeftBottom;
                }
                else
                {
                    //左上
                    m_corner = CornerType.LeftTop;
                }
            }
        }

        /// <summary>
        /// 拡大サイズ取得
        /// </summary>
        /// <returns></returns>
        public override MapTile GetBigTile()
        {
            if (m_zoom == 0)
                return null;

            return new VBigMapTile(this, m_tile_x / 2, m_tile_y / 2, m_zoom - 1);
        }

        /// <summary>
        /// 隅取得
        /// </summary>
        /// <returns></returns>
        public override Point GetCorner(CornerType corner)
        {
            return m_srctile.GetCorner(corner);
        }

        /// <summary>
        /// 画像取得
        /// </summary>
        /// <returns></returns>
        public override Image GetImage(string basefolder, int timeout)
        {
            Image img = base.GetImage(basefolder, timeout);
            if (img == null)
                return null;

            //サイズ累乗数
            int dRui = 2;

            //移動量算出
            Point offset = new Point(0,0);
            VBigMapTile parenttile = this;
            while (true)
            {
                int dWidthRui = img.Width / dRui;
                int dHeightRui = img.Height / dRui;

                switch (parenttile.m_corner)
                {
                    case CornerType.RightTop:
                        offset = new Point(offset.X + dWidthRui-1, offset.Y);
                        break;
                    case CornerType.LeftBottom:
                        offset = new Point(offset.X, offset.Y + dHeightRui-1);
                        break;
                    case CornerType.RightBottom:
                        offset = new Point(offset.X + dWidthRui-1, offset.Y + dHeightRui-1);
                        break;
                }

                parenttile = parenttile.m_parent;
                if (parenttile == null)
                    break;

                dRui = dRui*2;
            }

            Bitmap destimage = new Bitmap(img.Width, img.Height);
            using (Graphics srcgraphics = Graphics.FromImage(destimage))
            {
                srcgraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                srcgraphics.DrawImage(img, 
                    new Rectangle(0, 0, img.Width, img.Height),
                    offset.X, offset.Y, img.Width / dRui, img.Height / dRui,
                    GraphicsUnit.Pixel);
            }

            return destimage;
        }
    }
}
