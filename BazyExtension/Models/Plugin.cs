namespace BazyExtension.Models
{
    public class Plugin
    {
        private string _Name = string.Empty;

        /// <summary>
        /// Plugin index ordere from BazyExtension.xml file.
        /// </summary>
        public int Index { get; set; } = -1;

        /// <summary>
        /// Plugin name. This name will be used to display friendly plugin name in Plugin Manager.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Location for plugin file. If file with filename starting from TableName is missing, Plugin Manager will try to check this attribute and load Plugin from provided path in this FileName attribute.
        /// </summary>
        public string FileName { get; set; } = string.Empty;


        /// <summary>
        /// Plugin loaded from dll file by reflection mechanism.
        /// </summary>
        public PluginControl Control { get; set; }
    }
}
