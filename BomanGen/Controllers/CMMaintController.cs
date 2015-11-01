using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BomanGen.Models;
using System.Drawing;
using System.IO;
using System.Data;

namespace BomanGen.Controllers
{
    [Authorize(Users = "AdminA,AdminB,AdminC,AdminD,AdminE,AdminW")]
    public class CMMaintController : Controller
    {
        //class level variables below
        List<Person> nodeList = new List<Person>();
        private EFContextArtifacts db = new EFContextArtifacts();
        string genericPersonName = "Generic Person Name";

        //
        // GET: /CMMaint/

        public ActionResult Index()
        {
            // works! var myCollection = db.Artifact.ToArray();
            var myCollection = db.Artifact.ToArray();
            List<Artifact> artifacts = new List<Artifact>();

            artifacts = PersonDB.GetArtifacts(genericPersonName);

            //var www = db.Artifact.Find(" ") ;//  .ToList();

            //? needed ??
            //var xxx = db.Artifact.ToArray().First();


            //var yyy = db.Artifact.ToArray().Distinct(genericPersonName);

            return View(artifacts);
        }

        [HttpGet]
        //[Authorize]
        public ActionResult FileAssociate(string fileName)
        {
            Session["currentFileProcessed"] = fileName;
            ViewBag.Message = "It's FileAssociate Time";

            // Populate the list of Persons
            /* Following code works */ // nodeList = PersonDB.GetPersons();
            var artifactList = PersonDB.GetPersonsNames();
            int ttlNumOfNames = artifactList.Count;
            List<string> artifactNames = new List<string>();
            artifactNames = PersonDB.GetNamesByFile(fileName);

            //check the box on the screen for Persons currently associated with the file
            int ttlNumberOfNames = artifactNames.Count;
            for(int i = 0; i < ttlNumberOfNames; i++)
            {
                for (int x = 0; x < ttlNumOfNames; x++) 
                { 
                    if (artifactNames[i] == artifactList[x].Name)
                        artifactList[x].IfChecked = true; // check the checkbox in artifactNames list
                }
            }
            return View(artifactList);
        }

        [HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult FileAssociate([Bind(Include = "FileName,Name,ArtifactType,HeadStone,Description")]Artifact artifact)
        public ActionResult FileAssociate(List<BomanGen.Models.ArtifactModel> list)        
        {
            ViewBag.Message = "It's Test 1 2 3 Time";
            string currFileName = Session["currentFileProcessed"].ToString();
            
            // Retrieve and temporarily hold the generic row in an Artifact Model class
            var genericArtifactRow = db.Artifact.Find(currFileName, genericPersonName);

            // Delete all database rows for this fileName
            // in case the user de-selected any of them
            if (PersonDB.DelNamesByFile(currFileName) == true)
                ;

            // Re-Add the Generic Person database row since it has to exist
            PersonDB.AddNameToArtifacts(genericArtifactRow);

            /* bool trueOrFalse = PersonDB.DelNamesByFile(currFileName); */
            
            int ttlNumOfNames = list.Count;
            for (int x = 0; x < ttlNumOfNames; x++)
            {
                if (list[x].IfChecked == true )
                {
                    Artifact currArtifact = new Artifact();
                    //var myRow = db.Artifact.ToArray();
                    //var genericRow = db.Artifact.Find(currFileName);
                    currArtifact.FileName = currFileName;
                    currArtifact.Name = list[x].Name;
                    currArtifact.HeadStone = genericArtifactRow.HeadStone;
                    currArtifact.Description = genericArtifactRow.Description;
                    currArtifact.ArtifactType = genericArtifactRow.ArtifactType;
                    //BomanGen.Models.Artifact currArtifact;
                    PersonDB.AddNameToArtifacts(currArtifact);
                }
            }

            TempData["ReturnData"] = "The following Artifact or Photo was Updated successfully: " + currFileName;
            return View(list);
        }
        

        //private ActionResult DeleteArtifact(string fileName)
        //{
        //    DeleteFromFileSystem(fileName);
        //    //remove from database
            
        //     DeleteArtifactsByFileName(fileName);
        //      Artifact artifact = db.Artifact.Find(fileName);
        //    //db.Artifact.Remove().FileName(fileName);
        //    //db.Artifact.
        //    //db.SaveChanges();
        //}


        //[Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteArtifact(string fileName)         
        {
            // ensure at least one row exists 
            // by testing for the generic person row for this filename
            Artifact artifact = db.Artifact.Find(fileName, genericPersonName);

            // Delete the row from the database
            if (PersonDB.DelNamesByFile(fileName) == true)
            ;
            
            //build the paths to the file names
            string smallImageFilePath = Path.Combine(Server.MapPath("~/photoprowess/images/demo/gallery/") + "ThumbSize" + (artifact.FileName));
            string largeImageFilePath = Path.Combine(Server.MapPath("~/photoprowess/images/demo/gallery/") + (artifact.FileName));
           
            //Delete both the file and it's associated thumbnail from the FileSystem
            //If both file names exist, delete them from the file system
            if (System.IO.File.Exists(smallImageFilePath) && (System.IO.File.Exists(largeImageFilePath)))
            {
                try
                {
                    System.IO.File.Delete(smallImageFilePath);
                    System.IO.File.Delete(largeImageFilePath);
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    TempData["ReturnData"] = " Delete exception. DirectoryNotFoundException. The Details follow:  " + e.ToString();
                    return RedirectToAction("Index");
                }
                catch (System.IO.IOException e)
                {
                    TempData["ReturnData"] = " Delete exception. IOException. The Details follow:  " + e.ToString();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ReturnData"] = " Delete exception. Exception. The Details follow:  " + ex.ToString();
                    return RedirectToAction("Index");
                }
            }
            else 
            {
                TempData["ReturnData"] = " File Delete exception:  Either File " + smallImageFilePath + " Or File " + largeImageFilePath + " was not found.  As a result, neither File delete was attempted.";
                return RedirectToAction("Index");
            }
            TempData["ReturnData"] = "The following File was deleted: " + artifact.FileName;
            return RedirectToAction("Index");
        }


        [HttpGet]
        //[Authorize]
        public ActionResult Update(string fileName)
        {
            ViewBag.Message = "It's Update Time";

            // Retrieve the Generic person row for this file, then pass it to the View
            Artifact artifact = db.Artifact.Find(fileName, genericPersonName);

            if (artifact == null)
            {
                return HttpNotFound();
            }
            return View(artifact);
        }


        [HttpPost]
        //[Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "FileName,Name,ArtifactType,HeadStone,Description")]Artifact artifact)                
        {
            ViewBag.Message = "It's Update Time";
            List<string> artifactNames = new List<string>();
            artifactNames = PersonDB.GetNamesByFile(artifact.FileName);
            foreach (string name in artifactNames)
            {
                if(ModelState.IsValid)
                { 
                    var genericArtifactRow = db.Artifact.Find(artifact.FileName, name);
                    genericArtifactRow.Description = artifact.Description;
                    db.Entry(genericArtifactRow).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            TempData["ReturnData"] = "Photo was successfully updated";
            return RedirectToAction("Index");
            return View(artifact);
        }

        

        /* [Authorize] */
        [HttpGet]
        public ActionResult CreateArtifact()
        {
            ViewBag.Message = "Click Browse to choose a photo to upload.  Optionally, enter a Caption below";
            return View();
        }
        
        [HttpPost]        
        [ValidateAntiForgeryToken]
        //[Authorize]
        public ActionResult CreateArtifact(HttpPostedFileBase file, Models.Artifact artifactcm)
        {
            ViewBag.Message = "Testing Artifact File Create";

            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/photoprowess/images/demo/gallery"),
                                               Path.GetFileName(file.FileName));

                    //System.IO.File.SetAttributes(path, System.IO.FileAttributes.Normal);

                    //System.Drawing.Image MainImgPhotoVert = System.Drawing.Image.FromFile(path);
                    /*
                    System.Drawing.Image MainImgPhotoVert = System.Drawing.Image.FromStream(System.IO.Stream file);
                    Bitmap MainImgPhoto = (System.Drawing.Bitmap)ScaleByPercent(MainImgPhotoVert, 100);
                    MainImgPhoto.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                    MainImgPhoto.Dispose();
                    */

                    //if the file already exists redirect control to Index page!
                    if (System.IO.File.Exists(path))
                    {
                        TempData["ReturnData"] = "A Photo named: " + file.FileName + " already exists.  You may Delete it below.  Then re-Add the photo";
                        return RedirectToAction("Index");
                    }
                    {
                        file.SaveAs(path);
                    }

                    //file.InputStream.Flush(); //useless
                    //file.InputStream.Close(); //less than useless
                    //file.InputStream.Dispose(); //complete waste of keystrokes

                    //System.IO.File.SetAttributes(path, System.IO.FileAttributes.Normal);

                    // Validating whether the following commented code releases a recently created
                    // file from IIS for file Delete.  Problem occuring in the Visual Studio test environment.
                    //file.InputStream.Dispose();
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();

                    // Create the Thumbnail image
                    string smallImageFilePath = Path.Combine(Server.MapPath("~/photoprowess/images/demo/gallery/") + "ThumbSize" + (file.FileName));

                    //allocate an Image object from the uploaded full sized .jpg 

                    // works System.Drawing.Image imgPhotoVert = System.Drawing.Image.FromFile(path);
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    System.Drawing.Image imgPhotoVert = System.Drawing.Image.FromStream(fs);

                    Bitmap imgPhoto = (System.Drawing.Bitmap)ScaleByPercent(imgPhotoVert, 50);

                    //imgPhoto.Save(smallImageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imgPhoto.Save(smallImageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fs.Close();

                    imgPhoto.Dispose();
                    //((IDisposable)imgPhoto).Dispose();

                    var artifact = new Artifact();
                    //gallery.PhotoNumberID = 9;
                    artifact.FileName = file.FileName;
                    if (artifactcm.Description == null)
                        artifactcm.Description = " ";
                    artifact.Description = artifactcm.Description;
                    artifact.ArtifactType = " ";
                    artifact.HeadStone = " ";
                    artifact.Name = genericPersonName;
                    //artifact.Name = "Mildred B. Whelehon";

                    var artifactContext = new EFContextArtifacts();
                    artifactContext.Artifact.Add(artifact);
                    artifactContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    TempData["ReturnData"] = file.FileName + " Upload exception.  The Details follow:  " + ex.ToString();
                    return RedirectToAction("Index");
                }
            else
            {
                //ViewBag.Message = "You have not specified a file.";
                TempData["ReturnData"] = "A file was not chosen for upload.";
                return RedirectToAction("CreateArtifact");
            }
            TempData["ReturnData"] = file.FileName + " was successfully Added";
            return RedirectToAction("Index");
        }

        private System.Drawing.Image ScaleByPercent(System.Drawing.Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            //retain aspect ratio of thumbnail
            int bitMapWidth = 0, bitMapHeight = 0;
            if (destHeight > destWidth)
            {
                bitMapWidth = bitMapHeight = destHeight;
                destX = (bitMapWidth - destWidth) / 2;
                //destX = destHeight / 4;
            }
            else
            {
                bitMapHeight = bitMapWidth = destWidth;
                destY = (bitMapHeight - destHeight) / 2;
                //destY = destWidth / 4;
            }

            Bitmap bmPhoto = new Bitmap(bitMapWidth, bitMapHeight,
                                     System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                    imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.Clear(Color.FromArgb(225, 222, 218)); //clears entire drawing surface and colors it with padding

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

    }
}
