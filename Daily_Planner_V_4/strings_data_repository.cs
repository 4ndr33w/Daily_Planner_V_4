using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Planner_V_4
{
    public struct strings_data_repository
    {
        private string _grp_creation_my_or_delegated_mode; // = "my_grp";
        private string _create_or_edit_note_mode; // = "create";
        private string _btn_visibility; // = "Hidden";

        private static string _save_path = /*Properties.Resources.save_path_;*/ Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // ConfigurationManager.AppSettings.Get("xml_save_file_path");
        private static string _file_path = _save_path + @"\Planner";
        private static string _note_data_save = _file_path + @"\Note_Data.xml";
        private static string _expired_note_data_save = _file_path + @"\Expired_Note_Data.xml";
        private static string _completednote_data_save = _file_path + @"\Completed_Note_Data.xml";
        private static string _note_grps_save = _file_path + @"\Grps.xml";
        private static string _grps_fileName = @"\Grps.xml";
        public string Save_Path { get => _file_path; }
        public string Grps_Save_Path { get => _note_grps_save; }
        public string Grps_FileName { get => _grps_fileName; }
        public string Note_Data_Save { get => _note_data_save; }

        public string GrpCreation_MyOrDelegated_Mode
        {
            get => _grp_creation_my_or_delegated_mode;
            set => _grp_creation_my_or_delegated_mode = value == "delegated_grp" ? value : "my_grp";
        }
        public string CreateOrEditNote_Mode { get => _create_or_edit_note_mode; set => _create_or_edit_note_mode = value == "edit" ? value : "create"; }
        public string Btn_Visibility { get => _btn_visibility; set => _btn_visibility = value == "Visible" ? value : "Hidden"; }
    }
}
