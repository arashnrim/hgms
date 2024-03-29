﻿//==========================================================
// Student Number   : REDACTED
// Student Name     : Sim Ri Sheng
//==========================================================

namespace HotelApp
{
    /// <summary>
    /// Represents a premium room within the hotel.
    /// <para>
    /// Deluxe rooms have complimentary Wi-Fi and breakfast services. An
    /// additional bed may be requested for a surcharge.
    /// </para>
    /// </summary>
    internal class DeluxeRoom : Room
    {
        public bool AdditionalBed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeluxeRoom"/> class.
        /// </summary>
        public DeluxeRoom() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeluxeRoom"/> class with the room
        /// number, bed configuration, daily rate, and availability.
        /// </summary>
        /// <param name="i">The room number. It should be a unique value.</param>
        /// <param name="s">The configuration of the beds in the room.</param>
        /// <param name="d">The daily cost of occupying the room.</param>
        /// <param name="b">The availability of the room for check-in.</param>
        public DeluxeRoom(int i, string s, double d, bool b)
        {
            RoomNumber = i;
            BedConfiguration = s;
            DailyRate = d;
            IsAvail = b;
        }

        public override double CalculateCharges()
        {
            double dailyRate = 0;
            if (AdditionalBed) dailyRate += 25;

            switch (BedConfiguration)
            {
                case "Twin":
                    dailyRate += 140;
                    break;
                case "Triple":
                    dailyRate += 210;
                    break;
            }

            return dailyRate;
        }
        public override string ToString()
        {
            return base.ToString() + $" AdditionalBed: {AdditionalBed}";
        }
    }
}
