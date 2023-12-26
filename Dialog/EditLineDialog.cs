using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WhereTrainBuild.MapUtil.Data;

namespace WhereTrainBuild.Dialog
{
    /// <summary>
    /// 方面編集ダイアログ
    /// </summary>
    public partial class EditLineDialog : Form
    {
        /// <summary>
        /// メインフォーム
        /// </summary>
        protected MapForm m_form;

        /// <summary>
        /// メインフォームプロパティ
        /// </summary>
        public MapForm MainForm
        {
            set
            {
                m_form = value;
            }
        }

        /// <summary>
        /// ライン
        /// </summary>
        protected TrainLine m_line = null;

        /// <summary>
        /// ラインプロパティ
        /// </summary>
        public TrainLine Line
        {
            get
            {
                return m_line;
            }
            set
            {
                m_line = value;
            }
        }

        /// <summary>
        /// 駅リストセット
        /// </summary>
        /// <param name="stationlist"></param>
        public void SetStationList(StationInfoData[] stationlist)
        {
            StationList.Items.AddRange(stationlist);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditLineDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 駅ラベルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationLbl_Click(object sender, EventArgs e)
        {
            StationLbl.BackColor = Color.Red;

            SelectStation((station) =>
            {
                if (station != null)
                {
                    StationLbl.Text = station.Name;
                    StationLbl.Tag = station;
                }

                StationLbl.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            });
        }

        /// <summary>
        /// 選択コールバック型
        /// </summary>
        /// <param name="station"></param>
        protected delegate void SelectCallbackDelegate(StationInfoData station);

        /// <summary>
        /// 駅選択開始
        /// </summary>
        /// <param name="callback"></param>
        protected void SelectStation(SelectCallbackDelegate callback)
        {
            double HitDistance = 5.0;

            m_form.SetSelectPoint((viewrequestinfo, viewpoint, dLat, dLng) =>
            {
                //対象物探索
                Dictionary<int, double> result = new Dictionary<int, double>();
                foreach (var station in m_form.Factory.GetStationManager().StationList())
                {
                    //画面座標変換
                    var mypoint = viewrequestinfo.LatLongToViewPoint(station.Latitude, station.Longitude);

                    //距離算出
                    double dDistance = Math.Sqrt((mypoint.X - viewpoint.X) * (mypoint.X - viewpoint.X) + (mypoint.Y - viewpoint.Y) * (mypoint.Y - viewpoint.Y));
                    if (HitDistance >= dDistance)
                        result[station.UniqID] = dDistance;
                }
                if (result.Count > 0)
                {
                    //最短の駅とする
                    var min = result.Min(dr => dr.Value);
                    var uniqid = result.First(dr => dr.Value == min).Key;
                    var targetstation = m_form.Factory.GetStationManager().Get(uniqid);

                    if (callback != null)
                        callback(targetstation);
                    return;
                }

                if (callback != null)
                    callback(null);
            });
        }

        /// <summary>
        /// 追加ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if( StationLbl.Tag != null )
                StationList.Items.Add(StationLbl.Tag);

        }

        /// <summary>
        /// 駅リストマウスダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int selectidx = StationList.IndexFromPoint(e.Location);

                ContextMenuStrip popupmenu = new ContextMenuStrip();

                ToolStripMenuItem delpathitem = new ToolStripMenuItem("Del");
                delpathitem.Click += (owner, args) =>
                {
                    if (selectidx >= 0)
                    {
                        StationList.Items.RemoveAt(selectidx);
                    }
                };
                popupmenu.Items.Add(delpathitem);

                ToolStripMenuItem uppathitem = new ToolStripMenuItem("Up");
                uppathitem.Click += (owner, args) =>
                {
                    if (selectidx >= 1)
                    {
                        var tmp = StationList.Items[selectidx];
                        StationList.Items.RemoveAt(selectidx);
                        StationList.Items.Insert(selectidx - 1, tmp);
                    }
                };
                popupmenu.Items.Add(uppathitem);

                ToolStripMenuItem downpathitem = new ToolStripMenuItem("Down");
                downpathitem.Click += (owner, args) =>
                {
                    if (selectidx >= 0 && selectidx < StationList.Items.Count - 1)
                    {
                        var tmp = StationList.Items[selectidx];
                        StationList.Items.RemoveAt(selectidx);
                        StationList.Items.Insert(selectidx + 1, tmp);
                    }
                };
                popupmenu.Items.Add(downpathitem);

                popupmenu.Show(Cursor.Position);
            }
        }

        /// <summary>
        /// 構築ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildBtn_Click(object sender, EventArgs e)
        {
            List<TrainPath> result = new List<TrainPath>();

            var iLastID = -1;
            StationInfoData laststation = null;
            foreach (StationInfoData station in StationList.Items)
            {
                if (laststation != null)
                {
                    //ライン構築
                    var linetable = m_form.Factory.GetNetwork().BuildLine(laststation, station);
                    if (linetable.Count <= 0)
                    {
                        return;
                    }

                    //候補を選択
                    List<TrainPath> selectlist = null;
                    if (linetable.Count == 1)
                    {
                        selectlist = linetable[0];
                    }
                    else
                    {
                        using (var choisedlg = new ChoisePathListDialog())
                        {
                            choisedlg.MainForm = m_form;
                            choisedlg.PathListTable = linetable;
                            if (choisedlg.ShowDialog(this) != DialogResult.OK)
                                return;

                            selectlist = choisedlg.SelectList;
                        }

                        if (selectlist == null)
                            return;
                    }

                    result.AddRange(selectlist);
                }
                else
                    iLastID = station.UniqID; //先頭ID確保

                laststation = station;
            }

            
            for (int iIdx = 0; iIdx < result.Count; iIdx++)
            {
                //パス向き整列
                if (iLastID != result[iIdx].StationA.UniqID)
                    result[iIdx].Reverse();
                iLastID = result[iIdx].StationB.UniqID;

                result[iIdx].LineColor = Color.Lime;

                result[iIdx].Order = iIdx + 1;
            }

            PathListBox.Items.Clear();

            foreach (var path in result)
            {
                PathListBox.Items.Add(path);
            }

            if( m_viewpoint != null )
                m_form.DelViewPoint(m_viewpoint);

            m_viewpoint = new PathListViewPoint(result);

            m_form.AddViewPoint(m_viewpoint);
        }

        /// <summary>
        /// ライン
        /// </summary>
        protected PathListViewPoint m_viewpoint = null;

        /// <summary>
        /// ライン
        /// </summary>
        protected class PathListViewPoint : WhereTrainBuild.MapUtil.View.ViewPointIF
        {
            public List<TrainPath> PathList;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="pathlist"></param>
            /// <param name="name"></param>
            public PathListViewPoint(List<TrainPath> pathlist)
            {
                PathList = pathlist;
            }

            /// <summary>
            /// 描画
            /// </summary>
            /// <param name="viewreqinfo">描画情報</param>
            /// <returns>True..描画範囲内 False..範囲外</returns>
            public bool Draw(WhereTrainBuild.MapUtil.View.ViewRequestInfo viewreqinfo)
            {
                //経路描画
                foreach (var path in PathList)
                {
                    path.Draw(viewreqinfo);
                }

                return true;
            }

            /// <summary>
            /// 当たり判定
            /// </summary>
            /// <param name="iX">画像X座標</param>
            /// <param name="iY">画像Y座標</param>
            /// <returns>True..範囲内 False..範囲外</returns>
            public bool IsHit(int iX, int iY, WhereTrainBuild.MapUtil.View.ViewRequestInfo viewreqinfo)
            {
                return false;
            }
        }

        /// <summary>
        /// フォーム終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditLineDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_viewpoint != null)
                m_form.DelViewPoint(m_viewpoint);
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditLineDialog_Load(object sender, EventArgs e)
        {
            if(Line != null)
            {
                NameTxt.Text = Line.Name;
                DisplayTxt.Text = Line.Display;

                foreach( var path in Line.GetPathList() )
                {
                    PathListBox.Items.Add(path);
                }

                //種別の編集は不可能
                SetList.Visible = false;
                SetListLbl.Visible = false;
                LineAddBtn.Text = "OK";
            }

            foreach( var lineset in m_form.Factory.GetNetwork().GetLineSetList() )
            {
                SetList.Items.Add(lineset);
            }

            for ( int iIdx=0; iIdx< SetList.Items.Count; iIdx++ )
            {
                SetList.SetItemChecked(iIdx, true);
            }
        }

        /// <summary>
        /// ライン追加ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineAddBtn_Click(object sender, EventArgs e)
        {
            var line = new TrainLine();

            if( Line != null )
            {
                Line.ClearTrain();
                line = Line;
                line.ClearPath();
            }

            line.Name = NameTxt.Text;
            line.Display = DisplayTxt.Text;

            foreach (TrainPath path in PathListBox.Items)
            {
                line.AddPath(path.Clone() as TrainPath);
            }

            if (Line == null)
            {
                for (int iIdx = 0; iIdx < SetList.CheckedIndices.Count; iIdx++)
                {
                    //時刻表種別セット
                    String setname = (String)SetList.Items[SetList.CheckedIndices[iIdx]];
                    line.Network = m_form.Factory.GetNetwork();
                    m_form.Factory.GetNetwork().GetLineList(setname).Add(line.Clone() as TrainLine);
                }
            }

            MessageBox.Show("完了");
        }

        /// <summary>
        /// キャンセルボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
