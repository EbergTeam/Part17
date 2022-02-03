using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Part17.Data;
using Part17.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Part17.Controllers
{
    public class HomeController : Controller
    {
        MyAppContext db;
        List<User> users;
        List<Company> companies;
        // поскольку в классе Startup в методе ConfigureServices контекст данных устанавливается как сервис,
        // то в конструкторе контроллера мы можем получить переданный контекст данных
        public HomeController(MyAppContext myAppContext)
        {
            db = myAppContext;
            companies = db.Companies.ToList();
            users = db.users.ToList();
        }
        // Фильтрация и Сортировка Списка пользователей. Критерий фильтрации и сортировки передается в метод
        public IActionResult Index(string name, int compId, bool sortType)
        {
            // фильтрация          
            if (name != null)
            {
                users = users.Where(u => u.Name.Contains(name)).ToList(); // если поле не пустое, то ищем в users совпадения
            }
            if (compId != 0)
            {
                users = users.Where(u => u.CompanyId == compId).ToList(); // если выбрана компания, то ищем только их сотрудников
            }

            // сортировка
            if (sortType)
                users = users.OrderBy(u => u.Name).ToList();
            else
                users = users.OrderByDescending(u => u.Name).ToList();

            // создаем экзмепляры классов Sort и FilterViewModel
            FilterViewModel filterViewModel = new FilterViewModel(companies, compId, name);
            SortViewModel sortViewModel = new SortViewModel(sortType);
            
            // создаем экзмепляр класса IndexViewModel и передаем в View
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                user = users.ToList(),
                filterViewModel = filterViewModel,
                sortViewModel = sortViewModel
            };
            return View(indexViewModel);
        }
        // форма добавления пользователя. В view передаем SelectList для выбора компании
        public IActionResult Create()
        {         
            ViewBag.Companies = new SelectList(companies, "Id", "Name");
            return View();
        }
        // форма добавления пользователя в БД, переадресация в Index
        [HttpPost] 
        public IActionResult Create(User user)
        {
            if (user.Name != null && user.Age != 0)
            {
                db.users.Add(user);
                db.SaveChanges();
            }
            return Redirect("Index");
        }
        // из формы Index получаем id пользователя, находим объект и передаем инфу про user в view
        // .Include(c => c.Company) подтянет данные про компанию
        public IActionResult Details(int id)
        {
            return id != null ? View(db.users.Include(c => c.Company).FirstOrDefault(u => u.Id == id)) : NotFound();
        }
        // окно редактирования данных пользователя
        // db.users.FirstOrDefault(w => w.Id == id).CompanyId) устанавливает выбранный по-умолчанию элемент списка  
        public IActionResult Edit(int id)
        {
            ViewBag.Companies = new SelectList(companies, "Id", "Name", db.users.FirstOrDefault(w => w.Id == id).CompanyId);
            return id != null ? View(db.users.FirstOrDefault(u => u.Id == id)) : NotFound();
        }
        // форма редактирования пользователя в БД, переадресация в Index
        [HttpPost]
        public IActionResult Edit(User user)
        {
            db.users.Update(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // окно удаления пользователя
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return id != null ? View(db.users.Include(c => c.Company).FirstOrDefault(u => u.Id == id)) : NotFound();
        }
        // удаление пользователя из БД
        [HttpPost]
        public IActionResult Delete(User user)
        {
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Privacy(string name, int compId)
        {
            return View();
        }

        // Добавление компании
        public IActionResult AddCompany()
            {
                return View();
            }
            [HttpPost]
            public IActionResult AddCompany(Company company)
            {
                if (company.Name != null)
                {
                    db.Companies.Add(company);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
}