using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageOfStock
{
    public partial class GlobalInfo : Component
    {
        public GlobalInfo()
        {
            InitializeComponent();
        }

        public GlobalInfo(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
