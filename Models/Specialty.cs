using System.ComponentModel.DataAnnotations;

namespace Web1.Models
{
    public class Specialty
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле Шифр обязательно для заполнения")]
        public string Code { get; set; }

        // Убираем жесткие требования с описания, чтобы оно не выдавало ошибку
        public string? Description { get; set; }

        public int DurationYears { get; set; }
    }
}