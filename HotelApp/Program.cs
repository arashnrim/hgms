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
            break;
        case "2":
            ListAvailableRooms(rooms);
            break;
        case "3":
            // TODO: Register a new guest
            break;
        case "4":
            CheckinGuest(guests, rooms);
            break;
        case "5":
            // TODO: Show stay details for a guest
            break;
        case "6":
            break;
            // TODO: Extend a guest's stay
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

    string[] options = new string[] { "List all guests", "List all available rooms", "Register a new guest", "Check-in a guest", "Show stay details for a guest", "Extend a guest's stay" };
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

List<Guest> GetAvailableGuests(List<Guest> g)
{
    List<Guest> availableGuests = new List<Guest>();
    foreach (Guest guest in g)
    {
        if (!guest.IsCheckedin)
            availableGuests.Add(guest);
    }

    return availableGuests;
}

(bool, int?) ValidateIntInput(int lowerBound, int higherBound, bool offset, string prompt)
{
    Console.Write(prompt);
    try
    {
        int input = Convert.ToInt32(Console.ReadLine()) - (offset ? 1 : 0);
        if (input >= lowerBound && input <= higherBound)
            return (true, input);
        else
            throw new ArgumentOutOfRangeException(nameof(input));
    }
    catch (FormatException)
    {
        Console.WriteLine($"Input should be a numerical value.");
        return (false, null);
    }
    catch (ArgumentOutOfRangeException)
    {
        Console.WriteLine($"Input should be between {lowerBound} and {higherBound} inclusive.");
        return (false, null);
    }
}

(bool, DateTime?) ValidateDateTimeInput(string format, string prompt, DateTime? compareDate = null)
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

        return (true, input);
    }
    catch (FormatException)
    {
        Console.WriteLine($"Input should be in the format dd/MM/yyyy.");
        return (false, null);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(ex.Message);
        return (false, null);
    }
}

(bool, bool?) ValidateBooleanInput(string prompt)
{
    Console.Write(prompt);
    try
    {
        string input = Console.ReadLine().ToUpper();
        if (input != "Y" && input != "N")
            throw new ArgumentOutOfRangeException(nameof(input), "Input should be either Y or N.");
        bool inputBoolean = input == "Y";
        return (true, inputBoolean);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(ex.Message);
        return (false, null);
    }
}

// Configures the room with the additional information, like whether Wi-Fi, breakfast, or 
// an additional bed is required.
void ConfigureRoom(Room r)
{
    if (r is StandardRoom)
    {
        while (true)
        {
            var wfValues =
                ValidateBooleanInput($"Is Wi-Fi needed for room {r.RoomNumber}? (Y/N): ");
            if (wfValues.Item1)
            {
                ((StandardRoom)r).RequireWifi = wfValues.Item2.Value;
                break;
            }
        }

        while (true)
        {
            var brValues =
                ValidateBooleanInput($"Is breakfast needed for room {r.RoomNumber}? (Y/N): ");
            if (brValues.Item1)
            {
                ((StandardRoom)r).RequireBreakfast = brValues.Item2.Value;
                break;
            }
        }
    }
    else if (r is DeluxeRoom)
    {
        while (true)
        {
            var abValues =
                ValidateBooleanInput(
                    $"Is an additional bed needed for room {r.RoomNumber}? (Y/N): ");
            if (abValues.Item1)
            {
                ((DeluxeRoom)r).AdditionalBed = abValues.Item2.Value;
                break;
            }
        }
    }
}

void CheckinGuest(List<Guest> g, List<Room> r)
{
    // Shows a list of available guests and prompts the user to select a guest to check-in
    Guest? guest;
    ListGuests(GetAvailableGuests(g));
    while (true)
    {
        var values = ValidateIntInput(0, GetAvailableGuests(g).Count, true, $"Who is checking in? Enter 1 to {GetAvailableGuests(g).Count} inclusive: ");
        if (values.Item1)
        {
            guest = g[values.Item2.Value];
            break;
        }
    }

    // Prompts the user for the check-in and check-out date
    DateTime? checkinDate;
    DateTime? checkoutDate;
    while (true)
    {
        var values = ValidateDateTimeInput("dd/MM/yyyy", $"\nWhen is {guest.Name} checking in? Enter in the format dd/MM/yyyy: ");
        if (values.Item1)
        {
            checkinDate = values.Item2.Value;
            break;
        }
    }
    while (true)
    {
        var values = ValidateDateTimeInput("dd/MM/yyyy", $"When is {guest.Name} checking out? Enter in the format dd/MM/yyyy: ", checkinDate);
        if (values.Item1)
        {
            checkoutDate = values.Item2.Value;
            break;
        }
    }

    // Creates a new Stay object based on the provided information
    List<Room> stayRooms = new List<Room>();
    Stay stay = new Stay(checkinDate.Value, checkoutDate.Value, rooms);

    // Lists the available rooms and prompts the user to select a room
    bool repeat = true;
    do
    {
        Console.WriteLine();
        ListAvailableRooms(r);
        List<Room> availableRooms = GetAvailableRooms(r);

        while (true)
        {
            var values = ValidateIntInput(0, availableRooms.Count, true,
                $"Which room is {guest.Name} staying in? Enter 1 to {availableRooms.Count} inclusive: ");
            if (values.Item1)
            {
                Room bookedRoom = availableRooms[values.Item2.Value];

                // Makes the room unavailable
                r.Find(room => room.RoomNumber == bookedRoom.RoomNumber).IsAvail = false;

                ConfigureRoom(bookedRoom);

                stayRooms.Add(availableRooms[values.Item2.Value]);
                break;
            }
        }

        // Currently manually hard-coded restricted to 2 rooms per stay. Edit here if necessary.
        if (stayRooms.Count < 2)
        {
            while (true)
            {
                var rValues = ValidateBooleanInput("Add another room? (Y/N) ");
                if (rValues.Item1)
                {
                    repeat = rValues.Item2.Value;
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
    foreach (Room room in stayRooms)
    {
        Console.WriteLine(room.ToString());
    }
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
                List<Room> stayRooms = new List<Room>();

                while ((stayLine = staySR.ReadLine()) != null)
                {
                    string[] stayData = stayLine.Split(',');

                    // Ignores stays that don't match the guest's passport number
                    if (stayData[1] != data[1])
                        continue;

                    // Creates a list of rooms that the guest has stayed in
                    Room? room1 = r.Find(room => room.RoomNumber == Convert.ToInt32(stayData[5]));
                    InitializeRoom(stayRooms, room1, guest, stayData[2], new []{stayData[6], stayData[7], stayData[8]}, true);

                    if (stayData[9] != "")
                    {
                        Room? room2 = r.Find(room => room.RoomNumber == Convert.ToInt32(stayData[9]));
                        InitializeRoom(stayRooms, room2, guest, stayData[2], new []{stayData[10], stayData[11], stayData[12]}, true);
                    }

                    stay = new Stay(Convert.ToDateTime(stayData[3]), Convert.ToDateTime(stayData[4]), stayRooms);
                }
            }

            // Creates a membership object
            Membership membership = new Membership(data[2], Convert.ToInt32(data[3]));

            // Creates a guest object
            if (stay != null)
            {
                guest.Name = data[0];
                guest.PassportNum = data[1];
                guest.HotelStay = stay;
                guest.Member = membership;
                g.Add(guest);
            } else
                throw new Exception("Stay data may be wrongly formatted.");
        }
    }
}
