using System;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace text_parser
{
    public partial class MainPage : ContentPage
    {
        private int index;

        public MainPage(int index)
        {
            InitializeComponent();

            this.index = index;

            InitUI();
        }

        // initialize User interface with appropriate values
        private void InitUI()
        {
            if (index != -1)
            {
                Title.Text = "Edit User";

                User user = LiveData.UserList[index];

                name.Text = user.Name;
                age.Text = user.Age.ToString();
                city.Text = user.City;
                profession.Text = user.Profession;
                subjects.Text = user.SubString;
                married.IsChecked = user.IsMarried;
                diploma.IsChecked = user.HasDiploma;
            }
            else
            {
                Title.Text = "Create User";
            }
        }

        // return to list page
        private async void OnListClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        // data writing button : OnClick event
        private async void OnSaveClick(object sender, EventArgs e)
        {
            ValidateAndCommit();
        }

        private async void ValidateAndCommit()
        {
            // Name should start with Capital letter, and after each space the next letter should be capitalized
            Regex nameReg = new Regex("^([A-Z]([\\w]){1,})((\\s)([A-Z]([\\w]){1,})){0,}$");

            // Age should be between 00 and 99
            Regex ageReg = new Regex("^[0-9]{1,2}$");

            // City should start with a Capital letter
            Regex cityReg = new Regex("^([A-Z]([\\w\\W]){1,})$");

            // Profession should start with a Capital letter
            Regex proReg = new Regex("^([A-Z]([\\w\\W]){1,})$");

            // Subject should start with a Capital letter and comma seperated.
            Regex subReg = new Regex("^([A-Z]([\\w\\s]){1,})((,){1}( ){0,}[A-Z]([\\w\\s]){1,}){0,}$");

            // validate values
            // ToDo : subject
            if (
                !await TestValue(
                    name.Text,
                    "Name",
                    nameReg,
                    "The first letter and the ones after each space should be capitalized, Only one space allowed between words.")
                || !await TestValue(
                    age.Text,
                    "Age",
                    ageReg,
                    "Age value should be between 0 and 99")
                || !await TestValue(
                    city.Text,
                    "City",
                    cityReg,
                    "City name should be capitalized.")
                || !await TestValue(
                    profession.Text,
                    "Profession",
                    proReg,
                    "Profession should be capitalized")
                || !await TestValue(
                    subjects.Text,
                    "Subjects",
                    subReg,
                    "Subjects should be capitalized and comma seperated."))
            {
                return;
            }
            else
            {
                User user = BuildUser();

                // if user does not exists
                if (index == -1)
                {
                    LiveData.UserList.Add(user);
                    // Console.WriteLine($"User Name: {user.Name}, Age: {user.Age}, City: {user.City}, Married: {user.IsMarried}, Graduated: {user.HasDiploma}, Subects:{user.SubString}");
                }
                // user exists
                else
                {
                    LiveData.UserList.RemoveAt(index);

                    if (index <= LiveData.UserList.Count)
                    {
                        LiveData.UserList.Insert(index, user);
                    }
                    else
                    {
                        LiveData.UserList.Add(user);
                    }

                }

                Database.Save();

                ResetValues();

                await DisplayAlert("Saved", "Data successfully saved to the App local storage", "Ok");

                if (index != -1)
                    await Navigation.PopModalAsync();

            }

        }

        // Display an alert if a value is invalid.
        private async void InvalidDataAlert(string name, string value, string should)
        {
            await DisplayAlert($"Invalid Field : {name}", $"The field '{name}' has an invalid value.\n{should}", "Ok");
        }

        // Test the value with a given regEx and display an alert telling the user how it should be formatted.
        private async Task<bool> TestValue(string value, string name, Regex regEx, string should)
        {
            if (!regEx.IsMatch(value.Trim()))
            {
                InvalidDataAlert(name, value.Trim(), should);
                return false;
            }

            return true;
        }

        // reset input values with empty strings 
        private void ResetValues()
        {
            name.Text = "";
            age.Text = "";
            city.Text = "";
            profession.Text = "";
            married.IsChecked = false;
            diploma.IsChecked = false;
            subjects.Text = "";
        }

        // Create a user from the currently input values 
        private User BuildUser()
        {
            return new User(
                name.Text.Trim(),
                int.Parse(age.Text.Trim()),
                city.Text.Trim(),
                profession.Text.Trim(),
                married.IsChecked,
                diploma.IsChecked,
                User.ConvertSubjects(subjects.Text.Trim())
           );
        }
    }
}
