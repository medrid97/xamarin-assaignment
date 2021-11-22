using System.IO;
using System.Threading.Tasks;

namespace text_parser
{
    abstract class Database
{
        public static async void Save()
        {
            string text = "";

            foreach (User user in LiveData.UserList)
            {
                //
                text += user.Stringify() + "\n";
            }

            WriteToFile(text, "data.txt");
        }

        public static async void WriteToFile(string data,string FILE)
        {
            // get the local app storage path
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), FILE);

            // create the file and fill it with the input value
            // using... : GOOGLE IT
            using (var writer = File.CreateText(backingFile))
            {
                await writer.WriteLineAsync(data);
            }
        }

        public static async Task<string> LoadFromFile(string FILE)
        {
            // get the path of the file
            // methods applicable for both android and iOS
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), FILE);

            // check if the file exists
            if (backingFile == null || !File.Exists(backingFile))
            {
                return "";
            }

            var output = "";

            // read the content
            // using... : GOOGLE IT
            using (var reader = new StreamReader(backingFile, true))
            {
                output = await reader.ReadToEndAsync();
            }

            // return the content of the file
            return output;
        }
    }
}
