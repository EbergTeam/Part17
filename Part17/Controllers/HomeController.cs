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
        }
        // Сортировка Списка пользователей. Критерий сортировки передается в метод Index в виде параметра sortType
        public IActionResult Index(bool sortType)
        {
            ViewData["sortType"] = !sortType;
            if ((bool)ViewData["sortType"])
                users = db.users.OrderBy(u => u.Name).ToList();
            else
                users = db.users.OrderByDescending(u => u.Name).ToList();
            
            return View(users);
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
        // фильтрация по имени и компании
        public IActionResult Privacy(string name, int compId)
        {
            List<User> users = db.users.ToList();
            if (name != null)
            {
                users = users.Where(u => u.Name.Contains(name)).ToList(); // если поле не пустое, то ищем в users совпадения
            }
            if (compId != 0)
            {
                users = users.Where(u => u.CompanyId == compId).ToList(); // если выбрана компания, то ищем только их сотрудников
            }

            // устанавливаем начальный элемент, который позволит выбрать всех
            companies.Insert(0, new Company { Name = "Все", Id = 0 });

            // создаем экзмепляр класса UserListViewModel и передаем в View
            UserListViewModel viewModel = new UserListViewModel
            {
                Users = users.ToList(),
                Companies = new SelectList(companies, "Id", "Name"),
            };

            return View(viewModel);
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