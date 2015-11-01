using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BomanGen.Models;

namespace BomanGen.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //List<Person> myNodeList = new List<Person>();
        List<Person> nodeList = new List<Person>();

        StringBuilder treeStringbuilder = new StringBuilder("<div class=\"tree\">        </div> ");   
        //StringBuilder treeStringbuilder = new StringBuilder("<div class=\"tree\" style=\"border-style:solid;border-width:5px; \">        </div> ");

        public ActionResult Index()
        {
            ViewBag.Message = "This is current ViewBag.Message.";                         
            
            return View();
        }

        public ActionResult Tree(string topOfTree)
        {
            ViewBag.Message = "             Choose a Person by Name";

            //commented code below works!
            //string myString = "<div class=\"tree\"> <ul>" +
            //"<li> <a href=\"#\">Joseph C Boman / Marilyn Whelehan</a> " +
            //    "<ul>  <li> <a href=\"#\">Jean</a> " +         

            //        "<ul> <li> <a href=\"#\">Kristen</a></li>   " +
            //        "<li> <a href=\"#\">Allie</a> " +

            //"<ul> <li> <a href=\"#\">Ava</a> " +
            //"</li> </ul> </li> " + 
            //    "</ul> </li>" +
            //"</li> </ul> " + 
            //"</div> ";
            //StringBuilder myStringbuilder = new StringBuilder(myString);            
            //ViewBag.BuildTree = myStringbuilder.ToString();

            // commented code below works!                        
            //Person
            //n = new Person("Gary Brinker", "Gary MomHere", "Ron Brinker"); nodeList.Add(n);
            //n = new Person("Jayden Cagle", "Sue Boman", "Chris Cagle"); nodeList.Add(n);            
            //n = new Person("Joseph C. Boman", "", ""); nodeList.Add(n);
            //n = new Person("Allison Brinker - Boman", "Jean Brinker - Boman", "Gary Brinker"); nodeList.Add(n);
            //n = new Person("Jean Brinker - Boman", "Marilyn Whelehon", "Joseph C. Boman"); nodeList.Add(n);
            //n = new Person("Kristen Brinker - Boman", "Jean Brinker - Boman", "Gary Brinker"); nodeList.Add(n);
            //n = new Person("Ava Brinker - Boman", "Alley Brinker - Boman", "Fname Lname"); nodeList.Add(n);            
            //n = new Person("Sue Boman", "Marilyn Whelehon", "Joseph C. Boman"); nodeList.Add(n);
            //n = new Person("Joe Boman jr.", "Marilyn Whelehon", "Joseph C. Boman"); nodeList.Add(n);

            // Populate the list of Persons
            nodeList = PersonDB.GetPersons();

            bool firstChildIndicator = true;

            //Person topPerson = new Person('Joseph C. Boman', N'Agnes Lorenz', N'William T. Boman', N'', N'Father', N'Test Dad Desc');
            //Person topPerson = new Person("Joseph C. Boman",  "",  "", "Father",  "Test Dad Desc", "test", "");
            
            string name = "Joseph C. Boman"; //First person with no known parents
            // string name = "Marilyn Whelehon"; //First person with no known parents

            //Person topPerson = new Person("Sarah Ann Lovelace", "", "", "", "", "", "Test Desc"); 
            //FindChildren(topPerson, "any old name", "", firstChildIndicator);
            
            //Person topPerson = new Person("Permaness Lovelace", "", "", "", "", "", "Test Desc");
            Person topPerson = new Person(topOfTree, "", "", "", "", "", "Test Desc");
            FindChildren(topPerson, "any old name", "", firstChildIndicator);

            ViewBag.AnotherTree = //"<a href=\"/Home/Person\">" + "</a>";
                "This is AnotherTree";
            //" <a href=\"@Url.Action(\"Home/Person\")\">" + " </a>  ";

            //treeStringbuilder.Insert(17, " style=\"border-style:solid; border-width:5px;\"");

            ViewBag.BuildTree = treeStringbuilder.ToString();
            // UnComment This Line 

            return View();
        }

        private void FindChildren(Person currPerson, string name, string parentName ,bool firstChildIndicator)
        {            
            Stack<Person> childrenOnStack = new Stack<Person>();
            Person personFromStack = new Person();

            //*
            int ndxPositionOfCurrPerson = treeStringbuilder.ToString().IndexOf(currPerson.Name);
            //*

            //if (treeStringbuilder.ToString().Contains(name))
            if (ndxPositionOfCurrPerson == -1) // if -1 then then parent does not exist
                AddChild(currPerson, name, parentName, firstChildIndicator);

            var nodes = from node in nodeList 
                        where 
                        node.Father == currPerson.Name
                        || 
                        node.Mother == currPerson.Name
                            select node;

            int counter = 1;
            foreach (var node in nodes)
            {
                if (counter == 1)
                    firstChildIndicator = true;
                else
                    firstChildIndicator = false;                
                counter++;
                //if(node.BloodLine.Contains("Father"))


                //if (node.Father.Contains("Boman"))
                if (node.Father.Contains(currPerson.Name))
                {
                    node.ProcessingForWhichParent = node.Father;
                    AddChild(node, node.Name, node.Father, firstChildIndicator);
                }
                else
                {
                    node.ProcessingForWhichParent = node.Mother;
                    AddChild(node, node.Name, node.Mother, firstChildIndicator);
                }
                
                childrenOnStack.Push(node);
                firstChildIndicator = true;
            }
            
            while (childrenOnStack.Count > 0)
            {
                personFromStack = childrenOnStack.Pop();
                FindChildren(personFromStack, personFromStack.Name.ToString(), name, firstChildIndicator);
                firstChildIndicator = false;
            }            
        }

        private void AddChild(Person currPerson, string name, string parentName, bool firstChildIndicator)
        {
            int insertPoint, lengthOfStringObj = 0;            

            //* Test below code now
            string personString = "/Home/Person?name=" + currPerson.Name + "&" + "description=" + currPerson.Description;
            //* Test above code now
            if ( (currPerson.Mother == "") && (currPerson.Father == "") )
                treeStringbuilder.Insert(20, "<ul> <li> <a href=\"" + personString + "\">" + currPerson.Name + "</a>   </li>   </ul> ");
                //* works! treeStringbuilder.Insert(20, "<ul> <li> <a href=\"#\">" + currPerson.Name + "</a>   </li>   </ul> ");
                //treeStringbuilder.Insert(20, "<ul> <li> <a href=\"/Home/Person?name=Permaness Lovelace\">" + currPerson.Name + "</a>   </li>   </ul> ");
                //treeStringbuilder.Insert(20, "<ul> <li>" + "<a href=\"\\@Url.Action(\"Person\")\" > " + currPerson.Name + "</a>   </li>   </ul> ");
            else
            {
                insertPoint = treeStringbuilder.ToString().IndexOf(currPerson.ProcessingForWhichParent); /* Changed parentName */
                if (insertPoint == -1)
                {
                    /* this scenario occured when attempting to retrieve Mom's children
                     * when Dad's name was passed into the method as parentName */
                    System.Console.WriteLine("test message: Did not find parent");
                    System.Environment.Exit(insertPoint);
                }
                int i = insertPoint;
                if (firstChildIndicator == true)
                {
                    //first child logic
                    // find first space after the closing </a> tag of the parent                    
                    lengthOfStringObj = treeStringbuilder.Length;
                    do
                    {
                        if ((treeStringbuilder[i].Equals('<')) && (treeStringbuilder[i + 1].Equals('/')) && (treeStringbuilder[i + 2].Equals('a')) && (treeStringbuilder[i + 3].Equals('>')))
                        {
                            treeStringbuilder.Insert(i + 4, "<ul> <li> <a href=\"" + personString + "\">" + currPerson.Name + "</a>   </li>   </ul> ");
                            //* works! treeStringbuilder.Insert(i + 4, "<ul> <li> <a href=\"#\">" + currPerson.Name + "</a>   </li>   </ul> ");
                            break;
                        }                                                        
                        else
                            i++;
                    } while (i < lengthOfStringObj);               
                }
                else
                {
                    //second child logic
                    // find first space after the first closing </li> tag after parents name 
                    lengthOfStringObj = treeStringbuilder.Length;
                    do
                    {
                        if ((treeStringbuilder[i].Equals('<')) && (treeStringbuilder[i + 1].Equals('u')) && (treeStringbuilder[i + 2].Equals('l')) && (treeStringbuilder[i + 3].Equals('>') ))
                        {
                            treeStringbuilder.Insert(i + 5, "<li> <a href=\"" + personString + "\">" + currPerson.Name + "</a>  </li>  ");
                            //* works! treeStringbuilder.Insert(i + 5, "<li> <a href=\"#\">" + currPerson.Name + "</a>  </li>  ");
                            break;
                        }                                                     
                        else
                            i++;
                    } while (i < lengthOfStringObj);   
                }                    
            }                             
        }

        public ActionResult Person(string name, string description)
        {
            if (name == null)
                name = "Mildred B. Whelehon";
            ViewBag.Message = "Generic Person description message.";
            ViewBag.Person = name;
            ViewBag.Description = description;

            List<Artifact> artifacts = new List<Artifact>();

            artifacts = PersonDB.GetArtifacts(name);


            return View(artifacts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}
