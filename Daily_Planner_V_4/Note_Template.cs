using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Globalization;

namespace Daily_Planner_V_4
{
    public class Note_Template : Note_Data
    {
        MainWindow Form1;
        private string _urgent_or_expire_or_complete_title = string.Empty;
        private string _delete_btn_visibility = StrDataRepository.Visibility_hidden;
        private string _edit_btn_visibility = StrDataRepository.Visibility_hidden;
        private string _foreground = Colors.Black.ToString();
        private string _expired_foreground = Colors.White.ToString();
        private string _btn_hide_completed_or_expired_note_visibility = StrDataRepository.Visibility_hidden;
        private string _mark_to_complete_note_visibility = StrDataRepository.Visibility_hidden;
        private string _expired;

        public string Status_Title { get => _urgent_or_expire_or_complete_title; set => _urgent_or_expire_or_complete_title = value; }
        public string Delete_Btn_Visibility { get => _delete_btn_visibility; set => _delete_btn_visibility = value; }
        public string Edit_Btn_Visibility { get => _edit_btn_visibility; set => _edit_btn_visibility = value; }
        public string Foreground { get => _foreground; set => _foreground = value; }
        public string Expired_Foreground { get => _expired_foreground; set => _expired_foreground = value; }
        public string Btn_Hide_Compl_or_Exp_note_Visibility { get => _btn_hide_completed_or_expired_note_visibility; set => _btn_hide_completed_or_expired_note_visibility = value; }
        public string Mart_to_Complete_note_Visibility { get => _mark_to_complete_note_visibility; set => _mark_to_complete_note_visibility = value; }
        public string Expired { get => _expired; set => _expired = value; }
        public string Day_and_Month
        {
            get
            {
                string note_date_text;
                if (_date.ToShortDateString() != DateTime.Today.ToShortDateString())
                {
                    //string day = _date.ToString("d MMMM", CultureInfo.CreateSpecificCulture(Properties.Settings.Default.languageCode));
                    //string month = _date.ToString("MMMM", CultureInfo.CreateSpecificCulture(Properties.Settings.Default.languageCode));
                    note_date_text = _date.ToString("d MMMM", CultureInfo.CreateSpecificCulture(Properties.Settings.Default.languageCode));
                }
                if (_date.ToShortDateString() == DateTime.Today.ToShortDateString())
                {
                    note_date_text = Properties.Languages.Lang.Today_String;
                }
                else 
                {
                    note_date_text = _date.ToString("d MMMM", CultureInfo.CreateSpecificCulture(Properties.Settings.Default.languageCode));
                }
                return note_date_text;
            }
        }
        public string Time
        {
            get
            {
                string time = _date.ToString("HH' ':' ' mm");
                return time;
            }
        }
        public Note_Template()
        {
            _date = DateTime.Now;
            _note = String.Empty;
            _header = Properties.Languages.Lang.Note_Default_Header;
            _executor = Properties.Languages.Lang.Executor_string_Me;
            _creation_date = DateTime.Now;
            _status = "";
            _urgency = false;
            _group = _default_group;
            _color = _group.Color;

            _urgent_or_expire_or_complete_title = string.Empty;
            _delete_btn_visibility = StrDataRepository.Visibility_hidden;
            _edit_btn_visibility = StrDataRepository.Visibility_hidden;
            _foreground = Colors.Black.ToString();
            _expired_foreground = Colors.White.ToString();
            _btn_hide_completed_or_expired_note_visibility = StrDataRepository.Visibility_hidden;
            _mark_to_complete_note_visibility = StrDataRepository.Visibility_hidden;
            _expired = "";


        }
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
            _delete_btn_visibility = note._delete_btn_visibility == String.Empty ? StrDataRepository.Visibility_hidden : note._delete_btn_visibility;
            _edit_btn_visibility = note._edit_btn_visibility == String.Empty ? StrDataRepository.Visibility_hidden : note._edit_btn_visibility;
            _foreground = note._foreground == String.Empty ? Colors.Black.ToString() : note._foreground;
            _expired_foreground = note._expired_foreground == String.Empty ? Colors.White.ToString() : note._expired_foreground;
            _btn_hide_completed_or_expired_note_visibility = note._btn_hide_completed_or_expired_note_visibility == String.Empty ? StrDataRepository.Visibility_hidden : note._btn_hide_completed_or_expired_note_visibility;
            _mark_to_complete_note_visibility = note._mark_to_complete_note_visibility == String.Empty ? StrDataRepository.Visibility_hidden : note._mark_to_complete_note_visibility;
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
            _color = note.Group.Color;

            _urgent_or_expire_or_complete_title = string.Empty;
            _delete_btn_visibility = StrDataRepository.Visibility_hidden;
            _edit_btn_visibility = StrDataRepository.Visibility_hidden;
            _foreground = Colors.Black.ToString();
            _expired_foreground = Colors.White.ToString();
            _btn_hide_completed_or_expired_note_visibility = StrDataRepository.Visibility_hidden;
            _mark_to_complete_note_visibility = StrDataRepository.Visibility_hidden;
            _expired = "";
        }
    }
}
