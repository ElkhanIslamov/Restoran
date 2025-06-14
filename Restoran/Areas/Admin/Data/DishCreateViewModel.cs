﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Restoran.Areas.Admin.Data
{
    public class DishCreateViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> CategorySelectListItems { get; set; } = [];
        public required string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [NotMapped]
        public required IFormFile ImageFile { get; set; }
        public string? ImageUrl { get; set; }
    }
}
