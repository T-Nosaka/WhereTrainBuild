using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;


namespace WhereTrainBuild.MapUtil.View
{
    /// <summary>
    /// 描画エリア
    /// </summary>
    public class ViewArea
    {
        /// <summary>
        /// ビューポイントリスト
        /// </summary>
        protected List<ViewPointIF> m_viewpoint = new List<ViewPointIF>();

        /// <summary>
        /// 描画情報
        /// </summary>
        protected ViewRequestInfo m_viewrequestinfo = new ViewRequestInfo();

        /// <summary>
        /// 描画情報プロパティ
        /// </summary>
        public ViewRequestInfo ViewInfo
        {
            get
            {
                return m_viewrequestinfo;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewArea()
        {
        }

        /// <summary>
        /// ビューポイント追加
        /// </summary>
        /// <param name="vpif"></param>
        public void Add(ViewPointIF vpif)
        {
            m_viewpoint.Add(vpif);
        }

        /// <summary>
        /// ビューポイント削除
        /// </summary>
        /// <param name="vpif"></param>
        public void Del(ViewPointIF vpif)
        {
            m_viewpoint.Remove(vpif);
        }

        /// <summary>
        /// ビューポイントリスト取得
        /// </summary>
        /// <returns></returns>
        public List<ViewPointIF> GetList()
        {
            return new List<ViewPointIF>(m_viewpoint.ToArray());
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="image"></param>
        public virtual void DrawImage(Graphics gr, Size size )
        {
            //全点描画
            {
                m_viewrequestinfo.ViewSize = size;
                m_viewrequestinfo.ViewGraphics = gr;

                foreach (ViewPointIF viewpnt in m_viewpoint)
                {
                    viewpnt.Draw(m_viewrequestinfo);
                }
            }
        }

        /// <summary>
        /// 中心座標を平行移動
        /// ※画面座標
        /// </summary>
        /// <param name="iMoveX">X座標</param>
        /// <param name="iMoveY">Y座標</param>
        public void MoveCenter(int iMoveX, int iMoveY)
        {
            //メルカトル移動量に変換
            double dGlobalX = (double)iMoveX / m_viewrequestinfo.Scale;
            double dGlobalY = (double)iMoveY / m_viewrequestinfo.Scale;

            //描画座標クリップ領域
            Rectangle cliparea = m_viewrequestinfo.ClipArea();

            //グローバル座標へ移動
            dGlobalX += cliparea.Left;
            dGlobalY = cliparea.Bottom - dGlobalY;

            m_viewrequestinfo.Center = new Point((int)dGlobalX, (int)dGlobalY);
        }

        /// <summary>
        /// 中心座標を平行移動
        /// ※画面座標
        /// </summary>
        /// <param name="iDeltaX">X座標移動量</param>
        /// <param name="iDeltaY">Y座標移動量</param>
        /// <param name="mercatorcenter">移動元メルカトル座標</param>
        public void MoveCenter(int iDeltaX, int iDeltaY, Point mercatorcenter)
        {
            //メルカトル移動量に変換
            double dGlobalX = (double)iDeltaX / m_viewrequestinfo.Scale;
            double dGlobalY = (double)iDeltaY / m_viewrequestinfo.Scale;

            //グローバル座標へ移動
            dGlobalX += mercatorcenter.X;
            dGlobalY = -dGlobalY + mercatorcenter.Y;

            m_viewrequestinfo.Center = new Point((int)dGlobalX, (int)dGlobalY);
        }

        /// <summary>
        /// 中心座標自動設定
        /// </summary>
        /// <param name="iWidth">画像幅</param>
        /// <param name="iHeight">画像高</param>
        public void SetCenter( int iWidth, int iHeight )
        {
//            bool bFirst = true;

            //最大領域取得
            double dLeft = 0.0f;
            double dRight = 0.0f;
            double dTop = 0.0f;
            double dBottom = 0.0f;
/*
            foreach (ViewPointIF viewpnt in m_viewpoint)
            {
                double dMyPositionX=0;
                double dMyPositionY=0;

                MercatorTrans.Trans(viewpnt.Latitude / 180.0d * Math.PI, viewpnt.Longitude / 180.0d * Math.PI, ViewInfo.ZoomLevel, ref dMyPositionX, ref dMyPositionY);

                if (bFirst == true)
                {
                    //初期値
                    dLeft = dRight = dMyPositionX;
                    dTop = dBottom = dMyPositionY;
                    bFirst = false;
                }
                else
                {
                    if (dMyPositionX < dLeft)
                        dLeft = dMyPositionX;
                    if (dRight < dMyPositionX)
                        dRight = dMyPositionX;
                    if (dTop > dMyPositionY)
                        dTop = dMyPositionY;
                    if (dBottom < dMyPositionY)
                        dBottom = dMyPositionY;
                }
            }
*/
            //中心セット
            m_viewrequestinfo.Center = new Point( (int)((dLeft + dRight) / 2.0f), (int)((dTop + dBottom) / 2.0f));

            //画像全体に入るようにスケールを調整
            //座標、幅高
            double dTWidth = dRight - dLeft;
            double dTHeight = dBottom - dTop;

            double dTWk = ((double)iWidth) / dTWidth;
            double dTHk = ((double)iHeight) / dTHeight;
        }

        /// <summary>
        /// 指定座標に接触するポイントを取得
        /// </summary>
        /// <param name="iX">画像座標X</param>
        /// <param name="iY">画像座標Y</param>
        /// <returns></returns>
        public ViewPointIF[] GetInsidePoint(int iX, int iY)
        {
            List<ViewPointIF> result = new List<ViewPointIF>();

            foreach (ViewPointIF viewpnt in m_viewpoint)
            {
                if (viewpnt.IsHit(iX, iY, m_viewrequestinfo) == true)
                    result.Add(viewpnt);
            }

            return result.ToArray();
        }

    }
}
