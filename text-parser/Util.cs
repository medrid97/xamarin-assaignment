using System;
using System.Collections.Generic;
using System.IO;

namespace text_parser
{
    abstract class Util
    {

        public static string FirstData = "sándor János;33;Budapest;Student;false;false;Analysis 2,Algorithms 3,Computer Graphics,Epidemology,Physics\nAnna maria dömötör;22;Budapest;Dressmaker;true;false;Analysis 2,Algorithms 3,Computer Graphics, Psychology, Geoinformatics\nPéter AuRElio;30;Dunaújváros;Photographer;true;true;Computer Graphics, Epidemology, Physics, Psychology, Geoinformatics\nPeter Big;40;budapest;Student;false;false;Analysis 2,Algorithms 3,Computer Graphics, Physics, Geoinformatics\nmatteo houdini roberto;20;Wien;Software Engineer;false;true;Computer Graphics, Epidemology, Physics, Hydrology, Human ecology,Intelligent systems, Linear algebra";

        // convert raw data into a list of users
        public static List<User> Convert(string input)
        {
            // initialize output value
            List<User> output = new List<User>();

            // useres as an array of strings
            List<string> array = new List<string>();

            array.AddRange(input.Split('\n'));

            array.ForEach(delegate (string value)
            {
                if (value != "")
                {
                    List<string> values = new List<string>();

                    // sándor János;33;Budapest;Student;false;false;Analysis 2,Algorithms 3,Computer Graphics,Epidemology,Physics
                    // ["sándor János" , "33" , "Budapest" , "Student" , "false" , "false" , "Analysis 2,Algorithms 3,Computer Graphics,Epidemology,Physics"]
                    values.AddRange(value.Split(';'));
                    
                    string
                        name = values[0].Trim(),
                        age = values[1].Trim(),
                        city = values[2].Trim(),
                        profession = values[3].Trim(),
                        married = values[4].Trim(),
                        diploma = values[5].Trim(),
                        subjects = values[6].Trim();

                    User user = new User(
                        name,
                        int.Parse(age),
                        city,
                        profession,
                        married == "true",
                        diploma == "true" ? true : false,
                        User.ConvertSubjects(subjects));

                    output.Add(user);
                }

            });

            return output;
        }

        // write sample data into text file.
        public async static void OnFirstUse()
        {
            // get the path of the file
            // methods applicable for both android and iOS
            var backingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "first.txt");

            // check if the file exists
            if (backingFile == null || !File.Exists(backingFile))
            {
                // create the file and fill it with the input value
                using (var writer = File.CreateText(backingFile))
                {
                    await writer.WriteLineAsync("first");
                };

                // write data
                Database.WriteToFile(FirstData, "data.txt");

                return;
            }
        }
    }
}
