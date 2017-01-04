using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dnSpy.dnpatch
{
    /// <summary>
    /// Interaction logic for ToolWindowControl.xaml
    /// </summary>
    public partial class ToolWindowControl : UserControl
    {
        public static ListView ListView;
        private static ToolWindowControl instance;
        public static ToolWindowControl Instance { get { return instance; } }

        public ToolWindowControl()
        {
            InitializeComponent();
            instance = this;
            ListView = listView;
        }
    }
}
