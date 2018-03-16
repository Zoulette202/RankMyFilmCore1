using RankMyFilmCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace RankMyFilmCore
{
    public class UserModel : ModelBase
    {
        public virtual ApplicationUser User { get; set; }

    }
}
