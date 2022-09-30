using BazyExtension.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace BazyExtension
{
    public partial class MainManager : Form
    {
        private bool _DataChanged = false;
        private string _OLEDBConnectionString = string.Empty;
        private string _UILanguage = string.Empty;
        private string _table = string.Empty;
        private int _tabNo = -1;
        private int _recNo = -1;
        private event Action onClosePluginsMethods;

        public MainManager(string OLEDBConnectionString, string UILanguage, string table, int tabNo, int recNo)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(UILanguage);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(UILanguage);

            _OLEDBConnectionString = OLEDBConnectionString;
            _UILanguage = UILanguage;
            _table = table;
            _tabNo = tabNo;
            _recNo = recNo;

            InitializeComponent();
        }

        private void MainManager_Load(object sender, EventArgs e)
        {
            RestoreFormState();

            List<Plugin> plugins = new List<Plugin>();
            try
            {
                plugins = App_Code.PluginLoader.LoadPlugins(_OLEDBConnectionString, _UILanguage, _table, _tabNo, _recNo);
            }
            catch (Exception ex)
            {
                this.Close();
                MessageBox.Show(ex.Message, $"{GlobaResource.Title} - {GlobaResource.ErrorCaption}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (plugins.Count > 1)
            {

                tabControl1.Visible = true;
                foreach (Plugin plugin in plugins)
                {
                    TabPage tabPage = new TabPage($"{plugin.Index}. {plugin.Name}");

                    if (plugin.Control is PluginControlWPF wpf)
                    {
                        tabPage.Controls.Add(new ElementHost()
                        {
                            Child = wpf,
                            Dock = DockStyle.Fill
                        });
                    }
                    else if(plugin.Control is PluginControl wf)
                    {
                        tabPage.Controls.Add(wf);
                    }

                    //tabPage.Controls.Add(plugin.Control);
                    tabControl1.TabPages.Add(tabPage);
                    plugin.Control.DataChanged += Control_DataChanged1;
                    onClosePluginsMethods += plugin.Control.CloseActions;
                }
                this.Text = $"{GlobaResource.Title} - {tabControl1.SelectedTab.Text}";
            }
            else if (plugins.Count == 1)
            {
                Plugin item = plugins[0];
                this.Text = $"{GlobaResource.Title} - {item.Name}";
                tabControl1.Visible = false;

                if (item.Control is PluginControlWPF wpf)
                {
                    PluginPanel.Controls.Add(new ElementHost()
                    {
                        Child = wpf,
                        Dock = DockStyle.Fill
                    });
                }
                else if(item.Control is PluginControl wf)
                {
                    PluginPanel.Controls.Add(wf);
                }

                item.Control.DataChanged += Control_DataChanged1;
                onClosePluginsMethods += item.Control.CloseActions;
            }
            else
            {
                this.Close();
                MessageBox.Show($"{GlobaResource.MissingPluginsMessage} {_table}", GlobaResource.MissingPluginsCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RestoreFormState()
        {
            if (Properties.Settings.Default.Maximised)
            {
                Location = Properties.Settings.Default.Location;
                WindowState = FormWindowState.Maximized;
                Size = Properties.Settings.Default.Size;
            }
            else if (Properties.Settings.Default.Minimised)
            {
                Location = Properties.Settings.Default.Location;
                WindowState = FormWindowState.Minimized;
                Size = Properties.Settings.Default.Size;
            }
            else
            {
                Location = Properties.Settings.Default.Location;
                Size = Properties.Settings.Default.Size;
            }
        }

        private void Control_DataChanged1(object sender, EventArgs e)
        {
            _DataChanged = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = _DataChanged ? DialogResult.OK : DialogResult.Cancel;
            this.Close();
        }

        private void MainManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            onClosePluginsMethods?.Invoke();
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.Location = RestoreBounds.Location;
                Properties.Settings.Default.Size = RestoreBounds.Size;
                Properties.Settings.Default.Maximised = true;
                Properties.Settings.Default.Minimised = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.Location = Location;
                Properties.Settings.Default.Size = Size;
                Properties.Settings.Default.Maximised = false;
                Properties.Settings.Default.Minimised = false;
            }
            else
            {
                Properties.Settings.Default.Location = RestoreBounds.Location;
                Properties.Settings.Default.Size = RestoreBounds.Size;
                Properties.Settings.Default.Maximised = false;
                Properties.Settings.Default.Minimised = true;
            }
            Properties.Settings.Default.Save();
            this.DialogResult = _DataChanged ? DialogResult.OK : DialogResult.Cancel;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = $"{GlobaResource.Title} - {tabControl1.SelectedTab.Text}";
        }

    }
}
