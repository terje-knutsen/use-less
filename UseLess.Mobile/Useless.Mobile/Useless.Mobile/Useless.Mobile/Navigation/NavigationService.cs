﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Useless.Framework;
using Useless.Mobile;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Useless.Forms.Navigation
{
    internal sealed class NavigationService : INavigationService
    {
        readonly IDictionary<Type, Type> map = new Dictionary<Type, Type>();
        public void RegisterViewMapping(Type viewModel, Type view)
        {
            map.Add(viewModel, view);
        }
        public INavigation XamarinFormsNav { get; set; }
        public bool CanGoBack => XamarinFormsNav.NavigationStack != null && XamarinFormsNav.NavigationStack.Count > 0;

        public event PropertyChangedEventHandler CanGoBackChanged;

        public void ClearBackStack()
        {
            if (XamarinFormsNav.NavigationStack.Count < 2)
                return;
            for (var i = 0; i < XamarinFormsNav.NavigationStack.Count; i++)
            {
                XamarinFormsNav.RemovePage(XamarinFormsNav.NavigationStack[i]);
            }
        }

        public async Task GoBack()
        {
            if (CanGoBack)
            {
                await XamarinFormsNav.PopAsync(true);
                OnCanGoBackChanged();
            }
        }
        public async Task NavigateTo<TVM>() where TVM : BaseViewModel
        {
            await NavigateToView(typeof(TVM));
            if (XamarinFormsNav.NavigationStack.Last().BindingContext is BaseViewModel vm)
            {
                vm.Init();
            }
        }
        public async Task NavigateTo<TVM,K>(K id) where TVM : GenericBaseViewModel<K>
        {
            await NavigateToView(typeof(TVM));
            if (XamarinFormsNav.NavigationStack.Last().BindingContext is GenericBaseViewModel<K> vm)
                vm.Init(id);
        }

        public async Task NavigateToUri(Uri uri)
        {
            if (uri == null)
                throw new ArgumentException("Invalid URI");
            await Launcher.OpenAsync(uri);
        }

        public void RemoveLastView()
        {
            if (XamarinFormsNav.NavigationStack.Count < 2)
                return;
            var lastView = XamarinFormsNav.NavigationStack[XamarinFormsNav.NavigationStack.Count - 2];
            XamarinFormsNav.RemovePage(lastView);
        }
        private void OnCanGoBackChanged()
        {
            CanGoBackChanged?.Invoke(this, new PropertyChangedEventArgs("CanGoBack"));
        }
        private async Task NavigateToView(Type type)
        {
            if (!map.TryGetValue(type, out Type viewType))
                throw new ArgumentException($"No view found in view mapping for {nameof(type)}");
            try
            {
                var constructor = viewType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(dc => !dc.GetParameters().Any());
                var view = constructor.Invoke(null) as Page;
                var vm = ((App)Application.Current).Kernel.GetService(type);
                view.BindingContext = vm;
                await XamarinFormsNav.PushAsync(view, true);
            }
            catch (Exception e)
            {
                var test = e.Message;
            }
        }    
    }
}
