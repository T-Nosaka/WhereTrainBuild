using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// ファクトリ基底
    /// </summary>
    public abstract class BaseFactory : IFactory
    {
        /// <summary>
        /// 文字列置換マップ
        /// </summary>
        protected Dictionary<string, string> m_maptable = new Dictionary<string, string>();

        /// <summary>
        /// 文字列置換マッププロパティ
        /// </summary>
        public Dictionary<string, string> MapTable
        {
            get
            {
                return m_maptable;
            }
            set
            {
                m_maptable = value;
            }
        }

        /// <summary>
        /// 名前
        /// </summary>
        protected string m_name = string.Empty;

        /// <summary>
        /// 識別ID
        /// </summary>
        protected string m_uniqid = string.Empty;

        /// <summary>
        /// URL
        /// </summary>
        protected string m_url = string.Empty;

        /// <summary>
        /// RSS
        /// </summary>
        protected string m_rss = string.Empty;

        /// <summary>
        /// RSSキーワード
        /// </summary>
        protected string m_rsskeyword = string.Empty;

        /// <summary>
        /// 基本色
        /// </summary>
        protected Color m_basecolor = Color.Red; 

        /// <summary>
        /// 名前プロパティ
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
        /// 識別IDプロパティ
        /// </summary>
        public string UniqID
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
        /// URLプロパティ
        /// </summary>
        public string Url
        {
            get
            {
                return m_url;
            }
            set
            {
                m_url = value;
            }
        }

        /// <summary>
        /// RSSプロパティ
        /// </summary>
        public string RSS
        {
            get
            {
                return m_rss;
            }
            set
            {
                m_rss = value;
            }
        }

        /// <summary>
        /// RSSキーワードプロパティ
        /// </summary>
        public string RSSKeyword
        {
            get
            {
                return m_rsskeyword;
            }
            set
            {
                m_rsskeyword = value;
            }
        }

        /// <summary>
        /// 基本色
        /// </summary>
        public Color BaseColor
        {
            get
            {
                return m_basecolor;
            }
            set
            {
                m_basecolor = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseFactory()
        {
            m_stationmanager.Factory = this;
            m_network.Factory = this;
        }

        /// <summary>
        /// 日またぎ時間
        /// </summary>
        protected TimeSpan m_overtime = new TimeSpan();

        /// <summary>
        /// 日またぎ時間プロパティ
        /// </summary>
        public TimeSpan OverTime
        {
            get
            {
                return m_overtime;
            }
            set
            {
                m_overtime = value;
            }
        }

        /// <summary>
        /// 保存日時
        /// </summary>
        protected DateTime m_savedate = DateTime.Now;

        /// <summary>
        /// 保存日時プロパティ
        /// </summary>
        public DateTime SaveDate
        {
            get
            {
                return m_savedate;
            }
            set
            {
                m_savedate = value;
            }
        }

        /// <summary>
        /// 駅管理
        /// </summary>
        protected StationManager m_stationmanager = new StationManager();

        /// <summary>
        /// ネットワーク
        /// </summary>
        protected TrainNetwork m_network = new TrainNetwork();

        /// <summary>
        /// 駅管理取得
        /// </summary>
        /// <returns></returns>
        public StationManager GetStationManager()
        {
            return m_stationmanager;
        }

        /// <summary>
        /// ネットワーク取得
        /// </summary>
        /// <returns></returns>
        public TrainNetwork GetNetwork()
        {
            return m_network;
        }

        /// <summary>
        /// 電車クリア
        /// </summary>
        public void ClearTrain( string lineset )
        {
            GetNetwork().ChangeLine(lineset);
            foreach (var line in GetNetwork().GetLineList())
                line.ClearTrain();
        }

        /// <summary>
        /// XML出力
        /// </summary>
        /// <param name="parentnode"></param>
        public virtual XmlNode ToXML(XmlDocument doc)
        {
            var mynode = doc.CreateElement("Factory");

            mynode.SetAttribute("OverTime", OverTime.ToString());

            mynode.SetAttribute("SaveDate", SaveDate.ToString("yyyyMMddHHmmss"));

            mynode.SetAttribute("Name", Name);
            mynode.SetAttribute("UniqID", UniqID);
            mynode.SetAttribute("Url", Url);
            mynode.SetAttribute("Rss", RSS);
            mynode.SetAttribute("RssKeyword", RSSKeyword);

            var colornode = doc.CreateElement("Color");
            colornode.SetAttribute("A", m_basecolor.A.ToString());
            colornode.SetAttribute("R", m_basecolor.R.ToString());
            colornode.SetAttribute("G", m_basecolor.G.ToString());
            colornode.SetAttribute("B", m_basecolor.B.ToString());
            mynode.AppendChild(colornode);

            var leftbottomnode = doc.CreateElement("LeftBottom");
            mynode.AppendChild(leftbottomnode);
            leftbottomnode.SetAttribute("Latitude", string.Format("{0:0.000000}", m_leftbottom.lat));
            leftbottomnode.SetAttribute("Longitude", string.Format("{0:0.000000}", m_leftbottom.lng));
            var righttopnode = doc.CreateElement("RightTop");
            mynode.AppendChild(righttopnode);
            righttopnode.SetAttribute("Latitude", string.Format("{0:0.000000}", m_righttop.lat));
            righttopnode.SetAttribute("Longitude", string.Format("{0:0.000000}", m_righttop.lng));

            mynode.AppendChild( GetStationManager().ToXML(doc));
            mynode.AppendChild(GetNetwork().ToXML(doc));

            return mynode;
        }

        /// <summary>
        /// XML入力
        /// </summary>
        /// <param name="mynode"></param>
        public virtual bool FromXML(XmlNode mynode)
        {
            if (mynode.Name != "Factory")
                return false;

            var overtimeattr = mynode.Attributes["OverTime"];
            if(overtimeattr != null )
            {
                OverTime = TimeSpan.Parse(overtimeattr.Value);
            }

            var savedateattr = mynode.Attributes["SaveDate"];
            if (savedateattr != null)
            {
                SaveDate = DateTime.ParseExact(savedateattr.Value, "yyyyMMddHHmmss", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None);
            }

            var nameattr = mynode.Attributes["Name"];
            if (nameattr != null)
                Name = nameattr.Value;

            var uniqidattr = mynode.Attributes["UniqID"];
            if (uniqidattr != null)
                UniqID = uniqidattr.Value;

            var urlattr = mynode.Attributes["Url"];
            if (urlattr != null)
                Url = urlattr.Value;

            var rssattr = mynode.Attributes["Rss"];
            if (rssattr != null)
                RSS = rssattr.Value;

            var rsskeywordattr = mynode.Attributes["RssKeyword"];
            if (rsskeywordattr != null)
                RSSKeyword = rsskeywordattr.Value;

            var leftbottomnode = mynode.SelectSingleNode("LeftBottom");
            if(leftbottomnode != null)
            {
                m_leftbottom.lat = double.Parse(leftbottomnode.Attributes["Latitude"].Value);
                m_leftbottom.lng = double.Parse(leftbottomnode.Attributes["Longitude"].Value);
            }
            var righttopnode = mynode.SelectSingleNode("RightTop");
            if (righttopnode != null)
            {
                m_righttop.lat = double.Parse(righttopnode.Attributes["Latitude"].Value);
                m_righttop.lng = double.Parse(righttopnode.Attributes["Longitude"].Value);
            }

            var colornode = mynode.SelectSingleNode("Color");
            if(colornode != null)
            {
                var colora = int.Parse(colornode.Attributes["A"].Value);
                var colorr = int.Parse(colornode.Attributes["R"].Value);
                var colorg = int.Parse(colornode.Attributes["G"].Value);
                var colorb = int.Parse(colornode.Attributes["B"].Value);
                BaseColor = Color.FromArgb(colora, colorr, colorg, colorb); 
            }

            foreach (XmlNode childnode in mynode.ChildNodes)
            {
                if (childnode.Name == "Station")
                {
                    GetStationManager().FromXML(childnode);
                    break;
                }
            }

            foreach (XmlNode childnode in mynode.ChildNodes)
            {
                if (childnode.Name == "TrainNetwork")
                {
                    GetNetwork().FromXML(childnode, GetStationManager());
                    break;
                }
            }

            //色反映
            foreach (var path in GetNetwork().GetPathList())
            {
                path.LineColor = BaseColor;
            }

            return true;
        }


        /// <summary>
        /// 左上
        /// </summary>
        protected latlontool.latlng m_leftbottom = new latlontool.latlng();

        /// <summary>
        /// 左上プロパティ
        /// </summary>
        public latlontool.latlng LeftBottom
        {
            get
            {
                return m_leftbottom;
            }
        }

        /// <summary>
        /// 右上
        /// </summary>
        protected latlontool.latlng m_righttop = new latlontool.latlng();

        /// <summary>
        /// 右上プロパティ
        /// </summary>
        public latlontool.latlng RightUp
        {
            get
            {
                return m_righttop;
            }
        }

        /// <summary>
        /// 有効エリア計算
        /// </summary>
        public void CalcArea( bool bVisibleFalseContain = false )
        {
            var lefttop = new latlontool.latlng();
            var rightbottom = new latlontool.latlng();

            foreach (var path in GetNetwork().GetPathList())
            {
                foreach (var latlon in path.ToPositionList())
                {
                    if (bVisibleFalseContain == false && path.Visible == false)
                        continue;

                    if (lefttop.undefined == true)
                    {
                        lefttop.Set(latlon);
                    }
                    else
                    {
                        if (latlon.lat < lefttop.lat)
                            lefttop.lat = latlon.lat;
                        if (latlon.lng < lefttop.lng)
                            lefttop.lng = latlon.lng;
                    }
                    if (rightbottom.undefined == true)
                    {
                        rightbottom.Set(latlon);
                    }
                    else
                    {
                        if (rightbottom.lat < latlon.lat)
                            rightbottom.lat = latlon.lat;
                        if (rightbottom.lng < latlon.lng)
                            rightbottom.lng = latlon.lng;
                    }
                }
            }

            m_leftbottom.Set(lefttop);
            m_righttop.Set(rightbottom);
        }
    }
}
