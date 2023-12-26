using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// 電車情報
    /// </summary>
    public class TrainInfo
    {
        /// <summary>
        /// ライン
        /// </summary>
        protected TrainLine m_line;

        /// <summary>
        /// ラインプロパティ
        /// </summary>
        public TrainLine Line
        {
            get
            {
                return m_line;
            }
            set
            {
                m_line = value;
            }
        }

        /// <summary>
        /// スケジュール
        /// </summary>
        protected SceduleManager m_scedule = new SceduleManager();

        /// <summary>
        /// スケジュールプロパティ
        /// </summary>
        public SceduleManager Scedule
        {
            get
            {
                return m_scedule;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        protected string m_name = string.Empty;

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
        /// 表示名称
        /// </summary>
        protected string m_display = string.Empty;

        /// <summary>
        /// 表示名称プロパティ
        /// </summary>
        public string Display
        {
            get
            {
                return m_display;
            }
            set
            {
                m_display = value;
            }
        }

        /// <summary>
        /// 始発駅
        /// </summary>
        protected string m_startstation = string.Empty;

        /// <summary>
        /// 最終駅
        /// </summary>
        protected string m_endstation = string.Empty;

        /// <summary>
        /// 始発駅プロパティ
        /// </summary>
        public string StartStation
        {
            get
            {
                return m_startstation;
            }
            set
            {
                m_startstation = value;
            }
        }

        /// <summary>
        /// 最終駅プロパティ
        /// </summary>
        public string EndStation
        {
            get
            {
                return m_endstation;
            }
            set
            {
                m_endstation = value;
            }
        }

        /// <summary>
        /// 種類
        /// </summary>
        protected string m_kind = string.Empty;

        /// <summary>
        /// 種類プロパティ
        /// </summary>
        public string Kind
        {
            get
            {
                return m_kind;
            }
            set
            {
                m_kind = value;
            }
        }

        /// <summary>
        /// レベル
        /// </summary>
        protected int m_level = 200;

        /// <summary>
        /// レベルプロパティ
        /// </summary>
        public int Level
        {
            get
            {
                return m_level;
            }
            set
            {
                m_level = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainInfo()
        {
            Scedule.Train = this;
        }

        /// <summary>
        /// XML出力
        /// </summary>
        /// <param name="parentnode"></param>
        public XmlNode ToXML(XmlDocument doc)
        {
            var mynode = doc.CreateElement("TrainInfo");

            var schedulenode = m_scedule.ToXML(doc);
            mynode.AppendChild(schedulenode);

            if (Line.Network.Factory.MapTable.ContainsKey(Name) == true)
                mynode.SetAttribute("Name", Line.Network.Factory.MapTable[Name]);
            else
                mynode.SetAttribute("Name", Name);

            if (Line.Network.Factory.MapTable.ContainsKey(Display) == true)
                mynode.SetAttribute("Display", Line.Network.Factory.MapTable[Display]);
            else
                mynode.SetAttribute("Display", Display);

            if (Line.Network.Factory.MapTable.ContainsKey(StartStation) == true)
                mynode.SetAttribute("StartStation", Line.Network.Factory.MapTable[StartStation]);
            else
                mynode.SetAttribute("StartStation", StartStation);

            if (Line.Network.Factory.MapTable.ContainsKey(EndStation) == true)
                mynode.SetAttribute("EndStation", Line.Network.Factory.MapTable[EndStation]);
            else
                mynode.SetAttribute("EndStation", EndStation);

            if (Line.Network.Factory.MapTable.ContainsKey(Kind) == true)
                mynode.SetAttribute("Kind", Line.Network.Factory.MapTable[Kind]);
            else
                mynode.SetAttribute("Kind", Kind);

            mynode.SetAttribute("Level", Level.ToString());

            return mynode;
        }

        /// <summary>
        /// XML入力
        /// </summary>
        /// <param name="mynode"></param>
        public bool FromXML(XmlNode mynode, StationManager stationmanager)
        {
            if (mynode.Name != "TrainInfo")
                return false;

            var schedulenode = mynode.ChildNodes[0];
            if (m_scedule.FromXML(schedulenode, stationmanager) == false)
                return false;

            var name = mynode.Attributes["Name"].Value;
            if (Line.Network.Factory.MapTable.ContainsKey(name) == true)
                Name = Line.Network.Factory.MapTable[name];
            else
                Name = name;

            var display = mynode.Attributes["Display"].Value;
            if (Line.Network.Factory.MapTable.ContainsKey(display) == true)
                Display = Line.Network.Factory.MapTable[display];
            else
                Display = display;

            var startstation = mynode.Attributes["StartStation"].Value;
            if (Line.Network.Factory.MapTable.ContainsKey(startstation) == true)
                StartStation = Line.Network.Factory.MapTable[startstation];
            else
                StartStation = startstation;

            var endstation = mynode.Attributes["EndStation"].Value;
            if (Line.Network.Factory.MapTable.ContainsKey(endstation) == true)
                EndStation = Line.Network.Factory.MapTable[endstation];
            else
                EndStation = endstation;

            var kind = mynode.Attributes["Kind"].Value;
            if (Line.Network.Factory.MapTable.ContainsKey(kind) == true)
                Kind = Line.Network.Factory.MapTable[kind];
            else
                Kind = kind;

            var levelattr = mynode.Attributes["Level"];
            if (levelattr != null)
                Level = int.Parse(levelattr.Value);

            return true;
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0},{1},{2}",Name,Display,Kind);
        }
    }
}
