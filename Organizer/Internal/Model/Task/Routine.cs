namespace Organizer.Internal.Model.Task
{
    class Routine : BaseTask
    {
        private readonly string _sDays;

        public string SDays => _sDays;

        public Routine (int typeTask, string title, string text, string startTime, string endTime, string sDays) :
            base((int) BaseTask.Type.Routine, title, text, startTime, endTime)
        {
            _sDays = sDays;
        }

        public Routine (string sTask) : base(sTask)
        {
            _sDays = sTask.Split(TaskSep)[6];
        }

        public override string ToString () => base.ToString() + TaskSep + SDays;
    }
}