using Part17.Models;
using System.Collections.Generic;

namespace Part17.Models
{
    public class IndexViewModel
    {
        // общая модель IndexViewModel, которая объединяет все эти модели и полученные данные
        public List<User> user { get; set; }
        public FilterViewModel filterViewModel { get; set; }
        public SortViewModel sortViewModel { get; set; }
    }
}