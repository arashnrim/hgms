//==========================================================
// Student Number   : REDACTED
// Student Name     : Arash Nur Iman
//==========================================================

namespace HotelApp
{
    internal class Stay
    {
        private DateTime checkinDate;
        public DateTime CheckinDate { get; set; }

        private DateTime checkoutDate;
        public DateTime CheckoutDate { get; set; }

        private List<Room> roomList;
        public List<Room> RoomList { get; set; }

        public Stay() { }
        public Stay(DateTime cid, DateTime cod, List<Room> rl)
        {
            CheckinDate = cid;
            CheckoutDate = cod;
            RoomList = rl;
        }

        public void AddRoom(Room r)
        {
            RoomList.Add(r);
        }

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
            return $"Checkin date: {CheckinDate.ToString("dd/MM/yyyy")}, Checkout date: {CheckoutDate.ToString("dd/mm/yyyy")}, Number of rooms {RoomList.Count}";
        }
    }
}
