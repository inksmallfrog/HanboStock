using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    class DoubleBufferListView : ListView

    {

        public DoubleBufferListView()

        {
            SetStyle(ControlStyles.DoubleBuffer |

               ControlStyles.OptimizedDoubleBuffer |

               ControlStyles.AllPaintingInWmPaint, true);

            UpdateStyles();

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DoubleBufferListView
            // 
            this.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.AllowColumnReorder = true;
            this.AutoArrange = false;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LabelWrap = false;
            this.MultiSelect = false;
            this.OwnerDraw = true;
            this.Scrollable = false;
            this.ShowGroups = false;
            this.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ResumeLayout(false);

        }

    }
}
