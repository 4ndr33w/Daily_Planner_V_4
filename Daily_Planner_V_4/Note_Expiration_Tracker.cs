using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Planner_V_4
{
    internal class Note_Expiration_Tracker
    {
        //private TimeSpan _expiration;
        private TimeSpan _ckecking;
        private DateTime _expirationDate;
        private bool _isExpiring;

        private DateTime Tracking_Note_Creation_Date
        {
            set { _expirationDate = value; }
        }
        private TimeSpan Expiration_Ckeck_Method(DateTime expiration_date)
        {
            return _ckecking = _expirationDate - DateTime.Now;
        }

        public bool IsExpiring(DateTime expiration_date)
        {
            _ckecking = Expiration_Ckeck_Method(expiration_date);
            _isExpiring = false;
            if (_ckecking.Minutes == 5)
                _isExpiring = true;
            return _isExpiring;
        }
    }
}
