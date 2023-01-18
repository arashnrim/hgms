//==========================================================
// Student Number   : REDACTED
// Student Name     : Sim Ri Sheng
//==========================================================
namespace HotelApp
{
    internal class DeluxeRoom : Room
    {
        private bool additionalBed;
        public bool AdditionalBed
        {
            get { return additionalBed; } 
            set { additionalBed = value; }
        }
        public DeluxeRoom() { }

        public DeluxeRoom(int i, string s, double d, bool b)
        {
            RoomNumber = i;
            BedConfiguration = s;
            DailyRate = d;
            IsAvail = b;
        }

        public override double CalculateCharges()
        {
            double Daily_Rate = 0;
            if (AdditionalBed == true)
            {
                Daily_Rate += 25;
            }


            if (BedConfiguration == "Twin")
            {
                Daily_Rate += 140;
            }
            else if (BedConfiguration == "Triple")
            {
                Daily_Rate += 210;
            }


            return Daily_Rate;
        }
        public override string ToString()
        {
            return $"AdditionalBed: {AdditionalBed}";
        }
    }
}
