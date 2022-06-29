using Organizer.Internal.Data;
using Organizer.Internal.Model.Task;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Organizer.Internal.Model
{
    public class ListTasks : IEnumerable
    {
        public enum Mode { All }
        public static readonly string ListSep = "_";

        private List<BaseTask> _tasks;

        public ListTasks ()
        {
            _tasks = new List<BaseTask>();
        }

        public ListTasks (string sList) : this()
        {
            if (sList == "")
            {
                return;
            }
            foreach (string sTask in sList.Split(ListSep))
            {
                if (sTask == "")
                {
                    continue;
                }
                int typeTask = Int32.Parse(sTask.Split(BaseTask.TaskSep)[0].ToString());
                switch (typeTask)
                {
                    case (int) BaseTask.Type.Project:
                        _tasks.Add(new Project(sTask));
                        break;
                    case (int) BaseTask.Type.Regular:
                        _tasks.Add(new Regular(sTask));
                        break;
                    case (int) BaseTask.Type.Routine:
                        _tasks.Add(new Routine(sTask));
                        break;
                }
            }
        }

        public string Archive (Mode mode, string Sep = null, string final = "")
        {
            if (Sep is null)
            {
                Sep = ListSep;
            }
            foreach (BaseTask task in _tasks)
            {
                final += Sep + task.ToString();
            }
            return final == "" ? "" : final[Sep.Length..];
        }

        public BaseTask this[int position] => _tasks[position];

        public int Count => _tasks.Count;

        public void Add (BaseTask task) => _tasks.Add(task);

        public void Sort () => _tasks.Sort((taskOne, taskTwo) => TaskSorter.Compare(taskOne, taskTwo));

        public static ListTasks operator + (ListTasks listOne, ListTasks listTwo)
            => new ListTasks(listOne.Archive(Mode.All) + ListSep + listTwo.Archive(Mode.All));

        #region IEnumerable
        public IEnumerator GetEnumerator () => (IEnumerator) new TaskEnum(_tasks);

        public class TaskEnum : IEnumerator<BaseTask>
        {
            private BaseTask[] _tasks;
            private int _position = -1;

            public TaskEnum (List<BaseTask> tasks)
            {
                _tasks = tasks.ToArray();
            }

            public BaseTask Current => _tasks[_position];

            object IEnumerator.Current => Current;

            public void Dispose () => _tasks = null;

            public bool MoveNext ()
            {
                _position++;
                return (_position < _tasks.Length);
            }

            public void Reset () => _position = -1;
        }
        #endregion
    }
}