using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Daily_Planner_V_4
{
    public class Note_Template : Note_Data
    {
        private string _urgent_or_expire_or_complete_title = string.Empty;
        private string _delete_btn_visibility = "Hidden";
        private string _edit_btn_visibility = "Hidden";
        private string _foreground = Colors.Black.ToString();
        private string _expired_foreground = Colors.White.ToString();
        private string _btn_hide_completed_or_expired_note_visibility = "Hidden";
        private string _mark_to_complete_note_visibility = "Hidden";
        private string _expired;

        public string Status_Title { get => _urgent_or_expire_or_complete_title; set => _urgent_or_expire_or_complete_title = value; }
        public string Delete_Btn_Visibility { get => _delete_btn_visibility; set => _delete_btn_visibility = value; }
        public string Edit_Btn_Visibility { get => _edit_btn_visibility; set => _edit_btn_visibility = value; }
        public string Foreground { get => _foreground; set => _foreground = value; }
        public string Expired_Foreground { get => _expired_foreground; set => _expired_foreground = value; }
        public string Btn_Hide_Compl_or_Exp_note_Visibility { get => _btn_hide_completed_or_expired_note_visibility; set => _btn_hide_completed_or_expired_note_visibility = value; }
        public string _Mart_to_Complete_note_Visibility { get => _mark_to_complete_note_visibility; set => _mark_to_complete_note_visibility = value; }
        public string Expired { get => _expired; set => _expired = value; }

        public Note_Template(Note_Template note) : base()
        {
            _date = note.Date;
            _note = note.Note;
            _header = note.Header;
            _executor = note.Executor;
            _creation_date = note.Creation_Date;
            _status = note.Status;
            _urgency = note.Urgency;
            _group = note.Group;

            _urgent_or_expire_or_complete_title = note._urgent_or_expire_or_complete_title == String.Empty ? string.Empty : note._urgent_or_expire_or_complete_title;
            _delete_btn_visibility = note._delete_btn_visibility == String.Empty ? "Hidden" : note._delete_btn_visibility;
            _edit_btn_visibility = note._edit_btn_visibility == String.Empty ? "Hidden" : note._edit_btn_visibility;
            _foreground = note._foreground == String.Empty ? Colors.Black.ToString() : note._foreground;
            _expired_foreground = note._expired_foreground == String.Empty ? Colors.White.ToString() : note._expired_foreground;
            _btn_hide_completed_or_expired_note_visibility = note._btn_hide_completed_or_expired_note_visibility == String.Empty ? "Hidden" : note._btn_hide_completed_or_expired_note_visibility;
            _mark_to_complete_note_visibility = note._mark_to_complete_note_visibility == String.Empty ? "Hidden" : note._mark_to_complete_note_visibility;
            _expired = note._expired == String.Empty ? "" : note._expired;
        }
        public Note_Template(Note_Data note) : base(note)
        {
            _date = note.Date;
            _note = note.Note;
            _header = note.Header;
            _executor = note.Executor;
            _creation_date = note.Creation_Date;
            _status = note.Status;
            _urgency = note.Urgency;
            _group = note.Group;

            _urgent_or_expire_or_complete_title = string.Empty;
            _delete_btn_visibility = "Hidden";
            _edit_btn_visibility = "Hidden";
            _foreground = Colors.Black.ToString();
            _expired_foreground = Colors.White.ToString();
            _btn_hide_completed_or_expired_note_visibility = "Hidden";
            _mark_to_complete_note_visibility = "Hidden";
            _expired = "";
        }
    }
}
