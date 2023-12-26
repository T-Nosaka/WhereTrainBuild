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
    /// 駅ダイアログ
    /// </summary>
    public partial class StationDialog : Form
    {
        /// <summary>
        /// 駅
        /// </summary>
        protected StationInfoData m_station;

        /// <summary>
        /// 駅セット
        /// </summary>
        /// <param name="station"></param>
        public void SetStation(StationInfoData station)
        {
            m_station = station;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StationDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationDialog_Load(object sender, EventArgs e)
        {
            NameTxt.Text = m_station.Name;
            VisibleChk.Checked = m_station.Visible;

            LatLbl.Text = string.Format("{0:0.000000}",m_station.Latitude);
            LngLbl.Text = string.Format("{0:0.000000}", m_station.Longitude);

            CdNum.Value = m_station.StationCd;
            GCdNum.Value = m_station.StationGCd;
        }

        /// <summary>
        /// OKボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkBtn_Click(object sender, EventArgs e)
        {
            m_station.Name = NameTxt.Text;
            m_station.Visible = VisibleChk.Checked;

            m_station.StationCd = (int)CdNum.Value;
            m_station.StationGCd = (int)GCdNum.Value;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 駅コードラベル押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdLbl_Click(object sender, EventArgs e)
        {
            CdNum.Value = NameTxt.Text.GetHashCode();
        }

        /// <summary>
        /// 最大ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxBtn_Click(object sender, EventArgs e)
        {
            var maxcd = m_station.Manager.StationList().Max( info => info.StationCd ) + 1;
            CdNum.Value = maxcd;
        }
    }
}
