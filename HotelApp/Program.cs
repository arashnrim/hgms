using HotelApp;

List<Room> rooms = new List<Room>();
List<Guest> guests = new List<Guest>();

// Initializes the rooms and guests used in the application.
InitializeRooms(rooms);
InitializeGuests(guests, rooms);

bool cont = true;
while (cont)
{
    DisplayMenu();
    Console.Write("Enter an option: ");
    string choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            // TODO: List all guests
            guestlist();
            break;
        case "2":
            ListAvailableRooms(rooms);
            break;
        case "3":
            // TODO: Register a new guest
            guest_reg();
            break;
        case "4":
            CheckinGuest(guests, rooms);
            break;
        case "5":
            // TODO: Show stay details for a guest
            break;
        case "6":
            ExtendStay(guests);
            break;
        case "7":
            ShowRevenueBreakdown(guests);
            break;
        case "0":
            cont = false;
            break;
        default:
            Console.WriteLine("Invalid option, try again.");
            break;
    }

    Console.WriteLine();
}

//==========================================================
// METHODS
//==========================================================

void DisplayMenu()
{
    Console.WriteLine("========== ICT Hotel Guest Management System ==========");

    string[] options = { "List all guests", "List all available rooms", "Register a new guest", "Check-in a guest", "Show stay details for a guest", "Extend a guest's stay", "Display monthly breakdown for year" };
    for (int i = 0; i < options.Length; i++)
        Console.WriteLine($"[{i + 1}] {options[i]}");
    Console.WriteLine("[0] Exit");
}

List<Room> GetAvailableRooms(List<Room> r)
{
    List<Room> availableRooms = new List<Room>();
    foreach (Room room in r)
    {
        if (room.IsAvail)
            availableRooms.Add(room);
    }

    return availableRooms;
}

void ListAvailableRooms(List<Room> r)
{
    List<Room> availableRooms = GetAvailableRooms(r);
    Console.WriteLine("The following rooms are available for check-in:");
    foreach (Room room in availableRooms)
        Console.WriteLine(room);
}

void ListGuests(List<Guest> g)
{
    Console.WriteLine("The following guests are registered:");
    foreach (Guest guest in g)
    {
        Console.WriteLine(guest);
    }
}

List<Guest> GetCheckedoutGuests(List<Guest> g)
{
    List<Guest> checkedoutGuests = new List<Guest>();
    foreach (Guest guest in g)
    {
        if (!guest.IsCheckedin)
            checkedoutGuests.Add(guest);
    }

    return checkedoutGuests;
}

List<Guest> GetCheckedinGuests(List<Guest> g)
{
    List<Guest> checkedinGuests = new List<Guest>();
    foreach (Guest guest in g)
    {
        if (guest.IsCheckedin)
            checkedinGuests.Add(guest);
    }

    return checkedinGuests;
}

int? ValidateIntInput(int lowerBound, int higherBound, bool offset, string prompt)
{
    Console.Write(prompt);
    try
    {
        int input = Convert.ToInt32(Console.ReadLine()) - (offset ? 1 : 0);
        if (input >= lowerBound && input <= higherBound)
            return input;
        throw new ArgumentOutOfRangeException(nameof(input));
    }
    catch (FormatException)
    {
        Console.WriteLine($"Input should be a numerical value.");
        return null;
    }
    catch (ArgumentOutOfRangeException)
    {
        Console.WriteLine($"Input should be between {lowerBound + (offset ? 1 : 0)} and {higherBound + (offset ? 1 : 0)} inclusive.");
        return null;
    }
}

DateTime? ValidateDateTimeInput(string format, string prompt, DateTime? compareDate = null)
{
    Console.Write(prompt);
    try
    {
        DateTime input =
            DateTime.ParseExact(Console.ReadLine(), format, System.Globalization.CultureInfo.InvariantCulture);
        if (compareDate != null)
        {
            int difference = input.Subtract(compareDate.Value).Days;
            if (difference < 0)
                throw new ArgumentOutOfRangeException(nameof(input), "The given date is before the previous date.");
        }

        return input;
    }
    catch (FormatException)
    {
        Console.WriteLine($"Input should be in the format dd/MM/yyyy.");
        return null;
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(ex.Message);
        return null;
    }
}

bool? ValidateBooleanInput(string prompt)
{
    Console.Write(prompt);
    try
    {
        string input = Console.ReadLine().ToUpper();
        if (input != "Y" && input != "N")
            throw new ArgumentOutOfRangeException(nameof(input), "Input should be either Y or N.");
        bool inputBoolean = input == "Y";
        return inputBoolean;
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(ex.Message);
        return null;
    }
}

// Configures the room with the additional information, like whether Wi-Fi, breakfast, or 
// an additional bed is required.
void ConfigureRoom(Room r)
{
    string[] options = new string[] { "Wi-Fi", "breakfast", "an additional bed" };
    foreach (string option in options)
    {
        // Specifically blacklists the options based on what is allowed for each room type
        if (r is StandardRoom && option == "an additional bed")
            continue;
        if (r is DeluxeRoom && (option == "Wi-Fi" || option == "breakfast"))
            continue;

        // Prompts the user and configures the room
        while (true)
        {
            var input =
                ValidateBooleanInput($"Is {option} needed for room {r.RoomNumber}? (Y/N): ");
            if (input != null)
            {
                switch (option)
                {
                    case "Wi-Fi":
                        ((StandardRoom)r).RequireWifi = input.Value;
                        break;
                    case "breakfast":
                        ((StandardRoom)r).RequireBreakfast = input.Value;
                        break;
                    case "an additional bed":
                        ((DeluxeRoom)r).AdditionalBed = input.Value;
                        break;
                }
                break;
            }
        }
    }
}

void CheckinGuest(List<Guest> g, List<Room> r)
{
    // Shows a list of checked-out guests and prompts the user to select a guest to check-in
    Guest? guest;
    ListGuests(GetCheckedoutGuests(g));
    while (true)
    {
        var input = ValidateIntInput(0, GetCheckedoutGuests(g).Count - 1, true, $"Who is checking in? Enter 1 to {GetCheckedoutGuests(g).Count} inclusive: ");
        if (input != null)
        {
            guest = g[input.Value];
            break;
        }
    }

    // Prompts the user for the check-in and check-out date
    DateTime? checkinDate;
    DateTime? checkoutDate;
    while (true)
    {
        var input = ValidateDateTimeInput("dd/MM/yyyy", $"\nWhen is {guest.Name} checking in? Enter in the format dd/MM/yyyy: ");
        if (input != null)
        {
            checkinDate = input;
            break;
        }
    }
    while (true)
    {
        var input = ValidateDateTimeInput("dd/MM/yyyy", $"When is {guest.Name} checking out? Enter in the format dd/MM/yyyy: ", checkinDate);
        if (input != null)
        {
            checkoutDate = input;
            break;
        }
    }

    // Creates a new Stay object based on the provided information
    Stay stay = new Stay(checkinDate.Value, checkoutDate.Value);

    // Lists the available rooms and prompts the user to select a room
    bool repeat = true;
    do
    {
        Console.WriteLine();
        ListAvailableRooms(r);
        List<Room> availableRooms = GetAvailableRooms(r);

        while (true)
        {
            var input = ValidateIntInput(0, availableRooms.Count - 1, true,
                $"Which room is {guest.Name} staying in? Enter 1 to {availableRooms.Count} inclusive: ");
            if (input != null)
            {
                Room bookedRoom = availableRooms[input.Value];

                // Makes the room unavailable
                r.Find(room => room.RoomNumber == bookedRoom.RoomNumber).IsAvail = false;

                ConfigureRoom(bookedRoom);

                stay.AddRoom(availableRooms[input.Value]);
                break;
            }
        }

        // Currently manually hard-coded restricted to 2 rooms per stay. Edit here if necessary.
        if (stay.RoomList.Count < 2)
        {
            while (true)
            {
                var input = ValidateBooleanInput("Add another room? (Y/N) ");
                if (input != null)
                {
                    repeat = input.Value;
                    break;
                }
            }
        }
    } while (repeat);

    // Updates the check-in status of the guest
    guest.IsCheckedin = true;

    // Updates the guest's stay information
    guest.HotelStay = stay;

    Console.WriteLine("\n========== Check-in successful ==========");
    Console.WriteLine($"Guest {guest.Name} has been checked in from {stay.CheckinDate.ToString("dd/MM/yyyy")} to {stay.CheckoutDate.ToString("dd/MM/yyyy")}. The following rooms will be occupied:");
    foreach (Room room in stay.RoomList)
    {
        Console.WriteLine(room.ToString());
    }
}

void ExtendStay(List<Guest> g)
{
    // Shows a list of checked-in guests and prompts the user to select a guest
    Guest? guest;
    ListGuests(GetCheckedinGuests(g));
    while (true)
    {
        int? input = ValidateIntInput(0, GetCheckedinGuests(g).Count - 1, true,
            $"Who would like to extend their stay? Enter 1 to {GetCheckedoutGuests(g).Count} inclusive: ");
        if (input != null)
        {
            guest = GetCheckedinGuests(g)[input.Value];
            break;
        }
    }

    // Gets the stay information for the guest
    Stay stay = guest.HotelStay;
    DateTime checkoutDate = stay.CheckoutDate;
    while (true)
    {
        int? input = ValidateIntInput(1, 7, false, $"How many days would you like to extend the stay for {guest.Name}? Enter 1 to 7 inclusive: ");
        if (input != null)
        {
            DateTime newCheckoutDate = checkoutDate.AddDays(input.Value);
            bool? confirm =
                ValidateBooleanInput(
                    $"{guest.Name} will now check-out on {newCheckoutDate.ToString("dd/MM/yyyy")}. Is this correct? (Y/N) ");
            if (confirm != null && confirm == true)
            {
                stay.CheckoutDate = newCheckoutDate;
                break;
            }
        }
    }
    
    Console.WriteLine("\n========== Extension successful ==========");
    Console.WriteLine($"Guest {guest.Name}'s stay has been extended to {stay.CheckoutDate.ToString("dd/MM/yyyy")}. The following rooms will be occupied:");
    foreach (Room room in stay.RoomList)
    {
        Console.WriteLine(room.ToString());
    }
}

void ShowRevenueBreakdown(List<Guest> g)
{
    int currentYear = DateTime.Today.Year;
    int selectedYear;
    while (true)
    {
        // Additional assumption: the hotel began operations from 2000 onwards, meaning that values before 2000 are invalid.
        int? input = ValidateIntInput(2000, currentYear + 1, false, "Enter the year: ");
        if (input != null)
        {
            selectedYear = input.Value;
            break;
        }
    }

    double[] breakdown = new double[12];
    
    // Loops through each Guest and their Stay object and adds the information to the breakdown if the years match
    foreach (Guest guest in g)
    {
        if (guest.HotelStay.CheckoutDate.Year != selectedYear)
            continue;

        DateTime checkoutDate = guest.HotelStay.CheckoutDate;
        double revenue = guest.HotelStay.CalculateTotal();
        breakdown[checkoutDate.Month - 1] += revenue;
    }

    Console.WriteLine();
    string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    for (int i = 0; i < months.Length; i++)
    {
        Console.WriteLine($"{months[i]} {selectedYear}:\t${breakdown[i].ToString("0.00")}");
    }

    double total = breakdown.Sum();
    Console.WriteLine($"\nTotal:\t\t${total.ToString("0.00")}");
}

//==========================================================
// INITIALIZATION METHODS
// The functions below are created and called to initialize
// the required variables and objects for the program to
// work.
//==========================================================

void InitializeRooms(List<Room> r)
{
    using (StreamReader sr = new StreamReader("Rooms.csv"))
    {
        string? line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
            string[] data = line.Split(',');

            // Creates a specific room (with a specific room type depending on what is read from the file)
            Room room;
            if (data[0] == "Standard")
                room = new StandardRoom(Convert.ToInt32(data[1]), data[2], Convert.ToDouble(data[3]), true);
            else if (data[0] == "Deluxe")
                room = new DeluxeRoom(Convert.ToInt32(data[1]), data[2], Convert.ToDouble(data[3]), true);
            else
                throw new Exception($"Invalid room type for room {data[1]}.");

            // Adds the room to the list of rooms
            r.Add(room);
        }
    }
}

void InitializeRoom(List<Room> initRoomList, Room? r, Guest g, string checkedIn, string[] requirements, bool optional = false)
{
    try
    {
        if (r != null)
        {
            if (checkedIn == "TRUE")
            {
                g.IsCheckedin = true;
                r.IsAvail = false;
                if (requirements[0] == "TRUE" && r is StandardRoom)
                    ((StandardRoom)r).RequireWifi = true;
                if (requirements[1] == "TRUE" && r is StandardRoom)
                    ((StandardRoom)r).RequireBreakfast = true;
                if (requirements[2] == "TRUE" && r is DeluxeRoom)
                    ((DeluxeRoom)r).AdditionalBed = true;
            }

            initRoomList.Add(r);
        }
        else
            throw new ArgumentOutOfRangeException(nameof(initRoomList), "A stay must have at least one room.");
    }
    catch (ArgumentOutOfRangeException argEx)
    {
        Console.WriteLine(argEx.Message);
    }
    catch (FormatException)
    {
        if (!optional)
        {
            Console.WriteLine("Invalid data format. Please check the data file.");
        }
    }
}

void InitializeGuests(List<Guest> g, List<Room> r)
{
    // IMPORTANT: InitializeRooms() must be invoked before this function.

    using (StreamReader sr = new StreamReader("Guests.csv"))
    {
        string? line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
            string[] data = line.Split(',');
            Guest guest = new Guest();

            // Creates a stay object
            Stay? stay = null;
            using (StreamReader staySR = new StreamReader("Stays.csv"))
            {
                string? stayLine = staySR.ReadLine();

                while ((stayLine = staySR.ReadLine()) != null)
                {
                    string[] stayData = stayLine.Split(',');

                    // Ignores stays that don't match the guest's passport number
                    if (stayData[1] != data[1])
                        continue;

                    // Initializes the stay object
                    stay = new Stay(Convert.ToDateTime(stayData[3]), Convert.ToDateTime(stayData[4]));

                    // Adds the room that the guest has for a particular stay
                    Room? room1 = r.Find(room => room.RoomNumber == Convert.ToInt32(stayData[5]));
                    InitializeRoom(stay.RoomList, room1, guest, stayData[2], new []{stayData[6], stayData[7], stayData[8]}, true);

                    if (stayData[9] != "")
                    {
                        Room? room2 = r.Find(room => room.RoomNumber == Convert.ToInt32(stayData[9]));
                        InitializeRoom(stay.RoomList, room2, guest, stayData[2],
                            new[] { stayData[10], stayData[11], stayData[12] }, true);
                    }
                }
            }

            // Creates a membership object
            Membership membership = new Membership(data[2], Convert.ToInt32(data[3]));

            // Creates a guest object
            guest.Name = data[0];
            guest.PassportNum = data[1];
            guest.Member = membership;
            guest.HotelStay = stay ?? new Stay();
            g.Add(guest);
        }
    }
}

void guestlist()
{
    // Creates a loop to access each guest information in the list
    foreach (Guest g in guests)
    {
        // Checks if the guest have registered a room to stay in. If not Check in Date and Check out Date will be Null
        if (g.HotelStay.CheckinDate == DateTime.MinValue)
        {
            Console.WriteLine("\n\n===================================");
            Console.WriteLine("Guest's name: {0,0} \nPassport Number: {1,0} \nMembership: {2,0} \nPoints: {3,0} \nCheck in Date: {4,0} \nCheck out Date: {5,0}"
                , g.Name, g.PassportNum, g.Member.Status, g.Member.Points,"Null","Null");
        }
        else
        {
            Console.WriteLine("\n\n===================================");
            Console.WriteLine("Guest's name: {0,0} \nPassport Number: {1,0} \nMembership: {2,0} \nPoints: {3,0} \nCheck in Date: {4,0} \nCheck out Date: {5,0}"
                , g.Name, g.PassportNum, g.Member.Status, g.Member.Points, g.HotelStay.CheckinDate, g.HotelStay.CheckoutDate);
        }
        
    }
}

void guest_reg()
{
    // Prompts the user for the guest name
    Console.Write("Guest's name:");
    string name = Console.ReadLine();

    //Prompts the user for the guest's passport number
    Console.Write("Guest's passport number:");
    string p_number = Console.ReadLine();

    //Creates a new guest object and adds to the guest list
    guests.Add(new Guest(name, p_number,new Stay(),new Membership("Ordinary",0)));

    //Add guest to the Guest CSV file
    
}