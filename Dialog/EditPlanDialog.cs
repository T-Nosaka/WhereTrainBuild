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
    /// 計画ダイアログ
    /// </summary>
    public partial class EditPlanDialog : Form
    {
        /// <summary>
        /// 計画
        /// </summary>
        protected SceduleManager.Plan m_plan;

        /// <summary>
        /// 計画プロパティ
        /// </summary>
        public SceduleManager.Plan Plan
        {
            get
            {
                return m_plan;
            }
            set
            {
                m_plan = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditPlanDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// OKボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkBtn_Click(object sender, EventArgs e)
        {
            Plan.AliveTime = new TimeSpan((int)AliveDayNum.Value, AliveTimePicker.Value.Hour, AliveTimePicker.Value.Minute, AliveTimePicker.Value.Second);
            Plan.StartTime = new TimeSpan((int)StartDayNum.Value, StartTimePicker.Value.Hour, StartTimePicker.Value.Minute, StartTimePicker.Value.Second);

            Plan.Passing = PassChk.Checked;

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPlanDialog_Load(object sender, EventArgs e)
        {
            AliveDayNum.Value = Plan.AliveTime.Days;
            AliveTimePicker.Value = new DateTime(2000,1,1,Plan.AliveTime.Hours, Plan.AliveTime.Minutes, Plan.AliveTime.Seconds);
            StartDayNum.Value = Plan.StartTime.Days;
            StartTimePicker.Value = new DateTime(2000, 1, 1, Plan.StartTime.Hours, Plan.StartTime.Minutes, Plan.StartTime.Seconds);
            PassChk.Checked = Plan.Passing;
        }
    }
}
