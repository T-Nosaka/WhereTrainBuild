using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhereTrainBuild.MapUtil
{
    /// <summary>
    /// 緯度経度ツール
    /// </summary>
    public class latlontool
    {
        /// <summary>
        /// 線形座標
        /// </summary>
        public class xyz
        {
            public double x;
            public double y;
            public double z;

            public xyz()
            {
            }

            public xyz(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        /// <summary>
        /// 緯度経度
        /// </summary>
        public class latlng
        {
            /// <summary>
            /// 緯度
            /// </summary>
            protected double m_lat;

            /// <summary>
            /// 経度
            /// </summary>
            protected double m_lng;

            /// <summary>
            /// 緯度プロパティ
            /// </summary>
            public double lat
            {
                get
                {
                    return m_lat;
                }
                set
                {
                    m_lat = value;
                    undefined = false;
                }
            }

            /// <summary>
            /// 経度プロパティ
            /// </summary>
            public double lng
            {
                get
                {
                    return m_lng;
                }
                set
                {
                    m_lng = value;
                    undefined = false;
                }
            }

            /// <summary>
            /// 不定
            /// </summary>
            public bool undefined = true;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public latlng()
            {
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="t"></param>
            /// <param name="g"></param>
            public latlng( double t, double g ) {
                lat = t;
                lng = g;

                undefined = false;
            }

            /// <summary>
            /// 外部セット
            /// </summary>
            /// <param name="other"></param>
            public void Set(latlng other)
            {
                lat = other.lat;
                lng = other.lng;
            }
        }

        /// <summary>
        /// ラジアン
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double ToRadian(double angle)
        {
            return (double)(angle * Math.PI / 180);
        }

        /// <summary>
        /// デグリー
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double ToAngle(double radian)
        {
            return (double)(radian * 180 / Math.PI);
        }

        /// <summary>
        /// 線形変換
        /// </summary>
        /// <param name="latlng"></param>
        /// <returns></returns>
        public static xyz ToXyz(latlng latlng )
        {
            var rlat = ToRadian(latlng.lat);
            var rlng = ToRadian(latlng.lng);

            var coslat = Math.Cos(rlat);

            var result = new xyz();
            result.x = coslat * Math.Cos(rlng);
            result.y = coslat * Math.Sin(rlng);
            result.z = Math.Sin(rlat);

            return result;
        }

        /// <summary>
        /// 緯度経度変換
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static latlng ToLatLng(xyz val)
        {
            var rlat = Math.Asin(val.z);
            var coslat = Math.Cos(rlat);

            var result = new latlng();
            result.lat = ToAngle(rlat);
            result.lng = ToAngle(Math.Atan2(val.y / coslat, val.x / coslat));

            return result;
        }

        /// <summary>
        /// 二点間配分位置算出
        /// </summary>
        /// <param name="pos0"></param>
        /// <param name="pos1"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static latlng calcP( latlng pos0, latlng pos1, double z = 0.5 )
        {
            var xyz0 = ToXyz(pos0);
            var xyz1 = ToXyz(pos1);

            var theta = Math.Acos(xyz0.x * xyz1.x + xyz0.y * xyz1.y + xyz0.z * xyz1.z);
            var sin_th = Math.Sin(theta);

            var v0 = Math.Sin(theta * (1.0 - z)) / sin_th;
            var v1 = Math.Sin(theta * z ) / sin_th;

            var last = new xyz();
            last.x = xyz0.x * v0 + xyz1.x * v1;
            last.y = xyz0.y * v0 + xyz1.y * v1;
            last.z = xyz0.z * v0 + xyz1.z * v1;

            return ToLatLng(last);
        }

        /// <summary>
        /// 二点間距離算出
        /// </summary>
        /// <param name="pos0"></param>
        /// <param name="pos1"></param>
        /// <param name="mode"></param>
        /// <returns>メートル</returns>
        public static double calcS(latlng pos0, latlng pos1, bool mode = true)
        {
            // 緯度差
            var radlatdiff = ToRadian(pos0.lat) - ToRadian(pos1.lat);
            // 経度差算
            var radlondiff = ToRadian(pos0.lng) - ToRadian(pos1.lng);
            // 平均緯度
            var radlatave = (ToRadian(pos0.lat) + ToRadian(pos1.lat)) * 0.5;

            // 測地系による値の違い
            var a = mode == true ? 6378137.0 : 6377397.155; // 赤道半径
            var b = mode == true ? 6356752.314140356 : 6356078.963; // 極半径
            //$e2 = ($a*$a - $b*$b) / ($a*$a);
            var e2 = mode == true ? 0.00669438002301188 : 0.00667436061028297; // 第一離心率^2
            //$a1e2 = $a * (1 - $e2);
            var a1e2 = mode == true ? 6335439.32708317 : 6334832.10663254; // 赤道上の子午線曲率半径

            var sinLat = Math.Sin(radlatave);
            var W2 = 1.0 - e2 * (sinLat*sinLat);
            var M = a1e2 / (Math.Sqrt(W2)*W2); // 子午線曲率半径M
            var N = a / Math.Sqrt(W2); // 卯酉線曲率半径

            var t1 = M * radlatdiff;
            var t2 = N * Math.Cos(radlatave) * radlondiff;

            var dist = Math.Sqrt((t1*t1) + (t2*t2));

            return dist;
        }

    }
}
