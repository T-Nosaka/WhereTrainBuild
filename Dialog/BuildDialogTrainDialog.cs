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
    /// 電車構築ダイアログ
    /// </summary>
    public partial class BuildDialogTrainDialog : Form
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
        /// コンストラクタ
        /// </summary>
        public BuildDialogTrainDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildDialogTrainDialog_Load(object sender, EventArgs e)
        {
            RebuildTimeTableComboBox();

            TimeTableCmb.SelectedItem = m_form.Factory.GetNetwork().SetKind;

            RebuildLineSetListBox();
        }

        /// <summary>
        /// タイムテーブルコンボ再構築
        /// </summary>
        protected void RebuildTimeTableComboBox()
        {
            TimeTableCmb.Items.Clear();

            foreach( var tt in Enum.GetValues(typeof(TrainNetwork.SetKindType)) )
            {
                TimeTableCmb.Items.Add(tt);
            }
        }

        /// <summary>
        /// ラインセットリストボックス再構築
        /// </summary>
        protected void RebuildLineSetListBox()
        {
            LineSetListBox.Items.Clear();

            var linesetlist = m_form.Factory.GetNetwork().GetLineSetList();
            LineSetListBox.Items.AddRange(linesetlist);

            if (linesetlist.Count() <= 0)
            {
                return;
            }

            LineSetListBox.SelectedIndex = 0;
        }

        /// <summary>
        /// ラインボックス
        /// </summary>
        protected class LineBox : WhereTrainBuild.MapUtil.View.ViewPointIF
        {
            public TrainLine Line;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LineBox(TrainLine line)
            {
                Line = line;
            }

            /// <summary>
            /// 文字列化
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return string.Format("{0} {1}", Line.Name, Line.Display);
            }

            /// <summary>
            /// 描画
            /// </summary>
            /// <param name="viewreqinfo">描画情報</param>
            /// <returns>True..描画範囲内 False..範囲外</returns>
            public bool Draw(WhereTrainBuild.MapUtil.View.ViewRequestInfo viewreqinfo)
            {
                //経路描画
                foreach (var path in Line.GetPathList())
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
        /// ラインセット変化発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineSetListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lineset = (string)LineSetListBox.SelectedItem;

            RefreshLineList(lineset);
        }

        /// <summary>
        /// ラインリスト構築
        /// </summary>
        /// <param name="lineset"></param>
        protected void RefreshLineList( string lineset )
        {
            if (lineset == null)
            {
                ClearViewPoint(true);
                return;
            }

            var linelist = m_form.Factory.GetNetwork().GetLineList(lineset);

            ClearViewPoint(true);

            foreach (var line in linelist)
            {
                var linebox = new LineBox(line);
                LineListBox.Items.Add(linebox);
            }
        }

        /// <summary>
        /// ラインリスト変化発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var linebox = LineListBox.SelectedItem as LineBox;
            if (linebox == null)
                return;
            var line = linebox.Line;

            ClearViewPoint(false);

            m_form.AddViewPoint(linebox);

            RefreshTrainList(line);
        }

        /// <summary>
        /// 電車リスト構築
        /// </summary>
        protected void RefreshTrainList( TrainLine line )
        {
            TrainListBox.Items.Clear();
            
            foreach (var train in line.GetTrainList())
            {
                TrainListBox.Items.Add(train);
            }
        }

        /// <summary>
        /// リストクリア
        /// </summary>
        protected void ClearViewPoint(bool clear)
        {
            foreach (LineBox plb in LineListBox.Items)
            {
                m_form.DelViewPoint(plb);
            }

            if (clear == true)
                LineListBox.Items.Clear();
        }

        /// <summary>
        /// フェームクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildDialogTrainDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClearViewPoint(true);
        }

        /// <summary>
        /// ラインセット削除ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelSetBtn_Click(object sender, EventArgs e)
        {
            var lineset = (string)LineSetListBox.SelectedItem;
            if (lineset == null)
                return;

            m_form.Factory.GetNetwork().DeleteLineSet(lineset);

            RebuildLineSetListBox();
        }

        /// <summary>
        /// 方面削除ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelLineBtn_Click(object sender, EventArgs e)
        {
            var lineset = (string)LineSetListBox.SelectedItem;
            if (lineset == null)
                return;

            var linebox = LineListBox.SelectedItem as LineBox;
            if (linebox == null)
                return;

            m_form.Factory.GetNetwork().ChangeLine(lineset);
            m_form.Factory.GetNetwork().DelLine(linebox.Line);

            RebuildLineSetListBox();
        }

        /// <summary>
        /// 方面編集ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditLineBtn_Click(object sender, EventArgs e)
        {
            var linebox = LineListBox.SelectedItem as LineBox;
            if (linebox == null)
                return;

            var dlg = new EditLineDialog();
            dlg.MainForm = m_form;
            dlg.Line = linebox.Line;
            dlg.Show(this);
        }

        /// <summary>
        /// 方面追加ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLineBtn_Click(object sender, EventArgs e)
        {
            var lineset = (string)LineSetListBox.SelectedItem;
            if (lineset == null)
                return;

            var dlg = new EditLineDialog();
            dlg.MainForm = m_form;
            dlg.Show(this);

            dlg.FormClosed += ( dlgsender, dlge) =>
            {
                var plineset = (string)LineSetListBox.SelectedItem;

                RefreshLineList(plineset);
            };
        }

        /// <summary>
        /// 選択ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectSetBtn_Click(object sender, EventArgs e)
        {
            var lineset = (string)LineSetListBox.SelectedItem;
            if (lineset == null)
                return;

            m_form.Factory.GetNetwork().ChangeLine(lineset);
        }

        /// <summary>
        /// 編集ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditBtn_Click(object sender, EventArgs e)
        {
            var train = TrainListBox.SelectedItem as TrainInfo;
            if (train == null)
                return;

            using (var dlg = new EditTrainDialog())
            {
                dlg.Train = train;
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// 追加ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBtn_Click(object sender, EventArgs e)
        {
            var linebox = LineListBox.SelectedItem as LineBox;
            if (linebox == null)
                return;

            var train = new TrainInfo();
            train.Line = linebox.Line;

            using (var dlg = new EditTrainDialog())
            {
                dlg.Train = train;
                if( dlg.ShowDialog(this) == DialogResult.OK )
                {
                    linebox.Line.AddTrain(train);

                    RefreshTrainList(linebox.Line);
                }
            }
        }

        /// <summary>
        /// 削除ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelBtn_Click(object sender, EventArgs e)
        {
            var linebox = LineListBox.SelectedItem as LineBox;
            if (linebox == null)
                return;

            var train = TrainListBox.SelectedItem as TrainInfo;
            if (train == null)
                return;

            linebox.Line.DelTrain(train);

            RefreshTrainList(linebox.Line);
        }

        /// <summary>
        /// ソース選択ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSelectBtn_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.DefaultExt = "zip";
                dlg.Filter = "zip(*.zip)|*.zip|すべてのファイル(*.*)|*.*";
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                DiaSrcTxt.Text = dlg.FileName;
            }
        }

        /// <summary>
        /// ソース選択ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiaSrcTxt_TextChanged(object sender, EventArgs e)
        {
            if(DiaSrcTxt.Text.Length != 0 )
                BuildBtn.Enabled = true;
            else
                BuildBtn.Enabled = false;
        }

        /// <summary>
        /// 強制ロード
        /// </summary>
        protected void LoadAssembly()
        {
            var web = System.Web.HttpUtility.UrlEncode("web");
            var net = System.Net.HttpWebRequest.Create("http://localhost");
        }

        /// <summary>
        /// 構築ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildBtn_Click(object sender, EventArgs e)
        {
            LoadAssembly();

            var buildlogic = new DynamicBuildLogic();
            if (buildlogic.BuildDaia(m_form.Factory as BaseFactory, DiaSrcTxt.Text, this) == false)
            {
                var error = buildlogic.ErrorToString();

                MessageBox.Show(error);
                return;
            }

            MessageBox.Show("完了");
        }

        /// <summary>
        /// ダイアグラムボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiaBtn_Click(object sender, EventArgs e)
        {
            var linebox = LineListBox.SelectedItem as LineBox;
            if (linebox == null)
                return;

            using (var dlg = new DaiagramDialog())
            {
                dlg.Line = linebox.Line;
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// タイムテーブル種別選択ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeTableSetBtn_Click(object sender, EventArgs e)
        {
            var val = (TrainNetwork.SetKindType)TimeTableCmb.SelectedItem;
            m_form.Factory.GetNetwork().SetKind = val;

            RebuildLineSetListBox();
        }
    }
}
