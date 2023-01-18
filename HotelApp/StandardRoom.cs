//==========================================================
// Student Number   : REDACTED
// Student Name     : Sim Ri Sheng
//==========================================================

using System.Security.Cryptography.X509Certificates;

namespace HotelApp
{
    internal class StandardRoom :Room
    {
        private bool requireWifi;
        public bool RequireWifi
        {
            get { return requireWifi; }
            set { requireWifi = value; }
        }

        private bool requireBreakfast;
        public bool RequireBreakfast
        {
            get { return requireBreakfast; }
            set { requireBreakfast = value; }
        }

        public StandardRoom() {}
        public StandardRoom(int i, string s, double d, bool b)
        {
            RoomNumber = i;
            BedConfiguration= s;
            DailyRate = d;
            IsAvail= b;
        }
        public override double CalculateCharges()
        {
            double Daily_Rate = 0;
            if (BedConfiguration == "Single")
            {
                Daily_Rate += 90;
            }
            else if (BedConfiguration == "Twin")
            {
                Daily_Rate += 110;
            }
            else if (BedConfiguration == "Triple")
            {
                Daily_Rate += 120;
            }


            if (RequireWifi == true)
            {
                Daily_Rate += 10;
            }


            if (requireBreakfast == true)
            {
                Daily_Rate += 20;
            }
            return Daily_Rate;
        }
        public override string ToString()
        {
            return $"RequireWifi: {RequireWifi.ToString()},RequireBreakfast: {requireBreakfast.ToString()}";
        }
    }
}
