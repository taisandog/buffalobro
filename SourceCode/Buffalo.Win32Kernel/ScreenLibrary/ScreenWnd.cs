using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Buffalo.Win32Kernel.ScreenLibrary
{
    /// <summary>
    /// 屏幕截圖器。
    /// </summary>
    public partial class ScreenWnd : Form
    {
        private Bitmap ScreenMap;   //獲得的屏幕圖象。
        private Bitmap MapBuffer;   //屏幕圖像表現的緩存。
        private Graphics g_Wnd;     //繪制截圖表現的畫布。
        

        public ScreenWnd(Bitmap scrMap)
        {
            InitializeComponent();

            ScreenMap = scrMap;
            MapBuffer = (Bitmap)ScreenMap.Clone();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Activate();
        }


        /// <summary>
        /// 將屏幕圖象繪制為為本窗口的背景。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.DrawImage(MapBuffer, 0, 0, MapBuffer.Width, MapBuffer.Height);
            g_Wnd = this.CreateGraphics();
            DrawSelection();
        }

        /// <summary>
        /// 按 ESC 鍵時取消屏幕截圖。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScreenWnd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CancelScreenshots();
                e.Handled = true;
            }
        }

        private int _cursorX, _cursorY;   // 按下鼠標左鍵時的坐標。
        private bool _MouseDown = false;  // 用於記錄鼠標左鍵是否處於按下狀態。
        private bool _Selection = false;  // 標記是建立了選區。
        private bool _selectionChange = false;  // 選區是否已經改變。
        private bool _DragSelection = false;    // 標記是否開始拖動調整選區。
        private Rectangle sRect;                // 用於標記選區的矩形。
        private ResizeType _ReSelection;           // 標記選區調整的方式。

        /// <summary>
        /// 繪制選區表現。
        /// </summary>
        private void DrawSelection()
        {
            MapBuffer = (Bitmap)ScreenMap.Clone();
            Graphics g = Graphics.FromImage(MapBuffer);
            SolidBrush b = new SolidBrush(Color.FromArgb(90, Color.LightSkyBlue));
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new Rectangle(0, 0, MapBuffer.Width, MapBuffer.Height));
            path.AddRectangle(sRect);
            g.FillPath(b, path);
            g.DrawRectangle(new Pen(Color.DodgerBlue), sRect);
            g.FillRectangle(Brushes.DodgerBlue, sRect.X - 2, sRect.Y - 2, 4, 4);
            g.FillRectangle(Brushes.DodgerBlue, sRect.X + sRect.Width / 2 - 2, sRect.Y - 2, 4, 4);
            g.FillRectangle(Brushes.DodgerBlue, sRect.X + sRect.Width - 2, sRect.Y - 2, 4, 4);
            g.FillRectangle(Brushes.DodgerBlue, sRect.X + sRect.Width - 2, sRect.Y + sRect.Height / 2 - 2, 4, 4);
            g.FillRectangle(Brushes.DodgerBlue, sRect.X + sRect.Width - 2, sRect.Y + sRect.Height - 2, 4, 4);
            g.FillRectangle(Brushes.DodgerBlue, sRect.X + sRect.Width / 2 - 2, sRect.Y + sRect.Height - 2, 4, 4);
            g.FillRectangle(Brushes.DodgerBlue, sRect.X - 2, sRect.Y + sRect.Height - 2, 4, 4);
            g.FillRectangle(Brushes.DodgerBlue, sRect.X - 2, sRect.Y + sRect.Height / 2 - 2, 4, 4);
            g_Wnd.DrawImage(MapBuffer, 0, 0, MapBuffer.Width, MapBuffer.Height);
            _selectionChange = false;
        }

        /// <summary>
        /// 已選中區域的移動表現。
        /// </summary>
        /// <param name="MouseX">當前鼠標的 X 坐標</param>
        /// <param name="MouseY">當前鼠標的 Y 坐標</param>
        protected void MoveSelection(int MouseX, int MouseY)
        {
            sRect.X += MouseX - _cursorX;
            sRect.Y += MouseY - _cursorY;

            DrawSelection();
            _cursorX = MouseX;
            _cursorY = MouseY;
            _selectionChange = true;
        }

        /// <summary>
        /// 调整选区的大小。
        /// </summary>
        /// <param name="Resize">选区的大小调整方式。</param>
        /// <param name="X">当前鼠标的 X 坐标。</param>
        /// <param name="Y">当前鼠标的 Y 坐标。</param>
        protected void ResizeSelection(ResizeType resize, int X, int Y)
        {
            switch (resize)
            {
                case ResizeType.Left:
                    sRect.X = X;
                    sRect.Width += _cursorX - X;
                    break;
                case ResizeType.Right:
                    sRect.Width += X - _cursorX;
                    break;
                case ResizeType.Top:
                    sRect.Y = Y;
                    sRect.Height += _cursorY - Y;
                    break;
                case ResizeType.Bottom:
                    sRect.Height += Y - _cursorY;
                    break;
                case ResizeType.LeftTop:
                    sRect.X = X;
                    sRect.Y = Y;
                    sRect.Width += _cursorX - X;
                    sRect.Height += _cursorY - Y;
                    break;
                case ResizeType.RightTop:
                    sRect.Y = Y;
                    sRect.Width += X - _cursorX;
                    sRect.Height += _cursorY - Y;
                    break;
                case ResizeType.RightBottom:
                    sRect.Width += X - _cursorX;
                    sRect.Height += Y - _cursorY;
                    break;
                case ResizeType.LeftBottom:
                    sRect.X = X;
                    sRect.Width += _cursorX - X;
                    sRect.Height += Y - _cursorY;
                    break;
            }
            _cursorX = X;
            _cursorY = Y;
            DrawSelection();
            _selectionChange = true;
        }

        /// <summary>
        /// 当鼠标按下时。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                _MouseDown = true;
                _cursorX = e.X;
                _cursorY = e.Y;
            }
        }

        /// <summary>
        /// 当鼠标弹起时。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (sRect.Width > 0 || sRect.Height > 0)
                _Selection = true;
            _MouseDown = false;
            _DragSelection = false;
            if (_selectionChange) DrawSelection();
        }

        /// <summary>
        /// 当鼠标移动时。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!_Selection && _MouseDown)
            {
                int rW = e.X - _cursorX, rH = e.Y - _cursorY;
                sRect = new Rectangle(_cursorX, _cursorY, rW < 0 ? 0 : rW, rH < 0 ? 0 : rH);
                DrawSelection();
                _selectionChange = true;
            }
            else
            {
                if (!_DragSelection)
                {
                    if ((e.X >= sRect.X + 6) && (e.X <= sRect.X + sRect.Width - 6) && (e.Y >= sRect.Y + 6) && (e.Y <= sRect.Y + sRect.Height - 6))
                    {
                        this.Cursor = Cursors.SizeAll;
                        _ReSelection = ResizeType.Move;
                        if (_MouseDown) _DragSelection = true;
                    }
                    else
                    { this.Cursor = Cursors.Default; }

                    if ((e.X >= sRect.X - 6) && (e.X <= sRect.X + 6) && (e.Y >= sRect.Y - 6) && (e.Y <= sRect.Y + 6))
                    {
                        this.Cursor = Cursors.SizeNWSE;
                        _ReSelection = ResizeType.LeftTop;
                        if (_MouseDown) _DragSelection = true;
                    }
                    if ((e.X >= sRect.X + sRect.Width - 6) && (e.X <= sRect.X + sRect.Width + 6) && (e.Y >= sRect.Y - 6) && (e.Y <= sRect.Y + 6))
                    {
                        this.Cursor = Cursors.SizeNESW;
                        _ReSelection = ResizeType.RightTop;
                        if (_MouseDown) _DragSelection = true;
                    }
                    if ((e.X >= sRect.X + sRect.Width - 6) && (e.X <= sRect.X + Width + 6) && (e.Y >= sRect.Y + sRect.Height - 6) && (e.Y <= sRect.Y + sRect.Height + 6))
                    {
                        this.Cursor = Cursors.SizeNWSE;
                        _ReSelection = ResizeType.RightBottom;
                        if (_MouseDown) _DragSelection = true;
                    }
                    if ((e.X >= sRect.X - 6) && (e.X <= sRect.X + 6) && (e.Y >= sRect.Y + sRect.Height - 6) && (e.Y <= sRect.Y + sRect.Height + 6))
                    {
                        this.Cursor = Cursors.SizeNESW;
                        _ReSelection = ResizeType.LeftBottom;
                        if (_MouseDown) _DragSelection = true;
                    }
                    if ((e.X >= sRect.X + sRect.Width / 2 - 6) && (e.X <= sRect.X + sRect.Width / 2 + 6) && (e.Y >= sRect.Y - 6) && (e.Y <= sRect.Y + 6))
                    {
                        this.Cursor = Cursors.SizeNS;
                        _ReSelection = ResizeType.Top;
                        if (_MouseDown) _DragSelection = true;
                    }
                    if ((e.X >= sRect.X + sRect.Width - 6) && (e.X <= sRect.X + sRect.Width + 6) && (e.Y >= sRect.Y + sRect.Height / 2 - 6) && (e.Y <= sRect.Y + sRect.Height / 2 + 6))
                    {
                        this.Cursor = Cursors.SizeWE;
                        _ReSelection = ResizeType.Right;
                        if (_MouseDown) _DragSelection = true;
                    }
                    if ((e.X >= sRect.X + sRect.Width / 2 - 6) && (e.X <= sRect.X + sRect.Width / 2 + 6) && (e.Y >= sRect.Y + sRect.Height - 6) && (e.Y <= sRect.Y + sRect.Height + 6))
                    {
                        this.Cursor = Cursors.SizeNS;
                        _ReSelection = ResizeType.Bottom;
                        if (_MouseDown) _DragSelection = true;
                    }
                    if ((e.X >= sRect.X - 6) && (e.X <= sRect.X + 6) && (e.Y >= sRect.Y + sRect.Height / 2 - 6) && (e.Y <= sRect.Y + sRect.Height / 2 + 6))
                    {
                        this.Cursor = Cursors.SizeWE;
                        _ReSelection = ResizeType.Left;
                        if (_MouseDown) _DragSelection = true;
                    }
                }
                else
                {
                    switch (_ReSelection)
                    { 
                        case ResizeType.Move :
                            MoveSelection(e.X, e.Y);
                            break;
                        default :
                            ResizeSelection(_ReSelection, e.X, e.Y);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 取消截圖。
        /// </summary>
        public void CancelScreenshots()
        {
            //FreeResource();
            this.DialogResult = DialogResult.Cancel;
            
        }

        public void FreeResource() 
        {
            _cursorX = 0;
            _cursorY = 0;
            _DragSelection = false;
            _MouseDown = false;
            _selectionChange = false;
            _Selection = false;
            ScreenMap.Dispose();
            MapBuffer.Dispose();
            this.Dispose();
        }

        /// <summary>
        /// 完成截圖。
        /// </summary>
        public void ScreenshotsComplete()
        {
            resaultPicture = BitMapDeal.BitmapCut(ScreenMap, sRect.X, sRect.Y, sRect.Width, sRect.Height);
            //FreeResource();
            this.DialogResult = DialogResult.OK;
            //this.Dispose();
        }

        Bitmap resaultPicture;
        public Bitmap ResaultPicture 
        {
            get 
            {
                return resaultPicture;
            }
        }

        /// <summary>
        /// 控制截圖菜單的可用性。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (sRect.Width > 0 && sRect.Height > 0)
            {
                smi_Cancel.Enabled = true;
                smi_OK.Enabled = true;
            }
            else
            {
                smi_Cancel.Enabled = true;
                smi_OK.Enabled = false;
            }
        }

        /// <summary>
        /// 點擊取菜命令時。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smi_Cancel_Click(object sender, EventArgs e)
        {
            CancelScreenshots();
        }

        /// <summary>
        /// 點擊完成命令時。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smi_OK_Click(object sender, EventArgs e)
        {
            ScreenshotsComplete();
        }

        /// <summary>
        /// 選定區域後雙擊完成截圖。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScreenWnd_DoubleClick(object sender, EventArgs e)
        {
            if (sRect.Width > 0 && sRect.Height > 0)
                ScreenshotsComplete();
        }

        private void ScreenWnd_Load(object sender, EventArgs e)
        {

        }
    }
}
