using Android.Views;
using System;
using System.Linq;

namespace Organizer.Internal.Model.Task
{
    class Project : BaseTask
    {
        public static readonly string ProjectSep = "^";

        private ListTasks _tasks;

        public ListTasks Tasks => _tasks;

        public bool TasksVisible { get; set; }

        public Project (int typeTask, string title, string text, string startTime, string endTime, ListTasks tasks) :
            base((int) BaseTask.Type.Project, title, text, startTime, endTime)
        {
            _tasks = tasks;
            TasksVisible = true;
        }

        public Project (string sTask) : base(sTask)
        {
            _tasks = new ListTasks();
            TasksVisible = (sTask.Split(TaskSep)[6] == "1");

            string[] arrayProjectTasks = sTask.Split(ProjectSep);
            int indexTask = 1;
            while (indexTask < arrayProjectTasks.Length)
            {
                string sProjectTask = arrayProjectTasks[indexTask];
                if (sProjectTask == "")
                {
                    indexTask++;
                    continue;
                }
                int typeTask = Int32.Parse(sProjectTask[0].ToString());
                switch (typeTask)
                {
                    case (int) BaseTask.Type.Project:
                        int countProjectTasks = Int32.Parse(sProjectTask.Split(TaskSep)[7]);
                        string[] aProject = new ArraySegment<string>(arrayProjectTasks, indexTask, countProjectTasks + 1).ToArray();
                        string sProject = String.Join(ProjectSep, aProject);
                        _tasks.Add(new Project(sProject));
                        indexTask += countProjectTasks;
                        break;
                    case (int) BaseTask.Type.Regular:
                        _tasks.Add(new Regular(sTask));
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        public override string ToString () => base.ToString() + (TasksVisible ? "1" : "0") +
            TaskSep + _tasks.Count + TaskSep + ProjectSep + _tasks.Archive(ListTasks.Mode.All, ProjectSep);
    }
}