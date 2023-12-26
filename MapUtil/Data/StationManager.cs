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
    /// 駅管理
    /// </summary>
    public class StationManager : ViewPointIF
    {
        /// <summary>
        /// 駅テーブル
        /// </summary>
        protected Dictionary<int, StationInfoData> m_table = new Dictionary<int, StationInfoData>();

        /// <summary>
        /// ファクトリ
        /// </summary>
        protected IFactory m_factory;

        /// <summary>
        /// ファクトリプロパティ
        /// </summary>
        public IFactory Factory
        {
            get
            {
                return m_factory;
            }
            set
            {
                m_factory = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StationManager()
        {
        }

        /// <summary>
        /// 駅追加
        /// </summary>
        /// <param name="station"></param>
        public void Add(StationInfoData station)
        {
            m_table[station.UniqID] = station;
            station.Manager = this;
        }

        /// <summary>
        /// 駅取得
        /// </summary>
        /// <param name="uniqid"></param>
        /// <returns></returns>
        public StationInfoData Get(int uniqid)
        {
            if (m_table.ContainsKey(uniqid) == false)
                return null;

            return m_table[uniqid];
        }

        /// <summary>
        /// 駅削除
        /// </summary>
        /// <param name="uniqid"></param>
        public void Remove(int uniqid)
        {
            if (m_table.ContainsKey(uniqid) == true)
                m_table.Remove(uniqid);
        }

        /// <summary>
        /// 駅リスト
        /// </summary>
        /// <returns></returns>
        public StationInfoData[] StationList()
        {
            return m_table.Values.ToArray();
        }

        /// <summary>
        /// 駅取得
        /// </summary>
        /// <param name="uniqid"></param>
        /// <returns></returns>
        public StationInfoData Get(string name)
        {
            return m_table.SingleOrDefault(dr => dr.Value.Name == name).Value;
        }

        /// <summary>
        /// カウント
        /// </summary>
        public int Count
        {
            get
            {
                return m_table.Count;
            }
        }

        /// <summary>
        /// 一意識別子取得
        /// </summary>
        /// <returns></returns>
        public int NextUniqID()
        {
            for (int iId = 1; ; iId++)
            {
                if (m_table.ContainsKey(iId) == false)
                    return iId;
            }
        }

        /// <summary>
        /// 駅取得
        /// </summary>
        /// <param name="uniqid"></param>
        /// <returns></returns>
        public StationInfoData Get(Func<StationInfoData, bool> predicate)
        {
            return m_table.SingleOrDefault( dr => predicate( dr.Value )).Value;
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

            foreach (var dr in m_table)
            {
                var station = dr.Value;

                if (station.Visible == false)
                    continue;

                //自身の座標を計算
                double dMyPositionX = 0;
                double dMyPositionY = 0;

                //メルカトル座標に変換
                MercatorTrans.Trans(station.Latitude / 180.0d * Math.PI, station.Longitude / 180.0d * Math.PI, viewreqinfo.ZoomLevel, ref dMyPositionX, ref dMyPositionY);

                //クリップ領域チェック
                if (cliparea.X > dMyPositionX || dMyPositionX > cliparea.Right ||
                    cliparea.Y > dMyPositionY || dMyPositionY > cliparea.Bottom)
                    continue;

                int iX = (int)((dMyPositionX - cliparea.Left) * viewreqinfo.Scale);
                int iY = (int)((cliparea.Bottom - dMyPositionY) * viewreqinfo.Scale);

                //とりあえず、適当
                viewreqinfo.ViewGraphics.DrawArc(new Pen(Color.Black, 5), iX, iY, 5, 5, 0, 360);

                viewreqinfo.ViewGraphics.DrawString(station.Name, new Font("Arial", 12), new SolidBrush(Color.Red), new PointF((float)(iX), (float)(iY)));
            }

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
        /// XML出力
        /// </summary>
        /// <param name="parentnode"></param>
        public XmlNode ToXML(XmlDocument doc)
        {
            var mynode = doc.CreateElement("Station");

            foreach (var dr in m_table)
            {
                mynode.AppendChild(dr.Value.ToXML(doc));
            }

            return mynode;
        }

        /// <summary>
        /// XML入力
        /// </summary>
        /// <param name="mynode"></param>
        public bool FromXML(XmlNode mynode)
        {
            if (mynode.Name != "Station")
                return false;

            foreach( XmlNode staionnode in mynode.ChildNodes )
            {
                var station = new StationInfoData();
                station.Manager = this;
                if ( station.FromXML(staionnode) == true )
                {
                    Add(station);
                }
            }

            return true;
        }
    }
}
