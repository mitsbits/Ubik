using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Postal.Contracts;

namespace Ubik.Postal
{
    public class FileAttachmentInfo : IAttachmentInfo
    {
        public FileAttachmentInfo(string path, string contentType, string fileName = "")
        {
            FilePath = path;
            var mTypes = contentType.Split('/');
            MediaType = mTypes[0];
            MediaTSubType = mTypes[1];
            FileName = (string.IsNullOrWhiteSpace(FileName))? Path.GetFileName(path):fileName;
        }


        public string FileName { get; private set; }
        public string FilePath { get; private set; }

        public string MediaType { get; private set; }
        public string MediaTSubType { get; private set; }

        public Stream GetStream()
        {
            return File.OpenRead(FilePath);
        }
    }
}
