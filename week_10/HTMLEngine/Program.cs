using HTMLEngine.Models;
using HTMLEngineLibrary;
using System.Diagnostics;

namespace HTMLEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string template = @"
{{ (5 + 4) * 9}}
{{ 4 / (5.4 + 6)}}
{{ (45+6) - 4 }}
{{ (45+6) - 4 < 0}}
{{ 5 > 4}}
{{ (5 + 4) * 9 > 4 / (5.4 + 6) and ((45+6) - 4 < 0 or 5 > 4)}}";

            var proffessor = new Proffessor
            {
                FirstName = "Тимерхан",
                LastName = "Мухутдинов",
                MiddleName = "Аглямович",

                Disciplines = new List<Discipline>
                {
                    new Discipline
                    {
                        Name = "ОРИС 11-106",
                        Group = 106,
                        Students = new List<Student>
                        {
                            new Student("11-106-1", "Раиль", "Бариев", "Рустемович"),
                            new Student("11-106-2", "Михаил", "Галиуллин", "Сергеевич"),
                            new Student("11-106-3", "Камилла", "Гафарова", "Маратовна"),
                            new Student("11-106-4", "Эмиль", "Гиндуллин", "Рустемович"),
                            new Student("11-106-5", "Вадим", "Гурьянов", "Игоревич"),
                            new Student("11-106-6", "Эмиль", "Двалетшин", "Маратович"),
                            new Student("11-106-7", "Рамиль", "Зарипов", "Ринатович"),
                            new Student("11-106-8", "Наиль", "Зиннуров", "Марсович"),
                            new Student("11-106-9", "Антон", "Зотов", "Александрович"),
                            new Student("11-106-10", "Ильдар", "Исламов", "Ильдусович"),
                            new Student("11-106-11", "Рафаэль", "Ишмухаметов", "Ринатович"),
                            new Student("11-106-12", "Ольга", "Кучина", "Сергеевна"),
                            new Student("11-106-13", "Никита", "Лопухин", "Алексеевич"),
                            new Student("11-106-14", "Екатерина", "Мазуркевич", "Павловна"),
                            new Student("11-106-15", "Яков", "Мамин", "Алексеевич"),
                            new Student("11-106-16", "Глеб", "Петухов", "Александрович"),
                            new Student("11-106-17", "Александр", "Реутин", "Алексеевич"),
                            new Student("11-106-18", "Семён", "Савельев", "Алексеевич"),
                            new Student("11-106-19", "Анис", "Садыков", "Айдарович"),
                            new Student("11-106-20", "Аяз", "Сафиуллин", "Альбертович"),
                            new Student("11-106-21", "Руслан", "Хамзин", "Денисович"),
                            new Student("11-106-22", "Булат", "Хафизов", "Гусманович"),
                            new Student("11-106-23", "Андрей", "Хорьяков", "Павлович"),
                            new Student("11-106-24", "Артём", "Шаповалов", "Анатольевич"),
                            new Student("11-106-25", "Артур", "Якупов", "Булатович")
                        }
                    },
                    new Discipline
                    {
                        Name = "ОРИС 11-107",
                        Group = 107,
                        Students = new List<Student>()
                    }
                }
            };

            using (var fs = new FileStream("templates/index.template", FileMode.Open))
            {
                var outputDirectory = "templates/generated";
                var fileName = "example.html";
                new EngineHTMLService().GenerateAndSaveInDirectory(fs, outputDirectory, fileName, proffessor);
            }
        }
    }
}