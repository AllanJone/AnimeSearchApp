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
    public partial class AnimesPage : ContentPage
    {
        private IJikan jikan = new Jikan(true);

        private static readonly int MinLength = 3;

        private BindableProperty IsSearchingProperty =
            BindableProperty.Create("IsSearching", typeof(bool), typeof(AnimesPage), false);

        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
        }
        public AnimesPage()
        {
            BindingContext = this;

            InitializeComponent();
        }

        async void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null || e.NewTextValue.Length < MinLength)
                return;

            await FindAnimes(animeString: e.NewTextValue);
        }

        async Task FindAnimes(string animeString)
        {
            try
            {
                IsSearching = true;
                var animeSearchEntry = await jikan.SearchAnime(animeString);
                var animes = new List<AnimeSearchEntry>(animeSearchEntry.Results);
                animesListView.ItemsSource = animes;
                animesListView.IsVisible = animes.Any();
                notFound.IsVisible = !animesListView.IsVisible;
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Could not retrive the lsit of animes", "OK");
            }
            finally
            {
                IsSearching = false;
            }
        }

        async void OnAnimeSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var anime = e.SelectedItem as AnimeSearchEntry;

            animesListView.SelectedItem = null;

            await Navigation.PushAsync(new AnimeDetailsPage(anime));

        }
    }
}