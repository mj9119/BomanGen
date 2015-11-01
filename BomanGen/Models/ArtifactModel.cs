using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace BomanGen.Models
{
    public class ArtifactModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IfChecked { get; set; }
    }

    //// empty constructor
    //public ArtifactModel()
    //{
    //}

    //public ArtifactModel(int id ,string name, bool ifchecked)
    //{   
    //    Id = id;
    //    Name = name;
    //    IfChecked = ifchecked;
    //}


}