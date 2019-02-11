using System;
using Microsoft.AspNetCore.Http;

namespace DateApp.API.Dtos
{
    public class PhotoForCreationDto
    {
        public PhotoForCreationDto(string url, IFormFile file, string description, DateTime dateAdded)
        {
            this.Url = url;
            this.File = file;
            this.Description = description;
            this.DateAdded = dateAdded;

        }
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}