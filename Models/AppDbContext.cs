using Microsoft.EntityFrameworkCore;

namespace WebApi1.Models
{
    public class AppDbContext : DbContext
    {
        //Передаеи настройки подключения
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Эта строчка превращает наш класс Specialty в полноценную таблицу в базе данных
        public DbSet<Specialty> Specialties { get; set; }
    }
}