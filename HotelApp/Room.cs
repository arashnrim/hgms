//==========================================================
// Student Number   : REDACTED
// Student Name     : Sim Ri Sheng
//==========================================================

namespace HotelApp
{
    /// <summary>
    /// An abstract class that represents a room in a hotel.
    /// <para>
    /// The abstract class holds the common properties and methods of all rooms,
    /// including information about the room's number, bed configuration, daily
    /// rate, and availability.
    /// </para>
    /// </summary>
    internal abstract class Room
    {
        public int RoomNumber { get; set; }

        public string BedConfiguration { get; set; }

        public double DailyRate { get; set; }

        public bool IsAvail { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        public Room () {}
        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class with the room
        /// number, bed configuration, daily rate, and availability.
        /// </summary>
        /// <param name="rn">The room number. It should be a unique value.</param>
        /// <param name="bc">The configuration of the beds in the room.</param>
        /// <param name="dr">The daily cost of occupying the room.</param>
        /// <param name="ia">The availability of the room for check-in.</param>
        public Room(int rn, string bc, double dr, bool ia)
        {
            RoomNumber = rn;
            BedConfiguration = bc;
            DailyRate = dr;
            IsAvail = ia;
        }

        /// <summary>
        /// Calculates the total cost of occupying the room for a given number of days.
        /// </summary>
        /// <returns>A double denoting how much is payable for the room.</returns>
        public abstract double CalculateCharges();

        public override string ToString()
        {
            return $"RoomNumber: {RoomNumber}, BedConfiguration: {BedConfiguration}, DailyRate: {DailyRate}, IsAvail: {IsAvail}";
        }
    }
}
