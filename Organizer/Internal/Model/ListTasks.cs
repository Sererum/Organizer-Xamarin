using Organizer.Internal.Model.Task;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Organizer.Internal.Model
{
    class ListTasks : IEnumerable
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
                int typeTask = Int32.Parse(sTask[0].ToString());
                switch (typeTask)
                {
                    case (int) BaseTask.Type.Project:
                        break;
                    case (int) BaseTask.Type.Regular:
                        break;
                    case (int) BaseTask.Type.Routine:
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
            return final;
        }

        public int Count => _tasks.Count;

        public void Add (BaseTask task)
        {
            _tasks.Add(task);
            Sort();
        }

        public int Sort () => 0;

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