using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.General
{
    public class File
    {
        public IFormFile FileData { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }
        public string AbsoluteUri { get; set; }
        public string RelativeUri { get; set; }
        public bool IsNewUpload { get; set; }
        public byte[] OfferLetterData { get; set; }
        public string ContentType { get; set; }
        public byte[] fdata { get; set; }
        public string FilePath { get; set; }
    }
}
