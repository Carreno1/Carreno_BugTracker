using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Carreno_BugTracker.Helpers
{
    public class AttachmentHelper
    {

        public static string ShowIcon(string filePath)
        {

            string iconPath;
            switch(Path.GetExtension(filePath)) //.jpg
            {
                case ".pdf":
                    iconPath = "/Icons/pdfIcon.png";
                    break;
                case ".txt":
                    iconPath = "/Icons/txtIcon.png";
                    break;
                default:
                    iconPath = "/Icons/pdfIcon.png";
                    break;
            }
            return iconPath;
        }


    }
}