using Organizer.Internal.Data;
using System.Collections.Generic;

namespace Organizer.Internal.Resources
{
	public class Translater
	{
		public enum Language { English, Russian }

		private Language _currentLanguage = (Language) Server.Language;
		public Language CurrentLanguage
		{
			get { return _currentLanguage; }
			set
			{
				Server.Language = (int) value;
				_currentLanguage = value;
			}
		}

		public string GetString (int id) => _translateString[id][(int) _currentLanguage];

		private static Dictionary<int, string[]> _translateString = new Dictionary<int, string[]>()
		{
			{ Resource.String.app_name,  new string[] { "Organizer", "Органайзер" } },
			{ Resource.String.project, new string[] { "Project", "Проект" } },
			{ Resource.String.regular_task, new string[] { "Regular Task", "Обычная Задача" } },
			{ Resource.String.routine, new string[] { "Routine", "Рутина" } },
			{ Resource.String.hint_title, new string[] { "Title (necessary)", "Заголовок (обязательно)" } },
			{ Resource.String.hint_text, new string[] { "Content", "Содержание" } },
			{ Resource.String.start_time, new string[] { "Time Start Task", "Время Начала Задачи" } },
			{ Resource.String.end_time, new string[] { "Time End Task", "Время Конца Задачи" } },
			{ Resource.String.regular_priority, new string[] { "Priority Of Tasks", "Приоритет Задачи" } },
			{ Resource.String.routine_days, new string[] { "Days Of Routine", "Дни Рутин" } },
			{ Resource.String.complete_tasks, new string[] { "Today Completed Tasks", "Выполненные Сегодня Задания" } },
			{ Resource.String.current_language, new string[] { "Current Language - ", "Текущий Язык - " } },
			{ Resource.String.english, new string[] { "English", "Английский" } },
			{ Resource.String.russian, new string[] { "Russian", "Русский" } },
			{ Resource.String.move_next, new string[] { "Move To Next Date", "Переместить На Следующую Дату" } },
			{ Resource.String.move_lower, new string[] { "Lower To Shorter Date", "Спустить На Более Короткую Дату" } },
			{ Resource.String.edit, new string[] { "Edit", "Редактировать" } },
			{ Resource.String.delete, new string[] { "Delete", "Удалить" } },
			{ Resource.String.type_sort, new string[] { "Sort By Type", "Сортировать По Типу Задачи" } },
			{ Resource.String.time_start_sort, new string[] { "Sort By Start Time", "Сортировать По Времени Начала" } },
			{ Resource.String.last_day, new string[] { "Yesterday", "Вчера" } },
			{ Resource.String.this_day, new string[] { "Today", "Сегодня" } },
			{ Resource.String.next_day, new string[] { "Tomorrow", "Завтра" } },
			{ Resource.String.last_month, new string[] { "Last Month", "Прошлый Месяц" } },
			{ Resource.String.this_month, new string[] { "This Month", "Этот Месяц" } },
			{ Resource.String.next_month, new string[] { "Next Month", "Следующий Месяц" } },
			{ Resource.String.last_year, new string[] { "Last Year", "Предыдущий Год" } },
			{ Resource.String.this_year, new string[] { "This Year", "Этот Год" } },
			{ Resource.String.next_year, new string[] { "Next Year", "Следующий Год" } },
			{ Resource.String.global, new string[] { "Main Tasks", "Главные Задачи" } },
			{ Resource.String.sunday_short, new string[] { "Sun.", "Вс" } },
			{ Resource.String.monday_short, new string[] { "Mon.", "Пн" } },
			{ Resource.String.tuesday_short, new string[] { "Tues.", "Вт" } },
			{ Resource.String.wednesday_short, new string[] { "Wed.", "Ср" } },
			{ Resource.String.thursday_short, new string[] { "Thur.", "Чт" } },
			{ Resource.String.friday_short, new string[] { "Fri.", "Пт" } },
			{ Resource.String.saturday_short, new string[] { "Sat.", "Сб" } }
		};
	}
}