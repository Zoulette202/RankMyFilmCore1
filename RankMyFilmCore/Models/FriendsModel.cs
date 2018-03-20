using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace RankMyFilmCore
{
    public class FriendsModel :  ModelBase
    {

        [Display(Name = "idFriends")]
        public int idFriends { get; set; }

        [Display(Name = "idSuiveur")]
        public int idSuiveur { get; set; }

        
        [Display(Name = "idSuivi")]
        [StringLength(50)]
        public int idSuivi { get; set; }

    }
}
