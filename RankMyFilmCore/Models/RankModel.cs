using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace RankMyFilmCore
{
    public class RankModel : ModelBase
    {
        [Display(Name = "Utilisateur")]
        [ForeignKey("User")]
        public int idUser { get; set; }

        [Display(Name = "Film")]
        [ForeignKey("Film")]
        public int idFilm { get; set; }


        [Display(Name = "Vote")]
        public int Vote { get; set; }

        [Display(Name = "Déjà vu")]
        public bool vu { get; set; }


        [Display(Name = "Commentaire")]
        [StringLength(255)]
        public string Commentaire { get; set; }

    }
}
