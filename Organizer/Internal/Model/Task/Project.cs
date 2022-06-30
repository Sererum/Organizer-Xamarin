using Android.Util;
using Android.Views;
using System;
using System.Linq;

namespace Organizer.Internal.Model.Task
{
    class Project : BaseTask
    {
        public static readonly string ProjectSep = "^";

        private ListTasks _tasks;
        private bool _complete;

        public ListTasks Tasks => _tasks;
        public override bool Complete
        {
            get { return _complete; }
            set
            {
                CompleteProject(value);
                _complete = value;
            }
        }

        public bool TasksVisible { get; set; }

        public Project (string title, string text, string startTime, string endTime, ListTasks tasks) :
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
                        int indexInternalTask = indexTask + 1;
                        while (indexInternalTask < indexTask + countProjectTasks + 1)
                        {
                            string sInternalTask = arrayProjectTasks[indexInternalTask];
                            int typeInternalTask = Int32.Parse(sInternalTask[0].ToString());
                            if (typeInternalTask == (int) BaseTask.Type.Project)
                            {
                                int countInternalProjectTasks = Int32.Parse(sInternalTask.Split(TaskSep)[7]);
                                countProjectTasks += countInternalProjectTasks;
                            }
                            indexInternalTask++;
                        }

                        string[] aProject = new ArraySegment<string>(arrayProjectTasks, indexTask, countProjectTasks + 1).ToArray();
                        string sProject = String.Join(ProjectSep, aProject);
                        Log.Debug("Project", sProject);
                        _tasks.Add(new Project(sProject));
                        indexTask += countProjectTasks;
                        break;
                    case (int) BaseTask.Type.Regular:
                        _tasks.Add(new Regular(sProjectTask));
                        break;
                    default:
                        throw new ArgumentException();
                }
                indexTask++;
            }
        }

        private void CompleteProject (bool complete)
        {
            if (Tasks is null)
            {
                return;
            }
            foreach (BaseTask task in Tasks)
            {
                task.Complete = complete;
            }
        }

        public void UncompleteWitoutAllTask () => _complete = false;

        public override string ToString () => base.ToString() + TaskSep + (TasksVisible ? "1" : "0") +
            TaskSep + _tasks.Count + TaskSep + ProjectSep + _tasks.Archive(ListTasks.Mode.All, ProjectSep);
    }
}