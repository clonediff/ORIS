using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer2.Models
{
    public enum FilmType
    {
        Movie,
        Series
    }

    public class Film
    {
        public int Id { get; }
        public FilmType Type { get; }
        public string   Name { get; }
        public int Year { get; }
    }
}
