using System;
using System.Xml;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

using WhereTrainBuild.MapUtil.View;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// 基準点情報
    /// </summary>
    public class StationInfoData
    {
        /// <summary>
        /// 駅管理
        /// </summary>
        protected StationManager m_manager;

        /// <summary>
        /// 駅管理プロパティ
        /// </summary>
        public StationManager Manager
        {
            get
            {
                return m_manager;
            }
            set
            {
                m_manager = value;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        protected string m_name = "STATION";

        /// <summary>
        /// 名称プロパティ
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
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
        /// 駅コード
        /// </summary>
        protected int m_station_cd = 0;

        /// <summary>
        /// 駅コード
        /// </summary>
        public int StationCd
        {
            get
            {
                return m_station_cd;
            }
            set
            {
                m_station_cd = value;
            }
        }

        /// <summary>
        /// 駅グループコード
        /// </summary>
        protected int m_station_g_cd = 0;

        /// <summary>
        /// 駅グループコード
        /// </summary>
        public int StationGCd
        {
            get
            {
                return m_station_g_cd;
            }
            set
            {
                m_station_g_cd = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StationInfoData()
        {
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// XML出力
        /// </summary>
        /// <param name="parentnode"></param>
        public XmlNode ToXML( XmlDocument doc )
        {
            var mynode = doc.CreateElement("STATION");

            if( Manager.Factory.MapTable.ContainsKey(Name) == true )
                mynode.SetAttribute("Name", Manager.Factory.MapTable[ Name]);
            else
                mynode.SetAttribute("Name", Name);

            mynode.SetAttribute("UniqID", UniqID.ToString());
            mynode.SetAttribute("Cd", StationCd.ToString());
            mynode.SetAttribute("GCd", StationGCd.ToString());
            mynode.SetAttribute("Latitude", string.Format("{0:0.000000}", Latitude));
            mynode.SetAttribute("Longitude",string.Format("{0:0.000000}", Longitude));
            mynode.SetAttribute("Visible", Visible.ToString());

            return mynode;
        }

        /// <summary>
        /// XML入力
        /// </summary>
        /// <param name="mynode"></param>
        public bool FromXML( XmlNode mynode )
        {
            if (mynode.Name != "STATION")
                return false;

            var name = mynode.Attributes["Name"].Value;
            if (Manager.Factory.MapTable.ContainsKey(name) == true)
                Name = Manager.Factory.MapTable[name];
            else
                Name = name;

            UniqID = int.Parse(mynode.Attributes["UniqID"].Value);
            try
            {
                StationCd = int.Parse(mynode.Attributes["Cd"].Value);
                StationGCd = int.Parse(mynode.Attributes["GCd"].Value);
            }
            catch { }
            Latitude = double.Parse(mynode.Attributes["Latitude"].Value);
            Longitude = double.Parse(mynode.Attributes["Longitude"].Value);
            Visible = bool.Parse(mynode.Attributes["Visible"].Value);

            return true;
        }
    }
}
