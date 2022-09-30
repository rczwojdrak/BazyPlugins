using BazyExtension.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace BazyExtension.App_Code
{
    internal static class PluginLoader
    {
        internal static List<Plugin> LoadPlugins(string OLEDBConnectionString, string UILanguage, string table, int tabNo, int recNo)
        {
            int index = 1;
            List<Plugin> result = new List<Plugin>();

            string pathname = AppDomain.CurrentDomain.BaseDirectory;

            XDocument pluginsFile = XDocument.Load(Path.Combine(pathname, "BazyExtension.xml"));
            foreach (XElement item in pluginsFile.Descendants("Item").Where(x => x.Attribute("table").Value.ToUpper() == table.ToUpper()))
            {
                string fileName = (item.Attribute("fileName") != null) ? item.Attribute("fileName").Value : string.Empty;
                string name = (item.Attribute("pluginName") != null) ? item.Attribute("pluginName").Value : table;

                string path = string.Empty;
                foreach (string tempFileName in Directory.GetFiles(pathname, $@"{fileName}"))
                {
                    if (File.Exists(tempFileName))
                    {
                        path = tempFileName;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(path) && (File.Exists(Path.Combine(pathname, table))))
                    path = Path.Combine(pathname, fileName);


                if (!string.IsNullOrEmpty(path))
                {
                    Plugin plugin = new Plugin();
                    Assembly library = Assembly.LoadFrom(path);
                    Type[] types = library.GetTypes();
                    foreach (Type t in types)
                    {
                        if (typeof(PluginControlWPF).IsAssignableFrom(t))
                        {
                            try
                            {
                                ///If plucin contains params in constructor
                                plugin.Control = (PluginControlWPF)Activator.CreateInstance(t, OLEDBConnectionString, UILanguage, table, tabNo, recNo);
                            }catch
                            {
                                plugin.Control = (PluginControlWPF)Activator.CreateInstance(t);
                                plugin.Control.OLEDBConnectionString = OLEDBConnectionString;
                                plugin.Control.UILanguage = UILanguage;
                                plugin.Control.table = table;
                                plugin.Control.tabNo = tabNo;
                                plugin.Control.recNo = recNo;
                            }
                            break;
                        }
                        if (typeof(PluginControl).IsAssignableFrom(t))
                        {
                            try
                            {
                                ///If plucin contains params in constructor
                                plugin.Control = (PluginControl)Activator.CreateInstance(t, OLEDBConnectionString, UILanguage, table, tabNo, recNo);
                            }
                            catch
                            {
                                plugin.Control = (PluginControl)Activator.CreateInstance(t);
                                plugin.Control.OLEDBConnectionString = OLEDBConnectionString;
                                plugin.Control.UILanguage = UILanguage;
                                plugin.Control.table = table;
                                plugin.Control.tabNo = tabNo;
                                plugin.Control.recNo = recNo;
                                (plugin.Control as PluginControl).Dock = System.Windows.Forms.DockStyle.Fill;
                            }
                            
                            break;
                        }
                        
                    }
                    if (plugin.Control != null)
                    {
                        plugin.Index = index;
                        plugin.Name = name;
                        plugin.FileName = fileName;

                        result.Add(plugin);
                        index++;
                    }

                }
            }
            return result;
        }
    }
}