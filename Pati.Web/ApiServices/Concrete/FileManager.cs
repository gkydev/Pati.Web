﻿using Microsoft.AspNetCore.Http;
using Pati.Web.ApiServices.Interfaces;
using Pati.Web.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pati.Web.ApiServices.Concrete
{
    public class FileManager : IFileService
    {
        public async Task<string> GenerateFileName(string fileName)
        {
            var extension = fileName.Split(".")[1];
            var name = fileName.Split(".")[0];
            name += "_" + DateTime.Now.ToString();
            name = SHA1.Generate(name);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(name);
            stringBuilder.Append(".");
            stringBuilder.Append(extension);

            return stringBuilder.ToString();
        }

        public async Task<byte[]> ConvertToByte(IFormFile formFile)
        {
            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            return stream.ToArray();
        }

        public async Task<List<string>> UploadFile(List<IFormFile> files)
        {
            List<string> uploadedFileNames = new List<string>();

            foreach (var item in files)
            {
                var fileContent = await ConvertToByte(item);
                var fileName = await GenerateFileName(item.FileName);
                uploadedFileNames.Add(fileName);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(StaticVars.BaseFTPAddress + fileName);
                request.Credentials = new NetworkCredential("u247404070.pati", "Pati2020");
                request.Method = WebRequestMethods.Ftp.UploadFile;


                using (Stream ftpStream = request.GetRequestStream())
                {
                    ftpStream.Write(fileContent, 0, fileContent.Length);
                }

            }

            return uploadedFileNames;
        }
    }
}
