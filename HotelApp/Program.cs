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
            // TODO: Check-in a guest
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

void ListAvailableRooms(List<Room> r)
{
    Console.WriteLine("The following rooms are available for check-in:");
    foreach (Room room in r)
    {
        if (room.IsAvail)
            Console.WriteLine(room);
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

void InitializeGuests(List<Guest> g, List<Room> r)
{
    using (StreamReader sr = new StreamReader("Guests.csv"))
    {
        string? line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
            string[] data = line.Split(',');

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
                    if (room1 != null)
                    {
                        if (Convert.ToBoolean(stayData[2]))
                            room1.IsAvail = false;
                        if (Convert.ToBoolean(stayData[6]) && room1 is StandardRoom)
                            ((StandardRoom)room1).RequireWifi = true;
                        if (Convert.ToBoolean(stayData[7]) && room1 is StandardRoom)
                            ((StandardRoom)room1).RequireBreakfast = true;
                        if (Convert.ToBoolean(stayData[8]) && room1 is DeluxeRoom)
                            ((DeluxeRoom)room1).AdditionalBed = true;
                        stayRooms.Add(room1);
                    }
                    else
                        throw new Exception("A stay must have at least one room.");
                    
                    try
                    {
                        Room? room2 = r.Find(room => room.RoomNumber == Convert.ToInt32(stayData[9]));
                        if (room2 != null)
                        {
                            if (Convert.ToBoolean(stayData[2]))
                                room2.IsAvail = false;
                            if (Convert.ToBoolean(stayData[10]) && room2 is StandardRoom)
                                ((StandardRoom)room2).RequireWifi = true;
                            if (Convert.ToBoolean(stayData[11]) && room2 is StandardRoom)
                                ((StandardRoom)room2).RequireBreakfast = true;
                            if (Convert.ToBoolean(stayData[12]) && room2 is DeluxeRoom)
                                ((DeluxeRoom)room2).AdditionalBed = true;
                            stayRooms.Add(room2);
                        } else
                            throw new Exception($"Invalid room number {stayData[9]} under {stayData[0]}.");
                    } catch (FormatException) { }

                    stay = new Stay(Convert.ToDateTime(stayData[3]), Convert.ToDateTime(stayData[4]), stayRooms);
                }
            }

            // Creates a membership object
            Membership membership = new Membership(data[2], Convert.ToInt32(data[3]));

            // Creates a guest object
            if (stay != null)
            {
                g.Add(new Guest(data[0], data[1], stay, membership));
            } else
                throw new Exception("Stay data may be wrongly formatted.");
        }
    }
}
