using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// 時刻表管理
    /// </summary>
    public class SceduleManager
    {
        /// <summary>
        /// 電車情報
        /// </summary>
        protected TrainInfo m_train;

        /// <summary>
        /// 電車情報プロパティ
        /// </summary>
        public TrainInfo Train
        {
            get
            {
                return m_train;
            }
            set
            {
                m_train = value;
            }
        }

        /// <summary>
        /// 計画
        /// </summary>
        public class Plan : IComparable<Plan>
        {
            /// <summary>
            /// 順序
            /// </summary>
            protected int m_order = 0;

            /// <summary>
            /// 到着時間
            /// </summary>
            protected TimeSpan m_alivetime = new TimeSpan();

            /// <summary>
            /// 発車時間
            /// </summary>
            protected TimeSpan m_starttime = new TimeSpan();

            /// <summary>
            /// 対象駅
            /// </summary>
            protected StationInfoData m_station = null;

            /// <summary>
            /// 通過フラグ
            /// </summary>
            protected bool m_passing = false;

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
            /// 到着時間プロパティ
            /// </summary>
            public TimeSpan AliveTime
            {
                get
                {
                    return m_alivetime;
                }
                set
                {
                    m_alivetime = value;
                }
            }

            /// <summary>
            /// 発車時間プロパティ
            /// </summary>
            public TimeSpan StartTime
            {
                get
                {
                    return m_starttime;
                }
                set
                {
                    m_starttime = value;
                }
            }

            /// <summary>
            /// 対象駅プロパティ
            /// </summary>
            public StationInfoData Station
            {
                get
                {
                    return m_station;
                }
                set
                {
                    m_station = value;
                }
            }

            /// <summary>
            /// 通過プロパティ
            /// </summary>
            public bool Passing
            {
                get
                {
                    return m_passing;
                }
                set
                {
                    m_passing = value;
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public Plan()
            {
            }

            /// <summary>
            /// 比較演算子
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(Plan other)
            {
                return Order.CompareTo(other.Order);
            }

            /// <summary>
            /// XML出力
            /// </summary>
            /// <param name="parentnode"></param>
            public XmlNode ToXML(XmlDocument doc)
            {
                var mynode = doc.CreateElement("Plan");

                mynode.SetAttribute("Order", Order.ToString());
                mynode.SetAttribute("AliveTime", AliveTime.ToString());
                mynode.SetAttribute("StartTime", StartTime.ToString());
                mynode.SetAttribute("Station", Station.UniqID.ToString());
                mynode.SetAttribute("Passing", Passing.ToString());

                return mynode;
            }

            /// <summary>
            /// XML入力
            /// </summary>
            /// <param name="mynode"></param>
            public bool FromXML(XmlNode mynode, StationManager stationmanager )
            {
                if (mynode.Name != "Plan")
                    return false;

                Order = int.Parse(mynode.Attributes["Order"].Value);
                AliveTime = TimeSpan.Parse(mynode.Attributes["AliveTime"].Value);
                StartTime = TimeSpan.Parse(mynode.Attributes["StartTime"].Value);
                var stationid = int.Parse(mynode.Attributes["Station"].Value);
                Station = stationmanager.Get(stationid);

                var passingattr = mynode.Attributes["Passing"];
                if(passingattr != null)
                {
                    Passing = bool.Parse(passingattr.Value);
                }

                return true;
            }

            /// <summary>
            /// 文字列化
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return string.Format("{0},{1:00}:{2:00}:{3:00}-{4:00}:{5:00}:{6:00},{7}", Station.Name, 
                    AliveTime.Hours, AliveTime.Minutes, AliveTime.Seconds,
                    StartTime.Hours,StartTime.Minutes,StartTime.Seconds,Passing == true ? "Pass":"");
            }
        }

        /// <summary>
        /// 計画リスト
        /// </summary>
        protected List<Plan> m_planlist = new List<Plan>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SceduleManager()
        {
        }

        /// <summary>
        /// 計画追加
        /// </summary>
        /// <param name="plan"></param>
        public void Add(Plan plan)
        {
            m_planlist.Add(plan);

            m_planlist.Sort();
        }

        /// <summary>
        /// 計画削除
        /// </summary>
        /// <param name="plan"></param>
        public void Remove(Plan plan)
        {
            m_planlist.Remove(plan);
        }

        /// <summary>
        /// 計画破棄
        /// </summary>
        public void Clear()
        {
            m_planlist.Clear();
        }

        /// <summary>
        /// 計画リスト取得
        /// </summary>
        /// <returns></returns>
        public Plan[] GetList()
        {
            return m_planlist.ToArray();
        }

        /// <summary>
        /// 計画数
        /// </summary>
        public int Count
        {
            get
            {
                return m_planlist.Count;
            }
        }

        /// <summary>
        /// 一致している時刻が含まれているか判定
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsContain(SceduleManager other )
        {
            if (other.m_planlist.Count <= 0)
                return false;

            var otherplanlist = other.m_planlist.ToArray();

            int iotherFirstNo = -1;
            foreach ( var myplan in m_planlist )
            {
                if (iotherFirstNo < 0)
                {
                    //見つけていない場合
                    if (myplan.StartTime == otherplanlist[0].StartTime && myplan.Station.UniqID == otherplanlist[0].Station.UniqID)
                    {
                        iotherFirstNo = 1;
                    }
                }
                else
                {
                    if (myplan.StartTime == otherplanlist[iotherFirstNo].StartTime && myplan.Station.UniqID == otherplanlist[iotherFirstNo].Station.UniqID)
                    {
                        iotherFirstNo++;
                    }
                    else
                    {
                        return false;
                    }

                    //値域判定
                    if (iotherFirstNo == otherplanlist.Length)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 一致している計画数
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int ContainCount(SceduleManager other)
        {
            if (other.m_planlist.Count <= 0)
                return 0;

            int iMatchMyIdx = -1;
            int iMatchOtherIdx = -1;
            int iMatchCount = 0;

            for (int iMyIdx = 0; iMyIdx < m_planlist.Count; iMyIdx++)
            {
                if (iMatchMyIdx == -1)
                {
                    for (int iOtherIdx = 0; iOtherIdx < other.m_planlist.Count; iOtherIdx++)
                    {
                        if (m_planlist[iMyIdx].StartTime == other.m_planlist[iOtherIdx].StartTime && m_planlist[iMyIdx].Station.UniqID == other.m_planlist[iOtherIdx].Station.UniqID)
                        {
                            //発見
                            iMatchMyIdx = iMyIdx;
                            iMatchOtherIdx = iOtherIdx;

                            iMatchCount++;
                            iMatchOtherIdx++;
                            break;
                        }
                    }
                }
                else
                {
                    if (other.m_planlist.Count <= iMatchOtherIdx)
                        break;

                    if (m_planlist[iMyIdx].StartTime == other.m_planlist[iMatchOtherIdx].StartTime && m_planlist[iMyIdx].Station.UniqID == other.m_planlist[iMatchOtherIdx].Station.UniqID)
                        iMatchCount++;
                    else
                        break;
                    iMatchOtherIdx++;
                }
            }

            return iMatchCount;
        }

        /// <summary>
        /// 日またぎ敷居時刻
        /// </summary>
        protected TimeSpan m_overdaytime = new TimeSpan(0, 0, 0);

        /// <summary>
        /// 日またぎ敷居時刻プロパティ
        /// </summary>
        public TimeSpan OverDayTime
        {
            get
            {
                return m_overdaytime;
            }
            set
            {
                m_overdaytime = value;
            }
        }

        /// <summary>
        /// 位置算出
        /// </summary>
        /// <param name="nowtime"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        public Plan[] CurrentPoint(TimeSpan nowtime, ref double ratio)
        {
            if (nowtime < OverDayTime)
                nowtime = nowtime + new TimeSpan(1, 0, 0, 0);

            Plan lastplan = null;
            foreach (var plan in m_planlist)
            {
                if (lastplan != null && lastplan.AliveTime <= nowtime && nowtime < plan.AliveTime)
                {
                    var alltime = plan.AliveTime.Subtract(lastplan.StartTime);
                    var lasttime = plan.AliveTime.Subtract(nowtime);

                    ratio = 1.0 - lasttime.TotalMinutes / alltime.TotalMinutes;

                    //停止時間補正
                    if (ratio < 0.0)
                        ratio = 0.0;
                    if (1.0 < ratio)
                        ratio = 1.0;

                    return new Plan[] { lastplan, plan };
                }

                lastplan = plan;
            }

            return null;
        }

        /// <summary>
        /// XML出力
        /// </summary>
        /// <param name="parentnode"></param>
        public XmlNode ToXML(XmlDocument doc)
        {
            var mynode = doc.CreateElement("Scedule");

            foreach(var plan in m_planlist)
            {
                var plannode = plan.ToXML(doc);
                mynode.AppendChild(plannode);
            }

            mynode.SetAttribute("OverDayTime", OverDayTime.ToString());

            return mynode;
        }

        /// <summary>
        /// XML入力
        /// </summary>
        /// <param name="mynode"></param>
        public bool FromXML(XmlNode mynode, StationManager stationmanager)
        {
            if (mynode.Name != "Scedule")
                return false;

            OverDayTime = TimeSpan.Parse(mynode.Attributes["OverDayTime"].Value);

            foreach( XmlNode plannode in mynode.ChildNodes )
            {
                var plan = new Plan();
                if( plan.FromXML(plannode, stationmanager) == true )
                {
                    Add(plan);
                }
            }

            return true;
        }
    }
}
