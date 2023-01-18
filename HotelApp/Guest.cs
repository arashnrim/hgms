//==========================================================
// Student Number   : REDACTED
// Student Name     : Arash Nur Iman
//==========================================================

namespace HotelApp
{
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

        public Guest() { }
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
            return $"Name: {Name}, Passport number: {PassportNum}, Stay information: {HotelStay.ToString()}, Membership information: {Member.ToString()} Checked in: {(IsCheckedin ? "Yes" : "No")}";
        }
    }
}
