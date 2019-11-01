using AJG.GOSP2013.Common.Models;
using Microsoft.BusinessData.MetadataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSI_List_ReportProcessing
{
    /// <summary>
    /// This class provides methods for attachments for DBA Email
    /// </summary>
    class Utility
    {
        /// <summary>
        /// Creates the Entity for given "filename"
        /// </summary>
        /// <param name="base64Content"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Entity CreateEntity(string base64Content, string filename)
        {
           
            //return entity;
        }

        /// <summary>
        /// Convert the Base64 Content of file to string
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <returns></returns>
        public static string ConvertToBase64(Stream sourceStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                sourceStream.CopyTo(memoryStream);
                var bytes = memoryStream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// Get the Entity for specified "filename"
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Entity GetEntity(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                var fileContent = ConvertToBase64(fs);
                return CreateEntity(fileContent, new FileInfo(filename).Name);
            }

        }
    }
}
