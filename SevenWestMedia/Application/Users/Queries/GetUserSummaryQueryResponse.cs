namespace Application.Users.Queries
{
    public class GetUserSummaryQueryResponse
    {
        // The users full name for id=42
        public List<string> UserFullName { get; set; }

        // All the users first names (comma separated) who are 23
        public string FirstName { get; set; }

        // The number of genders per Age, displayed from youngest to oldest
        public List<GenderPerAge> GenderPerAges { get; set; }
    }

    public class GenderPerAge
    {
        public int Age { get; set; }
        public int Female {get;set;}
        public int Male { get;set;}

    }
}
