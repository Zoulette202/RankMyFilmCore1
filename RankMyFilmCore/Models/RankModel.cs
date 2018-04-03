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
        [ForeignKey("ApplicationUser")]
        [Key, Column(Order =0)]
        public string idUser { get; set; }

        
        [Display(Name = "Film")]
        [ForeignKey("Film")]
        [Key, Column(Order =1)]
        public string idFilm { get; set; }


        [Display(Name = "Note")]
        [Range(0.00,5.00, ErrorMessage ="La note doit être entre {1} et {2}")]
        public int Vote { get; set; }

        [Display(Name = "Déjà vu")]
        public bool vu { get; set; }

        [Display(Name = "Poster")]
        public string poster { get; set; }


        [Display(Name = "Title")]
        public string Title { get; set; }


        public DateTime dateCréation { get; set; }

        [Display(Name = "Commentaire")]
        [StringLength(255)]
        public string Commentaire { get; set; }

        [NotMapped]
        public double moyenneByFriend { get; set; }

        [NotMapped]
        public double moyenneByAllUser { get; set; }

        [NotMapped]
        public string pseudo { get; set; }
    }
}
