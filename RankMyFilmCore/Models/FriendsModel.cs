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
        [StringLength(50)]
        public int idFriends { get; set; }

        [Display(Name = "idUserFirst")]
        [StringLength(50)]
        public int idUserFirst { get; set; }

        
        [Display(Name = "idUserSecond")]
        [StringLength(50)]
        public int idUserSecond { get; set; }

    }
}
