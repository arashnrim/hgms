//==========================================================
// Student Number   : REDACTED
// Student Name     : Arash Nur Iman
//==========================================================

namespace HotelApp
{
    /// <summary>
    /// Represents a guest's stay at the hotel.
    /// <para>
    /// <c>Stay</c> contains information for a single time period that a guest stays at the hotel. It keeps
    /// track of the guest's check-in and check-out dates as well as a list of rooms the guest has stayed in.
    /// </para>
    /// </summary>
    internal class Stay
    {
        public DateTime CheckinDate { get; set; }

        public DateTime CheckoutDate { get; set; }
        
        public List<Room> RoomList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stay"/> class.
        /// </summary>
        public Stay()
        {
            RoomList = new List<Room>() { };
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Stay"/> class with the required parameters.
        /// </summary>
        /// <param name="cid">The date the guest checks into the hotel.</param>
        /// <param name="cod">The date the guest checks out of the hotel.</param>
        public Stay(DateTime cid, DateTime cod)
        {
            CheckinDate = cid;
            CheckoutDate = cod;
            RoomList = new List<Room>();
        }

        /// <summary>
        /// Adds a room to the list of rooms for this stay.
        /// </summary>
        /// <param name="r">A Room object denoting the room to be associated with this stay.</param>
        public void AddRoom(Room r)
        {
            RoomList.Add(r);
        }

        /// <summary>
        /// Calculates the total cost of the stay.
        /// </summary>
        /// <returns>A double denoting the total cost a guest needs to pay for their stay.</returns>
        public double CalculateTotal()
        {
            double total = 0;
            foreach (Room room in RoomList)
            {
                total += room.CalculateCharges();
            }
            return total;
        }

        public override string ToString()
        {
            return $"Checkin date: {CheckinDate:dd/MM/yyyy}, Checkout date: {CheckoutDate:dd/MM/yyyy}, Number of rooms {RoomList.Count}";
        }
    }
}
