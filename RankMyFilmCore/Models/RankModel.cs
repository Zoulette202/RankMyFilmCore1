using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace RankMyFilmCore
{
    public class RankModel : ModelBase
    {
        [Display(Name = "idUser")]
        [StringLength(50)]
        [ForeignKey("User")]
        public int idUser { get; set; }

        [Display(Name = "idFilm")]
        [StringLength(50)]
        [ForeignKey("Film")]
        public int idFilm { get; set; }


        [Display(Name = "vote")]
        [StringLength(1)]
        public int Vote { get; set; }


        [Display(Name = "Commentaire")]
        [StringLength(255)]
        public string Commentaire { get; set; }

    }
}
