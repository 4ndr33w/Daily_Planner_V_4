using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Planner_V_4
{
    public struct StrDataRepository
    {
        private string _grp_creation_my_or_delegated_mode; // = "my_grp";
        private string _create_or_edit_note_mode; // = "create";
        private string _btn_visibility; // = "Hidden";

        private static readonly string _save_path = /*Properties.Resources.save_path_;*/ Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // ConfigurationManager.AppSettings.Get("xml_save_file_path");
        public static readonly string directory = _save_path + @"\Planner";
        public static readonly string note_short_fileName = @"\Note_Data.xml";
        public static readonly string note_full_filePath = directory + note_short_fileName;
        public static readonly string expired_note_short_filePath = @"\Expired_Note_Data.xml";
        public static readonly string expired_note_full_filePath = directory + expired_note_short_filePath;
        public static readonly string completed_note_short_filePath = @"\Completed_Note_Data.xml";
        public static readonly string completed_note_full_filePath = directory + completed_note_short_filePath;
        public static readonly string grps_short_fileName = @"\Grps.xml";
        public static readonly string grps_fullFilePath = directory + grps_short_fileName;

        public static readonly string Visibility_hidden = "Hidden";
        public static readonly string Visibility_visible = "Visible";
        public static readonly string Status_Urgent_En = "!!! Urgent !!!";
        public static readonly string Status_Urgent_Ru = "!!! Срочно !!!";
        public static readonly string Status_Expired_En = "!!!   Expired   !!!";
        public static readonly string Status_Expired_Ru = "!!!   Просрочено   !!!";
        public static readonly string Status_Completed_En = "!!!   Task Completed   !!!";
        public static readonly string Status_Completed_Ru = "!!!   Задача Завершена   !!!";

        public static readonly string Executor_Me_En = "Me";
        public static readonly string Executor_Me_Ru = "Я";
        public static readonly string Executor_Deleg_En = "Delegated";
        public static readonly string Executor_Deleg_Ru = "Поручено";

        //public string

        public string Directory { get => directory; }
        public string Grps_Full_FilePath { get => grps_fullFilePath; }
        public string Grps_Short_FileName { get => grps_short_fileName; }
        public string Note_Data_Save { get => note_full_filePath; }
        public string Completed_Note_Data_Save { get => completed_note_full_filePath; }
        public string Expired_Note_Data_Save { get => expired_note_full_filePath; }

        public string GrpCreation_MyOrDelegated_Mode
        {
            get => _grp_creation_my_or_delegated_mode;
            set => _grp_creation_my_or_delegated_mode = value == "delegated_grp" ? value : "my_grp";
        }
        public string CreateOrEditNote_Mode { get => _create_or_edit_note_mode; set => _create_or_edit_note_mode = value == "edit" ? value : "create"; }
        public string Btn_Visibility { get => _btn_visibility; set => _btn_visibility = value == "Visible" ? value : "Hidden"; }
    }
}
