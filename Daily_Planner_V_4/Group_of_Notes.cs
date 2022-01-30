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
        protected string _color = Colors.Salmon.ToString();
        protected string _group_name = Properties.Languages.Lang.Default_string;
        protected string _execution_of = Properties.Languages.Lang.Executor_string_Me;

        //private static const string default_color = Colors.Salmon.ToString();
        //private string default_executor = Properties.Languages.Lang.Executor_string_Me;

        //private string test;

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
        {
            _color = Colors.Salmon.ToString();
            _group_name = Properties.Languages.Lang.Default_string;
            _execution_of = Properties.Languages.Lang.Executor_string_Me;
        }
        public Group_of_Notes(string color)
        {
            _color = color;
            _group_name = Properties.Languages.Lang.Default_string;
            _execution_of = Properties.Languages.Lang.Executor_string_Me;
        }
        public Group_of_Notes(string color, string group_name, string executor = "Me")
        {
            _color = color == Colors.Transparent.ToString() ? Colors.Salmon.ToString() : color;
            _group_name = group_name == null ? Properties.Languages.Lang.Default_string : group_name;
            _execution_of = executor == null ? Properties.Languages.Lang.Executor_string_Me : executor;
        }
        public Group_of_Notes(Group_of_Notes grp)
        {
            if (grp != null)
            {
                _color = grp.Color == Colors.Transparent.ToString() ? Colors.Salmon.ToString() : grp.Color;
                _group_name = grp.Group_Name == null ? Properties.Languages.Lang.Default_string : grp.Group_Name;
                _execution_of = grp.Execution_of == null ? Properties.Languages.Lang.Executor_string_Me : grp.Execution_of;
            }
        }

        public int Localization_Compare_Method(string localizationFieldString)
        {
            CompareInfo exec_compareInfo_en = new CultureInfo("en-GB", false).CompareInfo;
            CompareInfo exec_compareInfo_ru = new CultureInfo("en-GB", false).CompareInfo;
            SortKey sort_key_en = exec_compareInfo_en.GetSortKey(this._execution_of);
            SortKey sort_key_ru = exec_compareInfo_ru.GetSortKey(localizationFieldString);
            //MessageBox.Show(SortKey.Compare(sort_key_en, sort_key_ru).ToString();
            return SortKey.Compare(sort_key_en, sort_key_ru);
        }
        //public override string ToString()
        //{
        //    string s = this.Group_Name + ' ' + this.Color + ' ' + this._execution_of;
        //    return s; // base.ToString();
        //}
        public bool Grp_equals(Group_of_Notes compate_data)
        {
            //MainWindow Form1 = new MainWindow();
            bool result = false;

            if (Color == compate_data.Color && this.Group_Name == compate_data.Group_Name &&  
                (((Execution_of == StrDataRepository.Executor_Me_En || Execution_of == StrDataRepository.Executor_Me_Ru) && (compate_data.Execution_of == StrDataRepository.Executor_Me_En || compate_data.Execution_of == StrDataRepository.Executor_Me_Ru) ) || 
                ((this.Execution_of == StrDataRepository.Executor_Deleg_En || this.Execution_of == StrDataRepository.Executor_Deleg_Ru) && (compate_data.Execution_of == StrDataRepository.Executor_Deleg_En || compate_data.Execution_of == StrDataRepository.Executor_Deleg_Ru))))
            {
                result = true;
                return result;
            }


            //    ((Execution_of == StrDataRepository.Executor_Me_En || Execution_of == StrDataRepository.Executor_Me_Ru) && (compate_data.Execution_of == StrDataRepository.Executor_Me_En || compate_data.Execution_of == StrDataRepository.Executor_Me_Ru) )
                
            //    )


            //if (this.Color == compate_data.Color && this.Group_Name == compate_data.Group_Name)
            //{
            //    if (compate_data.Execution_of == StrDataRepository.Executor_Me_En || compate_data.Execution_of == StrDataRepository.Executor_Me_Ru)
            //    {
            //        if (this.Execution_of == StrDataRepository.Executor_Me_En || this.Execution_of == StrDataRepository.Executor_Me_Ru)
            //            result = true;
            //    }
            //    else if (compate_data.Execution_of == StrDataRepository.Executor_Deleg_En || compate_data.Execution_of == StrDataRepository.Executor_Deleg_Ru)
            //    {
            //        if (this.Execution_of == StrDataRepository.Executor_Deleg_En || this.Execution_of == StrDataRepository.Executor_Deleg_Ru)
            //            result = true;
            //    }
            //    return result;
            //}
            else return result;

        }

        public bool my_grps_equals (Group_of_Notes compate_data)
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
            //MainWindow Form1 = new MainWindow();
            if ((((this.Execution_of == StrDataRepository.Executor_Me_En || this.Execution_of == StrDataRepository.Executor_Me_Ru) && (compate_data.Execution_of == StrDataRepository.Executor_Me_En || compate_data.Execution_of == StrDataRepository.Executor_Me_Ru))) ||
                ((this.Execution_of == StrDataRepository.Executor_Deleg_En || this.Execution_of == StrDataRepository.Executor_Deleg_Ru) && (compate_data.Execution_of == StrDataRepository.Executor_Deleg_En || compate_data.Execution_of == StrDataRepository.Executor_Deleg_Ru)))
                return true;
            else return false;
        }

        //public override bool Equals(Group_of_Notes compare_data)
        //{
        //    MainWindow Form1 = new MainWindow();
        //    if (this.Color == compare_data.Color && this.Group_Name == compare_data.Group_Name &&
        //        Form1.Localization_Compare_Method(this._execution_of, compare_data.Execution_of) == 0)
        //        return true;
        //    else return false;
        //    //return base.Equals(obj);
        //}
    }
}
