using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace RankMyFilmCore
{
    public class FilmModel : ModelBase
    {
        [Display(Name = "title")]
        [Required(ErrorMessageResourceName = "required_name")]
        [StringLength(50, ErrorMessage = "Le titre doit contenir {1} caractères maximum.")]
        public string Title { get; set; }
    }
}
