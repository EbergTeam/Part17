using Microsoft.AspNetCore.Mvc.Rendering;
using Part17.Models;
using System.Collections.Generic;

namespace Part17.Models
{
    public class SortViewModel
    {
        public bool sortOrder { get; set; } // значение для сортировки по имени по возр/убыв
        public SortViewModel(bool sortType)
        {
            sortOrder = !sortType; // инвертируем для сортировки наоборот
        }
    }
}