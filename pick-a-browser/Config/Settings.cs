using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.NetworkInformation;
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

namespace pick_a_browser.Config
{
    public class Settings
    {
        public Settings(Browsers browsers, Transformations transformations, List<Rule> rules, UpdateCheck updateCheck)
        {
            Browsers = browsers;
            Transformations = transformations;
            Rules = rules;
            UpdateCheck = updateCheck;
        }
        public Browsers Browsers { get; }
        public Transformations Transformations { get; }
        public List<Rule> Rules { get; }
        public UpdateCheck UpdateCheck { get; set; }

        // TODO - add tranformation rules (link shorteners, regex rules)
        public static async Task<Settings> LoadAsync()
        {
            return await SettingsSerialization.LoadFromFileAsync(GetSettingsFilename());
        }

        private static string GetSettingsFilename()
        {
            // If PICK_A_BROWSER_CONFIG is set, use it
            var settingsFilename = Environment.GetEnvironmentVariable("PICK_A_BROWSER_CONFIG");
            if (!string.IsNullOrEmpty(settingsFilename))
                return settingsFilename;

            // Try user profile settings file first if it exists...
            var profilePath = Environment.GetEnvironmentVariable("USERPROFILE");
            if (string.IsNullOrEmpty(profilePath))
                throw new Exception("USERPROFILE not set");
            settingsFilename = Path.Join(profilePath, "pick-a-browser-settings.json");
            if (!string.IsNullOrEmpty(settingsFilename) && File.Exists(settingsFilename))
                return settingsFilename;

            // Lastly, look for settings next to the app
            settingsFilename = Path.Join(AppContext.BaseDirectory, "pick-a-browser-settings.json");
            return settingsFilename;
        }
    }
}
