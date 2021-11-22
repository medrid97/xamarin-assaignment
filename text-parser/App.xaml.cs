using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace text_parser
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            LiveData.UserList = new ObservableCollection<User>();

            // Write the first data sample
            Util.OnFirstUse();

            // 
            SetData();

            // Launch the ListPage
            MainPage = new ListPage();
        }

        // Retrieve data from local storage and load it into the live data.
        private async void SetData()
        {
            // uncomment if data gets corrupted for some reasons
            // await WriteData.WriteToFile("", "data.txt")

            string rawData = await Database.LoadFromFile("data.txt");

            Util.Convert(rawData).ForEach((User user) =>
            {
                LiveData.UserList.Add(user);
            });
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
    }
}
