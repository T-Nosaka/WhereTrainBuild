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
    /// ライン
    /// </summary>
    public class TrainLine : ICloneable
    {
        /// <summary>
        /// 経路ネットワーク
        /// </summary>
        protected TrainNetwork m_network;

        /// <summary>
        /// 経路ネットワークプロパティ
        /// </summary>
        public TrainNetwork Network
        {
            get
            {
                return m_network;
            }
            set
            {
                m_network = value;
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
        /// 経路リスト
        /// </summary>
        protected List<TrainPath> m_pathlist = new List<TrainPath>();

        /// <summary>
        /// 電車リスト
        /// </summary>
        protected List<TrainInfo> m_trainlist = new List<TrainInfo>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainLine()
        {
        }

        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var instance = MemberwiseClone() as TrainLine;
            instance.m_pathlist = new List<TrainPath>();

            foreach (var path in m_pathlist)
            {
                instance.AddPath(path.Clone() as TrainPath);
            }

            instance.m_trainlist = new List<TrainInfo>();

            instance.Network = Network;

            return instance;
        }

        /// <summary>
        /// 経路リスト取得
        /// </summary>
        /// <returns></returns>
        public TrainPath[] GetPathList()
        {
            return m_pathlist.ToArray();
        }

        /// <summary>
        /// 電車リスト取得
        /// </summary>
        /// <returns></returns>
        public TrainInfo[] GetTrainList()
        {
            return m_trainlist.ToArray();
        }

        /// <summary>
        /// 経路追加
        /// ※pathのOrderを変更する
        /// </summary>
        /// <param name="path"></param>
        public virtual void AddPath(TrainPath path)
        {
            m_pathlist.Add(path);
            path.Order = m_pathlist.Count+1;
            m_pathlist.Sort();
        }

        /// <summary>
        /// 経路破棄
        /// </summary>
        public void ClearPath()
        {
            m_pathlist.Clear();
        }

        /// <summary>
        /// 経路削除
        /// ※pathのOrderを変更する
        /// </summary>
        /// <param name="path"></param>
        public void RemovePath(TrainPath path)
        {
            m_pathlist.Remove(path);

            for (int iIdx = 0; iIdx < m_pathlist.Count; iIdx++)
            {
                m_pathlist[iIdx].Order = iIdx + 1;
            }

            m_pathlist.Sort();
        }

        /// <summary>
        /// 電車追加
        /// </summary>
        /// <param name="train"></param>
        public void AddTrain(TrainInfo train)
        {
            m_trainlist.Add(train);

            train.Line = this;
        }

        /// <summary>
        /// 電車削除
        /// </summary>
        /// <param name="train"></param>
        public void DelTrain(TrainInfo train)
        {
            m_trainlist.Remove(train);
        }

        /// <summary>
        /// 電車取得
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TrainInfo[] Get(Func<TrainInfo, bool> predicate)
        {
            return m_trainlist.Where(predicate).ToArray();
        }

        /// <summary>
        /// 電車存在チェック
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ExistTrain(string name)
        {
            return m_trainlist.Exists(train => train.Name == name);
        }

        /// <summary>
        /// 同じスケジュールを走行する列車の存在チェック
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public bool ExistTrain(SceduleManager schedule)
        {
            return m_trainlist.Exists(train => train.Scedule.IsContain(schedule));
        }

        /// <summary>
        /// スケジュールが重なる電車を取得
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public TrainInfo[] GetOverLapTrain(SceduleManager schedule)
        {
            return m_trainlist.Where(train => train.Scedule.IsContain(schedule)).ToArray();
        }

        /// <summary>
        /// 電車破棄
        /// </summary>
        public void ClearTrain()
        {
            m_trainlist.Clear();
        }

        /// <summary>
        /// パスリスト再構築
        /// </summary>
        public void RebuildPassList()
        {
            List<TrainPath> pathlist = new List<TrainPath>();

            foreach( var path in m_pathlist )
            {
                var findpath = Network.GetPathList().SingleOrDefault(pp => pp.UniqID == path.UniqID);
                if( findpath != null )
                {
                    if(findpath.StationA.UniqID == path.StationA.UniqID && findpath.StationB.UniqID == path.StationB.UniqID )
                    {
                        //そのまま
                        pathlist.Add(findpath.Clone() as TrainPath);
                    } else
                    if (findpath.StationA.UniqID == path.StationB.UniqID && findpath.StationB.UniqID == path.StationA.UniqID)
                    {
                        //逆転
                        var rev = findpath.Clone() as TrainPath;
                        rev.Reverse();
                        pathlist.Add(rev);
                    } else
                    {
                        pathlist.Add(path);
                    }
                }
            }

            //順序整列
            int iOrder = 1;
            pathlist.ForEach(path => path.Order = iOrder++);

            m_pathlist = pathlist;
            m_pathlist.Sort();
        }

        /// <summary>
        /// 駅間進捗位置結果
        /// </summary>
        protected struct ResultInternalPosition
        {
            public latlontool.latlng lastpath;
            public latlontool.latlng nextpath;
            public double interratio;
        }

        /// <summary>
        /// 駅間進捗位置算出
        /// </summary>
        /// <param name="stationa">駅A</param>
        /// <param name="stationb">駅B</param>
        /// <param name="ratio">進捗率</param>
        protected ResultInternalPosition CalcInternalPosition(StationInfoData stationa, StationInfoData stationb, double ratio)
        {
            var result = new ResultInternalPosition();

            //駅間パス取得(通過対応)
            var allpath = ToListPath(stationa, stationb);

            //データ不備
            if (allpath.StationA == null || allpath.StationB == null)
                return result;

            //全体距離
            var distance = allpath.CalcDistance();
            //全体距離に対する位置
            var nowdistance = distance * ratio;

            //経路間率計算
            double interratio = 0.0;
            double sum = 0.0;
            double lastsum = 0.0f;
            latlontool.latlng lastpath = null;
            latlontool.latlng nextpath = null;
            foreach (var latlon in allpath.ToPositionList())
            {
                if (lastpath != null)
                {
                    lastsum = sum;
                    sum += latlontool.calcS(lastpath, latlon);

                    if (nowdistance < sum)
                    {
                        //ここ
                        nextpath = latlon;

                        var interdistance = latlontool.calcS(lastpath, latlon);
                        var progressdistance = (interdistance - (nowdistance - lastsum));

                        //駅間配分
                        interratio = 1.0 - progressdistance / interdistance;
                        break;
                    }
                }

                lastpath = latlon;
            }

            //最終結果
            result.interratio = interratio;
            result.lastpath = lastpath;
            result.nextpath = nextpath;

            return result;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="viewreqinfo">描画情報</param>
        /// <returns>True..描画範囲内 False..範囲外</returns>
        public virtual bool Draw(ViewRequestInfo viewreqinfo, TimeSpan now )
        {
            //描画座標クリップ領域
            Rectangle cliparea = viewreqinfo.ClipArea();

            //電車位置
            foreach (var train in m_trainlist)
            {
                //駅間と時間よる駅間率取得
                double ratio = 0.0;
                var nowplan = train.Scedule.CurrentPoint(now, ref ratio);
                if (nowplan != null)
                {
                    var result = CalcInternalPosition(nowplan[0].Station, nowplan[1].Station, ratio);
                    if (result.nextpath != null && result.lastpath != null)
                    {
                        var interpoint = latlontool.calcP(result.lastpath, result.nextpath, result.interratio);

                        //自身の座標を計算
                        double dMyPositionX = 0;
                        double dMyPositionY = 0;

                        //メルカトル座標に変換
                        MercatorTrans.Trans(interpoint.lat / 180.0d * Math.PI, interpoint.lng / 180.0d * Math.PI, viewreqinfo.ZoomLevel, ref dMyPositionX, ref dMyPositionY);

                        //クリップ領域チェック
                        if (cliparea.X > dMyPositionX || dMyPositionX > cliparea.Right ||
                            cliparea.Y > dMyPositionY || dMyPositionY > cliparea.Bottom)
                            continue;

                        int iX = (int)((dMyPositionX - cliparea.Left) * viewreqinfo.Scale);
                        int iY = (int)((cliparea.Bottom - dMyPositionY) * viewreqinfo.Scale);

                        //とりあえず、適当
                        viewreqinfo.ViewGraphics.DrawArc(new Pen(Color.Yellow, 3), iX, iY, 5, 5, 0, 360);

                        viewreqinfo.ViewGraphics.DrawString(train.Name, new Font("Arial", 12), new SolidBrush(Color.Blue), new PointF((float)(iX), (float)(iY)));
                        viewreqinfo.ViewGraphics.DrawString(train.EndStation, new Font("Arial", 6), new SolidBrush(Color.Black), new PointF((float)(iX), (float)(iY + 20)));
                        if( train.Display != string.Empty )
                            viewreqinfo.ViewGraphics.DrawString(string.Format("{0}({1})", train.Kind, train.Display), new Font("Arial", 6), new SolidBrush(Color.Black), new PointF((float)(iX), (float)(iY + 30)));
                        else
                            viewreqinfo.ViewGraphics.DrawString(train.Kind, new Font("Arial", 6), new SolidBrush(Color.Black), new PointF((float)(iX), (float)(iY + 30)));
                    }
                    else
                    {
                        //データ不備
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 本体ネットワークより取得
        /// </summary>
        /// <param name="uniqid"></param>
        /// <returns></returns>
        protected TrainPath GetNetworkPath(int uniqid)
        {
            return Network.GetPath(uniqid);
        }

        /// <summary>
        /// 経路取得
        /// </summary>
        /// <param name="stationa"></param>
        /// <param name="stationb"></param>
        /// <returns></returns>
        protected TrainPath ToListPath(StationInfoData stationa, StationInfoData stationb)
        {
            int iOrder = 0;

            var trainpath = new TrainPath();

            bool bOverStation = false;
            foreach (var linepath in m_pathlist)
            {
                //本体のパスを取得、細かい変更が発生しても反映されないので
                TrainPath path = GetNetworkPath(linepath.UniqID);
                if (path != null)
                {
                    if (path.StationA.UniqID != linepath.StationA.UniqID)
                    {
                        //リバース対象
                        var revpath = path.Clone() as TrainPath;
                        revpath.Reverse();
                        path = revpath;
                    }
                }
                else
                    path = linepath;

                if (bOverStation == true)
                {
                    //通過駅
                    var positiona = new TrainPath.Position();
                    positiona.Order = iOrder++;
                    positiona.Latitude = path.StationA.Latitude;
                    positiona.Longitude = path.StationA.Longitude;

                    trainpath.AddPosition(positiona);

                    foreach (var position in path.GetPositionList())
                    {
                        var pos = new TrainPath.Position();
                        pos.Order = iOrder++;
                        pos.Latitude = position.Latitude;
                        pos.Longitude = position.Longitude;
                        trainpath.AddPosition(pos);
                    }
                }

                if (path.StationA.UniqID == stationa.UniqID)
                {
                    if(trainpath.StationA != null)
                    {
                        //ループ駅対策 都営大江戸線
                        trainpath.StationA = null;
                        trainpath.ClearPosition();
                    }

                    trainpath.StationA = path.StationA;

                    foreach (var position in path.GetPositionList())
                    {
                        var pos = new TrainPath.Position();
                        pos.Order = iOrder++;
                        pos.Latitude = position.Latitude;
                        pos.Longitude = position.Longitude;
                        trainpath.AddPosition(pos);
                    }

                    bOverStation = true;
                }

                if (path.StationB.UniqID == stationb.UniqID)
                {
                    trainpath.StationB = path.StationB;
                    break;
                }

            }

            return trainpath;
        }

        /// <summary>
        /// 逆順
        /// ※Pathの順序を再整列する
        /// </summary>
        public void Reverse()
        {
            m_pathlist.ForEach(path => path.Reverse());

            var originalarray = m_pathlist.ToArray();
            m_pathlist.Reverse();
            for (int iIdx = 0; iIdx < originalarray.Length; iIdx++)
            {
                m_pathlist[iIdx].Order = iIdx+1;
            }
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
        public RectangleF CalcArea(int zoomlevel)
        {
            if (m_cliparea_zoomlevel == zoomlevel)
                return m_cliparea;

            RectangleF area = new RectangleF();

            bool bFirst = true;
            foreach (var path in m_pathlist)
            {
                var cliparea = path.CalcArea(zoomlevel);

                if (bFirst == true)
                {
                    area.X = cliparea.X;
                    area.Y = cliparea.Y;
                    area.Width = cliparea.Width;
                    area.Height = cliparea.Height;
                }
                else
                {
                    if (cliparea.X < area.X)
                        area.X = cliparea.X;
                    if (area.Right < cliparea.Right)
                        area.Width = cliparea.Right - area.X;

                    if (cliparea.Y < area.Y)
                        area.Y = cliparea.Y;
                    if (area.Bottom < cliparea.Bottom)
                        area.Height = cliparea.Bottom - area.Y;
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
        public XmlNode ToXML(XmlDocument doc)
        {
            RebuildPassList();

            var mynode = doc.CreateElement("TrainLine");

            if (Network.Factory.MapTable.ContainsKey(Name) == true)
                mynode.SetAttribute("Name", Network.Factory.MapTable[Name]);
            else
                mynode.SetAttribute("Name", Name);

            if (Network.Factory.MapTable.ContainsKey(Display) == true)
                mynode.SetAttribute("Display", Network.Factory.MapTable[Display]);
            else
                mynode.SetAttribute("Display", Display);

            var pathlistnode = doc.CreateElement("PathList");
            mynode.AppendChild(pathlistnode);
            foreach ( var path in m_pathlist)
            {
                pathlistnode.AppendChild(path.ToXML(doc));
            }

            var trainlistnode = doc.CreateElement("TrainList");
            mynode.AppendChild(trainlistnode);
            foreach (var train in m_trainlist)
            {
                trainlistnode.AppendChild( train.ToXML(doc));
            }

            return mynode;
        }

        /// <summary>
        /// XML入力
        /// </summary>
        /// <param name="mynode"></param>
        public bool FromXML(XmlNode mynode, StationManager stationmanager)
        {
            if (mynode.Name != "TrainLine")
                return false;

            var name = mynode.Attributes["Name"].Value;
            if (Network.Factory.MapTable.ContainsKey(name) == true)
                Name = Network.Factory.MapTable[name];
            else
                Name = name;

            var display = mynode.Attributes["Display"].Value;
            if (Network.Factory.MapTable.ContainsKey(display) == true)
                Display = Network.Factory.MapTable[display];
            else
                Display = display;

            foreach (XmlNode childnode in mynode.ChildNodes)
            {
                if( childnode.Name == "PathList")
                {
                    foreach (XmlNode pathnode in childnode.ChildNodes)
                    {
                        var path = new TrainPath();
                        if (path.FromXML(pathnode, stationmanager) == true)
                        {
                            m_pathlist.Add(path);
                        }

                        path.LineColor = Color.LightBlue;
                        path.StationColor = Color.LightGreen;

                        m_pathlist.Sort();
                    }
                }
                if(childnode.Name == "TrainList")
                {
                    foreach (XmlNode trainnode in childnode.ChildNodes)
                    {
                        var train = new TrainInfo();
                        train.Line = this;
                        if( train.FromXML(trainnode, stationmanager) == true )
                        {
                            m_trainlist.Add(train);
                        }
                    }
                }
            }

            return true;
        }
    }
}
