using System;

namespace BazyExtension
{
    public interface IPluginControl
    {
        event EventHandler DataChanged;
        string OLEDBConnectionString { set; }
        string UILanguage { set; }
        string table { set; }
        int tabNo { set; }
        int recNo { set; }
    }
}
