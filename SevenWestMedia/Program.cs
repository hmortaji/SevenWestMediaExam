using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SevenWestMedia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DoTasks().Wait();
        }

        /// <summary>
        /// Perform Tasks in order.
        /// </summary>
        /// <returns></returns>
        public static async Task DoTasks()
        {
            var filePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\example_data.json";
            // Fetch People from Json File
            var people = (await new JsonControl<Person>().GetData(await File.ReadAllTextAsync(filePath))).ToList();

            //Task 1
            foreach (var item in people.GetPerson(31))
                Console.WriteLine(item);
            //Task 2
            Console.WriteLine(string.Join(',', people.Where(x => x.Age == 23)));
            //Task 3
            foreach (var (age, male, female) in people.GetGenderByAgeGroup())
                Console.WriteLine($"Age: {age} Female: {female} Male: {male}");
        }
    }

    public interface IJsonControl<T>
    {
        Task<IEnumerable<T>> GetData(string jsonData);
    }

    /// <summary>
    /// Represents the functionality to deserialize json object. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonControl<T>: IJsonControl<T>
    {
        /// <summary>
        /// Deserialize json into <c>T</c>.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetData(string jsonData)
        {
            return await Task.Run(() => JsonConvert.DeserializeObject<List<T>>(jsonData));
        }
    }

    /// <summary>
    /// Person Information.
    /// </summary>
    public class Person
    {
        [JsonProperty(PropertyName = "id")] public int Id { get; set; }

        [JsonProperty(PropertyName = "first")] public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last")] public string LastName { get; set; }

        [JsonProperty(PropertyName = "age")] public int Age { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        /// <inheritdoc />
        public override string ToString() => $"{FirstName} {LastName}";
    }

    /// <summary>
    /// Extension class
    /// </summary>
    public static class PeopleExtension
    {
        /// <summary>
        /// gets persons string by id.
        /// </summary>
        /// <param name="people"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetPerson(this IEnumerable<Person> people, int id)
        {
            return people.Where(x => x.Id == id).Select(x => x.ToString());
        }

        /// <summary>
        /// Gets the ordered group of people based on age and its total gender in group.
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        public static IEnumerable<(int Age, int Male, int Female)> GetGenderByAgeGroup(this IEnumerable<Person> people)
        {
            return people.GroupBy(x => x.Age).OrderBy(x => x.Key).Select(x =>
                (x.Key, x.Count(m => m.Gender == "M"), x.Count(m => m.Gender == "F")));
        }
    }
}
