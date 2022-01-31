using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Daily_Planner_V_4
{
    internal class Note_Counter_Fills
    {
        private int _today_counter;
        private int _group_note_counter;

        public int Today_Counter (ObservableCollection<Note_Template> note_data)
        {
            _today_counter = 0;
            foreach (var note in note_data)
            {
                if (note.Date.ToShortDateString() == DateTime.Today.ToShortDateString()) _today_counter++;
            }
            return _today_counter;
        }
        public int Group_Counter (ObservableCollection<Note_Template> note_data, Group_of_Notes grp_for_comparsion)
        {
            _group_note_counter = 0;
            if (note_data != null)
            {
                foreach (var note in note_data)
                {
                    if (grp_for_comparsion.Grp_equals(note.Group)) _group_note_counter++;
                }
            }
            return _group_note_counter;
        }
    }
}
