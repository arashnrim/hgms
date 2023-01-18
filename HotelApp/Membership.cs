//==========================================================
// Student Number   : REDACTED
// Student Name     : Arash Nur Iman
//==========================================================

namespace HotelApp
{
    internal class Membership
    {
        private string status;
        public string Status { get; set; }

        private int points;
        public int Points { 
            get { return points; } 
            set {
                if (value < 0) points = 0;
                else points = value;
            }
        }

        public Membership() { }
        public Membership(string s, int p)
        {
            Status = s;
            Points = p;
        }

        public void EarnPoints(double amount)
        {
            Points += Convert.ToInt32(amount / 10);
        }

        public bool RedeemPoints(int amount)
        {
            if (Status != "Silver" && Status != "Gold")
            {
                return false;
            }

            Points -= amount;
            return true;
        }

        public override string ToString()
        {
            return $"Membership status: {Status}, Current points: {Points}";
        }
    }
}
