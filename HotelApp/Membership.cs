//==========================================================
// Student Number   : REDACTED
// Student Name     : Arash Nur Iman
//==========================================================

namespace HotelApp
{
    /// <summary>
    /// Represents a membership of a customer in a loyalty program the hotel has.
    /// <para>
    /// <c>Membership</c> holds information related to membership including the status (Ordinary, Silver, or Gold)
    /// and the number of points the customer has earned.
    /// </para>
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Membership"/> class.
        /// </summary>
        public Membership() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Membership"/> class with
        /// the guest's status and points.
        /// </summary>
        /// <param name="s">The status of the membership. Should either be Ordinary, Silver, or Gold.</param>
        /// <param name="p">The number of points the guest has accumulated.</param>
        public Membership(string s, int p)
        {
            Status = s;
            Points = p;
        }

        /// <summary>
        /// Calculates the number of points a guest has earned when making a
        /// transaction with the hotel.
        /// <para>
        /// The number of points earned is equal to the amount spent divided
        /// by 10.
        /// </para>
        /// </summary>
        /// <param name="amount">The amount of money spent by a guest for a stay.</param>
        public void EarnPoints(double amount)
        {
            Points += Convert.ToInt32(amount / 10);
        }

        /// <summary>
        /// Uses the points a guest has and subtracts the amount for use in
        /// another case (e.g., offsetting a bill).
        /// <para>
        /// A guest must have a status of either Silver or Gold to redeem
        /// points.
        /// </para>
        /// </summary>
        /// <param name="amount">The number of points to redeem.</param>
        /// <returns>A Boolean value denoting if a guest can redeem their points.</returns>
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
