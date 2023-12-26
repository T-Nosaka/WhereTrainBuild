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
    /// パス編集ダイアログ
    /// </summary>
    public partial class EditPathDialog : Form
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
        /// パス
        /// </summary>
        protected TrainPath m_path = null;

        /// <summary>
        /// パスプロパティ
        /// </summary>
        public TrainPath Path
        {
            get
            {
                return m_path;
            }
            set
            {
                m_path = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditPathDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPathDialog_Load(object sender, EventArgs e)
        {
            VisibleChk.Checked = Path.Visible;
        }

        /// <summary>
        /// OKボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkBtn_Click(object sender, EventArgs e)
        {
            Path.Visible = VisibleChk.Checked;
        }
    }
}
