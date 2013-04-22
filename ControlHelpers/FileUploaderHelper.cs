using System;
using System.IO;
using System.Web.UI.WebControls;

namespace voidsoft.Zinc
{
    /// <summary>
    /// 
    /// </summary>
    public class FileUploaderHelper
    {

        /// <summary>
        /// Uploads to folder.
        /// </summary>
        /// <param name="uploader">The uploader.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public string UploadToFolder(ref FileUpload uploader, string folder)
        {

            if (!folder.EndsWith(@"\"))
            {
                folder += @"\";
            }

            string completeFilePath = GetImageFileNameToBeUploaded(uploader.FileName, folder);

            uploader.SaveAs(completeFilePath);

            return completeFilePath;
        }


        /// <summary>
        /// Uploads the file in random subfolder.
        /// </summary>
        /// <param name="uploader">The FileUploader control</param>
        /// <param name="rootFolder">The root folder which contains the subdirectories in which the file will be placed</param>
        /// <returns></returns>
        public string UploadFileInRandomSubfolder(ref FileUpload uploader, string rootFolder)
        {
            //context.Request.ApplicationPath + Path.DirectorySeparatorChar +
            string[] directories = Directory.GetDirectories(rootFolder);


            if (directories.Length == 0)
            {
                throw new ArgumentException("No subdirectories found");
            }

            Random rr = new Random();
            int next = rr.Next(directories.Length);

            string completeFilePath = GetImageFileNameToBeUploaded(uploader.FileName, directories[next] + @"\");

            uploader.SaveAs(completeFilePath);

            return completeFilePath;
        }


        /// <summary>
        /// Gets the image file name to be uploaded.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="uploadFolder">The upload folder.</param>
        /// <returns></returns>
        private string GetImageFileNameToBeUploaded(string fileName, string uploadFolder)
        {
            string imagePath = uploadFolder + fileName;

            while (File.Exists(imagePath))
            {
                string path = Path.GetDirectoryName(imagePath);

                string file = Path.GetFileName(imagePath);

                string ext = Path.GetExtension(imagePath);

                int index = file.LastIndexOf(".");

                string newFileName = file.Substring(0, index);
                newFileName += "1";

                imagePath = path + Path.DirectorySeparatorChar + newFileName + ext;
            }

            return imagePath;
        }
    }
}