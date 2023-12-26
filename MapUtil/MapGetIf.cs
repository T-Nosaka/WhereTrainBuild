using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WhereTrainBuild.MapUtil
{
    /// <summary>
    /// マップ画像取得インターフェース
    /// </summary>
    public interface MapGetIf : IDisposable
    {
        /// <summary>
        /// 地図画像取得
        /// </summary>
        /// <param name="iTileX"></param>
        /// <param name="iTileY"></param>
        /// <param name="iZoom"></param>
        /// <param name="timeout"></param>
        Image GetMap(int iTileX, int iTileY, int iZoom, int timeout);
    }
}
