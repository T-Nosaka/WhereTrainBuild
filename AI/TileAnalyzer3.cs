using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Web.Hosting;

namespace WhereTrainBuild.AI
{
    /// <summary>
    /// タイル分析
    /// 色判別
    /// レベル18専用
    /// </summary>
    public class TileAnalyzer3
    {
        /// <summary>
        /// 全体閾値
        /// </summary>
        protected int m_totallimit = 10;

        /// <summary>
        /// 全体閾値プロパティ
        /// </summary>
        public int TotalLimit
        {
            get
            {
                return m_totallimit;
            }
            set
            {
                m_totallimit = value;
            }
        }

        /// <summary>
        /// 閾値
        /// </summary>
        protected float m_limit = 0.70f;

        /// <summary>
        /// 閾値プロパティ
        /// </summary>
        public float Limit
        {
            get
            {
                return m_limit;
            }
            set
            {
                m_limit = value;
            }
        }

        /// <summary>
        /// 対象色
        /// </summary>
        protected Color m_target_color = Color.FromArgb(255, 109, 15, 15);

        /// <summary>
        /// 対象色プロパティ
        /// </summary>
        public Color TargetColor
        {
            get
            {
                return m_target_color;
            }
            set
            {
                m_target_color = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TileAnalyzer3()
        {
        }

        /// <summary>
        /// 判定
        /// </summary>
        /// <param name="image">画像</param>
        /// <returns>結果</returns>
        public (bool Result ,List<Point> Points ) Decide(Bitmap image)
        {
            //重み
            float iTotalWeight = 0;

            var target_hue = TargetColor.GetHue();
            var target_sat = TargetColor.GetSaturation();
            var target_br = TargetColor.GetBrightness();

            var pointlist = new List<Point>();

            for (int iW = 0; iW < image.Width; iW++)
            {
                for (int iH = 0; iH < image.Height; iH++)
                {
                    var color = image.GetPixel(iW, iH);

                    //対象物を絞り込む為に勾配をつける
                    var iTWeight = Weight(color, TargetColor);
                    if (iTWeight > Limit)
                    {
                        pointlist.Add(new Point(iW, iH));

                        iTotalWeight += iTWeight;
                    }
                }
            }

            if (iTotalWeight >= TotalLimit)
                return (true, pointlist);

            return ( false, pointlist);
        }

        /// <summary>
        /// ウエイト計算
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static float Weight(Color color, Color targetcolor )
        {
            var target_hue = targetcolor.GetHue();
            var target_sat = targetcolor.GetSaturation();
            var target_br = targetcolor.GetBrightness();

            //色相 0を中心にする
            var hue = (int)(color.GetHue() - target_hue);
            var iHWeight = (180 - ((hue < 180) ? hue % 180 : 180 - hue % 180)) / 180.0f;

            //色彩 0.75を中心とする
            var sat = color.GetSaturation();
            var iSWeight = 1.0f - (float)Math.Sqrt(Math.Pow(target_sat - sat, 2))*0.3f;

            //輝度 0.392 を中心とする
            var br = color.GetBrightness();
            var iBWeight = 1.0f - (float)Math.Sqrt(Math.Pow(target_br - br, 2))*0.8f;

            //対象物を絞り込む為に勾配をつける
            return (float)Math.Pow((iHWeight * iSWeight * iBWeight), 3);
        }
    }
}
