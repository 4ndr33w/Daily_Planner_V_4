using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Planner_V_4
{
    public class Time_Range_For_AddNoteWndw_TimePicker
    {
        public List<string> Choose_Time_Range(string data)
        {

            List<string> time_range = new List<string>();

            if (data == "1 hour" || data == "1 час")
            {
                time_range.Clear();
                time_range = new List<string>();
                for (int i = 0; i < 24; i++)
                {
                    time_range.Add($"{i}:00");
                }
                return time_range;
            }
            if (data == "30 min" || data == "30 мин")
            {
                time_range.Clear();
                time_range = new List<string>();
                for (int i = 0; i < 24; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                time_range.Add($"{i}:00");
                                break;
                            case 1:
                                time_range.Add($"{i}:30");
                                break;
                        }
                    }
                }
                return time_range;
            }
            if (data == "10 min" || data == "10 мин")
            {
                time_range.Clear();
                time_range = new List<string>();
                for (int i = 0; i < 24; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        time_range.Add($"{i}:{j}0");
                    }

                }
                return time_range;
            }
            if (data == "5 min" || data == "5 мин")
            {
                time_range.Clear();
                time_range = new List<string>();
                for (int i = 0; i < 24; i++)
                {
                    for (int d = 0; d < 6; d++)
                    {
                        for (int m = 0; m < 2; m++)
                        {
                            switch (m)
                            {
                                case 0:
                                    time_range.Add($"{i}:{d}0");
                                    break;
                                case 1:
                                    time_range.Add($"{i}:{d}5");
                                    break;
                            }
                        }
                    }

                }
                return time_range;
            }
            else return null;
        }
    }
}
