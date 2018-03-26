using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace RankMyFilmCore
{
    public class FriendsModel :  ModelBase
    {
        [Display(Name = "idSuiveur")]
        public string idSuiveur { get; set; }

        
        [Display(Name = "idSuivi")]
        [StringLength(50)]
        public string idSuivi { get; set; }

    }
}
