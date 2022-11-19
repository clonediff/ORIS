namespace HTMLEngine.Models
{
    public class Proffessor
    {
        //public string Result { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public List<Discipline> Disciplines { get; set; }
    }

    public record Student(string StudentId, string FirstName, string LastName, string MiddleName);

    public class Discipline
    {
        public string Name { get; init; }
        public int Group { get; init; }
        public List<Student> Students { get; init; }
    }
}
