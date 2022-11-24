using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer2.Models
{
    public enum Role
    {
        Actor,
        Director,
        Screenwriter,
        Producer
    }

    public class StuffToFilm
    {
        public int StuffId { get; }
        public int FilmId { get; }
        public Role Role { get; }
    }
}
