using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FreshMvvm;

namespace TestApp
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            LoadMasterDetail();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public class NavigationContainerNames
        {
            public const string AuthenticationContainer = "AuthenticationContainer";
            public const string IntroContainer = "IntroContainer";
            public const string DefaultNavigationServiceName = "DefaultNavigationServiceName";
        }

        public void LoadMasterDetail()
        {

            var masterDetailNav = new FreshMasterDetailNavigationContainer(NavigationContainerNames.DefaultNavigationServiceName);

            masterDetailNav.Init("Menu", "Menu.png");
            masterDetailNav.AddPage<MainPageModel>("Dashboard", null);

            MainPage = masterDetailNav;
        }
    }
}
