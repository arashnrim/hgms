//==========================================================
// Student Number   : REDACTED
// Student Name     : Sim Ri Sheng
//==========================================================

namespace HotelApp
{
    /// <summary>
    /// Represents a basic room within the hotel.
    /// <para>
    /// Standard rooms allow for Wi-Fi and breakfast add-ons to be purchased.
    /// </para>
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardRoom"/> class.
        /// </summary>
        public StandardRoom() {}
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardRoom"/> class with the room
        /// number, bed configuration, daily rate, and availability.
        /// </summary>
        /// <param name="i">The room number. It should be a unique value.</param>
        /// <param name="s">The configuration of the beds in the room.</param>
        /// <param name="d">The daily cost of occupying the room.</param>
        /// <param name="b">The availability of the room for check-in.</param>
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
            return $"RoomNumber: {RoomNumber.ToString()}, BedConfiguration: {BedConfiguration}, DailyRate: {DailyRate.ToString()}, IsAvail: {IsAvail.ToString()}, RequireWifi: {RequireWifi.ToString()}, RequireBreakfast: {requireBreakfast.ToString()}";
        }
    }
}
