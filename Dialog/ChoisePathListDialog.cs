using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WhereTrainBuild.MapUtil.Data;

namespace WhereTrainBuild.Dialog
{
    /// <summary>
    /// パスリスト選択ダイアログ
    /// </summary>
    public partial class ChoisePathListDialog : Form
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
        public ChoisePathListDialog()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// ラインボックス
        /// </summary>
        protected class LineViewPoint : WhereTrainBuild.MapUtil.View.ViewPointIF
        {
            public List<TrainPath> PathList;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LineViewPoint(List<TrainPath> pathlist)
            {
                PathList = pathlist;
            }

            /// <summary>
            /// 文字列化
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                if (PathList.Count > 0)
                {
                    string tmp = PathList[0].StationA.Name;

                    foreach (var str in PathList)
                    {
                        tmp += ("," + str.StationB.Name);
                    }

                    return tmp;
                }
                return "なし";
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
        /// パスリスト候補
        /// </summary>
        public List<List<TrainPath>> PathListTable;

        /// <summary>
        /// パスリスト変化発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pathlistbox = LineListBox.SelectedItem as LineViewPoint;
            if (pathlistbox == null)
                return;

            ClearViewPoint(false);
            m_form.AddViewPoint(pathlistbox);
        }

        /// <summary>
        /// パスリスト候補構築
        /// </summary>
        protected void BuildList()
        {
            LineListBox.Items.Clear();

            foreach (var pathlist in PathListTable)
            {
                pathlist.ForEach(path => path.LineColor = Color.RoyalBlue);

                var box = new LineViewPoint(pathlist);
                LineListBox.Items.Add(box);
            }
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoisePathListDialog_Load(object sender, EventArgs e)
        {
            BuildList();
        }

        /// <summary>
        /// 選択リスト
        /// </summary>
        public List<TrainPath> SelectList = null;

        /// <summary>
        /// OKボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkBtn_Click(object sender, EventArgs e)
        {
            var lvp = LineListBox.SelectedItem as LineViewPoint;
            if (lvp != null)
            {
                var factory = m_form.Factory as BaseFactory;

                lvp.PathList.ForEach(path => path.LineColor = factory.BaseColor);
                SelectList = lvp.PathList;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// リストクリア
        /// </summary>
        protected void ClearViewPoint(bool clear)
        {
            foreach (LineViewPoint plb in LineListBox.Items)
            {
                m_form.DelViewPoint(plb);
            }

            if (clear == true)
                LineListBox.Items.Clear();
        }

        /// <summary>
        /// フォームクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoisePathListDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClearViewPoint(true);
        }
    }
}
