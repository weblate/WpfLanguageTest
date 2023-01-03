using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Digimezzo.Foundation.Core.Settings;
using Microsoft.Extensions.DependencyInjection;
using WpfLanguageTest.I18n;

namespace WpfLanguageTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.Current.Services.GetRequiredService<II18nService>().LanguagesChanged += (_, __) => this.GetLanguagesAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => App.Current.Services.GetRequiredService<II18nService>().ApplyLanguageAsync("CS", true));
        }

        private async void GetLanguagesAsync()
        {
            List<Language> languagesList = App.Current.Services.GetRequiredService<II18nService>().GetLanguages();
            ObservableCollection<Language> localLanguages = new ObservableCollection<Language>();

            await Task.Run(() =>
            {
                foreach (Language lang in languagesList)
                {
                    localLanguages.Add(lang);
                }
            });

            //this.Languages = localLanguages;
            Language tempLanguage = null;

            await Task.Run(() =>
            {
                string savedLanguageCode = SettingsClient.Get<string>("Appearance", "Language");

                if (!string.IsNullOrEmpty(savedLanguageCode))
                {
                    tempLanguage = App.Current.Services.GetRequiredService<II18nService>().GetLanguage(savedLanguageCode);
                }

                // If SelectedLanguage is null (e.g. when the user deletes a language file), select the default language.
                if (tempLanguage == null)
                {
                    tempLanguage = App.Current.Services.GetRequiredService<II18nService>().GetDefaultLanguage();
                }
            });

            //this.selectedLanguage = tempLanguage;
            //RaisePropertyChanged(nameof(this.SelectedLanguage));
        }
    }
}
