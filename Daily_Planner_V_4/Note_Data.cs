using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Planner_V_4
{
    public class Note_Data : Group_of_Notes
    {
        protected DateTime _date;
        protected string _note;
        protected string _header;
        protected string _executor;
        protected DateTime _creation_date;
        protected bool _status = false;
        protected bool _urgency;
        protected Group_of_Notes _group;

        protected Group_of_Notes _default_group = new Group_of_Notes();

        public DateTime Date { get => _date; set => _date = value; }
        public string Note { get => _note; set => _note = value; }
        public string Header { get => _header; set => _header = value; }
        public string Executor { get => _executor; set => _executor = value; }
        public DateTime Creation_Date { get => _creation_date; set => _creation_date = value; }
        public bool Status { get => _status; set => _status = value; }
        public bool Urgency { get => _urgency; set => _urgency = value; }
        public Group_of_Notes Group { get => _group; set => _group = value; }

        public Note_Data()
        {
            _date = DateTime.Now;
            _note = String.Empty;
            _header = Properties.Languages.Lang.Note_Default_Header;
            _executor = Properties.Languages.Lang.Executor_string_Me;
            _creation_date = DateTime.Now;
            _status = false;
            _urgency = false;
            _group = _default_group;
        }
        public Note_Data(Note_Data note)
        {
            _date = note.Date;
            _note = note.Note;
            _header = note.Header;
            _executor = note.Executor;
            _creation_date = note.Creation_Date;
            _status = note.Status;
            _urgency = note.Urgency;
            _group = note.Group;
        }
        public Note_Data(DateTime date, string note, string header, string executor, DateTime creation_date, bool status, bool urgency, Group_of_Notes grp)
        {
            _date = date;
            _note = note;
            _header = header;
            _executor = executor;
            _creation_date = creation_date;
            _status = status;
            _urgency = urgency;
            _group = grp;
            _color = grp.Color;
        }
        public Note_Data(Group_of_Notes grp)
        {
            this._color = grp.Color;
            _creation_date = DateTime.Now;
        }
        public class Sort_By_Group : IComparer<Note_Data>
        {
            public int Compare(Note_Data x, Note_Data y)
            {
                return string.Compare(x.Group.Group_Name, y.Group.Group_Name);
            }
        }
        public class Sort_By_Date : IComparer<Note_Data>
        {
            public int Compare(Note_Data x, Note_Data y)
            {
                return DateTime.Compare(x.Date, y.Date);
            }
        }
        public class Sort_By_CreationDate : IComparer<Note_Data>
        {
            public int Compare(Note_Data x, Note_Data y)
            {
                return DateTime.Compare(x.Creation_Date, y.Creation_Date);
            }
        }
        public class Sort_By_Executor : IComparer<Note_Data>
        {
            public int Compare(Note_Data x, Note_Data y)
            {
                return string.Compare(x.Group.Execution_of, y.Group.Execution_of);
            }
        }
    }
}
