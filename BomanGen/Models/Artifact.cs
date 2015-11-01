using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BomanGen.Models
{
    [Table("tblArtifacts")]
    public class Artifact
    {
        [Key, Column(Order = 0)]        
        public string FileName { get; set; }
        [Key, Column(Order = 1)]
        public string Name { get; set; }        
        public string ArtifactType { get; set; }               
        public string HeadStone { get; set; }
        public string Description { get; set; }
        //public int Id { get; set; }
        //public bool   Checked { get; set; }

        // empty constructor
        public Artifact()
        {
        }

        public Artifact(string filename, string name, string artifactType, string headStone, string description)
        {            
            FileName = filename;
            Name = name;
            ArtifactType = artifactType;            
            HeadStone = headStone;
            Description = description;
            //Id = id;
            //Checked = ifchecked;
        }

    }    
}