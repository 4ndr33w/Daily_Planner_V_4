using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Daily_Planner_V_4
{
    public class Group_Panel_Data : Group_of_Notes
    {
        private int _filtered_notes_count = 0;
        private string _delete_Grp_Btn_visibility = "Hidden";
        public int Filtered_notes_count { get => _filtered_notes_count; set => _filtered_notes_count = value; }
        public string Delete_Grp_Btn_visibility
        {
            get { return _delete_Grp_Btn_visibility; }
            set
            {
                if (value == "Visible")
                    _delete_Grp_Btn_visibility = value;
            }
        }

        public Group_Panel_Data()
        { }
        public Group_Panel_Data(Group_of_Notes grp, int note_counter, string del_btn_visib) : base(grp)
        {
            _color = grp.Color;
            _group_name = grp.Group_Name;
            _execution_of = grp.Execution_of;
            _filtered_notes_count = note_counter;
            _delete_Grp_Btn_visibility = del_btn_visib;
        }
        public Group_Panel_Data(Group_of_Notes grp) : base(grp)
        {
            _color = /*grp == null ? Colors.Salmon.ToString() :*/ grp.Color;
            _group_name = /*grp == null ? Properties.Languages.Lang.Default_string :*/ grp.Group_Name;
            _execution_of = /*grp == null ? Properties.Languages.Lang.Executor_string_Me :*/ grp.Execution_of;
        }
        public Group_Panel_Data(string color, string grp_name, string executor) : base(color, grp_name, executor)
        {
            _color = color ?? Colors.Salmon.ToString(); // : color;
            _group_name = grp_name ?? Properties.Languages.Lang.Default_string; // : grp_name;
            _execution_of = executor ?? Properties.Languages.Lang.Executor_string_Me; // : executor;
        }

        public Group_Panel_Data(string color, string group_name, string executor, int note_counter, string del_btn_visib) : base(color, group_name, executor)
        {
            _color = color;
            _group_name = group_name;
            _execution_of = executor;
            _filtered_notes_count = note_counter;
            _delete_Grp_Btn_visibility = del_btn_visib;
        }
    }
}
