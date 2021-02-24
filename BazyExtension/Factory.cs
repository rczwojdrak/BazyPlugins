using System.Windows.Forms;

namespace BazyExtension
{
    public class Factory : UserExtensions.IBazyExtensionFactory
    {
        private string _OLEDBConnectionString = string.Empty;
        private string _UILanguage = string.Empty;
        private string _table = string.Empty;
        private int _recNo = -1;
        private int _tabNo = -1;

        public void Init(string OLEDBConnectionString, string UILanguage)
        {
            _UILanguage = UILanguage;
            _OLEDBConnectionString = OLEDBConnectionString;
        }

        public bool Run(string table, int recNo, int tabNo)
        {
            _table = table;
            _recNo = recNo;
            _tabNo = tabNo;

            MainManager manager = new MainManager(_OLEDBConnectionString, _UILanguage, _table, _tabNo, _recNo);
            DialogResult result = manager.ShowDialog();
            if (result == DialogResult.Cancel)
                return false;
            else
                return true;
        }
    }
}
