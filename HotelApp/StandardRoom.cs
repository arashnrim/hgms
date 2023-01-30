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
        public bool RequireWifi { get; set; }

        public bool RequireBreakfast { get; set; }

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
            double dailyRate = 0;
            switch (BedConfiguration)
            {
                case "Single":
                    dailyRate += 90;
                    break;
                case "Twin":
                    dailyRate += 110;
                    break;
                case "Triple":
                    dailyRate += 120;
                    break;
            }

            if (RequireWifi) dailyRate += 10;
            if (RequireBreakfast) dailyRate += 20;

            return dailyRate;
        }

        public override string ToString()
        {
            return base.ToString() + $" RequireWifi: {RequireWifi.ToString()}, RequireBreakfast: {RequireBreakfast.ToString()}";
        }
    }
}
