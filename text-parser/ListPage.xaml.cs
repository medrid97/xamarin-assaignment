using System;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace text_parser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();

            // Data binding
            userList.ItemsSource = LiveData.UserList;
            LiveData.UserList.CollectionChanged += UpdateQuestions;
        }

        // Update questions when the LiveData changes.
        private void UpdateQuestions(object sender, NotifyCollectionChangedEventArgs e)
        {
            UserWithMostSubjects.Text = UserHavingMostSubjects();
            UsersFromBudapest.Text = HowManyUsersFromBudapest().ToString();
            UserOver30.Text = UsersOverTheAge30();
        }

        // Remove a given user
        private async void RemoveUser(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Delete User", "Are you sure you want to delete this user?", "Confirm", "Cancel");

            if (confirm)
            {
                // get the targeted user
                var u = sender as Button;

                // delete the user
                LiveData.UserList.Remove((User)u.CommandParameter);

                // save data to the text file
                Database.Save();
            }
 
        }

        // Open the add user page
        private async void AddUser(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage(-1));
        }

        // Open the edit page
        private async void EditUser(object sender, EventArgs e)
        {
            // get the targeted user
            var u = sender as Button;

            // get the index of the user
            int index = LiveData.UserList.IndexOf((User)u.CommandParameter);

            // 
            await Navigation.PushModalAsync(new MainPage(index));
        }

        // Return the name of the user with most subjects
        private string UserHavingMostSubjects()
        {
            if (LiveData.UserList.Count > 0)
            {
                User user = LiveData.UserList[0];
                foreach (User u in LiveData.UserList)
                {
                    if (u.Subjects.Count > user.Subjects.Count)
                    {
                        user = u;
                    }
                }

                return user.Name;
            }

            return "None";
        }

        // Return the list (as string) of users over the age of 30
        private string UsersOverTheAge30()
        {
            string users = "";
            foreach (User u in LiveData.UserList)
            {
                if (u.Age > 30)
                {
                    users += u.Name+",";
                }
            }

            if (users == "")
            {
                return "None";
            } else
            {
                users.Remove(users.Length - 1, 1);
                return users;
            }
        }

        // Return the number of users from budapest
        private int HowManyUsersFromBudapest()
        {
            int number = 0;
            foreach (User u in LiveData.UserList)
            {
                if (u.City == "Budapest")
                {
                    number ++;
                }
            }

            return number;
        }

        private async void ShowRawData(object sender, EventArgs e)
        {
            // retrieve data from file
            string data = await Database.LoadFromFile("data.txt");

            // display alert message
            await DisplayAlert("Raw Data",data,"Ok");
        }
    }
}