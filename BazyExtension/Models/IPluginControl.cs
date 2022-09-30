using System;

namespace BazyExtension
{
    public interface IPluginControl
    {
        event EventHandler DataChanged;

        event Action OnCloseMethod;
        string OLEDBConnectionString { set; }
        string UILanguage { set; }
        string table { set; }
        int tabNo { set; }
        int recNo { set; }
        void CloseActions();
    }

}
