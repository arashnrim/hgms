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

        // TODO: Implement roomList

        public Stay() { }
        public Stay(DateTime cid, DateTime cod)
        {
            CheckinDate = cid;
            checkoutDate = cod;
        }

        // TODO: Implement AddRoom()

        // TODO: Call CalculateCharges() per room and calculate the total
        public double CalculateTotal()
        {
            throw new NotImplementedException();
        }

        // TODO: Add "Number of rooms: {RoomList.Count}" to ToString()
        public override string ToString()
        {
            return $"Checkin date: {CheckinDate.ToString("dd/MM/yyyy")}, Checkout date: {CheckoutDate.ToString("dd/mm/yyyy")}";
        }
    }
}
