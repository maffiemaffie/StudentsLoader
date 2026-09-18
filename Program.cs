using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace StudentsLoader
{
  static class Program
  {
    static void Main(string[] args)
    {
      // Open and read JSON file
      // https://learn.microsoft.com/en-us/dotnet/api/system.io.file.readalltext?view=net-10.0
      if (args.Length < 1)
      {
        Console.WriteLine("No file parameter");
        return;
      }
      if (!File.Exists(args[0]))
      {
        Console.WriteLine($"File not found: {args[0]}");
        return;
      }

      string json;

      try
      {
        json = File.ReadAllText(args[0]);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Failed to load file: {ex.Message}");
        return;
      }


      // Configure serializer to load enums by name
      // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/customize-properties
      JsonSerializerOptions options = new()
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
          new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
      };

      // Deserialize students
      List<Student>? unvalidatedStudents;

      try
      {
        unvalidatedStudents = JsonSerializer.Deserialize<List<Student>>(json, options);
      }
      catch (JsonException ex)
      {
        Console.WriteLine($"JSON is malformed, cannot load data: {ex.Message}");
        return;
      }

      // Deserialize returns null if the input is "null"
      if (unvalidatedStudents is null || unvalidatedStudents.Count == 0)
      {
        Console.WriteLine("JSON contains no students");
        return;
      }

      // Validate entries
      List<Student> validStudents = unvalidatedStudents.FindAll((student) => student.ValidateStudent().Count == 0);
      List<Student> invalidStudents = unvalidatedStudents.FindAll((student) => student.ValidateStudent().Count > 0);

      Console.WriteLine($"Successfully loaded {validStudents.Count} valid students");
      if (invalidStudents.Count == 0) return;

      Console.WriteLine($"The following {invalidStudents.Count} students could not be loaded:");

      foreach (Student student in invalidStudents)
      {
        List<string> errors = student.ValidateStudent();

        Console.WriteLine($"\n{student}");
        foreach (string error in errors)
        {
          Console.WriteLine(error);
        }
      }
    }

    // sample student class with some basic fields
    public class Student
    {
      public enum AcademicYear
      {
        Freshman,
        Sophomore,
        Junior,
        Senior,
        Graduate
      }

      public required string FirstName { get; set; }
      public required string LastName { get; set; }
      public required int Age { get; set; }
      [JsonPropertyName("academicYear")]
      public required AcademicYear Year { get; set; }

      [JsonConstructor]
      public Student(string firstName, string lastName, int age, AcademicYear year)
      {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Year = year;
      }

      // validates some fields and returns a list of errors
      public List<string> ValidateStudent()
      {
        List<string> errors = new();

        // example validators hardcoded. these would depend on the specifications for the app.
        if (FirstName.Length == 0 || FirstName.Length > 15)
        {
          errors.Add("First name: Invalid length");
        }
        if (LastName.Length == 0 || LastName.Length > 15)
        {
          errors.Add("Last name: Invalid length");
        }
        if (Age < 0 || Age > 100)
        {
          errors.Add("Age: Cannot be less than 0 or greater than 100");
        }

        string patternInvalidCharacters = @"[^a-zA-Z-_\s]";
        if (Regex.IsMatch(FirstName, patternInvalidCharacters))
        {
          errors.Add("First name: Contains invalid characters");
        }
        if (Regex.IsMatch(LastName, patternInvalidCharacters))
        {
          errors.Add("Last name: Contains invalid characters");
        }

        return errors;
      }

      public override string ToString()
      {
        return FirstName + " " + LastName + ": " + Year + ", age " + Age;
      }
    }
  }
}