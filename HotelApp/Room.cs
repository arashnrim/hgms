//==========================================================
// Student Number   : REDACTED
// Student Name     : Sim Ri Sheng
//==========================================================

namespace HotelApp
{
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

        public Room () {}

        public Room(int roomNum, string bedconfig, double dailyR, bool isavil)
        {
            RoomNumber = roomNum;
            BedConfiguration = bedconfig;
            DailyRate = dailyR;
            IsAvail = isavil;
        }

        public abstract double CalculateCharges();

        public override string ToString()
        {
            return $"RoomNumber: {RoomNumber.ToString()}, BedConfiguration: {BedConfiguration},DailyRate: {DailyRate.ToString()}, IsAvail: {IsAvail.ToString()}";
        }
    }
}
