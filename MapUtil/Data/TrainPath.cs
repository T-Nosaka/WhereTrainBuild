using System;
using System.Xml;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WhereTrainBuild.MapUtil.View;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// 経路
    /// </summary>
    public class TrainPath : IComparable<TrainPath>, ICloneable
    {
        /// <summary>
        /// 順序
        /// </summary>
        protected int m_order = 0;

        /// <summary>
        /// 順序プロパティ
        /// </summary>
        public int Order
        {
            get
            {
                return m_order;
            }
            set
            {
                m_order = value;
            }
        }

        /// <summary>
        /// 比較演算子
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(TrainPath other)
        {
            return Order.CompareTo(other.Order);
        }

        /// <summary>
        /// A駅
        /// </summary>
        protected StationInfoData m_station_a = null;

        /// <summary>
        /// B駅
        /// </summary>
        protected StationInfoData m_station_b = null;

        /// <summary>
        /// A駅プロパティ
        /// </summary>
        public StationInfoData StationA
        {
            get
            {
                return m_station_a;
            }
            set
            {
                m_station_a = value;
            }
        }

        /// <summary>
        /// B駅プロパティ
        /// </summary>
        public StationInfoData StationB
        {
            get
            {
                return m_station_b;
            }
            set
            {
                m_station_b = value;
            }
        }

        /// <summary>
        /// 識別子
        /// </summary>
        protected int m_uniqid = -1;

        /// <summary>
        /// 識別子プロパティ
        /// </summary>
        public int UniqID
        {
            get
            {
                return m_uniqid;
            }
            set
            {
                m_uniqid = value;
            }
        }

        /// <summary>
        /// 駅色
        /// </summary>
        protected Color m_station_color = Color.Brown;

        /// <summary>
        /// ライン色
        /// </summary>
        protected Color m_line_color = Color.Red;

        /// <summary>
        /// 駅色プロパティ
        /// </summary>
        public Color StationColor
        {
            get
            {
                return m_station_color;
            }
            set
            {
                m_station_color = value;
            }
        }

        /// <summary>
        /// ライン色プロパティ
        /// </summary>
        public Color LineColor
        {
            get
            {
                return m_line_color;
            }
            set
            {
                m_line_color = value;
            }
        }

        /// <summary>
        /// 描画有無
        /// </summary>
        protected bool m_visible = true;

        /// <summary>
        /// 描画有無プロパティ
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
        /// 描画
        /// </summary>
        /// <param name="viewreqinfo">描画情報</param>
        /// <returns>True..描画範囲内 False..範囲外</returns>
        public bool Draw(ViewRequestInfo viewreqinfo)
        {
            //描画座標クリップ領域
            Rectangle cliparea = viewreqinfo.ClipArea();

            var pathlist = ToPositionList();

            List<Point> lines = new List<Point>();
            foreach (var pointpath in pathlist)
            {
                //自身の座標を計算
                double dMyPositionX = 0;
                double dMyPositionY = 0;

                //メルカトル座標に変換
                MercatorTrans.Trans(pointpath.lat / 180.0d * Math.PI, pointpath.lng / 180.0d * Math.PI, viewreqinfo.ZoomLevel, ref dMyPositionX, ref dMyPositionY);


                //描画ポイントセット
                lines.Add(new Point((int)((dMyPositionX - cliparea.Left) * viewreqinfo.Scale), (int)((cliparea.Bottom - dMyPositionY) * viewreqinfo.Scale)));


                //経由地描画
                int iX = (int)((dMyPositionX - cliparea.Left) * viewreqinfo.Scale);
                int iY = (int)((cliparea.Bottom - dMyPositionY) * viewreqinfo.Scale);

                viewreqinfo.ViewGraphics.DrawArc(new Pen(StationColor, 5), iX, iY, 3, 3, 0, 360);
            }

            if (Visible == true)
                viewreqinfo.ViewGraphics.DrawLines(new Pen(LineColor, 2), lines.ToArray());
            else
                viewreqinfo.ViewGraphics.DrawLines(new Pen(Color.FromArgb( 100, LineColor.R, LineColor.G, LineColor.B), 2), lines.ToArray());

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
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}",StationA.Name, StationB.Name);
        }

        /// <summary>
        /// 経路位置
        /// </summary>
        public class Position : IComparable<Position>, ICloneable
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
            /// 順序
            /// </summary>
            protected int m_order = 0;

            /// <summary>
            /// 順序プロパティ
            /// </summary>
            public int Order
            {
                get
                {
                    return m_order;
                }
                set
                {
                    m_order = value;
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public Position()
            {
            }

            /// <summary>
            /// 比較演算子
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(Position other)
            {
                return Order.CompareTo(other.Order);
            }

            /// <summary>
            /// 複製
            /// </summary>
            /// <returns></returns>
            public object Clone()
            {
                var instance = new Position();
                instance.Latitude = Latitude;
                instance.Longitude = Longitude;
                instance.Order = Order;

                return instance;
            }

            /// <summary>
            /// XML出力
            /// </summary>
            /// <param name="parentnode"></param>
            public XmlNode ToXML(XmlDocument doc)
            {
                var mynode = doc.CreateElement("Position");

                mynode.SetAttribute("Latitude", string.Format("{0:0.000000}", Latitude));
                mynode.SetAttribute("Longitude", string.Format("{0:0.000000}", Longitude));
                mynode.SetAttribute("Order", Order.ToString());

                return mynode;
            }

            /// <summary>
            /// XML入力
            /// </summary>
            /// <param name="mynode"></param>
            public bool FromXML(XmlNode mynode)
            {
                if (mynode.Name != "Position")
                    return false;

                Latitude = double.Parse(mynode.Attributes["Latitude"].Value);
                Longitude = double.Parse(mynode.Attributes["Longitude"].Value);
                Order = int.Parse(mynode.Attributes["Order"].Value);

                return true;
            }
        }

        /// <summary>
        /// 経路位置リスト
        /// </summary>
        protected List<Position> m_list = new List<Position>();

        /// <summary>
        /// 経路位置削除
        /// </summary>
        public void ClearPosition()
        {
            m_list.Clear();
        }

        /// <summary>
        /// 経路位置追加
        /// </summary>
        /// <param name="pos"></param>
        public void AddPosition(Position pos)
        {
            m_list.Add(pos);

            m_list.Sort();
        }

        /// <summary>
        /// 経路位置削除
        /// </summary>
        /// <param name="pos"></param>
        public void RemovePosition(Position pos)
        {
            m_list.Remove(pos);

            var list = m_list.ToArray();
            int iNo = 1;
            foreach (var tpos in list)
            {
                tpos.Order = iNo++;
            }

            m_list.Sort();
        }

        /// <summary>
        /// 経路位置挿入
        /// </summary>
        /// <param name="iIdx"></param>
        /// <param name="pos"></param>
        public void InsertPosition(int iIdx, Position pos)
        {
            m_list.Insert(iIdx, pos);

            var list = m_list.ToArray();
            int iNo = 1;
            foreach (var tpos in list)
            {
                tpos.Order = iNo++;
            }

            m_list.Sort();
        }

        /// <summary>
        /// 経路位置リスト取得
        /// </summary>
        /// <returns></returns>
        public Position[] GetPositionList()
        {
            return m_list.ToArray();
        }

        /// <summary>
        /// 位置リスト作成
        /// </summary>
        /// <returns></returns>
        public List<latlontool.latlng> ToPositionList()
        {
            var pathlist = new List<latlontool.latlng>();
            pathlist.Add(new latlontool.latlng(StationA.Latitude, StationA.Longitude));
            
            foreach (var pos in m_list)
                pathlist.Add(new latlontool.latlng(pos.Latitude, pos.Longitude));

            pathlist.Add(new latlontool.latlng(StationB.Latitude, StationB.Longitude));

            return pathlist;
        }

        /// <summary>
        /// 距離計算
        /// </summary>
        public double CalcDistance()
        {
            var pathlist = ToPositionList();

            double sum = 0.0;
            latlontool.latlng lastpoint = null;
            foreach(var latlng in pathlist )
            {
                if (lastpoint != null)
                {
                    sum += latlontool.calcS(lastpoint, latlng);
                }

                lastpoint = latlng;
            }

            return sum;
        }

        /// <summary>
        /// 逆順
        /// </summary>
        public void Reverse()
        {
            var swap = StationA;
            StationA = StationB;
            StationB = swap;

            var originalarray = m_list.ToArray();
            m_list.Reverse();
            for (int iIdx = 0; iIdx < originalarray.Length; iIdx++)
            {
                m_list[iIdx].Order = iIdx+1;
            }
        }

        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var instance = new TrainPath();
            instance.Order = Order;
            instance.StationA = StationA;
            instance.StationB = StationB;
            instance.UniqID = UniqID;
            instance.StationColor = StationColor;
            instance.LineColor = LineColor;
            instance.Visible = Visible;

            foreach (var position in m_list)
            {
                instance.m_list.Add(position.Clone() as Position);
            }

            return instance;
        }

        /// <summary>
        /// クリップエリア
        /// </summary>
        protected RectangleF m_cliparea = new RectangleF();

        /// <summary>
        /// 最終クリップエリア計算ズームレベル
        /// </summary>
        protected int m_cliparea_zoomlevel = 0;

        /// <summary>
        /// クリップエリア計算
        /// </summary>
        /// <param name="zoomlevel"></param>
        /// <returns></returns>
        public RectangleF CalcArea( int zoomlevel )
        {
            if (m_cliparea_zoomlevel == zoomlevel)
                return m_cliparea;

            RectangleF area = new RectangleF();

            bool bFirst = true;
            foreach (var latlng in ToPositionList())
            {
                //自身の座標を計算
                double dMyPositionX = 0;
                double dMyPositionY = 0;

                //メルカトル座標に変換
                MercatorTrans.Trans(latlng.lat / 180.0d * Math.PI, latlng.lng / 180.0d * Math.PI, zoomlevel, ref dMyPositionX, ref dMyPositionY);

                if(bFirst == true )
                {
                    area.X = (float)dMyPositionX;
                    area.Y = (float)dMyPositionY;
                    area.Width = 0;
                    area.Height = 0;
                }
                else
                {
                    if (dMyPositionX < area.X)
                        area.X = (float)dMyPositionX;
                    else
                        area.Width = (float)dMyPositionX - area.X;

                    if (dMyPositionY < area.Y)
                        area.Y = (float)dMyPositionY;
                    else
                        area.Height = (float)dMyPositionY - area.Y;
                }
            }

            m_cliparea_zoomlevel = zoomlevel;
            m_cliparea = area;

            return m_cliparea;
        }


        /// <summary>
        /// XML出力
        /// </summary>
        /// <param name="parentnode"></param>
        public XmlNode ToXML(XmlDocument doc )
        {
            var mynode = doc.CreateElement("TrainPath");

            mynode.SetAttribute("Order", Order.ToString());
            mynode.SetAttribute("StationA", StationA.UniqID.ToString());
            mynode.SetAttribute("StationB", StationB.UniqID.ToString());
            mynode.SetAttribute("UniqID", UniqID.ToString());
            mynode.SetAttribute("Visible", Visible.ToString());

            foreach ( var position in m_list)
            {
                var positionnode = position.ToXML(doc);
                mynode.AppendChild(positionnode);
            }

            return mynode;
        }

        /// <summary>
        /// XML入力
        /// </summary>
        /// <param name="mynode"></param>
        public bool FromXML(XmlNode mynode, StationManager stationmanager)
        {
            if (mynode.Name != "TrainPath")
                return false;

            Order = int.Parse(mynode.Attributes["Order"].Value);
            var stationaid = int.Parse(mynode.Attributes["StationA"].Value);
            StationA = stationmanager.Get(stationaid);
            var stationbid = int.Parse(mynode.Attributes["StationB"].Value);
            StationB = stationmanager.Get(stationbid);
            UniqID = int.Parse(mynode.Attributes["UniqID"].Value);
            var visibleattr = mynode.Attributes["Visible"];
            if (visibleattr != null)
                Visible = bool.Parse(visibleattr.Value);

            foreach (XmlNode positionnode in mynode.ChildNodes)
            {
                var position = new Position();
                if (position.FromXML(positionnode) == true)
                {
                    AddPosition(position);
                }
            }

            return true;
        }
    }
}
