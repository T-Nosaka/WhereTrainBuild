using System;
using System.Collections.Generic;
using System.Text;

namespace WhereTrainBuild.MapUtil.View
{
    /// <summary>
    /// ビューポイントインターフェース
    /// </summary>
    public interface ViewPointIF
    {
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="viewreqinfo">描画情報</param>
        /// <returns>True..描画範囲内 False..範囲外</returns>
        bool Draw(ViewRequestInfo viewreqinfo);

        /// <summary>
        /// 当たり判定
        /// </summary>
        /// <param name="iX">画像X座標</param>
        /// <param name="iY">画像Y座標</param>
        /// <returns>True..範囲内 False..範囲外</returns>
        bool IsHit(int iX, int iY, ViewRequestInfo viewreqinfo);
    }
}
