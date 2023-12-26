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
    /// 電車編集ダイアログ
    /// </summary>
    public partial class EditTrainDialog : Form
    {
        /// <summary>
        /// 電車情報
        /// </summary>
        protected TrainInfo m_train = null;

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
        /// コンストラクタ
        /// </summary>
        public EditTrainDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTrainDialog_Load(object sender, EventArgs e)
        {
            NameTxt.Text = Train.Name;
            DisplayTxt.Text = Train.Display;
            KindTxt.Text = Train.Kind;
            StartTxt.Text = Train.StartStation;
            EndTxt.Text = Train.EndStation;
            LevelNum.Value = Train.Level;

            //ラインリスト
            LineListBox.Items.Clear();
            foreach ( var path in Train.Line.GetPathList() )
            {
                LineListBox.Items.Add(path);
            }

            //ダイヤ
            DiaListBox.Items.Clear();
            foreach( var plan in Train.Scedule.GetList() )
            {
                DiaListBox.Items.Add(CreateLVI(plan));
            }
        }

        /// <summary>
        /// ダイヤアイテム構築
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        protected ListViewItem CreateLVI( SceduleManager.Plan plan )
        {
            var lvi = new ListViewItem(plan.Station.Name);
            lvi.Tag = plan;
            var alive = new ListViewItem.ListViewSubItem(lvi, string.Format("{0}.{1:00}:{2:00}:{3:00}", plan.AliveTime.Days, plan.AliveTime.Hours, plan.AliveTime.Minutes, plan.AliveTime.Seconds));
            lvi.SubItems.Add(alive);
            var start = new ListViewItem.ListViewSubItem(lvi, string.Format("{0}.{1:00}:{2:00}:{3:00}", plan.StartTime.Days ,plan.StartTime.Hours, plan.StartTime.Minutes, plan.StartTime.Seconds));
            lvi.SubItems.Add(start);

            return lvi;
        }

        /// <summary>
        /// OKボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkBtn_Click(object sender, EventArgs e)
        {
            Train.Name = NameTxt.Text;
            Train.Display = DisplayTxt.Text;
            Train.Kind = KindTxt.Text;
            Train.StartStation = StartTxt.Text;
            Train.EndStation = EndTxt.Text;
            Train.Level = (int)LevelNum.Value;

            DialogResult = DialogResult.OK;

            Train.Scedule.Clear();

            int iOrd = 1;
            foreach( ListViewItem lvi in DiaListBox.Items)
            {
                var plan = lvi.Tag as SceduleManager.Plan;
                plan.Order = iOrd++;
                Train.Scedule.Add(plan);
            }
        }

        /// <summary>
        /// 追加ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddPlan(false);
        }

        /// <summary>
        /// 上書きボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            AddPlan(true);
        }

        /// <summary>
        /// 計画追加
        /// </summary>
        protected void AddPlan(bool clear)
        {
            List<StationInfoData> stationlist = new List<StationInfoData>();

            int lastid = -1;
            foreach (TrainPath path in LineListBox.SelectedItems)
            {
                if (lastid == -1)
                    stationlist.Add(path.StationA);
                stationlist.Add(path.StationB);

                lastid = path.StationB.UniqID;
            }

            if(clear == true)
                DiaListBox.Items.Clear();
            foreach (var station in stationlist)
            {
                var plan = new SceduleManager.Plan();
                plan.Station = station;
                DiaListBox.Items.Add(CreateLVI(plan));
            }
        }

        /// <summary>
        /// ダイヤグラムマウスダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiaListBox_MouseDown(object sender, MouseEventArgs e)
        {
            var selectlvi = DiaListBox.GetItemAt(e.Location.X, e.Location.Y);
            if (selectlvi == null)
                return;

            ContextMenuStrip popupmenu = new ContextMenuStrip();

            ToolStripMenuItem uppathitem = new ToolStripMenuItem("Up");
            uppathitem.Click += (owner, args) =>
            {
                var srcidx = DiaListBox.Items.IndexOf(selectlvi);
                if (srcidx > 0)
                {
                    DiaListBox.Items.Remove(selectlvi);
                    DiaListBox.Items.Insert(srcidx - 1, selectlvi);
                }
            };
            popupmenu.Items.Add(uppathitem);

            ToolStripMenuItem downpathitem = new ToolStripMenuItem("Down");
            downpathitem.Click += (owner, args) =>
            {
                var srcidx = DiaListBox.Items.IndexOf(selectlvi);
                if (srcidx < DiaListBox.Items.Count-1)
                {
                    DiaListBox.Items.Remove(selectlvi);
                    DiaListBox.Items.Insert(srcidx + 1, selectlvi);
                }
            };
            popupmenu.Items.Add(downpathitem);

            ToolStripMenuItem delpathitem = new ToolStripMenuItem("Del");
            delpathitem.Click += (owner, args) =>
            {
                DiaListBox.Items.Remove(selectlvi);
            };
            popupmenu.Items.Add(delpathitem);

            ToolStripMenuItem editpathitem = new ToolStripMenuItem("Edit");
            editpathitem.Click += (owner, args) =>
            {
                using (var dlg = new EditPlanDialog())
                {
                    var plan = selectlvi.Tag as SceduleManager.Plan;

                    dlg.Plan = plan;
                    if ( dlg.ShowDialog(this) == DialogResult.OK )
                    {
                        selectlvi.SubItems[1].Text = string.Format("{0}.{1:00}:{2:00}:{3:00}", plan.AliveTime.Days, plan.AliveTime.Hours, plan.AliveTime.Minutes, plan.AliveTime.Seconds);
                        selectlvi.SubItems[2].Text = string.Format("{0}.{1:00}:{2:00}:{3:00}", plan.StartTime.Days, plan.StartTime.Hours, plan.StartTime.Minutes, plan.StartTime.Seconds);
                    }
                }
            };
            popupmenu.Items.Add(editpathitem);



            popupmenu.Show(Cursor.Position);
        }
    }
}
