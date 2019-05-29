using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;

namespace SevenWestMedia.Test
{
    public class UnitTestMain
    {
        List<Person> People = new List<Person>
        {
            new Person {Id = 53, FirstName = "Bill", LastName = "Bryson", Age = 23, Gender = "M"},
            new Person {Id = 62, FirstName = "John", LastName = "Travolta", Age = 54, Gender = "M"},
            new Person {Id = 41, FirstName = "Frank", LastName = "Zappa", Age = 23, Gender = "T"},
            new Person {Id = 31, FirstName = "Jill", LastName = "Scott", Age = 66, Gender = "Y"},
            new Person {Id = 31, FirstName = "Anna", LastName = "Meredith", Age = 66, Gender = "Y"},
            new Person {Id = 31, FirstName = "Janet", LastName = "Jackson", Age = 66, Gender = "F"}
        };

        [Fact]
        public void TestGetPerson()
        {
            var result = People.GetPerson(53);
            Assert.Equal(new List<string> {"Bill Bryson"}, result);
        }

        [Fact]
        public void TestGetGenderByAgeGroup()
        {
            var result = People.GetGenderByAgeGroup();

            Assert.Equal(new List<(int, int, int)>
            {
                (23, 1, 0), (54, 1, 0), (66, 0, 1)
            }, result);
        }

        [Fact]
        public async void TestGetData()
        {
            var data = await new JsonControl<Person>().GetData(
                "[{ \"id\": 53, \"first\": \"Bill\", \"last\": \"Bryson\", \"age\":23, \"gender\":\"M\" },\r\n{ \"id\": 62, \"first\": \"John\", \"last\": \"Travolta\", \"age\":54, \"gender\":\"M\" },\r\n{ \"id\": 41, \"first\": \"Frank\", \"last\": \"Zappa\", \"age\":23, gender:\"T\" },\r\n{ \"id\": 31, \"first\": \"Jill\", \"last\": \"Scott\", \"age\":66, gender:\"Y\" },\r\n{ \"id\": 31, \"first\": \"Anna\", \"last\": \"Meredith\", \"age\":66, \"gender\":\"Y\" },\r\n{ \"id\": 31, \"first\": \"Janet\", \"last\": \"Jackson\", \"age\":66, \"gender\":\"F\" }]");

            Assert.Equal(JsonConvert.SerializeObject(People), JsonConvert.SerializeObject(data));
        }
    }
}


