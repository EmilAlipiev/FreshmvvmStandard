using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace FreshMvvm
{
    public class FreshTabbedNavigationContainer : Xamarin.Forms.TabbedPage, IFreshNavigationService
    {
        private readonly List<Xamarin.Forms.Page> _tabs = new List<Xamarin.Forms.Page>();
        public IEnumerable<Xamarin.Forms.Page> TabbedPages => _tabs;

        public FreshTabbedNavigationContainer(bool useBottomToolBar = false, Color? barTextColor = null, Color? barBackgroundColor = null, bool enableUwpIcons = false, Size headerIconSize = default) : this(Constants.DefaultNavigationServiceName)
        {
            if (useBottomToolBar)
            {
                On<Android>().SetToolbarPlacement(Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ToolbarPlacement.Bottom);
            }

            if (enableUwpIcons)
            {
                On<Windows>().SetHeaderIconsEnabled(true);
                On<Windows>().SetHeaderIconsSize(headerIconSize);
            }
            if (barBackgroundColor.HasValue)
                BarBackgroundColor = barBackgroundColor.Value;

            if (barTextColor.HasValue)
                BarTextColor = barTextColor.Value;

        }

        public FreshTabbedNavigationContainer(string navigationServiceName)
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
        }

        protected void RegisterNavigation()
        {
            FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
        }

        public virtual Xamarin.Forms.Page AddTab<T>(string title, string icon, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            _tabs.Add(page);
            var navigationContainer = CreateContainerPageSafe(page);
            navigationContainer.Title = title;
            if (!string.IsNullOrWhiteSpace(icon))
                navigationContainer.IconImageSource = icon;
            Children.Add(navigationContainer);
            return navigationContainer;
        }

        internal Xamarin.Forms.Page CreateContainerPageSafe(Xamarin.Forms.Page page)
        {
            if (page is NavigationPage || page is Xamarin.Forms.MasterDetailPage || page is Xamarin.Forms.TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        protected virtual Xamarin.Forms.Page CreateContainerPage(Xamarin.Forms.Page page)
        {
            return new NavigationPage(page);
        }

        public System.Threading.Tasks.Task PushPage(Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return this.CurrentPage.Navigation.PushModalAsync(CreateContainerPageSafe(page));
            return this.CurrentPage.Navigation.PushAsync(page);
        }

        public System.Threading.Tasks.Task PopPage(bool modal = false, bool animate = true)
        {
            if (modal)
                return this.CurrentPage.Navigation.PopModalAsync(animate);
            return this.CurrentPage.Navigation.PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return this.CurrentPage.Navigation.PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            foreach (var page in this.Children)
            {
                if (page is NavigationPage)
                    ((NavigationPage)page).NotifyAllChildrenPopped();
            }
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            if (page > -1)
            {
                CurrentPage = this.Children[page];
                var topOfStack = CurrentPage.Navigation.NavigationStack.LastOrDefault();
                if (topOfStack != null)
                    return Task.FromResult(topOfStack.GetModel());

            }
            return null;
        }
    }
}

