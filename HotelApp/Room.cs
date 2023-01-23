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
        private int roomNumber;
        public int RoomNumber { 
            get { return roomNumber; }
            set { roomNumber = value; } 
        }

        private string bedConfiguration;
        public string BedConfiguration
        { 
            get { return bedConfiguration; }
            set { bedConfiguration = value; }
        }

        private double dailyRate;
        public double DailyRate
        {
            get { return dailyRate; }
            set { dailyRate = value; }
        }

        private bool isAvail;
        public bool IsAvail
        {
            get { return isAvail; }
            set { isAvail = value; }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        public Room () {}
        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class with the room
        /// number, bed configuration, daily rate, and availability.
        /// </summary>
        /// <param name="roomNum">The room number. It should be a unique value.</param>
        /// <param name="bedconfig">The configuration of the beds in the room.</param>
        /// <param name="dailyR">The daily cost of occupying the room.</param>
        /// <param name="isavil">The availability of the room for check-in.</param>
        public Room(int roomNum, string bedconfig, double dailyR, bool isavil)
        {
            RoomNumber = roomNum;
            BedConfiguration = bedconfig;
            DailyRate = dailyR;
            IsAvail = isavil;
        }

        /// <summary>
        /// Calculates the total cost of occupying the room for a given number of days.
        /// </summary>
        /// <returns>A double denoting how much is payable for the room.</returns>
        public abstract double CalculateCharges();

        public override string ToString()
        {
            return $"RoomNumber: {RoomNumber.ToString()}, BedConfiguration: {BedConfiguration}, DailyRate: {DailyRate.ToString()}, IsAvail: {IsAvail.ToString()}";
        }
    }
}
