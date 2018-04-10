using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RankMyFilmCore.Models
{
    public class FilmModel : ModelBase
    {

        public string idFilm { get; set; }

        public string title {get; set;}

        public string poster { get; set; }

        public double moyenne { get; set; }

        public int nbRank { get; set; }
    }
}
