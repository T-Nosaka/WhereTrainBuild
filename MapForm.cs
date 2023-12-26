using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;

using WhereTrainBuild.MapUtil;
using WhereTrainBuild.MapUtil.Data;
using WhereTrainBuild.Dialog;
using WhereTrainBuild.AI;
using WhereTrainBuild.MapUtil.View;

namespace WhereTrainBuild
{
    /// <summary>
    /// DW地図フォーム
    /// </summary>
    public partial class MapForm : Form
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        protected string m_filename = string.Empty;

        /// <summary>
        /// 要求リスト
        /// </summary>
        protected List<MapTile> m_request = new List<MapTile>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapForm()
        {
            MapTile.SetParallelism(100);

            InitializeComponent();

            MapViewCnt.MouseWheel += new MouseEventHandler(OnMapMouseWheel);

        }

        /// <summary>
        /// マウスホイールイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMapMouseWheel(object sender, MouseEventArgs e)
        {
            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 60;

            if (ZoomBar.Value + numberOfTextLinesToMove > ZoomBar.Maximum)
                numberOfTextLinesToMove = ZoomBar.Maximum - ZoomBar.Value;
            if(ZoomBar.Value + numberOfTextLinesToMove < ZoomBar.Minimum)
                numberOfTextLinesToMove = ZoomBar.Minimum - ZoomBar.Value;

            ZoomBar.Value += numberOfTextLinesToMove;

            MapViewCnt.MapScale = (int)ZoomBar.Value;
            MapViewCnt.RefreshImage();
        }

        /// <summary>
        /// ズームバー変化発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomBar_Scroll(object sender, EventArgs e)
        {
            if (DesignMode == true)
                return;

            MapViewCnt.MapScale = (int)ZoomBar.Value;
            MapViewCnt.RefreshImage();
        }

        /// <summary>
        /// アプリ情報
        /// </summary>
        protected static Dictionary<string, string> m_appinfo = null;

        /// <summary>
        /// アプリ情報
        /// </summary>
        public static Dictionary<string, string> AppInfo()
        {
            if (m_appinfo == null)
            {
                Dictionary<string, string> table = new Dictionary<string, string>();

                var appinfo = Path.Combine(Application.LocalUserAppDataPath, "appinfo.xml");

                try
                {
                    var sysxml = new XmlDocument();
                    if (File.Exists(appinfo) == true)
                    {
                        sysxml.Load(appinfo);
                    }

                    var lroot = sysxml.DocumentElement;
                    var lmapnode = lroot.SelectSingleNode("map");
                    table["mappath"] = lmapnode.Attributes["path"].Value;
                    table["mapurl"] = lmapnode.Attributes["url"].Value;

                    table["center_lat"] = lmapnode.Attributes["center_lat"].Value;
                    table["center_lng"] = lmapnode.Attributes["center_lng"].Value;
                }
                catch
                {
                    //デフォルト
                    table["mappath"] = Path.Combine(Application.LocalUserAppDataPath, "mapimage");
                    table["mapurl"] = "http://cyberjapandata.gsi.go.jp/xyz/pale/{0}/{1}/{2}.png";
                    table["center_lat"] = "34.646482";
                    table["center_lng"] = "135.513932";
                }

                m_appinfo = table;
            }

            return m_appinfo;
        }

        /// <summary>
        /// アプリ情報保存
        /// </summary>
        protected void SaveAppInfo()
        {
            var appinfo = AppInfo();

            var sysxml = new XmlDocument();
            var root = sysxml.CreateElement("root");
            sysxml.AppendChild(root);

            var mapnode = sysxml.CreateElement("map");
            root.AppendChild(mapnode);
            mapnode.SetAttribute("path", appinfo["mappath"]);
            mapnode.SetAttribute("url", appinfo["mapurl"]);

            double lat = 0.0;
            double lng = 0.0;
            MapViewCnt.ViewArea.ViewInfo.GetCenterLatLon(ref lat, ref lng);
            mapnode.SetAttribute("center_lat", lat.ToString());
            mapnode.SetAttribute("center_lng", lng.ToString());

            sysxml.Save(Path.Combine(Application.LocalUserAppDataPath, "appinfo.xml"));
        }

        /// <summary>
        /// ロードイベント発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapForm_Load(object sender, EventArgs e)
        {
            if (DesignMode == true)
                return;

            string mapfolder = AppInfo()["mappath"];
            MapViewCnt.TileManager.NativeMathod = true;
            MapViewCnt.TileManager.BaseFolder = mapfolder;
            MapViewCnt.ViewArea.ViewInfo.SetCenterLatLon(double.Parse(AppInfo()["center_lat"]), double.Parse(AppInfo()["center_lng"]));
            MapViewCnt.MapScale = (int)ZoomBar.Value;

            MapViewCnt.TileManager.NotifyRequested += ( maptilemanager,  maptile) =>
            {
                lock (m_request)
                {
                    m_request.Add(maptile);
                }

                RefreshRequest();
            };
            MapViewCnt.TileManager.NotifyCompleted += (maptilemanager, maptile, result ) =>
            {
                lock (m_request)
                {
                    m_request.Remove(maptile);
                }

                RefreshRequest();
            };

            MapViewCnt.TileManager.SetBuildMapTile(( tile_x,  tile_y,  zoom) =>
            {
                var maptile = new MapTile(tile_x, tile_y, zoom);

                var osmget = maptile.MapGetter as MapUtil.OSM.OSMGet;
                osmget.URLFormat = AppInfo()["mapurl"];

                return maptile;
            });


            MapViewCnt.ViewArea.Add(m_factory.GetStationManager());

            MapViewCnt.ViewArea.Add(m_factory.GetNetwork());

            MapViewCnt.ViewArea.Add(m_line);

        }

        /// <summary>
        /// フォームクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveAppInfo();
        }

        /// <summary>
        /// 要求数更新
        /// </summary>
        protected void RefreshRequest()
        {
            int iCnt = 0;

            lock (m_request)
            {
                iCnt = m_request.Count;
            }

            var callback = new MethodInvoker(()=>
            {
                ReqLbl.Text = string.Format("要求={0}", iCnt);
            });

            try
            {
                if (InvokeRequired == true)
                {
                    Invoke(callback);
                }
                else
                {
                    callback();
                }
            }
            catch { } //終了対策
        }

        /// <summary>
        /// リサイズイベント発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapForm_Resize(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// タイマーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScheduleTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            m_factory.GetNetwork().Now = now.TimeOfDay;

            MapViewCnt.RefreshImage();
        }

        #region メニュー選択
        /// <summary>
        /// 地図フォルダ選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mapFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = MapViewCnt.TileManager.BaseFolder;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    MapViewCnt.TileManager.BaseFolder = dlg.SelectedPath;
                    AppInfo()["mappath"] = MapViewCnt.TileManager.BaseFolder;
                }
            }
        }

        /// <summary>
        /// 地図URLメニュー選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapURLMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new MapSelectDialog())
            {
                dlg.URLTxt.Text = AppInfo()["mapurl"];
                if( dlg.ShowDialog(this) == DialogResult.OK )
                {
                    AppInfo()["mapurl"] = dlg.URLTxt.Text;

                    MapViewCnt.TileManager.SetBuildMapTile((tile_x, tile_y, zoom) =>
                    {
                        var maptile = new MapTile(tile_x, tile_y, zoom);

                        var osmget = maptile.MapGetter as MapUtil.OSM.OSMGet;
                        osmget.URLFormat = AppInfo()["mapurl"];

                        return maptile;
                    });
                }
            }

        }

        /// <summary>
        /// Exitボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "WTX(*.wtx)|*.wtx|すべてのファイル(*.*)|*.*";
                dlg.DefaultExt = "wtx";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadFactory(dlg.FileName);
                }
            }
        }

        /// <summary>
        /// ファクトリロード
        /// </summary>
        /// <param name="filename"></param>
        protected void LoadFactory( string filename )
        {
            m_filename = filename;

            Text = string.Format("WhereTrainBuild({0})", Path.GetFileNameWithoutExtension(m_filename));

            var factory = new PlaneFactory();

            var doc = new XmlDocument();
            doc.Load(filename);

            var rootnode = doc.DocumentElement;

            foreach (XmlNode factorynode in rootnode.ChildNodes)
            {
                if (factory.FromXML(factorynode) == true)
                {
                    break;
                }
            }

            InitializeFactory(factory);
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.FileName = m_filename;
                dlg.Filter = "WTX(*.wtx)|*.wtx|すべてのファイル(*.*)|*.*";
                dlg.DefaultExt = "wtx";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var doc = new XmlDocument();

                    var rootnode = doc.CreateElement("root");
                    doc.AppendChild(rootnode);

                    m_factory.SaveDate = DateTime.Now;
                    var exportnode = m_factory.ToXML(doc);
                    rootnode.AppendChild(exportnode);

                    doc.Save(dlg.FileName);

                    m_filename = dlg.FileName;
                    Text = string.Format("WhereTrainBuild({0})", Path.GetFileNameWithoutExtension(m_filename));
                }
            }
        }

        /// <summary>
        /// 上書き保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (m_filename != string.Empty)
            {
                var doc = new XmlDocument();

                var rootnode = doc.CreateElement("root");
                doc.AppendChild(rootnode);

                m_factory.SaveDate = DateTime.Now;
                var exportnode = m_factory.ToXML(doc);
                rootnode.AppendChild(exportnode);

                doc.Save(m_filename);
            }
            else
                Save_Click(sender, e);
        }

        #endregion

        /// <summary>
        /// ファクトリ
        /// </summary>
        protected IFactory m_factory = new PlaneFactory();

        /// <summary>
        /// ファクトリプロパティ
        /// </summary>
        public IFactory Factory
        {
            get
            {
                return m_factory;
            }
        }

        #region 地図内操作イベント関連

        /// <summary>
        /// マウス位置変化発生
        /// </summary>
        /// <param name="dLat"></param>
        /// <param name="dLng"></param>
        private void MapViewCnt_OnMercatorMousePoint(Point viewpoint, double dLat, double dLng, MouseEventArgs mouseev)
        {
            if (DesignMode == true)
                return;

            InfoLbl.Text = string.Format("{0},{1}", dLat, dLng);

            if (m_mode == EditModeType.NORMAL)
            {
                if (IsCrossPath(viewpoint) != null )
                {
                    if (Cursor != System.Windows.Forms.Cursors.Cross)
                        Cursor = System.Windows.Forms.Cursors.Cross;
                }
                else
                {
                    if (Cursor != System.Windows.Forms.Cursors.Default)
                        Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            else
            if (m_mode == EditModeType.DRAGSTATION )
            {
                if (m_drag_station != null)
                {
                    if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                    {
                        m_drag_station = null;
                        m_mode = EditModeType.NORMAL;
                    }
                    else
                    {
                        m_drag_station.Latitude = dLat;
                        m_drag_station.Longitude = dLng;
                        MapViewCnt.RefreshImage();
                    }
                }
                else
                {
                    m_mode = EditModeType.NORMAL;
                }
            } else
                if (m_mode == EditModeType.ADDPATH)
                {
                    if (m_drag_station != null)
                    {
                        var latlonlist = m_line.ToList();
                        if (latlonlist.Count() > 0)
                        {
                            latlonlist[latlonlist.Count() - 1].lat = dLat;
                            latlonlist[latlonlist.Count() - 1].lng = dLng;
                            MapViewCnt.RefreshImage();
                        }
                    }
                    else
                    {
                        m_mode = EditModeType.NORMAL;
                    }
                } else
                    if (m_mode == EditModeType.DRAGPOSITION)
                    {
                        if (m_drag_position != null)
                        {
                            if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                            {
                                m_drag_position = null;
                                m_mode = EditModeType.NORMAL;
                            }
                            else
                            {
                                m_drag_position.Latitude = dLat;
                                m_drag_position.Longitude = dLng;
                                MapViewCnt.RefreshImage();
                            }
                        }
                    }
        }

        /// <summary>
        /// マウスアップイベント発生
        /// </summary>
        /// <param name="viewpoint"></param>
        /// <param name="dLat"></param>
        /// <param name="dLng"></param>
        private void MapViewCnt_OnMercatorUpPoint(Point viewpoint, double dLat, double dLng, MouseEventArgs mouseev )
        {
            if (mouseev.Button != MouseButtons.Right)
                return;

            ContextMenuStrip popupmenu = new ContextMenuStrip();

            ToolStripMenuItem centeritem = new ToolStripMenuItem("中心移動");
            centeritem.Click += (sender, args) =>
            {
                MapViewCnt.ViewArea.ViewInfo.SetCenterLatLon(dLat, dLng);
                MapViewCnt.RefreshImage();
            };
            popupmenu.Items.Add(centeritem);

            ToolStripMenuItem addstationitem = new ToolStripMenuItem("駅追加");
            addstationitem.Click += (sender, args) =>
            {
                using (var dlg = new Dialog.StationDialog())
                {
                    var stationinfo = new StationInfoData();
                    stationinfo.Latitude = dLat;
                    stationinfo.Longitude = dLng;
                    stationinfo.Manager = m_factory.GetStationManager();
                    dlg.SetStation(stationinfo);
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        stationinfo.UniqID = m_factory.GetStationManager().NextUniqID();
                        m_factory.GetStationManager().Add(stationinfo);
                        MapViewCnt.RefreshImage();
                    }
                }
            };
            popupmenu.Items.Add(addstationitem);

            popupmenu.Show(Cursor.Position);
        }

        /// <summary>
        /// 編集モード型
        /// </summary>
        protected enum EditModeType
        {
            NORMAL,
            DRAGSTATION,
            ADDPATH,
            DRAGPOSITION,
            SELECTPOINT
        }

        /// <summary>
        /// 編集モード
        /// </summary>
        protected EditModeType m_mode = EditModeType.NORMAL;

        /// <summary>
        /// ドラッグ中駅
        /// </summary>
        protected StationInfoData m_drag_station = null;

        /// <summary>
        /// ドラッグ中経由地
        /// </summary>
        protected TrainPath.Position m_drag_position = null;

        /// <summary>
        /// 描画中線
        /// </summary>
        protected LineInfo m_line = new LineInfo();

        /// <summary>
        /// ポイント選択型
        /// </summary>
        public delegate void SelectPointDelegate(WhereTrainBuild.MapUtil.View.ViewRequestInfo viewrequestinfo, Point viewpoint, double dLat, double dLng);

        /// <summary>
        /// ポイント選択イベント
        /// </summary>
        protected SelectPointDelegate OnSelectPoint;

        /// <summary>
        /// ポイント選択モード
        /// </summary>
        public bool SetSelectPoint( SelectPointDelegate callback )
        {
            if (m_mode != EditModeType.NORMAL)
                return false;

            OnSelectPoint = callback;

            m_mode = EditModeType.SELECTPOINT;

            return true;
        }

        /// <summary>
        /// マウスダウンイベント発生
        /// </summary>
        /// <param name="viewpoint"></param>
        /// <param name="dLat"></param>
        /// <param name="dLng"></param>
        /// <param name="e"></param>
        private void MapViewCnt_OnMercatorDownPoint(Point viewpoint, double dLat, double dLng, MouseEventArgs mouseev)
        {
            double HitDistance = 5.0;

            if (m_mode == EditModeType.SELECTPOINT)
            {
                //イベント発火
                if (OnSelectPoint != null)
                    OnSelectPoint(MapViewCnt.ViewArea.ViewInfo, viewpoint, dLat, dLng);

                m_mode = EditModeType.NORMAL;

                return;
            }

            //駅選択判定
            Dictionary<int, double> result = new Dictionary<int, double>();
            foreach (var station in m_factory.GetStationManager().StationList())
            {
                //画面座標変換
                var mypoint = MapViewCnt.ViewArea.ViewInfo.LatLongToViewPoint(station.Latitude, station.Longitude);

                //距離算出
                double dDistance = Math.Sqrt((mypoint.X - viewpoint.X) * (mypoint.X - viewpoint.X) + (mypoint.Y - viewpoint.Y) * (mypoint.Y - viewpoint.Y));
                if (HitDistance >= dDistance)
                    result[station.UniqID] = dDistance;
            }
            if (result.Count <= 0)
            {
                if (m_mode == EditModeType.NORMAL)
                {
                    //経由地判定
                    var interresult = IsInterPosition(viewpoint);
                    if (interresult != null)
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            //経由地移動
                            m_drag_position = interresult.Position;
                            m_mode = EditModeType.DRAGPOSITION;
                            return;
                        }
                        else
                        {
                            //経由地選択
                            PopupMenuSelectInterPosition(interresult);
                            return;
                        }
                    }

                    //交点判定
                    var crossresult = IsCrossPath(viewpoint);
                    if (crossresult != null)
                    {
                        PopupMenuSelectLine(crossresult);
                        return;
                    }
                } else
                if (m_mode == EditModeType.DRAGSTATION)
                {
                    m_drag_station = null;
                    m_mode = EditModeType.NORMAL;
                } else
                    if (m_mode == EditModeType.ADDPATH)
                    {
                        m_drag_station = null;
                        m_line.Visible = false;
                        m_mode = EditModeType.NORMAL;
                        MapViewCnt.RefreshImage();
                    } else
                        if (m_mode == EditModeType.DRAGPOSITION)
                        {
                            m_drag_position = null;
                            m_mode = EditModeType.NORMAL;
                        }
                return;
            }

            var min = result.Min( dr => dr.Value );
            var uniqid = result.First( dr => dr.Value == min ).Key;
            var target = m_factory.GetStationManager().Get(uniqid);
            if (target == null)
            {
                return;
            }

            if (m_mode == EditModeType.NORMAL)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    m_mode = EditModeType.DRAGSTATION;
                    //CTRL押下で、移動モード
                    m_drag_station = target;
                    return;
                }

                PopupMenuSelectStation(target);
            }
            else
                if (m_mode == EditModeType.DRAGSTATION)
                {
                    m_drag_station = null;
                    m_mode = EditModeType.NORMAL;
                    return;
                }
                else
                    if (m_mode == EditModeType.ADDPATH)
                    {
                        if (m_drag_station == null || m_drag_station == target)
                        {
                            m_mode = EditModeType.NORMAL;
                            m_line.Visible = false;
                            MapViewCnt.RefreshImage();
                            return;
                        }

                        //パス追加
                        var trainpath = new TrainPath();
                        trainpath.UniqID = m_factory.GetNetwork().NextPathUniqID();

                        //経路
                        trainpath.Order = trainpath.UniqID;
                        trainpath.StationA = m_drag_station;
                        trainpath.StationB = target;
                        m_factory.GetNetwork().AddPath(trainpath);

                        m_line.Visible = false;

                        m_drag_station = null;
                        m_mode = EditModeType.NORMAL;

                        MapViewCnt.RefreshImage();
                        return;
                    }
                    else
                    {
                        m_mode = EditModeType.NORMAL;
                    }
        }

        /// <summary>
        /// 経由地選択メニュー
        /// </summary>
        /// <param name="station"></param>
        protected void PopupMenuSelectInterPosition( OnIsInterPositionResult interresult )
        {
            ContextMenuStrip popupmenu = new ContextMenuStrip();

            ToolStripMenuItem delpathitem = new ToolStripMenuItem("曲点削除");
            delpathitem.Click += (sender, args) =>
            {
                //経由地削除
                DeleteInterPosition(interresult.Path, interresult.Position);

                MapViewCnt.RefreshImage();
            };
            popupmenu.Items.Add(delpathitem);

            ToolStripMenuItem delmultipathitem = new ToolStripMenuItem("曲点一括削除");
            delmultipathitem.Click += (sender, args) =>
            {
                double HitDistance = 5.0;

                SetSelectPoint((viewrequestinfo, viewpoint, dLat, dLng) =>
                {
                    //対象物探索
                    var result = new Dictionary<TrainPath.Position, double>();
                    foreach (var position in interresult.Path.GetPositionList())
                    {
                        if (position == interresult.Position)
                            continue;

                        //画面座標変換
                        var mypoint = viewrequestinfo.LatLongToViewPoint(position.Latitude, position.Longitude);

                        //距離算出
                        double dDistance = Math.Sqrt((mypoint.X - viewpoint.X) * (mypoint.X - viewpoint.X) + (mypoint.Y - viewpoint.Y) * (mypoint.Y - viewpoint.Y));
                        if (HitDistance >= dDistance)
                            result[position] = dDistance;
                    }
                    if (result.Count > 0)
                    {
                        //最短の駅とする
                        var min = result.Min(dr => dr.Value);
                        var targetposition = result.First(dr => dr.Value == min).Key;

                        TrainPath.Position Start, End;
                        if (interresult.Position.Order < targetposition.Order)
                        {
                            Start = interresult.Position;
                            End = targetposition;
                        }
                        else
                        {
                            Start = targetposition;
                            End = interresult.Position;
                        }

                        bool bFind = false;
                        foreach (var pos in interresult.Path.GetPositionList())
                        {
                            if (bFind == false)
                            {
                                if (pos == Start)
                                {
                                    bFind = true;
                                    interresult.Path.RemovePosition(pos);
                                }
                            }
                            else
                            {
                                interresult.Path.RemovePosition(pos);

                                if (pos == End)
                                    break;
                            }
                        }

                        return;
                    }

                });
            };
            popupmenu.Items.Add(delmultipathitem);

            popupmenu.Show(Cursor.Position);
        }

        /// <summary>
        /// 駅選択メニュー
        /// </summary>
        /// <param name="station"></param>
        protected void PopupMenuSelectStation(StationInfoData station)
        {
            ContextMenuStrip popupmenu = new ContextMenuStrip();

            ToolStripMenuItem edititem = new ToolStripMenuItem("編集");
            edititem.Click += (sender, args) =>
            {
                using (var dlg = new Dialog.StationDialog())
                {
                    dlg.SetStation(station);

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        MapViewCnt.RefreshImage();
                    }
                }
            };
            popupmenu.Items.Add(edititem);

            ToolStripMenuItem addpathitem = new ToolStripMenuItem("経路接続");
            addpathitem.Click += (sender, args) =>
            {
                m_mode = EditModeType.ADDPATH;
                m_drag_station = station;

                m_line.Clear();

                m_line.AddPosition( new latlontool.latlng(station.Latitude,station.Longitude));
                m_line.AddPosition(new latlontool.latlng(station.Latitude, station.Longitude));
                m_line.Visible = true;

                MapViewCnt.RefreshImage();
            };
            popupmenu.Items.Add(addpathitem);

            ToolStripMenuItem delpathitem = new ToolStripMenuItem("駅削除");
            delpathitem.Click += (sender, args) =>
            {
                DeleteStation(station);

                MapViewCnt.RefreshImage();
            };
            popupmenu.Items.Add(delpathitem);

            ToolStripMenuItem delmultipathitem = new ToolStripMenuItem("駅一括削除");
            delmultipathitem.Click += (sender, args) =>
            {
                double HitDistance = 5.0;

                StationInfoData endstation = null;
                SetSelectPoint((viewrequestinfo, viewpoint, dLat, dLng) =>
                {
                    //対象駅探索
                    var result = new Dictionary<TrainPath.Position, double>();

                    foreach (var ostation in Factory.GetStationManager().StationList())
                    {
                        if (station == ostation)
                            continue;

                        //画面座標変換
                        var mypoint = viewrequestinfo.LatLongToViewPoint(ostation.Latitude, ostation.Longitude);

                        //距離算出
                        double dDistance = Math.Sqrt((mypoint.X - viewpoint.X) * (mypoint.X - viewpoint.X) + (mypoint.Y - viewpoint.Y) * (mypoint.Y - viewpoint.Y));
                        if (HitDistance >= dDistance)
                        {
                            endstation = ostation;
                            break;
                        }
                    }
                    if(endstation != null )
                    {
                        var listtable = Factory.GetNetwork().BuildLine(station, endstation);
                        if (listtable.Count == 0)
                            return;

                        if (listtable.Count >= 2)
                        {
                            MessageBox.Show("複数経路があるので削除出来ません。");
                            return;
                        }

                        var deletetable = new Dictionary<int, StationInfoData>();
                        var deletelist = listtable[0];
                        foreach( var tpath in deletelist )
                        {
                            if( tpath.StationA != null )
                            {
                                if(deletetable.ContainsKey(tpath.StationA.UniqID) == false )
                                deletetable[tpath.StationA.UniqID] = tpath.StationA;
                            }
                            if (tpath.StationB != null)
                            {
                                if (deletetable.ContainsKey(tpath.StationB.UniqID) == false)
                                    deletetable[tpath.StationB.UniqID] = tpath.StationB;
                            }
                        }
                        deletetable.Values.ToList().ForEach(pstation => DeleteStation(pstation));
                    }
                });

                MapViewCnt.RefreshImage();
            };
            popupmenu.Items.Add(delmultipathitem);


            popupmenu.Show(Cursor.Position);
        }

        /// <summary>
        /// ライン選択メニュー
        /// </summary>
        /// <param name="station"></param>
        protected void PopupMenuSelectLine(OnCrossLineResult crossresult)
        {
            ContextMenuStrip popupmenu = new ContextMenuStrip();

            ToolStripMenuItem addpathitem = new ToolStripMenuItem("曲点追加");
            addpathitem.Click += (sender, args) =>
            {
                //経由地
                var position = new TrainPath.Position();
                position.Latitude = crossresult.CrossPoint.lat;
                position.Longitude = crossresult.CrossPoint.lng;

                int interIdx = Math.Min(crossresult.A, crossresult.B);

                crossresult.Path.InsertPosition(interIdx, position);

                MapViewCnt.RefreshImage();
            };
            popupmenu.Items.Add(addpathitem);

            ToolStripMenuItem insertstationitem = new ToolStripMenuItem("駅挿入");
            insertstationitem.Click += (sender, args) =>
            {
                using (var dlg = new Dialog.StationDialog())
                {
                    var stationinfo = new StationInfoData();
                    stationinfo.Latitude = crossresult.CrossPoint.lat;
                    stationinfo.Longitude = crossresult.CrossPoint.lng;
                    stationinfo.Manager = m_factory.GetStationManager();
                    dlg.SetStation(stationinfo);
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        stationinfo.UniqID = m_factory.GetStationManager().NextUniqID();
                        m_factory.GetStationManager().Add(stationinfo);

                        //パス分断
                        m_factory.GetNetwork().RemovePath(crossresult.Path);

                        //経路A
                        //パス追加
                        var trainpatha = new TrainPath();
                        trainpatha.LineColor = crossresult.Path.LineColor;
                        trainpatha.StationColor = crossresult.Path.StationColor;
                        trainpatha.Visible = crossresult.Path.Visible;
                        trainpatha.UniqID = m_factory.GetNetwork().NextPathUniqID();

                        trainpatha.Order = trainpatha.UniqID;
                        trainpatha.StationA = crossresult.Path.StationA;
                        trainpatha.StationB = stationinfo;
                        m_factory.GetNetwork().AddPath(trainpatha);

                        //経路B
                        //パス追加
                        var trainpathb = new TrainPath();
                        trainpathb.LineColor = crossresult.Path.LineColor;
                        trainpathb.StationColor = crossresult.Path.StationColor;
                        trainpathb.Visible = crossresult.Path.Visible;
                        trainpathb.UniqID = m_factory.GetNetwork().NextPathUniqID();

                        trainpathb.Order = trainpathb.UniqID;
                        trainpathb.StationA = stationinfo;
                        trainpathb.StationB = crossresult.Path.StationB;
                        m_factory.GetNetwork().AddPath(trainpathb);

                        //曲点移行
                        var positionlist = crossresult.Path.GetPositionList();
                        for (int iIdx = 0; iIdx < positionlist.Length; iIdx++)
                        {
                            if (iIdx < crossresult.A)
                                trainpatha.AddPosition(positionlist[iIdx]);
                            else
                                trainpathb.AddPosition(positionlist[iIdx]);
                        }

                        MapViewCnt.RefreshImage();
                    }
                }

                //経由地
                var position = new TrainPath.Position();
                position.Latitude = crossresult.CrossPoint.lat;
                position.Longitude = crossresult.CrossPoint.lng;

                int interIdx = Math.Min(crossresult.A, crossresult.B);

                crossresult.Path.InsertPosition(interIdx, position);

                MapViewCnt.RefreshImage();
            };
            popupmenu.Items.Add(insertstationitem);

            ToolStripMenuItem delpathitem = new ToolStripMenuItem("経路削除");
            delpathitem.Click += (sender, args) =>
            {
                m_factory.GetNetwork().DelPath(crossresult.Path);
                MapViewCnt.RefreshImage();
            };
            popupmenu.Items.Add(delpathitem);

            ToolStripMenuItem editpathitem = new ToolStripMenuItem("編集");
            editpathitem.Click += (sender, args) =>
            {
                using( var dlg = new EditPathDialog())
                {
                    dlg.Path = crossresult.Path;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        MapViewCnt.RefreshImage();
                    }
                }
            };
            popupmenu.Items.Add(editpathitem);

            //線路解析
            ToolStripMenuItem linewalkeritem = new ToolStripMenuItem("線路解析");
            linewalkeritem.Click += (sender, args) =>
            {
                WalkLine(crossresult);
            };
            popupmenu.Items.Add(linewalkeritem);

            //線路解析2
            ToolStripMenuItem linewalker2item = new ToolStripMenuItem("線路解析2");
            linewalker2item.Click += (sender, args) =>
            {
                WalkLine(crossresult, 1 );
            };
            popupmenu.Items.Add(linewalker2item);

            //線路解析3
            ToolStripMenuItem linewalker3item = new ToolStripMenuItem("線路解析3");
            linewalker3item.Click += (sender, args) =>
            {
                WalkLine(crossresult, 2);
            };
            popupmenu.Items.Add(linewalker3item);

            popupmenu.Show(Cursor.Position);
        }

        /// <summary>
        /// 解析中ポイント表示
        /// </summary>
        protected class WalkPoint : WhereTrainBuild.MapUtil.View.ViewPointIF
        {
            /// <summary>
            /// 緯度経度
            /// </summary>
            protected latlontool.latlng m_point = new latlontool.latlng();

            public latlontool.latlng Point
            {
                set
                {
                    lock (m_point)
                    {
                        m_point.Set(value);
                    }
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public WalkPoint()
            {
            }

            /// <summary>
            /// 描画
            /// </summary>
            /// <param name="viewreqinfo">描画情報</param>
            /// <returns>True..描画範囲内 False..範囲外</returns>
            public bool Draw(ViewRequestInfo viewreqinfo)
            {
                //描画座標クリップ領域
                Rectangle cliparea = viewreqinfo.ClipArea();

                double dMyPositionX = 0;
                double dMyPositionY = 0;

                lock (m_point)
                {
                    //メルカトル座標に変換
                    MercatorTrans.Trans(m_point.lat  / 180.0d * Math.PI, m_point.lng / 180.0d * Math.PI, viewreqinfo.ZoomLevel, ref dMyPositionX, ref dMyPositionY);
                }

                //クリップ領域チェック
                if (cliparea.X > dMyPositionX || dMyPositionX > cliparea.Right ||
                    cliparea.Y > dMyPositionY || dMyPositionY > cliparea.Bottom)
                    return false;

                int iX = (int)((dMyPositionX - cliparea.Left) * viewreqinfo.Scale);
                int iY = (int)((cliparea.Bottom - dMyPositionY) * viewreqinfo.Scale);

                viewreqinfo.ViewGraphics.DrawArc(new Pen(Color.Red, 5), iX, iY, 5, 5, 0, 360);

                return true;
            }

            /// <summary>
            /// 当たり判定
            /// </summary>
            /// <param name="iX">画像X座標</param>
            /// <param name="iY">画像Y座標</param>
            /// <returns>True..範囲内 False..範囲外</returns>
            public bool IsHit(int iX, int iY, ViewRequestInfo viewreqinfo)
            {
                return false;
            }
        }

        /// <summary>
        /// ライン解析
        /// </summary>
        /// <param name="crossresult"></param>
        protected async void WalkLine(OnCrossLineResult crossresult, int iFunc = 0 )
        {
            latlontool.latlng prevposition = null, targetposition = null, startposition = null, endposition = null;
            int iInterIdx = 0;
            int iIndexDelta = 1;
            var plist = crossresult.Path.ToPositionList();
            if (plist.Count == 2)
            {
                //曲点無し
                //この場合、始点終点を各駅にしてしまう。
                prevposition = plist[crossresult.A];
                targetposition = plist[crossresult.B];
                startposition = prevposition;
                endposition = targetposition;
                iInterIdx = 0;
                iIndexDelta = 1;
            }
            else
            {
                int startIdx = Math.Min(crossresult.A, crossresult.B);
                int endIdx = Math.Max(crossresult.A, crossresult.B);
                for (int iIdx = 0; iIdx < plist.Count; iIdx++)
                {
                    if (iIdx == startIdx)
                    {
                        targetposition = plist[iIdx];
                        iInterIdx = iIdx;
                        iIndexDelta = 1;
                        break;
                    }
                    prevposition = plist[iIdx];
                }
                if (targetposition == null || prevposition == null)
                {
                    startIdx = Math.Max(crossresult.A, crossresult.B);
                    endIdx = Math.Min(crossresult.A, crossresult.B);
                    for (int iIdx = plist.Count - 1; iIdx >= 0; iIdx--)
                    {
                        if (iIdx == startIdx)
                        {
                            targetposition = plist[iIdx];
                            iInterIdx = iIdx - 1;
                            iIndexDelta = 0;
                            break;
                        }
                        prevposition = plist[iIdx];
                    }
                }

                startposition = targetposition;
                endposition = plist[endIdx];
            }

            //方向
            //メルカトルによる角度
            double dX1 = 0, dY1 = 0;
            double dX2 = 0, dY2 = 0;
            MercatorTrans.Trans(latlontool.ToRadian(prevposition.lat), latlontool.ToRadian(prevposition.lng), MapViewCnt.ViewArea.ViewInfo.ZoomLevel, ref dX1, ref dY1);
            MercatorTrans.Trans(latlontool.ToRadian(startposition.lat), latlontool.ToRadian(startposition.lng), MapViewCnt.ViewArea.ViewInfo.ZoomLevel, ref dX2, ref dY2);

            var vdo = Math.Atan2(dY2 - dY1, dX2 - dX1);
            var vector = new PointF((float)Math.Cos(vdo), (float)Math.Sin(vdo));
            var walkpoint = new WalkPoint();

            try
            {
                Enabled = false;
                MapViewCnt.ViewArea.Add(walkpoint);

                using( var walkdlg = new WalkingDialog() )
                {
                    var walker = new LineWalker(MapViewCnt.TileManager);

                    walker.OnProgress += (owner, walkingpoint, walkcount) =>
                    {
                        if (walkdlg.Terminate == true)
                        {
                            return false;
                        }

                        if(walkcount%10 == 0 )
                        {
                            Invoke((Action)(() =>
                            {
                                walkdlg.CountLbl.Text = $"{walkcount}";

                                walkpoint.Point = walkingpoint;

                                MapViewCnt.ViewArea.ViewInfo.SetCenterLatLon(walkingpoint.lat, walkingpoint.lng);
                                MapViewCnt.RefreshImage();
                            }));
                        }

                        return true;
                    };

                    walkdlg.Load += async (dlg, args) =>
                    {
                        await Task.Run(() =>
                        {
                            try
                            {
                                List<latlontool.latlng> walkpointlist = null;

                                switch (iFunc)
                                {
                                    case 1:
                                        walkpointlist = walker.Walk2(startposition, vector, endposition);
                                        break;

                                    case 2:
                                        walkpointlist = walker.Walk3(startposition, vector, endposition);
                                        break;

                                    default:
                                        walkpointlist = walker.Walk(startposition, vector, endposition);
                                        break;
                                }

                                //最適化
                                walkpointlist = walker.Optimisation(walkpointlist);

                                bool bFirst = false;
                                int iPathIdx = iInterIdx;
                                foreach (var walk in walkpointlist)
                                {
                                    //先頭は、ゴミになりやすいので捨てる
                                    if (bFirst == false)
                                    {
                                        bFirst = true;
                                        continue;
                                    }

                                    //経由地
                                    var position = new TrainPath.Position();
                                    position.Latitude = walk.lat;
                                    position.Longitude = walk.lng;

                                    int interIdx = Math.Min(crossresult.A, crossresult.B);

                                    crossresult.Path.InsertPosition(iPathIdx, position);
                                    iPathIdx += iIndexDelta;
                                }
                            }
                            finally
                            {
                                Invoke((Action)(() =>
                                {
                                    //終了
                                    walkdlg.Close();
                                }));
                            }
                        });
                    };

                    walkdlg.ShowDialog(this);
                }
            }
            finally
            {
                Enabled = true;

                MapViewCnt.ViewArea.Del(walkpoint);
            }

            MapViewCnt.RefreshImage();
        }

        /// <summary>
        /// 駅削除
        /// </summary>
        protected void DeleteStation(StationInfoData station)
        {
            foreach (var line in m_factory.GetNetwork().GetLineList())
            {
                //経路
                List<TrainPath> removelist = new List<TrainPath>();

                foreach (var path in line.GetPathList())
                {
                    if (path.StationA.UniqID == station.UniqID || path.StationB.UniqID == station.UniqID)
                    {
                        removelist.Add(path);
                    }
                }

                removelist.ForEach( path => line.RemovePath(path) );

                //時刻表
                foreach (var train in line.GetTrainList())
                {
                    List<SceduleManager.Plan> removeplanlist = new List<SceduleManager.Plan>();

                    foreach (var plan in train.Scedule.GetList())
                    {
                        if (plan.Station.UniqID == station.UniqID)
                        {
                            removeplanlist.Add(plan);
                        }
                    }

                    removeplanlist.ForEach(plan => train.Scedule.Remove(plan) );
                }
            }

            //全体経路
            List<TrainPath> removealllist = new List<TrainPath>();
            foreach (var path in m_factory.GetNetwork().GetPathList())
            {
                if (path.StationA.UniqID == station.UniqID || path.StationB.UniqID == station.UniqID)
                {
                    removealllist.Add(path);
                }
            }

            removealllist.ForEach(path => m_factory.GetNetwork().RemovePath(path));

            //駅削除
            m_factory.GetStationManager().Remove(station.UniqID);
        }

        /// <summary>
        /// 経由地削除
        /// </summary>
        protected void DeleteInterPosition(TrainPath srcpath, TrainPath.Position position)
        {
            foreach (var line in m_factory.GetNetwork().GetLineList())
            {
                //経路
                foreach (var path in line.GetPathList())
                {
                    if (path.StationA.UniqID == srcpath.StationA.UniqID && path.StationB.UniqID == srcpath.StationB.UniqID)
                    {
                        var removelist = path.GetPositionList().Where(pos => pos.Latitude == position.Latitude && pos.Longitude == position.Longitude).ToList();

                        removelist.ForEach(removepos => path.RemovePosition(removepos));
                    }
                }
            }

            //全体経路
            foreach (var path in m_factory.GetNetwork().GetPathList())
            {
                if (path.StationA.UniqID == srcpath.StationA.UniqID && path.StationB.UniqID == srcpath.StationB.UniqID)
                {
                    var removelist = path.GetPositionList().Where(pos => pos.Latitude == position.Latitude && pos.Longitude == position.Longitude).ToList();

                    removelist.ForEach(removepos => path.RemovePosition(removepos));
                }
            }
        }

        /// <summary>
        /// 経由地判定結果
        /// </summary>
        public class OnIsInterPositionResult
        {
            public TrainPath Path;
            public TrainPath.Position Position;
        }

        /// <summary>
        /// 経由地判定
        /// </summary>
        /// <param name="viewpoint"></param>
        protected OnIsInterPositionResult IsInterPosition(PointF viewpoint)
        {
            double HitDistance = 5.0;

            foreach (var path in m_factory.GetNetwork().GetPathList())
            {
                var positionlist = path.GetPositionList();
                foreach (var position in positionlist)
                {
                    //画面座標変換
                    var point = MapViewCnt.ViewArea.ViewInfo.LatLongToViewPoint( position.Latitude, position.Longitude);

                    //交点有り
                    //交点と対象位置の距離
                    double distance = Math.Sqrt(Math.Pow(viewpoint.X - point.X, 2) + Math.Pow(viewpoint.Y - point.Y, 2));
                    if (distance <= HitDistance)
                    {
                        OnIsInterPositionResult result = new OnIsInterPositionResult();
                        result.Path = path;
                        result.Position = position;

                        return result;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 交点情報結果
        /// </summary>
        public class OnCrossLineResult
        {
            public OnLineResult LineResult;

            public TrainPath Path;

            public int A;
            public int B;

            public latlontool.latlng CrossPoint;
        }

        /// <summary>
        /// 交点判定
        /// </summary>
        /// <param name="viewpoint"></param>
        protected OnCrossLineResult IsCrossPath(PointF viewpoint)
        {
            double HitDistance = 5.0;

            //全体パス一覧
            foreach (var path in m_factory.GetNetwork().GetPathList())
            {
                List<PointF> pointlist = new List<PointF>();
                //パス内位置リスト
                var positionlist = path.ToPositionList();
                foreach (var position in positionlist)
                {
                    //画面座標変換
                    pointlist.Add(MapViewCnt.ViewArea.ViewInfo.LatLongToViewPoint(position.lat, position.lng));
                }

                var result = IsOnLine(HitDistance, viewpoint, pointlist);

                if( result != null )
                {
                    //交点有り
                    var crossresult = new OnCrossLineResult(); 
                    crossresult.LineResult = result;
                    crossresult.Path = path;

                    var iNoA = pointlist.FindIndex(pp => pp.Equals(result.PointA));
                    var iNoB = pointlist.FindIndex(pp => pp.Equals(result.PointB));

                    crossresult.A = iNoA;
                    crossresult.B = iNoB;

                    var gCross = MapViewCnt.ReverseTrans(result.CrossPoint);
                    crossresult.CrossPoint = gCross;

                    return crossresult;
                }
            }

            return null;
        }


        #region 交点判定

        /// <summary>
        /// ライン上にあるか判定戻り値
        /// </summary>
        public class OnLineResult
        {
            /// <summary>
            /// ポイントA
            /// </summary>
            public PointF PointA = new PointF();

            /// <summary>
            /// ポイントB
            /// </summary>
            public PointF PointB = new PointF();

            /// <summary>
            /// 交点
            /// </summary>
            public PointF CrossPoint = new PointF();

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="pointa"></param>
            /// <param name="pointb"></param>
            /// <param name="crosspoint"></param>
            public OnLineResult(PointF pointa, PointF pointb, PointF crosspoint)
            {
                PointA = pointa;
                PointB = pointb;
                CrossPoint = crosspoint;
            }
        }

        /// <summary>
        /// ライン上にあるか判定
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pointlist"></param>
        protected static OnLineResult IsOnLine(double HitDistance, PointF point, List<PointF> pointlist)
        {
            for (int iIdx = 1; iIdx < pointlist.Count; iIdx++)
            {
                PointF current = pointlist[iIdx];
                PointF prev = pointlist[(pointlist.Count + iIdx - 1) % pointlist.Count];

                PointF cross = CrossRectPoint(point, prev, current);

                //値域判定
                bool bInner = false;
                if (prev.X < current.X)
                {
                    if (prev.X <= cross.X && cross.X <= current.X)
                    {
                        //OK
                        bInner = true;
                    }
                }
                else
                {
                    if (current.X <= cross.X && cross.X <= prev.X)
                    {
                        //OK
                        bInner = true;
                    }
                }
                if (bInner == false)
                    continue;

                bInner = false;
                if (prev.Y < current.Y)
                {
                    if (prev.Y <= cross.Y && cross.Y <= current.Y)
                    {
                        //OK
                        bInner = true;
                    }
                }
                else
                {
                    if (current.Y <= cross.Y && cross.Y <= prev.Y)
                    {
                        //OK
                        bInner = true;
                    }
                }
                if (bInner == false)
                    continue;

                //交点有り
                //交点と対象位置の距離
                double distance = Math.Sqrt(Math.Pow(cross.X - point.X, 2) + Math.Pow(cross.Y - point.Y, 2));
                if (distance <= HitDistance)
                {
                    return new OnLineResult(prev, current, new PointF(cross.X, cross.Y));
                }
            }

            return null;
        }

        /// <summary>
        /// 直交ライン交点算出
        /// </summary>
        /// <param name="point">直交ライン上座標</param>
        /// <param name="pa">ラインA点</param>
        /// <param name="pb">ラインB点</param>
        /// <returns></returns>
        public static PointF CrossRectPoint(PointF point, PointF pa, PointF pb)
        {
            if (pb.X == pa.X)
            {
                //垂直ライン
                return new PointF(pb.X, point.Y);
            }
            if (pb.Y == pa.Y)
            {
                //水平ライン
                return new PointF(point.X, pb.Y);
            }

            //ライン方程式 y=ax+b
            float a = (float)(pb.Y - pa.Y) / (float)(pb.X - pa.X);
            float b = pb.Y - a * pb.X;

            //直交ライン y=-ax+c
            float reva = -(float)(pb.X - pa.X) / (float)(pb.Y - pa.Y);
            float c = point.Y - reva * point.X;

            //交点 x=(b-c)/(reva-a)
            float cross_x = (b - c) / (reva - a);
            float cross_y = a * cross_x + b;

            return new PointF(cross_x, cross_y);
        }


        #endregion

        #endregion

        /// <summary>
        /// ライン構築メニュー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildLineMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new EditLineDialog();
            dlg.MainForm = this;
            dlg.Show(this);
        }

        /// <summary>
        /// ビューポイント追加
        /// </summary>
        /// <param name="line"></param>
        public void AddViewPoint(WhereTrainBuild.MapUtil.View.ViewPointIF viewpoint)
        {
            if (viewpoint == null)
                return;

            MapViewCnt.ViewArea.Add(viewpoint);

            MapViewCnt.RefreshImage();
        }

        /// <summary>
        /// ビューポイント削除
        /// </summary>
        /// <param name="line"></param>
        public void DelViewPoint(WhereTrainBuild.MapUtil.View.ViewPointIF viewpoint)
        {
            if (viewpoint == null)
                return;

            MapViewCnt.ViewArea.Del(viewpoint);

            MapViewCnt.RefreshImage();
        }

        /// <summary>
        /// 電車クリア
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearTrainMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var setname in m_factory.GetNetwork().GetLineSetList())
            {
                m_factory.GetNetwork().ChangeLine(setname);

                foreach (var line in m_factory.GetNetwork().GetLineList())
                {
                    line.ClearTrain();
                }
            }
        }

        /// <summary>
        /// 新規メニュー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            InitializeFactory(new PlaneFactory());

            MapViewCnt.RefreshImage();

            m_filename = string.Empty;
            Text = "WhereTrainBuild";

        }

        /// <summary>
        /// ファクトリ新規
        /// </summary>
        /// <param name="factory"></param>
        protected void InitializeFactory( IFactory factory )
        {
            MapViewCnt.ViewArea.Del(m_factory.GetStationManager());
            MapViewCnt.ViewArea.Del(m_factory.GetNetwork());

            m_factory = factory;

            MapViewCnt.ViewArea.Add(m_factory.GetStationManager());
            MapViewCnt.ViewArea.Add(m_factory.GetNetwork());
        }

        /// <summary>
        /// プロパティ選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new FactoryProDialog();
            dlg.MainForm = this;
            dlg.Factory = m_factory as BaseFactory;
            dlg.Show(this);
        }

        /// <summary>
        /// 電車ダイヤグラム構築選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildTrainMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new BuildDialogTrainDialog();
            dlg.MainForm = this;
            dlg.Show(this);
        }

        /// <summary>
        /// ドラッグ侵入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var drags = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (drags != null)
                {
                    bool bWtx = false;
                    foreach (var file in drags)
                    {
                        if (Path.GetExtension(file).ToUpper() == ".WTX")
                        {
                            bWtx = true;
                            break;
                        }
                    }
                    if (bWtx == true)
                        e.Effect = DragDropEffects.Copy;
                }
            }
        }

        /// <summary>
        /// ドラッグ完了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapForm_DragDrop(object sender, DragEventArgs e)
        {
            var drags = e.Data.GetData(DataFormats.FileDrop) as string[];
            if( drags != null )
            {
                foreach (var file in drags)
                {
                    if (Path.GetExtension(file).ToUpper() == ".WTX")
                    {
                        LoadFactory(file);

                        break;
                    }
                }
            }
        }


    }
}
