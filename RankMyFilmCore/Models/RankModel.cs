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
        [StringLength(50)]
        [ForeignKey("User")]
        public int idUser { get; set; }

        [Display(Name = "Film")]
        [StringLength(50)]
        [ForeignKey("Film")]
        public int idFilm { get; set; }


        [Display(Name = "Vote")]
        [StringLength(255)]
        public int Vote { get; set; }

        [Display(Name = "Déjà vu")]
        public bool vu { get; set; }


        [Display(Name = "Commentaire")]
        [StringLength(255)]
        public string Commentaire { get; set; }

    }
}
