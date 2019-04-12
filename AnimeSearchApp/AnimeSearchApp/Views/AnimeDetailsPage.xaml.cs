using JikanDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnimeSearchApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AnimeDetailsPage : ContentPage
	{
        IJikan jikan = new Jikan(true);
        private AnimeSearchEntry _anime;

        public AnimeDetailsPage(AnimeSearchEntry anime)
        {
            if (anime == null)
                throw new ArgumentNullException(nameof(anime));

            _anime = anime;

            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            BindingContext = await jikan.GetAnime(_anime.MalId);

            base.OnAppearing();
        }
    }
}