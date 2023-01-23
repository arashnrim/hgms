//==========================================================
// Student Number   : REDACTED
// Student Name     : Arash Nur Iman
//==========================================================

namespace HotelApp
{
    /// <summary>
    /// Represents a person who stays at the hotel.
    /// <para>
    /// <c>Guest</c> contains details about the person including their name, passport number, stays, and membership.
    /// </para>
    /// </summary>
    internal class Guest
    {
        private string name;
        public string Name { get; set; }

        private string passportNum;
        public string PassportNum { get; set; }

        private Stay hotelStay;
        public Stay HotelStay { get; set; }

        private Membership member;
        public Membership Member { get; set; }

        private bool isCheckedin;
        public bool IsCheckedin { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Guest"/> class.
        /// </summary>
        public Guest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Guest"/> class with the required parameters.
        /// </summary>
        /// <param name="n">The name of the guest.</param>
        /// <param name="pn">The passport number of the guest.</param>
        /// <param name="hs">The <see cref="Stay"/> object representing the guest's stay at the hotel.</param>
        /// <param name="m">The <see cref="Membership"/> object representing the membership the guest has with the hotel.</param>
        public Guest (string n, string pn, Stay hs, Membership m)
        {
            Name = n;
            PassportNum = pn;
            HotelStay = hs;
            Member = m;
            IsCheckedin = true;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Passport number: {PassportNum}, Stay information: {HotelStay.ToString()}, Membership information: {Member.ToString()}, Checked in: {(IsCheckedin ? "Yes" : "No")}";
        }
    }
}
