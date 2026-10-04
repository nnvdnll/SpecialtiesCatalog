using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web1.Models;

namespace Web1.Controllers
{
    public class SpecialtiesController : Controller
    {
        private readonly AppDbContext _context;

        // Конструктор: внедряем базу данных
        public SpecialtiesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. ПОКАЗАТЬ СПИСОК (GET: /Specialties/)
        // 1. ПОКАЗАТЬ СПИСОК + ФИЛЬТРАЦИЯ (GET: /Specialties)
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            int pageSize = 6; // Количество элементов на одной странице (например, по 6 штук)
            int pageNumber = page ?? 1; // Если страница не указана, открывается 1-я

            var specialties = _context.Specialties.AsQueryable();

            // Фильтрация по поиску (если вы его делали)
            if (!string.IsNullOrEmpty(searchString))
            {
                specialties = specialties.Where(s => s.Name.Contains(searchString) || s.Code.Contains(searchString));
            }

            // Считаем общее количество записей после фильтрации
            int totalItems = await specialties.CountAsync();

            // Получаем только те записи, которые нужны для текущей страницы
            var items = await specialties
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Передаем данные и информацию о страницах во View через ViewBag
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchString = searchString;

            return View(items);
        }
        // 2. ОТКРЫТЬ ФОРМУ СОЗДАНИЯ (GET: /Specialties/Create)
        public IActionResult Create()
        {
            return View();
        }

        // 3. СОХРАНИТЬ НОВУЮ СПЕЦИАЛЬНОСТЬ (POST: /Specialties/Create)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Code,Description,DurationYears")] Specialty specialty)
        {
            // Проверяем шифр
            if (_context.Specialties.Any(s => s.Code == specialty.Code))
            {
                ModelState.AddModelError("", "Ошибка: Такой шифр уже существует в базе!");
            }

            // Проверяем название
            if (_context.Specialties.Any(s => s.Name == specialty.Name))
            {
                ModelState.AddModelError("", "Ошибка: Такое название уже существует в базе!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(specialty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(specialty);
        }
        // 4. ОТКРЫТЬ ФОРМУ РЕДАКТИРОВАНИЯ (GET: /Specialties/Edit/5)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null) return NotFound();

            return View(specialty);
        }

        // 5. СОХРАНИТЬ ИЗМЕНЕНИЯ (POST: /Specialties/Edit/5)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,Description,DurationYears")] Specialty specialty)
        {
            if (id != specialty.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Specialties.Any(e => e.Id == specialty.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(specialty);
        }

        // 6. СТРАНИЦА ПОДТВЕРЖДЕНИЯ УДАЛЕНИЯ (GET: /Specialties/Delete/5)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var specialty = await _context.Specialties.FirstOrDefaultAsync(m => m.Id == id);
            if (specialty == null) return NotFound();

            return View(specialty);
        }

        // 7. УДАЛИТЬ ИЗ БАЗЫ (POST: /Specialties/Delete/5)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty != null)
            {
                _context.Remove(specialty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}