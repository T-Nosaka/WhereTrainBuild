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
using WhereTrainBuild.MapUtil;

namespace WhereTrainBuild.Dialog
{
    /// <summary>
    /// ファクトリプロパティ
    /// </summary>
    public partial class FactoryProDialog : Form
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
        /// ファクトリ
        /// </summary>
        public BaseFactory Factory;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FactoryProDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FactoryProDialog_Load(object sender, EventArgs e)
        {
            NameTxt.Text = Factory.Name;
            UniqIDTxt.Text = Factory.UniqID;
            UrlTxt.Text = Factory.Url;
            RssTxt.Text = Factory.RSS;
            if (RssTxt.Text == string.Empty)
                RssTxt.Text = @"http://api.tetsudo.com/traffic/rss20.xml";
            RssKeywordTxt.Text = Factory.RSSKeyword;
            if (RssKeywordTxt.Text == string.Empty)
                RssKeywordTxt.Text = @".*キーワード.*";

            LeftBottomLbl.Text = string.Format("{0:0.000000},{1:0.000000}", Factory.LeftBottom.lat, Factory.LeftBottom.lng);
            RightUpLbl.Text = string.Format("{0:0.000000},{1:0.000000}", Factory.RightUp.lat, Factory.RightUp.lng);

            m_leftbottom.Set(Factory.LeftBottom);
            m_rightup.Set(Factory.RightUp);

            OverTimePicker.Value = new DateTime(2018, 1, 1).Date.Add(Factory.OverTime);

            ColorLbl.BackColor = Factory.BaseColor;
        }

        /// <summary>
        /// OKボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkBtn_Click(object sender, EventArgs e)
        {
            Factory.Name = NameTxt.Text;
            Factory.UniqID = UniqIDTxt.Text;
            Factory.Url = UrlTxt.Text;
            Factory.RSS = RssTxt.Text;
            Factory.RSSKeyword = RssKeywordTxt.Text;

            Factory.LeftBottom.Set(m_leftbottom);
            Factory.RightUp.Set(m_rightup);

            Factory.OverTime = OverTimePicker.Value.TimeOfDay;

            Factory.BaseColor = ColorLbl.BackColor;
            foreach (var path in Factory.GetNetwork().GetPathList())
            {
                path.LineColor = Factory.BaseColor;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;

            Close();
        }

        /// <summary>
        /// 左下
        /// </summary>
        protected latlontool.latlng m_leftbottom = new latlontool.latlng();

        /// <summary>
        /// 左下ラベル
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftBottomLbl_Click(object sender, EventArgs e)
        {
            LeftBottomLbl.BackColor = Color.Red;

            SelectGeo(( dLat,  dLng) =>
            {
                m_leftbottom.lat = dLat;
                m_leftbottom.lng = dLng;

                LeftBottomLbl.Text = string.Format("{0:0.000000},{1:0.000000}", dLat, dLng);
                LeftBottomLbl.BackColor = Color.White;
            });
        }

        /// <summary>
        /// 右上
        /// </summary>
        protected latlontool.latlng m_rightup = new latlontool.latlng();

        /// <summary>
        /// 右上ラベル
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightUpLbl_Click(object sender, EventArgs e)
        {
            RightUpLbl.BackColor = Color.Red;

            SelectGeo((dLat, dLng) =>
            {
                m_rightup.lat = dLat;
                m_rightup.lng = dLng;

                RightUpLbl.Text = string.Format("{0:0.000000},{1:0.000000}", dLat, dLng);
                RightUpLbl.BackColor = Color.White;
            });
        }

        /// <summary>
        /// 選択型
        /// </summary>
        /// <param name="dLat"></param>
        /// <param name="dLng"></param>
        protected delegate void SelectDelegate(double dLat, double dLng);

        /// <summary>
        /// 選択開始
        /// </summary>
        /// <param name="callback"></param>
        protected void SelectGeo(SelectDelegate callback)
        {
            m_form.SetSelectPoint((viewrequestinfo, viewpoint, dLat, dLng) =>
            {
                if (callback != null)
                    callback(dLat, dLng);
            });
        }

        /// <summary>
        /// エリア取得ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalcAreaBtn_Click(object sender, EventArgs e)
        {
            Factory.CalcArea();

            LeftBottomLbl.Text = string.Format("{0:0.000000},{1:0.000000}", Factory.LeftBottom.lat, Factory.LeftBottom.lng);
            RightUpLbl.Text = string.Format("{0:0.000000},{1:0.000000}", Factory.RightUp.lat, Factory.RightUp.lng);

            m_leftbottom.Set(Factory.LeftBottom);
            m_rightup.Set(Factory.RightUp);
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

        /// <summary>
        /// 色ラベル押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorLbl_Click(object sender, EventArgs e)
        {
            using ( var dlg = new ColorDialog())
            {
                dlg.Color = ColorLbl.BackColor;
                if( dlg.ShowDialog(this) == DialogResult.OK )
                {
                    ColorLbl.BackColor = dlg.Color;
                }
            }
        }
    }
}
