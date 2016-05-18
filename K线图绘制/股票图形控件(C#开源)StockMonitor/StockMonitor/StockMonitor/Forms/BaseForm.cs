using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;

namespace StockMonitor.Forms
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
            this.MouseDown+=new MouseEventHandler(CustomForm_MouseDown);
            this.MouseUp += new MouseEventHandler(CustomForm_MouseUp);
            this.MouseMove+=new MouseEventHandler(CustomForm_MouseMove);
            this.MouseLeave+=new EventHandler(CustomForm_MouseLeave);
            currentWindowState = this.WindowState;
        }

        /// <summary>
        /// Get the GraphicsPath by cornerRadius and Rectangle.
        /// </summary>
        /// <param name="cornerRadius"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public GraphicsPath GetGraphicsPath(int cornerRadius, Rectangle rect)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }

        /// <summary>
        /// 当前的窗体状态
        /// </summary>
        FormWindowState currentWindowState;



        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(HandleRef hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (currentWindowState != this.WindowState)
            {
                currentWindowState = this.WindowState;
            }
            try
            {
                base.WndProc(ref m);
            }
            catch (Exception e)
            {
            }

        }
        #region 拖动窗体大小
        internal static class MState
        {
            private static int m_Left;
            private static int m_Top;
            private static int m_Right;
            private static int m_Bottom;
            private static int m_LastX;
            private static int m_LastY;
            private static MouseAction m_State;
            private static MouseAction m_LastState;

            public static int Left
            {
                get { return MState.m_Left; }
            }

            public static int Top
            {
                get { return MState.m_Top; }
            }

            public static int Right
            {
                get { return MState.m_Right; }
            }

            public static int Bottom
            {
                get { return MState.m_Bottom; }
            }

            public static MouseAction State
            {
                get { return MState.m_State; }
            }

            public static int LastX
            {
                get { return MState.m_LastX; }
            }

            public static int LastY
            {
                get { return MState.m_LastY; }
            }

            public static MouseAction LastState
            {
                get { return MState.m_LastState; }
            }

            public static void SetData(Rectangle bounds, Point point)
            {
                m_Left = bounds.Left;
                m_Top = bounds.Top;
                m_Right = bounds.Right;
                m_Bottom = bounds.Bottom;
                m_LastX = point.X;
                m_LastY = point.Y;
                m_LastState = m_State;
            }

            public static void SetState(MouseAction state)
            {
                m_State = state;
            }

            public static void ResetState()
            {
                m_State = MouseAction.None;
                m_LastState = MouseAction.None;
            }
        }

        internal enum MouseAction
        {
            None,
            Left,
            LeftTop,
            Top,
            RightTop,
            Right,
            RightBottom,
            Bottom,
            LeftBottom
        }

        private int m_BorderWidth = 4;

        private void CustomForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                MState.SetData(this.Bounds, Cursor.Position);
            }
        }

        private void CustomForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
            { 
                int dx = 0;
                int dy = 0;
                int dr = 0;
                int db = 0;
                Point cp = Cursor.Position;
                if (MState.LastState == MouseAction.Left)
                {
                    dx = cp.X - MState.LastX;
                }
                else if (MState.LastState == MouseAction.Right)
                {
                    dr = cp.X - MState.LastX;
                }
                else if (MState.LastState == MouseAction.Top)
                {
                    dy = cp.Y - MState.LastY;
                }
                else if (MState.LastState == MouseAction.Bottom)
                {
                    db = cp.Y - MState.LastY;
                }
                else if (MState.LastState == MouseAction.LeftTop)
                {
                    dx = cp.X - MState.LastX;
                    dy = cp.Y - MState.LastY;
                }
                else if (MState.LastState == MouseAction.RightTop)
                {
                    dr = cp.X - MState.LastX;
                    dy = cp.Y - MState.LastY;
                }
                else if (MState.LastState == MouseAction.LeftBottom)
                {
                    dx = cp.X - MState.LastX;
                    db = cp.Y - MState.LastY;
                }
                else if (MState.LastState == MouseAction.RightBottom)
                {
                    dr = cp.X - MState.LastX;
                    db = cp.Y - MState.LastY;
                }
                int left = MState.Left + dx;
                int top = MState.Top + dy;
                int right = MState.Right + dr;
                int bottom = MState.Bottom + db;
                if (dx != 0)
                {
                    if (left >= this.Right - MinimumSize.Width)
                    {
                        left = this.Right - MinimumSize.Width;
                    }
                }
                if (dy != 0)
                {
                    if (top >= this.Bottom - MinimumSize.Height)
                    {
                        top = this.Bottom - MinimumSize.Height;
                    }
                }
                if (dr != 0)
                {
                    if (right <= this.Left + MinimumSize.Width)
                    {
                        right = this.Left + MinimumSize.Width;
                    }
                }
                if (db != 0)
                {
                    if (bottom <= this.Top + MinimumSize.Height)
                    {
                        bottom = this.Top + MinimumSize.Height;
                    }
                }
                this.Bounds = Rectangle.FromLTRB(left, top, right, bottom);
                this.Size = new Size(this.Bounds.Width, this.Bounds.Height);
                this.Invalidate();
                MState.ResetState();
            }
            this.Cursor = Cursors.Default;
        }

        Rectangle mouseRect = Rectangle.Empty;

        private void CustomForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.Capture && WindowState != FormWindowState.Maximized)
            {
                MState.SetState(this.setMouse(e.Location));
                return;
            }
        }

        private void CustomForm_MouseLeave(object sender, EventArgs e)
        {
            if (this.Bounds.Contains(Cursor.Position))
            {
                this.Cursor = Cursors.Default;
            }
        }

        private MouseAction setMouse(Point point)
        {
            Rectangle rect = this.ClientRectangle;

            if ((point.X <= rect.Left + m_BorderWidth) && (point.Y <= rect.Top + m_BorderWidth))
            {
                this.Cursor = Cursors.SizeNWSE;
                return MouseAction.LeftTop;
            }
            else if ((point.X >= rect.Left + rect.Width - m_BorderWidth) && (point.Y <= rect.Top + m_BorderWidth))
            {
                this.Cursor = Cursors.SizeNESW;
                return MouseAction.RightTop;
            }
            else if ((point.X <= rect.Left + m_BorderWidth) && (point.Y >= rect.Top + rect.Height - m_BorderWidth))
            {
                this.Cursor = Cursors.SizeNESW;
                return MouseAction.LeftBottom;
            }
            else if ((point.X >= rect.Left + rect.Width - m_BorderWidth) && (point.Y >= rect.Top + rect.Height - m_BorderWidth))
            {
                this.Cursor = Cursors.SizeNWSE;
                return MouseAction.RightBottom;
            }
            else if ((point.X <= rect.Left + m_BorderWidth - 1))
            {
                this.Cursor = Cursors.SizeWE;
                return MouseAction.Left;
            }
            else if ((point.X >= rect.Left + rect.Width - m_BorderWidth))
            {
                this.Cursor = Cursors.SizeWE;
                return MouseAction.Right;
            }
            else if ((point.Y <= rect.Top + m_BorderWidth - 1))
            {
                this.Cursor = Cursors.SizeNS;
                return MouseAction.Top;
            }
            else if ((point.Y >= rect.Top + rect.Height - m_BorderWidth))
            {
                this.Cursor = Cursors.SizeNS;
                return MouseAction.Bottom;
            }
            this.Cursor = Cursors.Default;
            return MouseAction.None;
        }
        #endregion
    }
}