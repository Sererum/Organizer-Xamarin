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
			{ Resource.String.title_time_management, new string[] { "Time management", "Тайм-менеджмент" } },
			{ Resource.String.complete_tasks, new string[] { "Completed tasks", "Выполненные задачи" } },
			{ Resource.String.start_tutorial, new string[] { "Start tutorial", "Пройти обучение" } },
			{ Resource.String.current_language, new string[] { "Language", "Язык" } },
			{ Resource.String.current_screen, new string[] { "Start screen", "Начальный экран" } },
			{ Resource.String.current_theme, new string[] { "Theme", "Тема" } },
			{ Resource.String.current_sort, new string[] { "Sorting tasks", "Сортировка задач" } },

			{ Resource.String.english, new string[] { "English", "Английский" } },
			{ Resource.String.russian, new string[] { "Russian", "Русский" } },

			{ Resource.String.calendar, new string[] { "Calendar", "Календарь" } },
			{ Resource.String.schedule, new string[] { "Schedule", "Расписание" } },
			{ Resource.String.list_tasks, new string[] { "List tasks", "Список задач" } },
			{ Resource.String.inbox, new string[] { "Inbox list", "Список входящих задач" } },
			{ Resource.String.account, new string[] { "Account", "Аккаунт" } },

			{ Resource.String.main, new string[] { "Main", "Основная" } },
			{ Resource.String.soft, new string[] { "Soft", "Мягкая" } },
			{ Resource.String.purple, new string[] { "Purple", "Фиолетовая" } },
			{ Resource.String.main_dark, new string[] { "Dark", "Темная" } },
			{ Resource.String.deep_water, new string[] { "Deep water", "Глубоководная" } },
			{ Resource.String.dark_purple, new string[] { "Dark purple", "Темно-фиолетовая" } },

			{ Resource.String.type, new string[] { "Type of task", "Тип задачи" } },

			{ Resource.String.search, new string[] { "Search", "Поиск" } },

			{ Resource.String.move_next, new string[] { "Move to next date", "Переместить на следующую дату" } },
			{ Resource.String.move_lower, new string[] { "Move to shorter date", "Переместить на более короткую дату" } },
			{ Resource.String.move_today, new string[] { "Move to list on today", "Переместить в список на сегодня" } },
			{ Resource.String.move_this_month, new string[] { "Move to list on month", "Переместить в список на месяц" } },
			{ Resource.String.move_this_year, new string[] { "Move to list on year", "Переместить в список на год" } },
			{ Resource.String.move_main_tasks, new string[] { "Move to main list", "Переместить в главный список" } },

			{ Resource.String.edit, new string[] { "Edit", "Редактировать" } },
			{ Resource.String.delete, new string[] { "Delete", "Удалить" } },

			{ Resource.String.last_day, new string[] { "Yesterday", "Вчера" } },
			{ Resource.String.this_day, new string[] { "Today", "Сегодня" } },
			{ Resource.String.next_day, new string[] { "Tomorrow", "Завтра" } },
			{ Resource.String.last_month, new string[] { "Last month", "Старый месяц" } },
			{ Resource.String.this_month, new string[] { "This month", "Этот месяц" } },
			{ Resource.String.next_month, new string[] { "Next month", "Новый месяц" } },
			{ Resource.String.last_year, new string[] { "Last year", "Старый год" } },
			{ Resource.String.this_year, new string[] { "This year", "Этот год" } },
			{ Resource.String.next_year, new string[] { "Next year", "Новый год" } },
			{ Resource.String.global, new string[] { "Main tasks", "Главные задачи" } },

			{ Resource.String.sunday_short, new string[] { "Sun.", "Вс" } },
			{ Resource.String.monday_short, new string[] { "Mon.", "Пн" } },
			{ Resource.String.tuesday_short, new string[] { "Tues.", "Вт" } },
			{ Resource.String.wednesday_short, new string[] { "Wed.", "Ср" } },
			{ Resource.String.thursday_short, new string[] { "Thur.", "Чт" } },
			{ Resource.String.friday_short, new string[] { "Fri.", "Пт" } },
			{ Resource.String.saturday_short, new string[] { "Sat.", "Сб" } },

			{ Resource.String.text_step_one, new string[] {
				"In the center is the main list in the application, for viewing " +
				"tasks for four time intervals: day, month, year and without time.",
				"По центру расположен главный список в приложении, для просмотра задач " +
				"на четыре временных периода: день, месяц, год и без времени." } },
			{ Resource.String.text_step_two, new string[] {
				"The date of the list is changed by the upper side arrows, the time period is changed by clicking on the text.",
				"Дата списка меняется верхними боковыми стрелками, временной период меняется нажатием на текст." } },
			{ Resource.String.text_step_three, new string[] {
				"On the left there is information about the user: the progress of completed tasks, settings, repeated training.",
				"Слева расположена информация о пользователе: прогресс выполненных задач, настройки, повторное обучение." } },
			{ Resource.String.text_step_four, new string[] {
				"Next comes the calendar, here the task is added on the selected date.",
				"Далее идет календарь, здесь задача добавляется на выбранную дату." } },
			{ Resource.String.text_step_five, new string[] {
				"The Council:\n\nA task with a specific date should be immediately " +
				"entered into the calendar, this helps not to stuff up your head.",
				"Совет:\n\nЗадачу с конкретной датой следует сразу заносить в календарь, это помогает не загружать голову." } },
			{ Resource.String.text_step_six, new string[] {
				"Next comes a list for tasks that cannot be assigned to other lists at once.\n" +
				"The list is also intended for notes that can be found using the search bar.",
				"Далее идет список для задач, которые не получается определить в другие списки сразу.\n" +
				"Также список предназначен для заметок, которые можно найти с помощью поисковой строки." } },
			{ Resource.String.text_step_seven, new string[] {
				"On the right there is a schedule divided into hourly segments.\n" +
				"To be displayed in the schedule, the task must have a start and end time.\n" +
				"Clicking on the selected hour opens the task creation window.",
				"Справа расположено расписание, разделенное на часовые отрезки.\n" +
				"Для отображения в расписании задание должно иметь время начала и конца.\n" +
				"Нажав на выбранный час открывается окно создания задачи." } },
			{ Resource.String.text_step_eight, new string[] {
				"The Council:\n\nBetween tasks, leave free time for unforeseen situations and avoid excessive planning.\n" +
				"A useful habit is to create a task list in the evening of the previous day and edit the list in the morning.",
				"Совет:\n\nМежду задачами оставляйте свободное время на непредвиденные ситуации и избегайте избыточного планирования.\n" +
				"Полезной привычкой будет создание списка задач вечером предыдущего дня и редактирование списка утром." } },
			{ Resource.String.text_step_nine, new string[] { "That's all, good lack", "На этом все, удачи." } }
		};
	}
}