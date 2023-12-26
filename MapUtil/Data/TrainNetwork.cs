using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using WhereTrainBuild.MapUtil.View;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// 経路ネットワーク
    /// </summary>
    public class TrainNetwork : ViewPointIF
    {
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
        /// セット種類型
        /// </summary>
        public enum SetKindType
        {
            平日日祝日,
            平日土日祝日
        }

        /// <summary>
        /// セット種類
        /// </summary>
        protected SetKindType m_setkind = SetKindType.平日日祝日;

        /// <summary>
        /// セット種類プロパティ
        /// </summary>
        public SetKindType SetKind
        {
            get
            {
                return m_setkind;
            }
            set
            {
                var oldvalue = m_setkind;

                m_setkind = value;

                if(oldvalue != m_setkind)
                {
                    RebuildSetTable();
                }
            }
        }

        /// <summary>
        /// 経路リスト
        /// </summary>
        protected List<TrainPath> m_pathlist = new List<TrainPath>();

        /// <summary>
        /// ラインテーブル
        /// </summary>
        protected Dictionary<string, List<TrainLine>> m_linetable = new Dictionary<string, List<TrainLine>>();

        /// <summary>
        /// ラインセット名
        /// </summary>
        protected string m_linename = "平日";

        /// <summary>
        /// ラインセット名取得
        /// </summary>
        /// <returns></returns>
        public string GetLineName()
        {
            return m_linename;
        }

        /// <summary>
        /// ラインリスト
        /// </summary>
        protected List<TrainLine> m_linelist = new List<TrainLine>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainNetwork()
        {
            m_linetable["平日"] = m_linelist;
            m_linetable["土日祝日"] = new List<TrainLine>();
        }

        /// <summary>
        /// ラインセットテーブル再構築
        /// </summary>
        protected void RebuildSetTable()
        {
            switch (SetKind)
            {
                case SetKindType.平日土日祝日:
                    {
                        m_linelist = GetLineList("平日");
                        GetLineList("土曜");
                        GetLineList("日祝日");
                    }
                    break;

                default:
                    {
                        m_linelist = GetLineList("平日");
                        GetLineList("土日祝日");
                    }
                    break;
            }
        }

        /// <summary>
        /// ラインセット名変更
        /// </summary>
        /// <param name="setname">セット名</param>
        public void ChangeLine( string setname)
        {
            m_linename = setname;
            if (m_linetable.ContainsKey(setname) == false )
                m_linetable[setname] = new List<TrainLine>();

            m_linelist = m_linetable[setname];
        }

        /// <summary>
        /// ラインリスト取得
        /// </summary>
        /// <param name="setname">セット名</param>
        /// <returns></returns>
        public List<TrainLine> GetLineList( string setname )
        {
            if (m_linetable.ContainsKey(setname) == false)
                m_linetable[setname] = new List<TrainLine>();

            return m_linetable[setname];
        }

        /// <summary>
        /// ラインセット削除
        /// </summary>
        /// <param name="setname">セット名</param>
        public void DeleteLineSet(string setname)
        {
            if (m_linetable.ContainsKey(setname) == true)
                m_linetable.Remove(setname);
        }

        /// <summary>
        /// ライン削除
        /// </summary>
        public void DeleteLine()
        {
            m_linetable.Clear();
        }

        /// <summary>
        /// ラインセット名リスト
        /// </summary>
        /// <returns></returns>
        public string[] GetLineSetList()
        {
            return m_linetable.Keys.ToArray();
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        /// <param name="path"></param>
        public virtual void AddPath(TrainPath path)
        {
            m_pathlist.Add(path);
            m_pathlist.Sort();
        }

        /// <summary>
        /// 経路削除
        /// </summary>
        /// <param name="path"></param>
        public virtual void DelPath(TrainPath path)
        {
            m_pathlist.Remove(path);
        }

        /// <summary>
        /// ライン追加
        /// </summary>
        /// <param name="setname"></param>
        /// <param name="line"></param>
        public void AddLine(string setname, TrainLine line)
        {
            GetLineList(setname).Add(line);
            line.Network = this;
        }

        /// <summary>
        /// ライン追加
        /// </summary>
        /// <param name="line"></param>
        public virtual void AddLine(TrainLine line)
        {
            m_linelist.Add(line);

            line.Network = this;
        }

        /// <summary>
        /// ライン削除
        /// </summary>
        /// <param name="line"></param>
        public void DelLine(TrainLine line)
        {
            m_linelist.Remove(line);
        }

        /// <summary>
        /// 経路取得
        /// </summary>
        /// <param name="uniqid"></param>
        /// <returns></returns>
        public TrainPath GetPath(int uniqid)
        {
            return m_pathlist.SingleOrDefault(path => path.UniqID == uniqid);
        }

        /// <summary>
        /// パスカウント
        /// </summary>
        /// <returns></returns>
        public int CountPath()
        {
            return m_pathlist.Count;
        }

        /// <summary>
        /// 一意識別子取得
        /// </summary>
        /// <returns></returns>
        public int NextPathUniqID()
        {
            for (int iId = 1; ; iId++)
            {
                if (m_pathlist.Exists(path => path.UniqID == iId) == false)
                    return iId;
            }
        }

        /// <summary>
        /// ライン取得
        /// </summary>
        /// <param name="uniqid"></param>
        /// <returns></returns>
        public TrainLine GetLine(string name)
        {
            return m_linelist.SingleOrDefault(line => line.Name == name);
        }

        /// <summary>
        /// ライン取得
        /// </summary>
        /// <param name="setname">セット名</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public TrainLine GetLine(string setname, Func<TrainLine, bool> predicate)
        {
            return GetLineList(setname).SingleOrDefault(predicate);
        }

        /// <summary>
        /// パスリスト取得
        /// </summary>
        /// <returns></returns>
        public TrainPath[] GetPathList()
        {
            return m_pathlist.ToArray();
        }

        /// <summary>
        /// ラインリスト取得
        /// </summary>
        /// <returns></returns>
        public TrainLine[] GetLineList()
        {
            return m_linelist.ToArray();
        }

        /// <summary>
        /// 経路削除
        /// </summary>
        public void RemovePath(TrainPath path)
        {
            m_pathlist.Remove(path);
            m_pathlist.Sort();
        }

        /// <summary>
        /// 時間軸
        /// </summary>
        protected TimeSpan m_now = new TimeSpan();

        /// <summary>
        /// 時間軸プロパティ
        /// </summary>
        public TimeSpan Now
        {
            get
            {
                return m_now;
            }
            set
            {
                m_now = value;
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="viewreqinfo">描画情報</param>
        /// <returns>True..描画範囲内 False..範囲外</returns>
        public virtual bool Draw(ViewRequestInfo viewreqinfo)
        {
            //経路描画
            foreach (var path in m_pathlist)
            {
                path.Draw(viewreqinfo);
            }

            //各ライン描画
            foreach (var line in m_linelist)
            {
                line.Draw(viewreqinfo, Now);
            }

            return true;
        }

        /// <summary>
        /// ライン構築
        /// </summary>
        /// <param name="from">先頭</param>
        /// <param name="to">最終</param>
        public List<List<TrainPath>> BuildLine(StationInfoData from, StationInfoData to)
        {
            return BuildLine( from, to, 500 );
        }

        /// <summary>
        /// ライン構築
        /// </summary>
        /// <param name="from">先頭</param>
        /// <param name="to">最終</param>
        public List<List<TrainPath>> BuildLine( StationInfoData from, StationInfoData to, int limit )
        {
            List<List<TrainPath>> linetable = new List<List<TrainPath>>();

            List<TrainPath> deeplinelist = new List<TrainPath>();

            if (BuildLineNest(from.UniqID, null, to.UniqID, deeplinelist, linetable, limit) == true)
            {
                linetable.Add(deeplinelist);
            }

            //Order整列
            linetable.ForEach(pathlist =>
            {
                if (pathlist.Count > 0)
                {
                    //先頭
                    if (pathlist[0].StationA.UniqID != from.UniqID)
                    {
                        pathlist[0].Reverse();
                    }
                }

                int iLastID = pathlist[0].StationA.UniqID;

                for( int iIdx=0; iIdx< pathlist.Count; iIdx++ )
                {
                    //パス向き整列
                    if (iLastID != pathlist[iIdx].StationA.UniqID)
                        pathlist[iIdx].Reverse();
                    iLastID = pathlist[iIdx].StationB.UniqID;

                    pathlist[iIdx].Order = iIdx + 1;
                }
            } );

            return linetable;
        }

        /// <summary>
        /// ライン構築探索処理
        /// </summary>
        /// <param name="lastuniqid">先頭</param>
        /// <param name="interpath">経路</param>
        /// <param name="touniqid">最終</param>
        /// <param name="deeplinelist">蓄積経路</param>
        /// <param name="linetable"></param>
        /// <returns></returns>
        protected bool BuildLineNest(int lastuniqid, TrainPath interpath, int touniqid, List<TrainPath> deeplinelist, List<List<TrainPath>> linetable, int limit)
        {
            //到着判定
            if (lastuniqid == touniqid)
            {
                return true;
            }

            if (linetable.Count() > limit)
                return false;

            //次の候補(先頭から繋がる複数候補)
            var secondpathlist = m_pathlist.Where(path => path.StationA.UniqID == lastuniqid || path.StationB.UniqID == lastuniqid);
            if (secondpathlist.Count() <= 0)
            {
                //終了
                return false;
            }

            //さらに探索
            var secondpatharray = secondpathlist.ToArray();
            foreach (var nextpath in secondpatharray)
            {
                //逆走禁止
                if (interpath != null && ( nextpath.StationA.UniqID == interpath.StationA.UniqID || nextpath.StationB.UniqID == interpath.StationA.UniqID ) )
                    continue;

                //StationAをFromとする
                var newpath = nextpath.Clone() as TrainPath;
                if (newpath.StationB.UniqID == lastuniqid)
                    newpath.Reverse();

                //ループチェック
                if (deeplinelist.Exists(path => path.StationA.UniqID == newpath.StationB.UniqID || path.StationB.UniqID == newpath.StationB.UniqID) == true)
                    continue;

                //deeplinelistのクローン
                List<TrainPath> linelist = new List<TrainPath>();
                deeplinelist.ForEach(ppp => linelist.Add(ppp.Clone() as TrainPath));
                linelist.Add(newpath);

                if (BuildLineNest(newpath.StationB.UniqID, newpath, touniqid, linelist, linetable, limit) == true)
                {
                    linetable.Add( linelist );
                }
            }

            return false;
        }

        /// <summary>
        /// 当たり判定
        /// </summary>
        /// <param name="iX">画像X座標</param>
        /// <param name="iY">画像Y座標</param>
        /// <returns>True..範囲内 False..範囲外</returns>
        public virtual bool IsHit(int iX, int iY, ViewRequestInfo viewreqinfo)
        {
            return false;
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
            foreach ( var path in m_pathlist)
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
                    if(area.Right < cliparea.Right)
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
            var mynode = doc.CreateElement("TrainNetwork");

            mynode.SetAttribute("SetKind", SetKind.ToString());

            var pathlistnode = doc.CreateElement("PathList");
            mynode.AppendChild(pathlistnode);
            foreach (var path in m_pathlist)
            {
                pathlistnode.AppendChild(path.ToXML(doc));
            }

            var linetablenode = doc.CreateElement("LineTable");
            mynode.AppendChild(linetablenode);
            foreach( var dr in m_linetable )
            {
                foreach (var line in dr.Value)
                {
                    var linelistnode = line.ToXML(doc) as XmlElement;
                    linelistnode.SetAttribute("SetName", dr.Key);
                    linetablenode.AppendChild(linelistnode);
                }
            }

            return mynode;
        }

        /// <summary>
        /// XML入力
        /// </summary>
        /// <param name="mynode"></param>
        public bool FromXML(XmlNode mynode, StationManager stationmanager)
        {
            if (mynode.Name != "TrainNetwork")
                return false;

            var setkindattr = mynode.Attributes["SetKind"];
            if(setkindattr != null)
            {
                SetKind = (SetKindType)Enum.Parse(typeof(SetKindType), setkindattr.Value);
            }

            foreach (XmlNode childnode in mynode.ChildNodes)
            {
                if (childnode.Name == "PathList")
                {
                    foreach (XmlNode pathnode in childnode.ChildNodes)
                    {
                        var path = new TrainPath();
                        if (path.FromXML(pathnode, stationmanager) == true)
                        {
                            m_pathlist.Add(path);
                        }

                        m_pathlist.Sort();
                    }
                }
                if (childnode.Name == "LineTable")
                {
                    foreach (XmlNode linenode in childnode.ChildNodes)
                    {
                        var line = new TrainLine();
                        line.Network = this;
                        if (line.FromXML(linenode, stationmanager) == true)
                        {
                            XmlElement linelistnode = linenode as XmlElement;

                            var setname = linelistnode.Attributes["SetName"].Value;

                            GetLineList(setname).Add(line);
                        }
                    }

                    ChangeLine(GetLineName());
                }
            }

            return true;
        }
    }
}
