using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThePlanner.Models
{
    /// <summary>
    /// кастомное input поле
    /// </summary>
    public class InputField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public int OccasionId { get; set; }
        public Occasion Occasion { get; set; }

    }

    /// <summary>
    /// Мероприятие
    /// </summary>
    public class Occasion
    {
        public int Id { get; set; }

        /// <summary>
        /// Тема
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Дата проведения
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Индекс мероприятия (в представлении)
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Место проведения
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Возможное количество участников
        /// </summary>
        public int MembersLimitCount { get; set; }

        /// <summary>
        /// Текущее количество участников
        /// </summary>
        public int MembersCount { get; set; }

        /// <summary>
        /// Кастомные поля ввода данных
        /// </summary>
        public ICollection<InputField> InputFields { get; set; }

        /// <summary>
        /// Участники
        /// </summary>
        public ICollection<ApplicationUser> Members { get; set; }

        public Occasion()
        {
            InputFields = new List<InputField>();
            Members = new List<ApplicationUser>();
            throw new NotImplementedException("Миграция бд INDEX события");
        }

    }
}