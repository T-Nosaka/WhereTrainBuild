using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// ファクトリーインターフェース
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// 文字列置換マップ
        /// </summary>
        Dictionary<string, string> MapTable
        {
            get;
            set;
        }

        /// <summary>
        /// 日またぎ時間プロパティ
        /// </summary>
        TimeSpan OverTime
        {
            get;
            set;
        }

        /// <summary>
        /// 保存日時プロパティ
        /// </summary>
        DateTime SaveDate
        {
            set;
        }

        /// <summary>
        /// 駅管理取得
        /// </summary>
        /// <returns></returns>
        StationManager GetStationManager();

        /// <summary>
        /// ネットワーク取得
        /// </summary>
        /// <returns></returns>
        TrainNetwork GetNetwork();

        /// <summary>
        /// XML出力
        /// </summary>
        /// <param name="parentnode"></param>
        XmlNode ToXML(XmlDocument doc);

        /// <summary>
        /// XML入力
        /// </summary>
        /// <param name="mynode"></param>
        bool FromXML(XmlNode mynode);
    }
}
