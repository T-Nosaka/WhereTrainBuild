using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Diagnostics;

using WhereTrainBuild.MapUtil.Data;

namespace WhereTrainBuild.Net
{
    /// <summary>
    /// 駅.jp
    /// </summary>
    public class ekijp
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ekijp()
        {
        }

        /// <summary>
        /// 都道府県型
        /// </summary>
        public enum StateType
        {
            北海道 = 1,
            青森県,
            岩手県,
            宮城県,
            秋田県,
            山形県,
            福島県,
            茨城県,
            栃木県,
            群馬県,
            埼玉県,
            千葉県,
            東京都,
            神奈川県,
            新潟県,
            富山県,
            石川県,
            福井県,
            山梨県,
            長野県,
            岐阜県,
            静岡県,
            愛知県,
            三重県,
            滋賀県,
            京都府,
            大阪府,
            兵庫県,
            奈良県,
            和歌山県,
            鳥取県,
            島根県,
            岡山県,
            広島県,
            山口県,
            徳島県,
            香川県,
            愛媛県,
            高知県,
            福岡県,
            佐賀県,
            長崎県,
            熊本県,
            大分県,
            宮崎県,
            鹿児島県,
            沖縄県,
        }

        /// <summary>
        /// 県情報
        /// </summary>
        /// <param name="no">都道府県</param>
        public Dictionary<int, string> getState(StateType no)
        {
            var table = new Dictionary<int, string>();

            string sURL = string.Format(string.Format("http://www.ekidata.jp/api/p/{0}.xml", (int)no));
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sURL);
            WebResponse res = req.GetResponse();

            XmlDocument doc = new XmlDocument();
            doc.Load(res.GetResponseStream());

            var root = doc.DocumentElement;

            foreach (XmlElement linenode in root.ChildNodes)
            {
                if (linenode.Name == "line")
                {
                    var cdnode = linenode.SelectSingleNode("line_cd") as XmlElement;
                    var namenode = linenode.SelectSingleNode("line_name") as XmlElement;
                    table[int.Parse(cdnode.InnerText)] = namenode.InnerText;
                }
            }

            return table;
        }

        /// <summary>
        /// 路線取得
        /// </summary>
        /// <param name="no">33001..京阪</param>
        public void getLine( int no, IFactory factory )
        {
            string sURL = string.Format(string.Format("http://www.ekidata.jp/api/l/{0}.xml", no));
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sURL);
            WebResponse res = req.GetResponse();

            XmlDocument doc = new XmlDocument();
            doc.Load(res.GetResponseStream());

            var root = doc.DocumentElement;

            StationInfoData laststation = null;

            foreach ( XmlElement stationnode in root.ChildNodes )
            {
                if (stationnode.Name == "line")
                {

                }

                if ( stationnode.Name == "station" )
                {
                    var snamenode = stationnode.SelectSingleNode("station_name") as XmlElement;
                    var lonnode = stationnode.SelectSingleNode("lon") as XmlElement;
                    var latnode = stationnode.SelectSingleNode("lat") as XmlElement;
                    var station_cd = stationnode.SelectSingleNode("station_cd") as XmlElement;
                    var station_g_cd = stationnode.SelectSingleNode("station_g_cd") as XmlElement;

                    var station = new StationInfoData();
                    station.UniqID = factory.GetStationManager().NextUniqID();
                    station.StationCd = int.Parse(station_cd.InnerText);
                    station.StationGCd = int.Parse(station_g_cd.InnerText);
                    station.Name = snamenode.InnerText;
                    station.Longitude = double.Parse(lonnode.InnerText);
                    station.Latitude = double.Parse(latnode.InnerText);

                    var findstation = factory.GetStationManager().Get((ss) => { return ss.StationGCd == station.StationGCd; });
                    if (findstation == null)
                    {
                        factory.GetStationManager().Add(station);
                    }
                    else
                    {
                        station = findstation;
                    }

                    if (laststation != null )
                    {
                        //二重線を防ぐ
                        if (new List<TrainPath>(factory.GetNetwork().GetPathList()).Exists(tpath => (tpath.StationA.UniqID == laststation.UniqID && tpath.StationB.UniqID == station.UniqID) || (tpath.StationA.UniqID == station.UniqID && tpath.StationB.UniqID == laststation.UniqID)) == false)
                        {
                            TrainPath path = new TrainPath();
                            path.StationA = laststation;
                            path.StationB = station;
                            path.UniqID = factory.GetNetwork().NextPathUniqID();

                            factory.GetNetwork().AddPath(path);
                        }
                    }

                    laststation = station;
                }
            }
        }

        /// <summary>
        /// デバッグ出力
        /// </summary>
        protected void DebugOut(MemoryStream stream)
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));

            stream.Seek(0, SeekOrigin.Begin);
            var sss = Encoding.UTF8.GetString(stream.ToArray());

            Debug.Write(sss);
        }
    }
}
