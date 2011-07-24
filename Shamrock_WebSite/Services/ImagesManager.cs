using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using Shamrock_WebSite.Infrastructure;

namespace Shamrock_WebSite.Services
{
    public static class ImagesManager
    {
        #region Private

        private static string _baseImagesPath;
        public static string BaseImagesPath
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_baseImagesPath))
                {
                    var appPath = HttpRuntime.AppDomainAppPath;
                    var imagesPath = "Content/Images/";
                    _baseImagesPath = Path.Combine(appPath, imagesPath);
                }
                return _baseImagesPath;
            }
        }

        private static int _imageWidth = 640;
        private static int _imageHeight = 480;

        private static string _thumbExtension = "thumb_";
        private static string _thumbFolder = "Thumb";

        #endregion

        public static IEnumerable<string> GetImages(string directory)
        {
            var imagesPath = Path.Combine(BaseImagesPath, directory);
            var images = new DirectoryInfo(imagesPath).GetFiles();
            return images.Select(f => "~/Content/Images/" + directory + "/" + f.Name);
        }

        public static string UploadImage(WebImage image, string directory, bool thumbCreationEnabled = false, int thumbWidth = 120, int thumbHeight = 90, bool cropThumb = false)
        {
            try
            {
                var imagesPath = Path.Combine(BaseImagesPath, directory);

                var fileName = Path.GetFileName(image.FileName);
                fileName = LookForVacantFileName(fileName, imagesPath);
                var imagePath = Path.Combine(imagesPath, fileName);

                bool isPortraitOrientation = image.Width < image.Height ? true : false;

                if (isPortraitOrientation)
                    image.Resize(_imageHeight, _imageWidth, true, true);
                else
                    image.Resize(_imageWidth, _imageHeight, true, true);

                image.Save(imagePath);

                if (thumbCreationEnabled)
                {
                    WebImage thumbImage = image.Clone();

                    if (cropThumb)
                    {
                        if (isPortraitOrientation)
                        {
                            thumbImage = thumbImage.Resize(thumbWidth, thumbImage.Height, true, true);
                            var heightToRemove = (thumbImage.Height - thumbHeight) / 2;
                            if (heightToRemove > 0)
                                thumbImage = thumbImage.Crop(heightToRemove, 0, heightToRemove, 0);
                        }
                        else
                        {
                            thumbImage = thumbImage.Resize(thumbImage.Width, thumbHeight, true, true);
                            var widthToRemove = (thumbImage.Width - thumbWidth) / 2;
                            if (widthToRemove > 0)
                                thumbImage = thumbImage.Crop(0, widthToRemove, 0, widthToRemove);
                        }
                    }
                    else
                    {
                        if (isPortraitOrientation)
                            thumbImage = image.Clone().Resize(thumbHeight, thumbWidth, false, true);
                        else
                            thumbImage = image.Clone().Resize(thumbWidth, thumbHeight, false, true);
                    }

                    var thumbFileName = _thumbExtension + fileName;
                    var thumbFolderPath = Path.Combine(imagesPath, _thumbFolder);
                    if (!Directory.Exists(thumbFolderPath))
                        Directory.CreateDirectory(thumbFolderPath);
                    var thumbImagePath = Path.Combine(thumbFolderPath, thumbFileName);

                    thumbImage.Save(thumbImagePath);
                }

                return "~/Content/Images/" + directory + "/" + fileName;
            }
            catch
            {
                return null;
            }
        }

        private static string LookForVacantFileName(string fileName, string imagesPath)
        {
            var imagePath = Path.Combine(imagesPath, fileName);
            //   if folder/dd.png exist -> create folder/dd(0).png
            if (File.Exists(imagePath))
            {
                int counter = 0;
                string tempFileName;
                string tempImagePath;
                do
                {
                    var insertString = "(" + counter.ToString() + ")";
                    tempFileName = PathExtensions.InsertStringBeforeExtension(fileName, insertString);
                    tempImagePath = Path.Combine(imagesPath, tempFileName);
                    counter++;
                }
                while (File.Exists(tempImagePath));

                return tempFileName;
            }
            return fileName;
        }

        public static void DeleteImage(string fileName, string directory)
        {
            var imagePath = Path.Combine(BaseImagesPath, directory, fileName);
            File.Delete(imagePath);

            var thumbFileName = _thumbExtension + fileName;
            var thumbFilePath = Path.Combine(BaseImagesPath, directory, _thumbFolder, thumbFileName);
            if (File.Exists(thumbFilePath))
                File.Delete(thumbFilePath);
        }

    }
}