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
        [ForeignKey("ApplicationUser")]
        [Key, Column(Order = 0)]
        public string idSuiveur { get; set; }

        
        [Display(Name = "idSuivi")]
        [ForeignKey("ApplicationUser")]
        [Key, Column(Order = 1)]
        public string idSuivi { get; set; }

        [NotMapped]
        public string pseudoFollower { get; set; }

        [NotMapped]
        public string pseudoFollowed { get; set; }

    }
}
