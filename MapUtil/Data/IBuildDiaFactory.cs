using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// ダイア構築インターフェース
    /// </summary>
    public interface IBuildDiaFactory: IFactory
    {
        /// <summary>
        /// 初期ビルド
        /// </summary>
        void InitBuild();

        /// <summary>
        /// ダイヤ構築
        /// </summary>
        /// <param name="targetdt"></param>
        void BuildDaia(DateTime targetdt, string lineset);
    }
}
