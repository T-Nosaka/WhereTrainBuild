using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhereTrainBuild.Dialog
{
    /// <summary>
    /// 線路解析ダイアログ
    /// </summary>
    public partial class WalkingDialog : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WalkingDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 停止フラグ
        /// </summary>
        public volatile bool Terminate = false;

        /// <summary>
        /// 停止ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopBtn_Click(object sender, EventArgs e)
        {
            Terminate = true;
        }

        /// <summary>
        /// 閉じた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WalkingDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            Terminate = true;
        }
    }
}
