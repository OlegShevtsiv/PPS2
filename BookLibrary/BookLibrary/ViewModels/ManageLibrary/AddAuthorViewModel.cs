using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.ViewModels.ManageLibrary
{
    public class AddAuthorViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Image")]
        public IFormFile Image { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
