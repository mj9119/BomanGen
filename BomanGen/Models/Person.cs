using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BomanGen.Models
{
    public class Person
    {
        public string Name { get; set; }
        public string Mother { get; set; }
        public string Father { get; set; }               
        public string MaidenName { get; set; }
        public string BloodLine { get; set; }
        public string Description { get; set; }
        public string ProcessingForWhichParent { get; set; }        

        //public List<person> Children { get; set; }

        // empty constructor
        public Person()
        {

        }
        
        public Person(string name, string mother, string father,string maidenName,string bloodLine,string description, string processingforwhichparent)
        {
            Name = name;
            Mother = mother;
            Father = father;
            MaidenName = maidenName;            
            BloodLine = bloodLine;
            Description = description;
            ProcessingForWhichParent = processingforwhichparent;
        }

    }
}