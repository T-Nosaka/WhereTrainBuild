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
    /// ダイアグラムダイアログ
    /// </summary>
    public partial class DaiagramDialog : Form
    {
        /// <summary>
        /// ライン
        /// </summary>
        protected TrainLine m_line = null;

        /// <summary>
        /// ラインプロパティ
        /// </summary>
        public TrainLine Line
        {
            set
            {
                m_line = value;

                m_stationlist = new List<StationInfoData>();
                bool bFirst = true;
                foreach (var path in m_line.GetPathList())
                {
                    if (bFirst == true)
                    {
                        bFirst = false;
                        m_stationlist.Add(path.StationA);
                    }
                    m_stationlist.Add(path.StationB);
                }
            }
        }

        /// <summary>
        /// 駅一覧
        /// </summary>
        protected List<StationInfoData> m_stationlist = new List<StationInfoData>();

        /// <summary>
        /// 駅の縦位置テーブル
        /// </summary>
        protected Dictionary<int, int> m_tStationVPos = new Dictionary<int, int>();

        /// <summary>
        /// 時間軸左オフセット
        /// </summary>
        protected int m_iOffsetLeft = 0;

        /// <summary>
        /// フォント基準高
        /// </summary>
        protected int m_font_height = 0;

        /// <summary>
        /// フォント基準幅
        /// </summary>
        protected int m_font_width = 0;

        /// <summary>
        /// 時間間幅
        /// </summary>
        protected int m_iWidthHour = 0;

        /// <summary>
        /// 時間幅間隔
        /// </summary>
        protected float m_fWidthHourCount = 32.0f;

        /// <summary>
        /// 駅高間隔
        /// </summary>
        protected float m_fHeightStationCount = 3.0f;

        /// <summary>
        /// ダブルバッファ対策パネル
        /// </summary>
        protected class PanelEx : Panel
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PanelEx()
            {
                //ダブルバッファリングを有効にする
                SetStyle(
                   ControlStyles.DoubleBuffer |
                   ControlStyles.UserPaint |
                   ControlStyles.AllPaintingInWmPaint,
                   true);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DaiagramDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ダイアグラムサイズ計算
        /// </summary>
        /// <param name="graphics"></param>
        /// <returns></returns>
        protected Point CalcDiagramSize(Graphics graphics)
        {
            //基準サイズ
            m_font_height = (int)(Font.GetHeight(graphics) * m_fHeightStationCount);
            m_font_width = (int)graphics.MeasureString("　", Font).Width;

            //駅一覧
            m_tStationVPos = new Dictionary<int, int>(); //駅の縦位置
            int Y = m_font_height * 1;
            m_iOffsetLeft = 0;
            foreach (var station in m_stationlist)
            {
                var str = string.Format("{0}", station.Name);
                m_tStationVPos[station.UniqID] = Y + (int)(m_font_height * 0.5f);

                Y += m_font_height;

                m_iOffsetLeft = (int)Math.Max(m_iOffsetLeft, graphics.MeasureString(str, Font).Width);
            }

            //時間軸
            int X = m_iOffsetLeft;
            var overtime = m_line.Network.Factory.OverTime;
            m_iWidthHour = (int)(m_font_width * m_fWidthHourCount);

            for (int iHour = overtime.Hours; iHour < overtime.Hours + 24; iHour++)
            {
                X += m_iWidthHour;
            }

            return new Point(X, Y);
        }

        /// <summary>
        /// ダイアグラム構築
        /// </summary>
        /// <param name="graphics"></param>
        protected void BuildDiagram( Graphics graphics, Point PSize )
        {
            graphics.Clear(Color.White);

            var brush = new SolidBrush(ForeColor);
            var dotpen = new Pen(new SolidBrush(Color.Black));
            dotpen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            //駅一覧
            int iTop = m_font_height * 1;
            int Y = iTop;
            foreach (var station in m_stationlist)
            {
                var str = string.Format("{0}", station.Name);
                graphics.DrawString(str, Font, brush, new PointF(0, Y));

                graphics.DrawLine(dotpen, m_iOffsetLeft, Y + (int)(m_font_height * 0.5f), PSize.X, Y + (int)(m_font_height * 0.5f));

                Y += m_font_height;
            }
            int iBottom = Y;

            //時間軸
            int X = m_iOffsetLeft;
            var overtime = m_line.Network.Factory.OverTime;

            for (int iHour = overtime.Hours; iHour < overtime.Hours + 24; iHour++)
            {
                graphics.DrawString(string.Format("{0}", iHour), Font, brush, new PointF(X, 0));

                graphics.DrawLine(dotpen, X, iTop, X, iBottom);

                X += m_iWidthHour;
            }

            //ダイアグラム
            var pen = new Pen(brush, 1.0f);
            foreach (var train in m_line.GetTrainList())
            {
                float fStartLastX = 0;
                int iStationLastY = 0;

                var drawlist = new List<Point>();

                bool bTop = true;
                foreach (var plan in train.Scedule.GetList())
                {
                    if (m_tStationVPos.ContainsKey(plan.Station.UniqID) == false)
                        continue;

                    var iStationY = m_tStationVPos[plan.Station.UniqID];

                    //時間軸位置
                    var iAliveHour = plan.AliveTime.Days * 24 + plan.AliveTime.Hours - overtime.Hours;
                    var iAliveX = m_iOffsetLeft + iAliveHour * m_iWidthHour + (plan.AliveTime.Minutes / 60.0f) * (float)m_iWidthHour;
                    var iStartHour = plan.StartTime.Days * 24 + plan.StartTime.Hours - overtime.Hours;
                    var iStartX = m_iOffsetLeft + iStartHour * m_iWidthHour + (plan.StartTime.Minutes / 60.0f) * (float)m_iWidthHour;

                    //到着時刻 駅
                    graphics.DrawArc(pen, (int)iAliveX, iStationY, 3, 3, 0, 360);

                    if (bTop == true)
                    {
                        bTop = false;
                    }
                    else
                    {
                        //前駅出発時刻
                        drawlist.Add(new Point((int)fStartLastX, iStationLastY));
                        //到着時刻
                        drawlist.Add(new Point((int)iAliveX, iStationY));
                        //出発時刻
                        drawlist.Add(new Point((int)iStartX, iStationY));
                    }
                    drawlist.Add(new Point((int)iAliveX, iStationY));
                    drawlist.Add(new Point((int)iStartX, iStationY));

                    fStartLastX = iStartX;
                    iStationLastY = iStationY;
                }

                graphics.DrawLines(pen, drawlist.ToArray());
            }
        }

        /// <summary>
        /// 画像
        /// </summary>
        protected Image m_image = null;

        /// <summary>
        /// 画像構築
        /// </summary>
        protected void BuildImage()
        {
            Point psize = new Point(0,0);
            using (var img = new Bitmap(1, 1))
            {
                psize = CalcDiagramSize(Graphics.FromImage(img));
            }

            m_image = new Bitmap(psize.X, psize.Y);
            BuildDiagram(Graphics.FromImage(m_image), psize);
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DaiagramDialog_Load(object sender, EventArgs e)
        {
            BuildImage();

            //パネルサイズ調整
            DiaPanel.Size = new Size( m_image.Width, m_image.Height);

            DiaPanel.BackgroundImage = m_image;
        }
    }
}
