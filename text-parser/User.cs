using System.Collections.Generic;

class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
    public string Profession { get; set; }
    public bool IsMarried { get; set; }
    public bool HasDiploma { get; set; }
    public List<string> Subjects { get; set; }
    public string SubString { get; set; }

    public User(string name, int age, string city, string profession, bool isMarried, bool hasDiploma, List<string> subjects)
    {
        // capitalized
        Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());

        // 0 - 99
        Age = age;

        // capitalized
        City = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(city.ToLower());

        // Capitalized
        Profession = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(profession.ToLower());

        // true - false
        IsMarried = isMarried;

        // true - false
        HasDiploma = hasDiploma;

        // comma seperated 
        Subjects = subjects;

        // list of subjects formatted to the wanted pattern
        SubString = SubjectsToString();

    }

    // return a string with the needed pattern.
    // x;y;w;z...
    public string Stringify()
    {
        return $"{Name};{Age};{City};{Profession};{IsMarried.ToString().ToLower()};{HasDiploma.ToString().ToLower()};{SubjectsToString()}";
    }

    // return a string containing a list of subject in the correct pattern.
    // x,y,w,z
    private string SubjectsToString()
    {
        string output = "";

        Subjects.ForEach(delegate (string subject)
        {
            output += subject + ",";
        });

        // remove last comma
        if (output.EndsWith(","))
        {
            output = output.Remove(output.Length - 1, 1);
        }

        return output;
    }

    // "Physic, Math" => ["Physics","Math"]
    public static List<string> ConvertSubjects(string input)
    {
        List<string> holder = new List<string>();

        holder.AddRange(input.Split(','));

        return holder;
    }
}

