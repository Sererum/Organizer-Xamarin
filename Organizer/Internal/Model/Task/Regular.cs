using System;

namespace Organizer.Internal.Model.Task
{
    class Regular : BaseTask
    {
        private int _priority;

        public int Priority => _priority;

        public Regular (string title, string text, string startTime, string endTime, int priority) :
            base((int) BaseTask.Type.Regular, title, text, startTime, endTime)
        {
            _priority = priority;
        }

        public Regular (string sTask) : base(sTask)
        {
            _priority = Int32.Parse(sTask.Split(TaskSep)[6]);
        }

        public override string ToString () => base.ToString() + TaskSep + Priority;
    }
}