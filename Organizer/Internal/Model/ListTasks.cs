using Organizer.Internal.Data;
using Organizer.Internal.Model.Task;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Organizer.Internal.Model
{
    public class ListTasks : IEnumerable
    {
        public enum TaskCounter { WithoutProject, Complete_WithoutProject }
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
                try
                {
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
                catch (Exception) { }
            }
        }

        public string Archive (string Sep = null, string final = "")
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

        public void Remove(BaseTask removeTask)
        {
            foreach (BaseTask task in _tasks)
            {
                if (task is Project)
                {
                    (task as Project).Tasks.Remove(removeTask);
                }
                if (removeTask.Equals(task))
                {
                    _tasks.Remove(task);
                    return;
                }
            }
        }

        public ListTasks CutRoutines ()
        {
            ListTasks routines = new ListTasks();
            foreach (BaseTask task in _tasks)
            {
                if (task is Routine)
                {
                    routines.Add(task);
                }
            }
            foreach (BaseTask routine in routines)
            {
                _tasks.Remove(routine);
            }
            return routines;
        }

        public ListTasks CutUncompleteTask ()
        {
            ListTasks uncompletes = new ListTasks();
            foreach (BaseTask task in _tasks)
            {
                if (task.Complete == false && task is Routine == false)
                {
                    uncompletes.Add(task);
                }
            }
            foreach (BaseTask uncomplete in uncompletes)
            {
                _tasks.Remove(uncomplete);
            }
            return uncompletes;
        }

        public ListTasks GetRootList(BaseTask findTask)
        {
            ListTasks rootList;
            foreach (BaseTask task in _tasks)
            {
                if (task is Project)
                {
                    rootList = (task as Project).Tasks.GetRootList(findTask);
                    if (rootList != null)
                    {
                        return rootList;
                    }
                }
                if (findTask.Equals(task))
                {
                    return this;
                }
            }
            return null;
        }

        public bool Contains(BaseTask containTask)
        {
            foreach (BaseTask task in _tasks)
            {
                if (task is Project && (task as Project).Tasks.Contains(containTask))
                {
                    return true;
                }
                if (containTask.Equals(task))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsAllTaskComplete ()
        {
            bool allTaskComplete = true;
            foreach (BaseTask task in _tasks)
            {
                if (task is Project)
                {
                    if ((task as Project).Tasks.IsAllTaskComplete())
                    {
                        task.Complete = true;
                    }
                    else
                    {
                        (task as Project).UncompleteWitoutAllTask();
                    }
                }
                if (task.Complete == false)
                {
                    allTaskComplete = false;
                }
            }
            return allTaskComplete;
        }

        public void Sort ()
        {
            IsAllTaskComplete();
            _tasks.Sort((taskOne, taskTwo) => TaskSorter.Compare(taskOne, taskTwo));
        }

        public void Sort (Comparison<BaseTask> comparison) => _tasks.Sort(comparison);

        public void Synchronize(ListTasks list)
        {
            Sort();
            list.Sort();
            int indexTask = 0;
            while (indexTask < Count)
            {
                if (_tasks[indexTask].Equals(list[indexTask]) == false)
                {

                }
            }
        }

        public int GetCountTasks (TaskCounter counter)
        {
            int count = 0;
            foreach (BaseTask task in _tasks)
            {
                if (task is Project)
                {
                    count += (task as Project).Tasks.GetCountTasks(counter);
                }
                else
                {
                    switch (counter)
                    {
                        case TaskCounter.WithoutProject:
                            count++;
                            break;
                        case TaskCounter.Complete_WithoutProject:
                            if (task.Complete)
                            {
                                count++;
                            }
                            break;
                    }
                }
            }
            return count;
        }

        public static ListTasks operator + (ListTasks listOne, ListTasks listTwo)
        {
            ListTasks finalList = new ListTasks(listOne.Archive());
            for (int i = 0; i < listTwo.Count; i++)
            {
                if (finalList.Contains(listTwo[i]) == false)
                {
                    finalList.Add(listTwo[i]);
                }
            }
            return finalList;
        }


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