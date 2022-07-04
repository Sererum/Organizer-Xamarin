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
			{ Resource.String.regular, new string[] { "Task", "Задача" } },
			{ Resource.String.routine, new string[] { "Routine", "Рутина" } },

			{ Resource.String.hint_title, new string[] { "Title (necessary)", "Заголовок (обязательно)" } },
			{ Resource.String.hint_text, new string[] { "Content", "Содержание" } },
			{ Resource.String.start_time, new string[] { "Start time", "Время начала" } },
			{ Resource.String.end_time, new string[] { "End time", "Время конца" } },
			{ Resource.String.regular_priority, new string[] { "Priority of task", "Приоритет задачи" } },
			{ Resource.String.routine_days, new string[] { "Days of routine", "Дни рутин" } },

			{ Resource.String.title_statistics, new string[] { "Statistics", "Статистика" } },
			{ Resource.String.title_settings, new string[] { "Settings", "Настройки" } },
			{ Resource.String.complete_tasks, new string[] { "Completed tasks", "Выполненные задачи" } },
			{ Resource.String.current_language, new string[] { "Language", "Язык" } },
			{ Resource.String.current_screen, new string[] { "Start screen", "Начальный экран" } },
			{ Resource.String.current_theme, new string[] { "Theme", "Тема" } },
			{ Resource.String.current_sort, new string[] { "Sorting tasks", "Сортировка задач" } },

			{ Resource.String.english, new string[] { "English", "Английский" } },
			{ Resource.String.russian, new string[] { "Russian", "Русский" } },

			{ Resource.String.calendar, new string[] { "Calendar", "Календарь" } },
			{ Resource.String.schedule, new string[] { "Schedule", "Расписание" } },
			{ Resource.String.list_tasks, new string[] { "List tasks", "Список задач" } },
			{ Resource.String.account, new string[] { "Account", "Аккаунт" } },

			{ Resource.String.main, new string[] { "Main", "Основная" } },
			{ Resource.String.soft, new string[] { "Soft", "Мягкая" } },
			{ Resource.String.purple, new string[] { "Purple", "Фиолетовая" } },
			{ Resource.String.main_dark, new string[] { "Dark", "Темная" } },
			{ Resource.String.deep_water, new string[] { "Deep water", "Глубоководная" } },
			{ Resource.String.dark_purple, new string[] { "Dark purple", "Темно-фиолетовая" } },

			{ Resource.String.type, new string[] { "Type of task", "Тип задачи" } },

			{ Resource.String.move_next, new string[] { "Move to next date", "Переместить на следующую дату" } },
			{ Resource.String.move_lower, new string[] { "Move to shorter date", "Переместить на более короткую дату" } },
			{ Resource.String.edit, new string[] { "Edit", "Редактировать" } },
			{ Resource.String.delete, new string[] { "Delete", "Удалить" } },

			{ Resource.String.last_day, new string[] { "Yesterday", "Вчера" } },
			{ Resource.String.this_day, new string[] { "Today", "Сегодня" } },
			{ Resource.String.next_day, new string[] { "Tomorrow", "Завтра" } },
			{ Resource.String.last_month, new string[] { "Last month", "Прошлый месяц" } },
			{ Resource.String.this_month, new string[] { "This month", "Этот месяц" } },
			{ Resource.String.next_month, new string[] { "Next month", "Следующий месяц" } },
			{ Resource.String.last_year, new string[] { "Last year", "Предыдущий год" } },
			{ Resource.String.this_year, new string[] { "This year", "Этот год" } },
			{ Resource.String.next_year, new string[] { "Next year", "Следующий год" } },
			{ Resource.String.global, new string[] { "Main tasks", "Главные задачи" } },

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