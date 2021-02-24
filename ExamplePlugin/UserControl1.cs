using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamplePlugin
{
    public partial class UserControl1: BazyExtension.PluginControl
    {
        private string _OLEDBConnectionString = string.Empty;
        private string _UILanguage = string.Empty;
        private string _table = string.Empty;
        private int _tabNo = -1;
        private int _recNo = -1;


        public UserControl1()
        {
            InitializeComponent();
        }

        public override string OLEDBConnectionString { set => _OLEDBConnectionString = value; }
        public override string UILanguage { set => _UILanguage = value; }
        public override string table { set => _table = value; }
        public override int tabNo { set => _tabNo = value; }
        public override int recNo { set => _recNo = value; }

        public override event EventHandler DataChanged;

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show($@"
Data from Bazy:

_OLEDBConnectionString = {_OLEDBConnectionString}
_UILanguage = {_UILanguage}
_table = {_table}
_tabNo = {_tabNo}
_recNo = {_recNo}

This will force Bazy to do reload UI.
");
            DataChanged(null, null);
        }
    }
}
