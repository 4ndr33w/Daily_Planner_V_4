using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Globalization;


namespace Daily_Planner_V_4
{
    public class Group_of_Notes
    {
        protected string _color = Colors.LawnGreen.ToString();
        protected string _group_name = Properties.Languages.Lang.Default_Group_Name;
        protected string _execution_of = Properties.Languages.Lang.Executor_string_Me;

        public string Color { get => _color; set => _color = value; }
        public string Group_Name
        {
            get { return _group_name; }
            set
            {
                if (value.Length <= 13)
                {
                    _group_name = value;
                }
                else
                {
                    _group_name = value.Substring(0, 12) + '.';
                }
            }
        }
        public string Execution_of { get => _execution_of; set => _execution_of = value; }

        public Group_of_Notes()
        { }
        public Group_of_Notes(string color)
        {
            _color = color;
        }
        public Group_of_Notes(string color, string group_name, string executor = "Me")
        {
            _color = color == Colors.Transparent.ToString() ? _color : color;
            _group_name = group_name == null ?  _group_name : group_name;
            _execution_of = executor == null ?  _execution_of : executor;
        }
        public Group_of_Notes(Group_of_Notes grp)
        {
            if (grp != null)
            {
                _color = grp.Color == Colors.Transparent.ToString() ?  _color : grp.Color;
                _group_name = grp.Group_Name == null ? Properties.Languages.Lang.Default_string : grp.Group_Name;
                _execution_of = grp.Execution_of == null ?  _execution_of : grp.Execution_of;
            }
        }

        public int Localization_Compare_Method(string localizationFieldString)
        {
            CompareInfo exec_compareInfo_en = new CultureInfo("en-GB", false).CompareInfo;
            CompareInfo exec_compareInfo_ru = new CultureInfo("en-GB", false).CompareInfo;
            SortKey sort_key_en = exec_compareInfo_en.GetSortKey(this._execution_of);
            SortKey sort_key_ru = exec_compareInfo_ru.GetSortKey(localizationFieldString);

            return SortKey.Compare(sort_key_en, sort_key_ru);
        }
       
        public bool Grp_equals(Group_of_Notes compate_data)
        {
            bool result = false;

            if (Color == compate_data.Color && this.Group_Name == compate_data.Group_Name &&  
                (((Execution_of == StrDataRepository.Executor_Me_En || Execution_of == StrDataRepository.Executor_Me_Ru) && (compate_data.Execution_of == StrDataRepository.Executor_Me_En || compate_data.Execution_of == StrDataRepository.Executor_Me_Ru) ) || 
                ((this.Execution_of == StrDataRepository.Executor_Deleg_En || this.Execution_of == StrDataRepository.Executor_Deleg_Ru) && (compate_data.Execution_of == StrDataRepository.Executor_Deleg_En || compate_data.Execution_of == StrDataRepository.Executor_Deleg_Ru))))
            {
                result = true;
                return result;
            }
            else return result;
        }

        public bool my_grps_equals(Group_of_Notes compate_data)
        {
            bool result = false;

            if (this.Color == compate_data.Color && this.Group_Name == compate_data.Group_Name)
            {
                if (compate_data.Execution_of == StrDataRepository.Executor_Me_En || compate_data.Execution_of == StrDataRepository.Executor_Me_Ru)
                {
                    if (this.Execution_of == StrDataRepository.Executor_Me_En || this.Execution_of == StrDataRepository.Executor_Me_Ru)
                        result = true;
                }
                return result;
            }
            else return result;
        }
        public bool deleg_grps_equals(Group_of_Notes compate_data)
        {
            bool result = false;

            if (this.Color == compate_data.Color && this.Group_Name == compate_data.Group_Name)
            {
                if (compate_data.Execution_of == StrDataRepository.Executor_Deleg_En || compate_data.Execution_of == StrDataRepository.Executor_Deleg_Ru)
                {
                    if (this.Execution_of == StrDataRepository.Executor_Deleg_En || this.Execution_of == StrDataRepository.Executor_Deleg_Ru)
                        result = true;
                }
                return result;
            }
            else return result;
        }

        public bool Grp_Executor_Equals(Group_of_Notes compate_data)
        {
            if ((((this.Execution_of == StrDataRepository.Executor_Me_En || this.Execution_of == StrDataRepository.Executor_Me_Ru) && (compate_data.Execution_of == StrDataRepository.Executor_Me_En || compate_data.Execution_of == StrDataRepository.Executor_Me_Ru))) ||
                ((this.Execution_of == StrDataRepository.Executor_Deleg_En || this.Execution_of == StrDataRepository.Executor_Deleg_Ru) && (compate_data.Execution_of == StrDataRepository.Executor_Deleg_En || compate_data.Execution_of == StrDataRepository.Executor_Deleg_Ru)))
                return true;
            else return false;
        }
    }
}
