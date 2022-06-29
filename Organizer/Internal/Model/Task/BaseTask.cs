using System;

namespace Organizer.Internal.Model.Task
{
    public abstract class BaseTask
    {
        #region Fields and Properties

        public static readonly string TaskSep = "|";

        private int _typeTask;
        private string _title;
        private string _text;
        private string _startTime;
        private string _endTime;

        public int TypeTask => _typeTask;

        public string Title => _title;

        public string Text => _text;

        public string StartTime => _startTime;

        public string EndTime => _endTime;

        public bool Complete { get; set; }

        public enum Type { Project, Regular, Routine }

        #endregion

        public BaseTask (int typeTask, string title, string text, string startTime, string endTime)
        {
            _typeTask = typeTask;
            _title = ToStandart(title);
            _text = ToStandart(text);
            _startTime = startTime ?? throw new ArgumentNullException();
            _endTime = endTime ?? throw new ArgumentNullException();
            Complete = false;
        }

        public BaseTask (string sTask)
        {
            string[] properties = sTask.Split(TaskSep);

            _typeTask = Int32.Parse(properties[0]);
            _title = properties[1];
            _text = properties[2];
            _startTime = properties[3];
            _endTime = properties[4];
            Complete = properties[5] == "1";
        }

        public override string ToString () => _typeTask + TaskSep + _title + TaskSep +
            _text + TaskSep + _startTime + TaskSep + _endTime + TaskSep + (Complete ? "1" : "0");

        public static string ToStandart (string text, string Sep = " ", string final = "")
        {
            string[] words = text?.Split(Sep) ?? throw new ArgumentNullException();
            foreach (string word in words)
            {
                final += (word == "") ? "" : (Sep + word);
            }
            return (final == "") ? "" : final[(Sep.Length)..];
        }

        public bool Equals (BaseTask task) => (TypeTask == task.TypeTask) && (Title == task.Title) &&
            (Text == task.Text) && (StartTime == task.StartTime) && (EndTime == task.EndTime);
    }
}