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
            guestlist();
            break;
        case "2":
            ListAvailableRooms(rooms);
            break;
        case "3":
            guest_reg();
            break;
        case "4":
            CheckinGuest(guests, rooms);
            break;
        case "5":
            checkoutguest();
            break;
        case "6":
            // TODO: Show stay details for a guest
            guest_details();
            break;
        case "7":
            ExtendStay(guests);
            break;
        case "8":
            ShowRevenueBreakdown(guests);
            break;
        case "9":
            AlterStay();
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

    string[] options = { "List all guests", "List all available rooms", "Register a new guest", "Check-in a guest", "Check-out a guest", "Show stay details for a guest", "Extend a guest's stay", "Display monthly breakdown for year", "Alter a member's stay" };
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
    Console.WriteLine($"{"Room No.",-11} {"Type",-11} {"Bed Config.",-14} Daily rate");
    foreach (Room room in availableRooms)
        Console.WriteLine($"{room.RoomNumber,-11} {((room is StandardRoom) ? "Standard" : "Deluxe"), -11} {room.BedConfiguration,-14} ${room.DailyRate:0.00}");
}

void ListGuests(List<Guest> g)
{
    Console.WriteLine("The following guests are registered:");
    Console.WriteLine($"{"Name",-15} {"Passport No.",-15} {"Status",-11} Points");
    foreach (Guest guest in g)
        Console.WriteLine($"{guest.Name,-15} {guest.PassportNum,-15} {guest.Member.Status,-11} {guest.Member.Points}");
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

DateTime? ValidateDateTimeInput(string format, string prompt, DateTime? compareDate = null, string? comparison = null)
{
    Console.Write(prompt);
    try
    {
        DateTime input =
            DateTime.ParseExact(Console.ReadLine(), format, System.Globalization.CultureInfo.InvariantCulture);
        if (compareDate == null || comparison == null) return input;
        int difference = input.Subtract(compareDate.Value).Days;
        switch (comparison)
        {
            case "greater":
                if (difference > 0)
                    return input;
                throw new ArgumentOutOfRangeException(nameof(input),
                    $"The given date is before the date {compareDate:dd/MM/yyyy}.");
            case "lesser":
                if (difference < 0)
                    return input;
                throw new ArgumentOutOfRangeException(nameof(input),
                    $"The given date is after the date {compareDate:dd/MM/yyyy}.");
            default:
                throw new ArgumentException("Invalid comparison type.");
        }
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
    catch (ArgumentException ex)
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
    string[] options = { "Wi-Fi", "breakfast", "an additional bed" };
    foreach (string option in options)
    {
        switch (r)
        {
            // Specifically blacklists the options based on what is allowed for each room type
            case StandardRoom when option == "an additional bed":
            case DeluxeRoom when option is "Wi-Fi" or "breakfast":
                continue;
        }

        // Prompts the user and configures the room
        while (true)
        {
            var input =
                ValidateBooleanInput($"Is {option} needed for room {r.RoomNumber}? (Y/N): ");
            if (input == null) continue;
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

void CheckinGuest(List<Guest> g, List<Room> r)
{
    // Shows a list of checked-out guests and prompts the user to select a guest to check-in
    Guest? guest;
    ListGuests(GetCheckedoutGuests(g));
    while (true)
    {
        var input = ValidateIntInput(0, GetCheckedoutGuests(g).Count - 1, true, $"Who is checking in? Enter 1 to {GetCheckedoutGuests(g).Count} inclusive: ");
        if (input == null) continue;
        guest = g[input.Value];
        break;
    }

    // Prompts the user for the check-in and check-out date
    DateTime? checkinDate;
    DateTime? checkoutDate;
    while (true)
    {
        var input = ValidateDateTimeInput("dd/MM/yyyy", $"\nWhen is {guest.Name} checking in? Enter in the format dd/MM/yyyy: ", DateTime.Today, "lesser");
        if (input == null) continue;
        checkinDate = input;
        break;
    }
    while (true)
    {
        var input = ValidateDateTimeInput("dd/MM/yyyy", $"When is {guest.Name} checking out? Enter in the format dd/MM/yyyy: ", checkinDate, "greater");
        if (input == null) continue;
        checkoutDate = input;
        break;
    }

    // Creates a new Stay object based on the provided information
    Stay stay = new Stay(checkinDate.Value, checkoutDate.Value);

    // Lists the available rooms and prompts the user to select a room
    bool repeat;
    do
    {
        Console.WriteLine();
        ListAvailableRooms(r);
        List<Room> availableRooms = GetAvailableRooms(r);

        while (true)
        {
            var input = ValidateIntInput(0, availableRooms.Count - 1, true,
                $"Which room is {guest.Name} staying in? Enter 1 to {availableRooms.Count} inclusive: ");
            if (input == null) continue;
            Room bookedRoom = availableRooms[input.Value];

            // Makes the room unavailable
            r.Find(room => room.RoomNumber == bookedRoom.RoomNumber).IsAvail = false;

            ConfigureRoom(bookedRoom);

            stay.AddRoom(availableRooms[input.Value]);
            break;
        }
        
        while (true)
        {
            var input = ValidateBooleanInput("Add another room? (Y/N) ");
            if (input == null) continue;
            repeat = input.Value;
            break;
        }
    } while (repeat);

    // Updates the check-in status of the guest
    guest.IsCheckedin = true;

    // Updates the guest's stay information
    guest.HotelStay = stay;

    Console.WriteLine("\n========== Check-in successful ==========");
    Console.WriteLine($"Guest {guest.Name} has been checked in from {stay.CheckinDate:dd/MM/yyyy} to {stay.CheckoutDate:dd/MM/yyyy}. The following rooms will be occupied:");
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
        if (input == null) continue;
        guest = GetCheckedinGuests(g)[input.Value];
        break;
    }

    // Gets the stay information for the guest
    Stay stay = guest.HotelStay;
    DateTime checkoutDate = stay.CheckoutDate;
    while (true)
    {
        int? input = ValidateIntInput(1, 7, false, $"How many days would you like to extend the stay for {guest.Name}? Enter 1 to 7 inclusive: ");
        if (input == null) continue;
        DateTime newCheckoutDate = checkoutDate.AddDays(input.Value);
        bool? confirm =
            ValidateBooleanInput(
                $"{guest.Name} will now check-out on {newCheckoutDate:dd/MM/yyyy}. Is this correct? (Y/N) ");
        if (confirm is not true) continue;
        stay.CheckoutDate = newCheckoutDate;
        break;
    }
    
    Console.WriteLine("\n========== Extension successful ==========");
    Console.WriteLine($"Guest {guest.Name}'s stay has been extended to {stay.CheckoutDate:dd/MM/yyyy}. The following rooms will be occupied:");
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
        if (input == null) continue;
        selectedYear = input.Value;
        break;
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
        Console.WriteLine($"{months[i]} {selectedYear}:\t${breakdown[i]:0.00}");
    }

    double total = breakdown.Sum();
    Console.WriteLine($"\nTotal:\t\t${total:0.00}");
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
    if (name == "")
    {
        Console.WriteLine("Please input a name");
        guest_reg();
    }
    string p_number;
    //Prompts the user for the guest's passport number
    while (true)
    {
        Console.Write("Guest's passport number:");
        p_number = Console.ReadLine();
        if (p_number != "")
        {
            break; 
        }
        Console.WriteLine("Please input a passport number");
    }
    
    //To check if user is already registered//
    bool already_registered = false;
    foreach (Guest g in guests)
    {
    if (p_number == g.PassportNum)
    {
        Console.WriteLine("Guest is already registered");
        already_registered = true;
        break;
    }
        
}
    
    //Exit if the guest is already registered//
    if (already_registered == false)
    {
        //Creates a new guest object and adds to the guest list
        Guest add = new Guest(name, p_number, new Stay(), new Membership("Ordinary", 0));
        add.IsCheckedin= false;
        guests.Add(add);

        //Add guest to the Guest CSV file
        using (StreamWriter sr = new StreamWriter("Guests.csv", false))
        {
            sr.WriteLine("Name, PassportNumber, MembershipStatus, MembershipPoints");
            foreach (Guest g in guests)
            {
                sr.WriteLine("{0,0},{1,0},{2,0},{3,0}",g.Name,g.PassportNum,g.Member.Status,g.Member.Points);
            }
        }
    }
}


void guest_details()
{
    //Creates a variable//

    int choice;
    // Prompts the user for the Guest to view//
    while (true)
    {
        int i = 1;
        Console.WriteLine("Select a Guest to View:");
        //Display the guest with number options using the variable created before as number indicator//
        foreach (Guest g in guests)
        {
            Console.WriteLine("{0,0}. {1,0}", i, g.Name);
            i += 1;
        }
        //Check if the option selected is valid if not retry//
        int? user_choice = ValidateIntInput(0, guests.Count(), true, "Your Choice?");
        choice = Convert.ToInt32(user_choice);
        if (choice >= guests.Count())
        {
            Console.WriteLine("Please enter a valid option");
        }
        else if (choice < 0)
        {
            Console.WriteLine("Please enter a valid option");
        }
        else
        {
            break;
        }
    }
    displayguestdeatils(choice);
}
void displayguestdeatils(int num)
    {
    //Creates a table for the selected option//
    Console.WriteLine(
        "------------------------------------------------------------------------------------------------\n" +
        "|{0,-18}|{1,-18}|{2,-18}|{3,-18}|{4,-18}|\n" +
        "|{5,-18}|{6,-18}|{7,-18}|{8,-18}|{9,-18}|\n" +
        "------------------------------------------------------------------------------------------------\n" +
        "|{8,-18}|{9,-18}|{10,-18}|{11,-18}|{12,-18}|\n" +
        "|{13,-18}|{14,-18}|{15,-18}|{16,-18}|{17,-18}|\n" +
        "------------------------------------------------------------------------------------------------"
        , "Name", "Passport Number", "Checkin Date", "Checkout Date", ""
        , guests[num].Name, guests[num].PassportNum, guests[num].HotelStay.CheckinDate.ToString("dd/mm/yyyy"), guests[num].HotelStay.CheckoutDate.ToString("dd/mm/yyyy"),""
        ,"Number of rooms", "Membership Status", "Current points", "Checked in",""
        ,guests[num].HotelStay.RoomList.Count, guests[num].Member.Status, guests[num].Member.Points, guests[num].IsCheckedin, "");
    foreach (Room r in guests[num].HotelStay.RoomList)
    {
    if (r.GetType() == typeof(StandardRoom))
    {
        StandardRoom s = (StandardRoom)r;
        Console.WriteLine(
                    "------------------------------------------------------------------------------------------------\n" +
                    "|{0,-18}|{1,-18}|{2,-18}|{3,-18}|{4,-18}|\n" +
                    "|{5,-18}|{6,-18}|{7,-18}|{8,-18}|{9,-18}|\n" +
                    "------------------------------------------------------------------------------------------------"
                    , "Room Number", "Bed Configuration", "Daily Rate", "RequireWifi", "RequireBreakfast"
                    , s.RoomNumber, s.BedConfiguration, s.DailyRate, s.RequireWifi, s.RequireBreakfast);
    }
    else if (r.GetType() == typeof(DeluxeRoom))
        {
            DeluxeRoom s = (DeluxeRoom)r;
            Console.WriteLine(
                        "------------------------------------------------------------------------------------------------\n" +
                        "|{0,-18}|{1,-18}|{2,-18}|{3,-18}|{4,-18}|\n" +
                        "|{5,-18}|{6,-18}|{7,-18}|{8,-18}|{9,-18}|\n" +
                        "------------------------------------------------------------------------------------------------"
                        , "Room Number", "Bed Configuration", "Daily Rate", "AdditionalBed",""
                        , s.RoomNumber, s.BedConfiguration, s.DailyRate, s.AdditionalBed, "");
        }

    }
}
    



void checkoutguest()
{
    //Creates a variable//
    int num = 1;
    int num_of_people_not_checked_in = 0;
    // Prompts the user for the Guest to Checkout//
    Console.WriteLine("Select a Guest to Checkout:");
    //Display the guest with number options using the variable created before as number indicator//
    //Create a temporary list to store people who have checked in
    List<Guest> temp_list = new List<Guest>();

    //Only people who are checked in will be displayed//
    foreach (Guest g in guests)
    {
        
        if (g.IsCheckedin == true)
        {   //There is a chance of 2 guest having the same name but the passport num is unique
            Console.WriteLine("{0,0}. {1,0}  Passport Num: {2,0}", num, g.Name,g.PassportNum);
            num += 1;
            temp_list.Add(g);
        }
        else
        {
            num_of_people_not_checked_in += 1;
        }
    }
    if (num_of_people_not_checked_in == guests.Count)
    {
        Console.WriteLine("There are currently No one checked in");
    }
    else
    {
        //Check if the option selected is valid if not retry//
        int? user_choice = ValidateIntInput(0, guests.Count, true, "Your Choice?");
        if (user_choice > guests.Count)
        {
            Console.WriteLine("Please enter a valid option");
            guest_details();
        }
        else if (user_choice == null)
        {
            guest_details();
        }
        int choice = Convert.ToInt32(user_choice);
        
        // Add 1 to the total days_stayed as calculate the date diff
        int days_stayed = temp_list[choice].HotelStay.CheckoutDate.Subtract(temp_list[choice].HotelStay.CheckinDate).Days + 1;
        // Cost will be tallied here
        double total_cost = 0;

        Console.WriteLine(
                "======================================\n" +
                "|         HOTEL       BILL           |\n" +
                "======================================");
        foreach (Room r in temp_list[choice].HotelStay.RoomList)
        {
            if (r.GetType() == typeof(StandardRoom))
            {
                StandardRoom s_room = (StandardRoom)r;
                total_cost += s_room.CalculateCharges()*days_stayed;
                    Console.WriteLine(
                        "|Room Number: {0,-23}|\n" +
                        "|Room Type: {1,-25}|\n" +
                        "|Bed Configureation: {2,-16}|\n" +
                        "|Days Stayed: {3,-23}|\n" +
                        "|Wi-Fi $10 Added: {4,-19}|\n" +
                        "|Breakfast $20 Added: {5,-15}|\n" +
                        "|Daily Rate: ${6,-23}|\n" +
                        "|Cost : ${7,-28}|\n" +
                        "======================================"
                        , r.RoomNumber, "Standard Room",s_room.BedConfiguration,days_stayed, s_room.RequireWifi, s_room.RequireBreakfast,s_room.CalculateCharges().ToString("0.00"),(r.CalculateCharges()*days_stayed).ToString("0.00"));
            }
            else if (r.GetType() == typeof(DeluxeRoom))
                {
                    DeluxeRoom s_room = (DeluxeRoom)r;
                    total_cost += s_room.CalculateCharges() * days_stayed;
                    Console.WriteLine(
                        "|Room Number: {0,-23}|\n" +
                        "|Room Type: {1,-25}|\n" +
                        "|Bed Configuration: {2,-17}|\n" +
                        "|Days Stayed: {3,-23}|\n" +
                        "|Additional Bed $25: {4,-16}|\n" +
                        "|Daily Rate: ${5,-23}|\n" +
                        "|Cost : ${6,-28}|\n" +
                        "======================================"
                        , r.RoomNumber, "Deluxe Room",s_room.BedConfiguration, days_stayed,s_room.AdditionalBed, s_room.CalculateCharges().ToString("0.00"),  (s_room.CalculateCharges() * days_stayed).ToString("0.00"));
            }
            
        }
        Console.WriteLine(
            "|Total Bill: ${0,-23}|\n" +
            "" +
            "======================================"
            , total_cost.ToString("0.00"));
        
        while (true)
        {   // check if the user is eligible for the points system
            if (temp_list[choice].Member.Status != "Silver" && temp_list[choice].Member.Status != "Gold")
            {
                Console.WriteLine("You are not eligible to redeem points as your membership status is below Silver");
                break;
            }
            else
            {   // Ask if the guest want to use their points
                bool loop = true;
                while (loop == true)
                {
                bool? verify = ValidateBooleanInput($"You have {temp_list[choice].Member.Points} points to redeem. Do you want to redeem your points? (Y/N) ");
                // Loop if any other answer is given
                if (verify is null)
                    {

                    }
                
                else if (verify is not true)
                {
                    Console.WriteLine($"You will not use your points");
                    Console.Write("Please enter any key to make payment: ");
                    //This readline is soley for they guest to key random stuff in and will not be recorded
                    Console.ReadLine();
                    loop = false;
                }
                else if (verify is true)
                {
                    int points_to_redeem = 0;
                    while (true)
                    {
                    int? use_points = ValidateIntInput(0, temp_list[choice].Member.Points, false, $"You have {temp_list[choice].Member.Points} points, how many do you want to redeem?");
                    if (use_points >= 0 && use_points <= temp_list[choice].Member.Points)
                        {
                            points_to_redeem = Convert.ToInt32(use_points);
                            break;
                        }
                    }
                    Console.Write("Please enter any key to make payment: ");
                    //This readline is soley for they guest to key random stuff in and will not be recorded
                    Console.ReadLine();
                    Console.WriteLine($"Your redeemed {points_to_redeem} out of {temp_list[choice].Member.Points} points ");
                    temp_list[choice].Member.RedeemPoints(points_to_redeem);
                    total_cost -= points_to_redeem;
                    break;
                    }
                    loop = false;
                }
                break;
            }
        }
        temp_list[choice].Member.EarnPoints(total_cost);
        Console.WriteLine($"Your earn {(total_cost/10).ToString("0")} points and currently have {temp_list[choice].Member.Points} points.");
        // This is to offically check out the guest
        foreach(Guest g in guests)
        {
            if (g.PassportNum == temp_list[choice].PassportNum)
            {
                g.IsCheckedin= false;
                g.Member.Points = temp_list[choice].Member.Points;
                if (g.Member.Status == "Sliver")
                    {
                    if (g.Member.Points >= 200)
                    {
                        g.Member.Status = "Gold";
                        Console.WriteLine("Congrats! You are now a Gold Member!");
                    }
                    break;
                    }
                else if (g.Member.Status == "Ordinary")
                {
                    if (g.Member.Points >= 200)
                    {
                        g.Member.Status = "Gold";
                        Console.WriteLine("Congrats! You are now a Gold Member!");
                    }
                    else if (g.Member.Points >= 100)
                    {
                        g.Member.Status = "Silver";
                        Console.WriteLine("Congrats! You are now a Silver Member!");
                    }
                }
            }
        }
    }
}

void AlterStay()
{
    int num = 1;
    int num_of_people_not_checked_in = 0;
    // Prompts the user for the Guest to Checkout//
    Console.WriteLine("Select a Guest to Alter:");
    //Display the guest with number options using the variable created before as number indicator//
    //Create a temporary list to store people who have checked in. Guest who are not checked in will not be stored
    List<Guest> temp_list = new List<Guest>();

    //Only people who are checked in will be displayed//
    foreach (Guest g in guests)
    {

        if (g.IsCheckedin == true)
        {   //There is a chance of 2 guest having the same name but the passport num is unique
            Console.WriteLine("{0,0}. {1,0}  Passport Num: {2,0}", num, g.Name, g.PassportNum);
            num += 1;
            temp_list.Add(g);
        }
        else
        {
            num_of_people_not_checked_in += 1;
        }
    }
    if (num_of_people_not_checked_in == guests.Count)
    {
        Console.WriteLine("There are currently No one checked in");
    }
    else
    {
        //Check if the option selected is valid if not retry//
        int? user_choice = ValidateIntInput(0, guests.Count, true, "Your Choice?");
        if (user_choice > guests.Count)
        {
            Console.WriteLine("Please enter a valid option");
            AlterStay();
        }
        else if (user_choice == null)
        {
            AlterStay();
        }

        // int g will be the guest we edit when we call guests[g]
        int g = 0;
        foreach (Guest i in guests)
        {
            if (i.PassportNum == temp_list[Convert.ToInt32(user_choice)].PassportNum)
            {
                break;
            }
            g += 1;
        }
        displayguestdeatils(g);
        if (guests[g].HotelStay.RoomList.Count > 1) 
        {
            Console.WriteLine("Which Room do you want to change? (Input Room Number)");
            //////////////////////////////////////////////////////////////////////////
        }

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
    using StreamReader sr = new StreamReader("Rooms.csv");
    string? line = sr.ReadLine();
    while ((line = sr.ReadLine()) != null)
    {
        string[] data = line.Split(',');

        // Creates a specific room (with a specific room type depending on what is read from the file)
        Room room = data[0] switch
        {
            "Standard" => new StandardRoom(Convert.ToInt32(data[1]), data[2], Convert.ToDouble(data[3]), true),
            "Deluxe" => new DeluxeRoom(Convert.ToInt32(data[1]), data[2], Convert.ToDouble(data[3]), true),
            _ => throw new Exception($"Invalid room type for room {data[1]}.")
        };

        // Adds the room to the list of rooms
        r.Add(room);
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
                if (requirements[0] == "TRUE" && r is StandardRoom sWRoom)
                    sWRoom.RequireWifi = true;
                if (requirements[1] == "TRUE" && r is StandardRoom sBRoom)
                    sBRoom.RequireBreakfast = true;
                if (requirements[2] == "TRUE" && r is DeluxeRoom dRoom)
                    dRoom.AdditionalBed = true;
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

    using StreamReader sr = new StreamReader("Guests.csv");
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

                if (stayData[9] == "") continue;
                Room? room2 = r.Find(room => room.RoomNumber == Convert.ToInt32(stayData[9]));
                InitializeRoom(stay.RoomList, room2, guest, stayData[2],
                    new[] { stayData[10], stayData[11], stayData[12] }, true);
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