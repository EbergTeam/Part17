using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Part17.Models
{
    public class FilterViewModel
    {
        public SelectList Companies { get; set; } // список компаний
        public int SelectedCompany { get; set; } // выбранная компания
        public string SelectedName { get; set; } // введенное имя
        public FilterViewModel(List<Company> companies, int company, string name)
        {
            // устанавливаем начальный элемент, который позволит выбрать всех
            companies.Insert(0, new Company { Name = "Все", Id = 0 });
            Companies = new SelectList(companies, "Id", "Name", company); 
            // сохраняем введенное/выбранное значения и используем в View, чтобы не обнулялся при обновлении страницы
            SelectedCompany = company;
            SelectedName = name;
        }
        
    }
}
