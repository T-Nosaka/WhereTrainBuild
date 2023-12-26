using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhereTrainBuild.MapUtil;

namespace WhereTrainBuild.AI
{
    /// <summary>
    /// 鉄道ライン解析機
    /// </summary>
    public class LineWalker
    {
        /// <summary>
        /// 進捗コール型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="walkingpoint"></param>
        /// <param name="walkcount"></param>
        public delegate bool ProgressDelegate(LineWalker sender, latlontool.latlng walkingpoint, int walkcount);

        /// <summary>
        /// 進捗イベント
        /// </summary>
        public event ProgressDelegate OnProgress;

        /// <summary>
        /// ズームレベル
        /// </summary>
        protected const int m_zoom = 18;

        /// <summary>
        /// ズームレベルプロパティ
        /// </summary>
        public int Zoom
        {
            get
            {
                return m_zoom;
            }
        }

        /// <summary>
        /// 最大歩数プロパティ
        /// </summary>
        public int MaxWalking { get; set; }

        /// <summary>
        /// タイル管理
        /// </summary>
        protected MapTitleManager m_maptilemanager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LineWalker(MapTitleManager maptilemanager)
        {
            MaxWalking = 90000;
            m_maptilemanager = maptilemanager;
        }

        /// <summary>
        /// 歩数
        /// </summary>
        protected int m_walking = 0;

        protected class ThreshPack
        {
            /// <summary>
            /// 位置
            /// </summary>
            public latlontool.latlng Position ;
            /// <summary>
            /// 方向
            /// </summary>
            public PointF Vector;
            /// <summary>
            /// 角度差
            /// </summary>
            public double AngleDiff;
            /// <summary>
            /// 距離
            /// </summary>
            public double Distance;
            /// <summary>
            /// 距離差
            /// </summary>
            public double Diff;

            /// <summary>
            /// 判定用ウエイト
            /// </summary>
            /// <returns></returns>
            public double Weight()
            {
                return Math.Abs(AngleDiff) * 0.8 + Diff * 1.1;
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        public List<latlontool.latlng> Walk(latlontool.latlng start, PointF vector, latlontool.latlng end )
        {
            var walkinglist = new List<latlontool.latlng>();

            m_walking = 0;

            var nvector = vector;
            var nlatlng = start;

            while (true)
            {
                //まず、進行方向へ歩む
                (bool Result, latlontool.latlng next) result;

                var findlist = new List<ThreshPack>();

                // 進行方向 -65..65度 で、線路を索敵する
                for( int iD = 0; iD <= 65; iD += 1)
                {
                    bool bFind = false;
                    int iR = iD;

                    for (int iSign = 0; iSign < 2; iSign++)
                    {
                        if (iD == 0 && iSign == 1)
                            continue;
                        if (iSign == 1)
                            iR = -iD;

                        var vdo = latlontool.ToRadian(latlontool.ToAngle(Math.Atan2(nvector.Y, nvector.X)) + iR);
                        var testvector = new PointF((float)Math.Cos(vdo), (float)Math.Sin(vdo));
                        for (float fDistance = 3.0f; fDistance <= 40.0f; fDistance += 3.0f)// ここを調整する必要があるか 自身よりの距離
                        {
                            result = Move(nlatlng, testvector, fDistance);
                            if (result.Result == true)
                            {
                                bFind = true;
                                iSign = 1;//符号ループ脱出

                                //道を発見
                                findlist.Add(new ThreshPack() { Position = result.next, Vector = testvector, AngleDiff = iR });
                                break;
                            }
                        }
                    }

                    //発見始まりから終わりまで収集し、この中央値を採用する。
                    if (findlist.Count > 0 && bFind == false)
                        break;
                }
                if (findlist.Count > 0)
                {
                    result.Result = true;
                    result.next = findlist[findlist.Count / 2].Position;
                    nvector = findlist[findlist.Count / 2].Vector;
                    m_walking++;
                    walkinglist.Add(result.next);
                    //次の位置へ更新
                    nlatlng = result.next;

                    //終点チェック
                    if (latlontool.calcS(result.next, end) < 50.0f)
                        return walkinglist;
                }
                else
                {
                    //見つからない
                    return walkinglist;
                }

                if (m_walking >= MaxWalking)
                    return walkinglist;

                if (OnProgress != null)
                    if (OnProgress(this, nlatlng, m_walking) == false)
                        return walkinglist;
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        public List<latlontool.latlng> Walk2(latlontool.latlng start, PointF vector, latlontool.latlng end)
        {
            var walkinglist = new List<latlontool.latlng>();

            m_walking = 0;

            var nvector = vector;
            var nlatlng = start;

            while (true)
            {
                var findlist = new List<ThreshPack>();

                for (float fDistance = 2.0f; fDistance <= 60.0f; fDistance += 2.0f)
                {
                    double dDeltaR = 1 / fDistance * 2.0;

                    //とりあえず、前方向をサーチ
                    List<double> loopsrc = new List<double>();
                    for (double iD = -45.0; iD <= 45.0; iD += dDeltaR)
                        loopsrc.Add(iD);

                    Parallel.ForEach(loopsrc, (iD) =>
                    {
                        var vdo = latlontool.ToRadian(latlontool.ToAngle(Math.Atan2(nvector.Y, nvector.X)) + iD);
                        var testvector = new PointF((float)Math.Cos(vdo), (float)Math.Sin(vdo));
                        var result = Move(nlatlng, testvector, fDistance);
                        if (result.Result == true)
                        {
                            lock (findlist)
                            {
                                //発見
                                findlist.Add(new ThreshPack() { Position = result.next, Vector = testvector, AngleDiff = iD, Distance = fDistance });
                            }
                        }
                    });

                    if (findlist.Count > 0 && fDistance > 10.0f)
                        break;
                }

                if (findlist.Count <= 0)
                    //見つからない
                    return walkinglist;

                //ライン候補
                findlist.Sort((x, y)=> 
                {
                    var bSortResult = x.Distance.CompareTo(y.Distance);
                    if (bSortResult != 0)
                        return bSortResult;
                    return x.AngleDiff.CompareTo(y.AngleDiff);
                });

                //集団分布
                var mothertable = new Dictionary<int, List<ThreshPack>>();
                foreach( var pack in findlist)
                {
                    bool bFind = false;
                    //母集合内で近い集団見つける
                    foreach( var grp in mothertable)
                    {
                        //各集団より 相応しい集団を発見する
                        foreach (var fpack in grp.Value)
                        {
                            var fDistance = latlontool.calcS(fpack.Position, pack.Position);
                            if (fDistance < 3.0f )
                            {
                                //集団を発見したので、ここに含める
                                grp.Value.Add(pack);
                                //元位置よりの距離を計算しておく
                                pack.Diff = latlontool.calcS(fpack.Position, nlatlng);
                                bFind = true;
                                break;
                            }
                        }
                        if (bFind == true)
                            break;
                    }

                    //みつからない場合、新集団とする
                    if (bFind == false)
                    {
                        var grp = new List<ThreshPack>();
                        grp.Add(pack);
                        //元位置よりの距離を計算しておく
                        pack.Diff = latlontool.calcS(pack.Position, nlatlng);
                        mothertable[mothertable.Count] = grp;
                    }
                }

                var mothermintable = new Dictionary<int, double>();
                //各グループ内で最も近い点を持っている集団の中で最も近い点を採用する
                foreach ( var grp in mothertable)
                    mothermintable[grp.Key] = grp.Value.Min(ppp => ppp.Diff);
                var motherkey = mothermintable.Where(dr => dr.Value == mothermintable.Min(dr2 => dr2.Value)).First().Key;
                var linestable = mothertable[motherkey];

                //最近距離を採用する
                var finalpacklist = linestable.Where(lp => Math.Abs(lp.AngleDiff) == linestable.Min(lpp => Math.Abs(lpp.AngleDiff))).ToArray();
                var next = finalpacklist[finalpacklist.Length / 2];

                m_walking++;
                walkinglist.Add(next.Position);
                //次の位置へ更新
                nlatlng = next.Position;
                nvector = next.Vector;

                //終点チェック
                if (latlontool.calcS(nlatlng, end) < 50.0f)
                    return walkinglist;

                if (m_walking >= MaxWalking)
                    return walkinglist;

                if (OnProgress != null)
                    if (OnProgress(this, nlatlng, m_walking) == false)
                        return walkinglist;
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        public List<latlontool.latlng> Walk3(latlontool.latlng start, PointF vector, latlontool.latlng end)
        {
            var walkinglist = new List<latlontool.latlng>();

            m_walking = 0;

            var nvector = vector;
            var nlatlng = start;

            while (true)
            {
                var findlist = new List<ThreshPack>();

                for (float fDistance = 2.0f; fDistance <= 75.0f; fDistance += 2.0f)
                {
                    double dDeltaR = 1 / fDistance * 2.0;
                    if (dDeltaR < 0.3)
                        dDeltaR = 0.3;

                    double dSetaRange = 0.0f;
                    if( fDistance > 30.0f )
                    {
                        dSetaRange = 20.0f * (fDistance - 30.0f) / 30.0f;
                    }

                    //とりあえず、前方向をサーチ
                    List<double> loopsrc = new List<double>();
                    for (double iD = -40.0 + dSetaRange; iD <= 40.0 - dSetaRange; iD += dDeltaR)
                        loopsrc.Add(iD);

                    Parallel.ForEach(loopsrc, (iD) =>
                    {
                        var vdo = latlontool.ToRadian(latlontool.ToAngle(Math.Atan2(nvector.Y, nvector.X)) + iD);
                        var testvector = new PointF((float)Math.Cos(vdo), (float)Math.Sin(vdo));
                        var result = Move(nlatlng, testvector, fDistance);
                        if (result.Result == true)
                        {
                            lock (findlist)
                            {
                                //発見
                                findlist.Add(new ThreshPack() { Position = result.next, Vector = testvector, AngleDiff = iD, Distance = fDistance, Diff = latlontool.calcS(result.next, nlatlng) });
                            }
                        }
                    });
                }

                if (findlist.Count <= 0)
                    //見つからない
                    return walkinglist;

                //ライン候補
                findlist.Sort((x, y) =>
                {
                    var bSortResult = x.AngleDiff.CompareTo(y.AngleDiff);
                    if (bSortResult != 0)
                        return bSortResult;
                    return x.Distance.CompareTo(y.Distance);
                });

                //連続で対象を発見した候補の真ん中だけピックアップする。
                //前点との距離で連続性を判断する
                var linestable = new List<ThreshPack>();
                var lines = new List<ThreshPack>();
                ThreshPack iLastPoint = null;
                foreach (var pack in findlist)
                {
                    if (iLastPoint == null)
                    {
                        iLastPoint = pack;
                        lines.Add(pack);
                    }
                    else
                    {
                        var dDiffDistance = latlontool.calcS(iLastPoint.Position, pack.Position);
                        if (dDiffDistance > 1.0f)
                        {
                            //連続性が無いので、別ライン候補
                            var pppp = (lines.Where(lp => Math.Abs(lp.AngleDiff) == lines.Min(lpp => Math.Abs(lpp.AngleDiff)))).ToArray();
                            //貯めたライン達の真ん中のやつを候補とする。
                            linestable.Add(pppp[pppp.Length / 2]);

                            lines.Clear();
                            iLastPoint = null;
                        }
                        else
                        {
                            lines.Add(pack);
                            iLastPoint = pack;
                        }
                    }
                }
                if (lines.Count > 0)
                {
                    var pppp = (lines.Where(lp => Math.Abs(lp.AngleDiff) == lines.Min(lpp => Math.Abs(lpp.AngleDiff)))).ToArray();
                    linestable.Add(pppp[pppp.Length / 2]);
                }

                //差分の少ない候補とする
                var finalpacklist = linestable.Where(lp => lp.Weight() == linestable.Min(lpp => lpp.Weight())).ToArray();
                var next = finalpacklist[finalpacklist.Length / 2];
                m_walking++;
                walkinglist.Add(next.Position);
                //次の位置へ更新
                nlatlng = next.Position;
                nvector = next.Vector;

                //終点チェック
                if (latlontool.calcS(nlatlng, end) < 50.0f)
                    return walkinglist;

                if (m_walking >= MaxWalking)
                    return walkinglist;

                if (OnProgress != null)
                    if (OnProgress(this, nlatlng, m_walking) == false)
                        return walkinglist;
            }
        }

        /// <summary>
        /// キャッシュ
        /// </summary>
        protected Dictionary<string, MapTile> m_cache = new Dictionary<string, MapTile>();

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="pnt"></param>
        /// <param name="vector"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        protected (bool Result, latlontool.latlng next) Move(latlontool.latlng pnt, PointF vector, float distance )
        {
            //メルカトル変換
            double dX = 0, dY = 0;
            MercatorTrans.Trans( latlontool.ToRadian(pnt.lat), latlontool.ToRadian(pnt.lng), m_zoom, ref dX, ref dY);

            //進める
            dX += (vector.X * distance);
            dY += (vector.Y * distance);

            int iTileX = (int)(dX / MercatorTrans.ViewSize);
            int iTileY = (int)(-dY / MercatorTrans.ViewSize);

            int iLeft = (int)(iTileX * MercatorTrans.ViewSize);
            int iTop = (int)(iTileY* MercatorTrans.ViewSize);

            var x = Math.Round(dX - iLeft);
            var y = Math.Round(-iTop - dY);

            if (x < 0)
            {
                x = MercatorTrans.ViewSize;
                iTileX--;
            }
            if( y<0)
            {
                y = MercatorTrans.ViewSize;
                iTileY--;
            }
            if(x>= MercatorTrans.ViewSize)
            {
                x = 0;
                iTileX++;
            }
            if (y >= MercatorTrans.ViewSize)
            {
                y = 0;
                iTileY++;
            }

            Color color;

            lock (this)
            {
                MapTile maptile = null;
                if (m_cache.ContainsKey(MapTile.MakeKey(iTileX, iTileY, m_zoom)) == false)
                {
                    maptile = m_maptilemanager.GetMapTile(iTileX, iTileY, m_zoom);
                    if (maptile == null)
                        maptile = m_maptilemanager.BuildMapTile(iTileX, iTileY, m_zoom);
                    if (maptile != null)
                        m_cache[MapTile.MakeKey(iTileX, iTileY, m_zoom)] = maptile;
                }
                else
                    maptile = m_cache[MapTile.MakeKey(iTileX, iTileY, m_zoom)];

                lock (maptile)
                {
                    var image = maptile.GetImage() as Bitmap;
                    if (image == null)
                    {
                        image = maptile.GetImage(m_maptilemanager.BaseFolder, m_maptilemanager.Timeout) as Bitmap;
                    }
                    if (image == null)
                    {
                        return (Result: false, next: pnt);
                    }
                    //画像取得
                    color = image.GetPixel((int)x, (int)y);
                }
            }

            //判定
            var weight = TileAnalyzer3.Weight(color, Color.FromArgb(255, 109, 15, 15));
            if (weight > 0.7f)
            {
                //リバース変換
                double dNextLat = 0, dNextLng = 0;
                MercatorTrans.Reverse(dX, dY, m_zoom, ref dNextLat, ref dNextLng);
                latlontool.latlng nextpnt = new latlontool.latlng(latlontool.ToAngle(dNextLat), latlontool.ToAngle(dNextLng));

                return (Result: true, next: nextpnt);
            }

            return (Result : false, next : pnt);
        }

        /// <summary>
        /// 最適化
        /// </summary>
        /// <param name="vallist"></param>
        /// <returns></returns>
        public List<latlontool.latlng> Optimisation(List<latlontool.latlng> vallist)
        {
            var tslist = ThinningStandardDeviation(vallist,20);
            var dlist =  DistanceDeviation(tslist, 50.0f);
            return DistanceDeviation(OnTheLineDeviation(dlist,2.2f),5f);
        }

        /// <summary>
        /// 同一線上間引き
        /// </summary>
        /// <returns></returns>
        protected List<latlontool.latlng> OnTheLineDeviation(List<latlontool.latlng> vallist, double distance)
        {
            List<latlontool.latlng> result = new List<latlontool.latlng>();

            latlontool.latlng startpoint = null, midpoint = null;

            foreach (var pnt in vallist)
            {
                if(pnt == vallist.Last())
                {
                    result.Add(pnt);
                    break;
                }

                //始点終点中点が存在する場合のみ
                if (startpoint != null && midpoint != null)
                {
                    var lastpoint = pnt;

                    //メルカトル変換
                    double x0 = 0, y0 = 0;
                    double x1 = 0, y1 = 0;
                    double x2 = 0, y2 = 0;
                    MercatorTrans.Trans(latlontool.ToRadian(midpoint.lat), latlontool.ToRadian(midpoint.lng), m_zoom, ref x0, ref y0);
                    MercatorTrans.Trans(latlontool.ToRadian(startpoint.lat), latlontool.ToRadian(startpoint.lng), m_zoom, ref x1, ref y1);
                    MercatorTrans.Trans(latlontool.ToRadian(lastpoint.lat), latlontool.ToRadian(lastpoint.lng), m_zoom, ref x2, ref y2);

                    //中点と線の距離
                    double dDistance = 0.0f;
                    var a = x2 - x1;
                    var b = y2 - y1;
                    var a2 = a * a;
                    var b2 = b * b;
                    var r2 = a2 + b2;
                    var tt = -(a * (x1 - x0) + b * (y1 - y0));
                    if (tt < 0)
                    {
                        dDistance = (x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0);
                    }
                    else
                    if (tt > r2)
                    {
                        dDistance = (x2 - x0) * (x2 - x0) + (y2 - y0) * (y2 - y0);
                    }
                    else
                    {
                        var f1 = a * (y1 - y0) - b * (x1 - x0);
                        dDistance = (f1 * f1) / r2;
                    }

                    if(dDistance > distance)
                    {
                        //線上にはないので、この線上の最適化は終了
                        startpoint = midpoint;
                        result.Add(midpoint);
                        midpoint = null;
                    }
                    else
                    {
                        //線上にある .. 間引き対象
                        midpoint = lastpoint;
                    }
                }

                //始点
                if (startpoint == null)
                {
                    startpoint = pnt;

                    //追加
                    result.Add(pnt);
                }
                else
                //中点
                if (startpoint != null && midpoint == null)
                {
                    midpoint = pnt;
                }
            }

            return result;
        }

        /// <summary>
        /// 距離間引き
        /// </summary>
        /// <param name="vallist"></param>
        protected List<latlontool.latlng> DistanceDeviation(List<latlontool.latlng> vallist, double distance )
        {
            List<latlontool.latlng> result = new List<latlontool.latlng>();

            latlontool.latlng last = null;

            foreach (var pnt in vallist)
            {
                if (last != null )
                {
                    if( latlontool.calcS(last, pnt) > distance || pnt == vallist.Last())
                    {
                        last = null;
                    }
                }

                if (last == null)
                {
                    last = pnt;

                    //追加
                    result.Add(pnt);
                }
            }

            return result;
        }

        /// <summary>
        /// 標準偏差間引き
        /// </summary>
        /// <param name="vallist"></param>
        protected List<latlontool.latlng> ThinningStandardDeviation(List<latlontool.latlng> vallist, int block )
        {
            List<latlontool.latlng> result = new List<latlontool.latlng>();

            latlontool.latlng last = null;

            var anglelist = new List<double>();
            int iCount = 0;
            int iVIndex = 0;
            foreach ( var pnt in vallist )
            {
                if( last != null )
                {
                    //角度
                    double dX1 = 0, dY1 = 0;
                    double dX2 = 0, dY2 = 0;
                    MercatorTrans.Trans(latlontool.ToRadian(last.lat), latlontool.ToRadian(last.lng), m_zoom, ref dX1, ref dY1);
                    MercatorTrans.Trans(latlontool.ToRadian(pnt.lat), latlontool.ToRadian(pnt.lng), m_zoom, ref dX2, ref dY2);

                    var vdo = latlontool.ToAngle(Math.Atan2(dY2 - dY1, dX2 - dX1));
                    anglelist.Add(vdo);
                    iCount++;
                }

                if(iCount > block)
                {
                    int iTopIndex = iVIndex - anglelist.Count;

                    //標準偏差
                    var average = anglelist.Average();
                    var standarddeviation = Math.Sqrt(anglelist.Sum(angle => Math.Pow(angle - average, 2)) / (double)anglelist.Count);

                    //閾値
                    var minimum = average - standarddeviation;
                    var maximum = average + standarddeviation;

                    for( int iIdx=0; iIdx< anglelist.Count; iIdx++ )
                    {
                        if(minimum < anglelist[iIdx] && anglelist[iIdx] < maximum )
                        {
                            result.Add(vallist[iTopIndex + iIdx]);
                        }
                    }

                    anglelist.Clear();
                    iCount = 0;
                }

                last = pnt;
                iVIndex++;
            }

            if( iCount > 0 )
            {
                int iTopIndex = iVIndex - anglelist.Count;
                for (int iIdx = 0; iIdx < anglelist.Count; iIdx++)
                {
                    result.Add(vallist[iTopIndex + iIdx]);
                }
            }

            return result;

        }
    }
}
