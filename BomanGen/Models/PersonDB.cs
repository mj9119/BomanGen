using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using BomanGen.Models;

namespace BomanGen
{
    public class PersonDB
    {
        public static List<Person> GetPersons() 
        {
            List<Person> persons = new List<Person>();

            SqlConnection con = new SqlConnection(GetConnectionString());
            string sel = "SELECT * " +
                "FROM tblPersons" ;
            
            SqlCommand cmd = new SqlCommand(sel, con);
            con.Open();
            SqlDataReader rdr =
                cmd.ExecuteReader(CommandBehavior.CloseConnection);        
                        
            while (rdr.Read())
            {
                Person p = new Person();
                p.Name = rdr["Name"].ToString();
                p.Mother = rdr["Mother"].ToString();
                p.Father = rdr["Father"].ToString();
                p.MaidenName = rdr["MaidenName"].ToString();
                p.BloodLine = rdr["BloodLine"].ToString();
                p.Description = rdr["Description"].ToString();                
                
                persons.Add(p);                
            }
            rdr.Close();

            return persons;
        }

        public static List<ArtifactModel> GetPersonsNames()
        {
            List<ArtifactModel> personsNames = new List<ArtifactModel>();

            SqlConnection con = new SqlConnection(GetConnectionString());
            string sel = "SELECT * " +
                "FROM tblPersons";

            SqlCommand cmd = new SqlCommand(sel, con);
            con.Open();
            SqlDataReader rdr =
                cmd.ExecuteReader(CommandBehavior.CloseConnection);

            int i = 0;
            while (rdr.Read())
            {
                ArtifactModel a = new ArtifactModel();
                //Person p = new Person();
                a.Id = i++;
                a.Name = rdr["Name"].ToString();
                a.IfChecked = false;
                //p.Mother = rdr["Mother"].ToString();
                //p.Father = rdr["Father"].ToString();
                //p.MaidenName = rdr["MaidenName"].ToString();
                //p.BloodLine = rdr["BloodLine"].ToString();
                //p.Description = rdr["Description"].ToString();

                personsNames.Add(a);
            }
            rdr.Close();
            return personsNames;
        }

        public static bool AddNameToArtifacts(Artifact person)
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string insertStatement = "INSERT tblArtifacts " +
                "(Name, FileName, ArtifactType, HeadStone, Description) " +
                "VALUES (@Name, @FileName, @ArtifactType, @HeadStone, @Description)";

            SqlCommand cmd = 
                new SqlCommand(insertStatement, con);
            cmd.Parameters.AddWithValue("@Name", person.Name);
            cmd.Parameters.AddWithValue("@FileName", person.FileName);
            cmd.Parameters.AddWithValue("@ArtifactType", person.ArtifactType);
            cmd.Parameters.AddWithValue("@HeadStone", person.HeadStone);
            cmd.Parameters.AddWithValue("@Description", person.Description);
            try
            {
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
                con.Close();
            }
        }



        public static bool DelNamesByFile(string fileName)
        {         
            SqlConnection con = new SqlConnection(GetConnectionString());
            string del = "DELETE FROM tblArtifacts " +             
                "WHERE FileName = @fileName ";

            SqlCommand cmd = new SqlCommand(del, con);
            cmd.Parameters.AddWithValue("@FileName", fileName);
            try
            {
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
                con.Close();
            }
        }


        public static List<string> GetNamesByFile(string fileName)
        {
        List<string> artifactNames = new List<string>();

        SqlConnection con = new SqlConnection(GetConnectionString());
        string sel = "SELECT Name " +
            "FROM tblArtifacts " +
            "WHERE FileName = @fileName " +
            "ORDER BY Name ";

            SqlCommand cmd = new SqlCommand(sel, con);
            cmd.Parameters.AddWithValue("@FileName", fileName);
            try
            {
                con.Open();
                SqlDataReader rdr =
                    cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    Artifact a = new Artifact();
                    a.Name = rdr["Name"].ToString();
                    //a.FileName = rdr["FileName"].ToString();
                    //a.ArtifactType = rdr["ArtifactType"].ToString();
                    //a.HeadStone = rdr["HeadStone"].ToString();
                    //a.Description = rdr["Description"].ToString();
                    artifactNames.Add(a.Name);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
                con.Close();
            }
            return artifactNames;
        }





        public static List<Artifact> GetArtifacts(string name)
        {
            List<Artifact> artifacts = new List<Artifact>();

            SqlConnection con = new SqlConnection(GetConnectionString());
            string sel = "SELECT * " +
                "FROM tblArtifacts " +
                "WHERE Name = @Name " +
                "ORDER BY ArtifactType DESC ";

            SqlCommand cmd = new SqlCommand(sel, con);
            cmd.Parameters.AddWithValue("@Name", name);
            try
            {
                con.Open();
                SqlDataReader rdr =
                    cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    Artifact a = new Artifact();
                    a.Name = rdr["Name"].ToString();
                    a.FileName = rdr["FileName"].ToString();
                    a.ArtifactType = rdr["ArtifactType"].ToString();
                    a.HeadStone = rdr["HeadStone"].ToString();
                    a.Description = rdr["Description"].ToString();
                    artifacts.Add(a);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
                con.Close();
            }

            return artifacts;
        }

        private static string GetConnectionString()
        {
            //return "Data Source=localhost\\sqlexpress;" +
            //       "Initial Catalog=BomanGen;Integrated Security=True";


            return ConfigurationManager.ConnectionStrings[
                "ApplicationConnectionString"].ConnectionString;

        }
    }
}