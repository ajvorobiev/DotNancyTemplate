namespace DotNancyTemplate.Helpers
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    /// <summary>
    /// The <see cref="AppSettingsManager"/> takes care or reading and writing settings.
    /// </summary>
    public static class AppSettingsManager
    {
        /// <summary>
        /// Reads all app settings into a <see cref="NameValueCollection"/>.
        /// </summary>
        public static NameValueCollection ReadAllSettings()
        {
            try
            {
                return ConfigurationManager.AppSettings;
            }
            catch (ConfigurationErrorsException)
            {
                return null;
            }
        }

        /// <summary>
        /// Reads a single setting based on the key.
        /// </summary>
        /// <param name="key">The key identifying the setting.</param>
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key];
            }
            catch (ConfigurationErrorsException)
            {
                return null;
            }
        }

        /// <summary>
        /// Updates or adds a setting based on its key.
        /// </summary>
        /// <param name="key">The key identifying the setting.</param>
        /// <param name="value">The value to add or update.</param>
        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
