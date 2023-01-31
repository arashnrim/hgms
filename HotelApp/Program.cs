using System.Globalization;
using HotelApp;

List<Room> rooms = new();
List<Guest> guests = new();

// Initializes the rooms and guests used in the application.
InitializeRooms(rooms);
InitializeGuests(guests, rooms);

bool cont = true;
while (cont)
{
    DisplayMenu("ICT Hotel Guest Management System", new[] { "List all guests", "List all available rooms", "Register a new guest", "Check-in a guest", "Check-out a guest", "Show stay details for a guest", "Extend a guest's stay", "Display monthly breakdown for year", "Alter a guest's stay" });
    Console.Write("Enter an option: ");
    string choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            guestlist();
            break;
        case "2":
            ListRooms(rooms.Where(room => room.IsAvail).ToList(), "The following rooms are available for check-in:");
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
            guest_details();
            break;
        case "7":
            ExtendStay(guests);
            break;
        case "8":
            ShowRevenueBreakdown(guests);
            break;
        case "9":
            AlterStay(guests);
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

void DisplayMenu(string title, string[] options)
{
    Console.WriteLine($"========== {title} ==========");
    for (int i = 0; i < options.Length; i++)
        Console.WriteLine($"[{i + 1}] {options[i]}");
    Console.WriteLine("[0] Exit");
}

void ListRooms(List<Room> roomsList, string? prompt)
{
    if (prompt != null) Console.WriteLine(prompt);
        Console.WriteLine($"{"Room No.",-11} {"Type",-11} {"Bed Config.",-14} Daily rate");
    foreach (Room room in roomsList)
        Console.WriteLine($"{room.RoomNumber,-11} {((room is StandardRoom) ? "Standard" : "Deluxe"), -11} {room.BedConfiguration,-14} ${room.DailyRate:0.00}");
}

void ListGuests(List<Guest> guestsList)
{
    Console.WriteLine("The following guests are registered:");
    Console.WriteLine($"{"Name",-15} {"Passport No.",-15} {"Status",-11} Points");
    foreach (Guest guest in guestsList)
        Console.WriteLine($"{guest.Name,-15} {guest.PassportNum,-15} {guest.Member.Status,-11} {guest.Member.Points}");
}

int? ValidateIntInput(string prompt, int? lowerBound = null, int? upperBound = null, bool? offsetBounds = null)
{
    Console.Write(prompt);
    try
    {
        string input = Console.ReadLine();
        if (input.ToUpper() == "X") return -1;
        int intInput = Convert.ToInt32(input) - (offsetBounds is true ? 1 : 0);
        if (lowerBound == null && upperBound == null)
            return intInput;
        if (lowerBound != null && upperBound == null)
            return (intInput >= lowerBound) ? intInput : throw new ArgumentOutOfRangeException(nameof(intInput));
        if (lowerBound == null && upperBound != null)
            return (intInput <= upperBound) ? intInput : throw new ArgumentOutOfRangeException(nameof(intInput));
        return (intInput >= lowerBound && intInput <= upperBound) ? intInput : throw new ArgumentOutOfRangeException(nameof(intInput));;
    }
    catch (FormatException)
    {
        Console.WriteLine("Input should be a numerical value.");
        return null;
    }
    catch (ArgumentOutOfRangeException)
    {
        if (lowerBound != null && upperBound == null) Console.WriteLine($"Input should be at least {lowerBound + (offsetBounds is true ? 1 : 0)}.");
        else if (lowerBound == null && upperBound != null) Console.WriteLine($"Input should be at most {upperBound + (offsetBounds is true ? 1 : 0)}.");
        else Console.WriteLine($"Input should be between {lowerBound + (offsetBounds is true ? 1 : 0)} and {upperBound + (offsetBounds is true ? 1 : 0)} inclusive.");
        return null;
    }
}

DateTime? ValidateDateTimeInput(string format, string prompt, DateTime? compareDate = null, string? comparison = null)
{
    Console.Write(prompt);
    try
    {
        string input = Console.ReadLine();
        if (input.ToUpper() == "X") return DateTime.MinValue;
        DateTime iInput =
            DateTime.ParseExact(input, format, CultureInfo.InvariantCulture);
        if (compareDate == null || comparison == null) return iInput;
        int difference = iInput.Subtract(compareDate.Value).Days;
        switch (comparison)
        {
            case "greater":
                if (difference >= 0)
                    return iInput;
                throw new ArgumentOutOfRangeException(nameof(iInput),
                    $"The given date is before the date {compareDate:dd/MM/yyyy}.");
            case "lesser":
                if (difference <= 0)
                    return iInput;
                throw new ArgumentOutOfRangeException(nameof(iInput),
                    $"The given date is after the date {compareDate:dd/MM/yyyy}.");
            default:
                throw new ArgumentException("Invalid comparison type.");
        }
    }
    catch (FormatException)
    {
        Console.WriteLine($"Input should be in the format {format}.");
        return null;
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine(e.Message);
        return null;
    }
    catch (ArgumentException e)
    {
        Console.WriteLine(e.Message);
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
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine(e.Message);
        return null;
    }
}

// Configures the room with the additional information, like whether Wi-Fi, breakfast, or 
// an additional bed is required.
void ConfigureRoom(Room roomsList)
{
    string[] options = { "Wi-Fi", "breakfast", "an additional bed" };
    foreach (string option in options)
    {
        switch (roomsList)
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
                ValidateBooleanInput($"Is {option} needed for room {roomsList.RoomNumber}? (Y/N): ");
            if (input == null) continue;
            switch (option)
            {
                case "Wi-Fi":
                    ((StandardRoom)roomsList).RequireWifi = input.Value;
                    break;
                case "breakfast":
                    ((StandardRoom)roomsList).RequireBreakfast = input.Value;
                    break;
                case "an additional bed":
                    ((DeluxeRoom)roomsList).AdditionalBed = input.Value;
                    break;
            }
            break;
        }
    }
}

void CheckinGuest(List<Guest> guestsList, List<Room> roomsList)
{
    // Shows a list of checked-out guests and prompts the user to select a guest to check-in
    List<Guest> checkedoutGuests = guestsList.Where(guest => !guest.IsCheckedin).ToList();
    Guest? guest;
    ListGuests(checkedoutGuests);
    while (true)
    {
        var input = ValidateIntInput($"Who is checking in? Enter 1 to {checkedoutGuests.Count} inclusive or X to cancel: ", 0, checkedoutGuests.Count - 1, true);
        if (input == null) continue;
        if (input == -1) return;
        guest = guestsList[input.Value];
        break;
    }

    // Prompts the user for the check-in and check-out date
    DateTime? checkinDate;
    DateTime? checkoutDate;
    while (true)
    {
        var input = ValidateDateTimeInput("dd/MM/yyyy", $"\nWhen is {guest.Name} checking in? Enter in the format dd/MM/yyyy or X to cancel: ", DateTime.Today, "lesser");
        if (input == null) continue;
        if (input == DateTime.MinValue) return;
        checkinDate = input;
        break;
    }
    while (true)
    {
        var input = ValidateDateTimeInput("dd/MM/yyyy", $"When is {guest.Name} checking out? Enter in the format dd/MM/yyyy or X to cancel: ", checkinDate, "greater");
        if (input == null) continue;
        if (input == DateTime.MinValue) return;
        checkoutDate = input;
        break;
    }

    // Creates a new Stay object based on the provided information
    Stay stay = new(checkinDate.Value, checkoutDate.Value);

    // Lists the available rooms and prompts the user to select a room
    bool repeat;
    do
    {
        Console.WriteLine();
        List<Room> availableRooms = roomsList.Where(room => room.IsAvail).ToList();
        ListRooms(availableRooms, "The following rooms are available for check-in:");
        while (true)
        {
            var input = ValidateIntInput($"Which room is {guest.Name} staying in? Enter 1 to {availableRooms.Count} inclusive or X to cancel: ", 0, availableRooms.Count - 1, true);
            if (input == null) continue;
            if (input == -1) return;
            Room bookedRoom = availableRooms[input.Value];

            // Makes the room unavailable
            roomsList.Find(room => room.RoomNumber == bookedRoom.RoomNumber).IsAvail = false;

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
    Console.WriteLine();
}

void ExtendStay(List<Guest> guestsList)
{
    // Shows a list of checked-in guests and prompts the user to select a guest
    List<Guest> checkedinGuests = guestsList.Where(guest => guest.IsCheckedin).ToList();
    Guest? guest;
    ListGuests(checkedinGuests);
    while (true)
    {
        int? input = ValidateIntInput($"Who would like to extend their stay? Enter 1 to {checkedinGuests.Count} inclusive or X to cancel: ", 0, checkedinGuests.Count - 1, true);
        if (input == null) continue;
        if (input == -1) return;
        guest = checkedinGuests[input.Value];
        break;
    }

    // Gets the stay information for the guest
    Stay stay = guest.HotelStay;
    DateTime checkoutDate = stay.CheckoutDate;
    while (true)
    {
        Console.WriteLine();
        int? input = ValidateIntInput($"How many days would you like to extend the stay for {guest.Name}? Alternatively, enter X to cancel: ", lowerBound: 1);
        if (input == null) continue;
        if (input == -1) return;
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
    Console.WriteLine();
}

void ShowRevenueBreakdown(List<Guest> guestsList)
{
    int selectedYear;
    while (true)
    {
        int? input = ValidateIntInput("Enter the year or X to cancel: ");
        if (input == null) continue;
        if (input == -1) return;
        selectedYear = input.Value;
        break;
    }

    double[] breakdown = new double[12];
    
    // Loops through each Guest and their Stay object and adds the information to the breakdown if the years match
    foreach (Guest guest in guestsList)
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
                , g.Name, g.PassportNum, g.Member.Status, g.Member.Points, g.HotelStay.CheckinDate.ToString("dd/MM/yyyy"), g.HotelStay.CheckoutDate.ToString("dd/MM/yyyy"));
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
        int? user_choice = ValidateIntInput("Your choice? Alternatively, enter X to cancel ", 0, guests.Count(), true);
        if (user_choice == -1) return;
        choice = Convert.ToInt32(user_choice);
        if (choice >= guests.Count())
        {
            Console.WriteLine("Please enter a valid option.");
        }
        else if (choice < 0)
        {
            Console.WriteLine("Please enter a valid option.");
        }
        else
        {
            break;
        }
    }
    displayguestdeatils(choice);
}

// This method is created as there is more than one use of the table
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
        , guests[num].Name, guests[num].PassportNum, guests[num].HotelStay.CheckinDate.ToString("dd/MM/yyyy"), guests[num].HotelStay.CheckoutDate.ToString("dd/MM/yyyy"),""
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
        
        if (g.IsCheckedin)
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
        int? user_choice = ValidateIntInput("Your choice? Alternatively, enter X to cancel: ", 0, guests.Count-1, true);
        if (user_choice == -1) return;
        if (user_choice > guests.Count-1)
        {
            Console.WriteLine("Please enter a valid option");
            checkoutguest();
        }
        else if (user_choice == null)
        {
            checkoutguest();
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
        {// This is to display what the guest got
            if (r.GetType() == typeof(StandardRoom))
            {
                StandardRoom s_room = (StandardRoom)r;
                total_cost += s_room.CalculateCharges()*days_stayed;
                    Console.WriteLine(
                        "|Room Number: {0,23}|\n" +
                        "|Room Type: {1,25}|\n" +
                        "|Bed Configureation: {2,16}|\n" +
                        "|Days Stayed: {3,23}|\n" +
                        "|Wi-Fi $10 Added: {4,19}|\n" +
                        "|Breakfast $20 Added: {5,15}|\n" +
                        "|Daily Rate: {6,24}|\n" +
                        "|Cost : {7,29}|\n" +
                        "======================================"
                        , r.RoomNumber, "Standard Room",s_room.BedConfiguration,days_stayed, s_room.RequireWifi, s_room.RequireBreakfast,s_room.CalculateCharges().ToString("$0.00"),(r.CalculateCharges()*days_stayed).ToString("$0.00"));
            }
            else if (r.GetType() == typeof(DeluxeRoom))
                {
                    DeluxeRoom s_room = (DeluxeRoom)r;
                    total_cost += s_room.CalculateCharges() * days_stayed;
                    Console.WriteLine(
                        "|Room Number: {0,23}|\n" +
                        "|Room Type: {1,25}|\n" +
                        "|Bed Configuration: {2,17}|\n" +
                        "|Days Stayed: {3,23}|\n" +
                        "|Additional Bed $25: {4,16}|\n" +
                        "|Daily Rate: {5,24}|\n" +
                        "|Cost : {6,29}|\n" +
                        "======================================"
                        , r.RoomNumber, "Deluxe Room",s_room.BedConfiguration, days_stayed,s_room.AdditionalBed, s_room.CalculateCharges().ToString("$0.00"),  (s_room.CalculateCharges() * days_stayed).ToString("$0.00"));
            }
            
        }
        // This is the total bill for the guest before any deduction from the points
        Console.WriteLine(
            "|Total Bill: {0,24}|\n" +
            "" +
            "======================================"
            , total_cost.ToString("$0.00"));
        
        while (true)
        {
            // check if the user is eligible for the points system
            if (temp_list[choice].Member.Status != "Silver" && temp_list[choice].Member.Status != "Gold")
            {
                Console.WriteLine("You are not eligible to redeem points as your membership status is below Silver");
                break;
            }  // Ask if the guest want to use their points
            bool loop = true;
            while (loop)
            {
                bool? verify = ValidateBooleanInput($"You have {temp_list[choice].Member.Points} points to redeem. Do you want to redeem your points? (Y/N) ");
                    // Loop if any other answer is given
                    // This if is just a precaution
                if (verify is null)
                {
                
                }
                
                else if (verify is not true)
                {
                    Console.WriteLine("You will not use your points");
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
                        int? use_points = ValidateIntInput($"You have {temp_list[choice].Member.Points} points, how many do you want to redeem?", 0, temp_list[choice].Member.Points, false);
                        if (use_points >= 0 && use_points <= temp_list[choice].Member.Points)
                        {
                            points_to_redeem = Convert.ToInt32(use_points);
                            break;
                        }
                    }
                    total_cost -= points_to_redeem;
                        // Prints the total price after deduction
                    Console.WriteLine(
                        "======================================\n" +
                        "|Total Bill: {0,24}|\n" +
                        "" +
                        "======================================"
                        , total_cost.ToString("$0.00"));
                    Console.Write("Please enter any key to make payment: ");
                    //This readline is soley for they guest to key random stuff in and will not be recorded
                    Console.ReadLine();
                    Console.WriteLine($"Your redeemed {points_to_redeem} out of {temp_list[choice].Member.Points} points ");
                    temp_list[choice].Member.RedeemPoints(points_to_redeem);
                    loop = false;
                    break;
                }
                
            }
            break;
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
                    {// This will display is there is an upgrade to the membership of the guest
                    if (g.Member.Points >= 200)
                    {
                        g.Member.Status = "Gold";
                        Console.WriteLine("Congrats! You are now a Gold Member!");
                    }
                    break;
                    }

                if (g.Member.Status == "Ordinary")
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

void AlterStay(List<Guest> guestsList)
{
    int num = 1;
    int num_of_people_not_checked_in = 0;
    // Prompts the user for the Guest to Checkout//
    Console.WriteLine("Select a Guest to Alter:");
    //Display the guest with number options using the variable created before as number indicator//
    //Create a temporary list to store people who have checked in. Guest who are not checked in will not be stored
    List<Guest> temp_list = new List<Guest>();

    //Only people who are checked in will be displayed//
    foreach (Guest guest in guestsList)
    {

        if (guest.IsCheckedin)
        {   //There is a chance of 2 guest having the same name but the passport num is unique
            Console.WriteLine("{0,0}. {1,0}  Passport Num: {2,0}", num, guest.Name, guest.PassportNum);
            num += 1;
            temp_list.Add(guest);
        }
        else
        {
            num_of_people_not_checked_in += 1;
        }
    }
    if (num_of_people_not_checked_in == guestsList.Count())
    {
        Console.WriteLine("There are currently no one checked in.");
    }
    else
    {
        //Check if the option selected is valid if not retry//
        int? user_choice = ValidateIntInput("Your choice? Alternatively, enter X to cancel: ", 0, temp_list.Count()-1, true);
        if (user_choice == -1) return;
        if (user_choice > temp_list.Count())
        {
            Console.WriteLine("Please enter a valid option.");
            AlterStay(guestsList);
        }
        else if (user_choice == null)
        {
            AlterStay(guestsList);
        }

        // int g will be the guest we edit when we call guests[g]
        int g = 0;
        foreach (Guest i in guestsList)
        {
            if (i.PassportNum == temp_list[Convert.ToInt32(user_choice)].PassportNum)
            {
                break;
            }
            g += 1;
        }
        displayguestdeatils(g);
        Guest guest = guestsList[g];
        Stay stay = guest.HotelStay;

        // Shows a short menu of what to change
        while (true)
        {
            Console.WriteLine();
            DisplayMenu("Alter a guest's stay", new[] { "Change check-in date", "Change check-out date", "Change room configuration" });
            Console.Write("Enter an option: ");
            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                case "2":
                    // Changes the check-in or check-out date
                    do
                    {
                        string situation = choice == "1" ? "check-in" : "check-out";
                        DateTime? newDate = ValidateDateTimeInput("dd/MM/yyyy",
                            $"Enter the new {situation} date in dd/MM/yyyy format or X to cancel: ",
                            choice == "1" ? DateTime.Today : stay.CheckinDate.AddDays(1), choice == "1" ? "lesser" : "greater");
                        if (newDate == null) continue;
                        if (newDate == DateTime.MinValue) return;
                        while (true)
                        {
                            bool? confirm =
                                ValidateBooleanInput(
                                    $"Are you sure you want to change the {situation} date from {(choice == "1" ? stay.CheckinDate : stay.CheckoutDate):dd/MM/yyyy} to {newDate:dd/MM/yyyy}? (Y/N) ");
                            if (confirm is null) continue;
                            string capitalizedString = situation[..1].ToUpper() + situation[1..];
                            if (confirm is true)
                            {
                                stay.CheckinDate = Convert.ToDateTime(newDate);
                                Console.WriteLine($"{capitalizedString} date changed successfully!");
                                break;
                            }

                            Console.WriteLine($"{capitalizedString} date not changed.");
                            break;
                        }

                        break;
                    } while (true);
                    break;
                case "3":
                    do
                    {
                        // If the guest has more than one room, prompt them to make a choice
                        Room room;
                        if (stay.RoomList.Count > 1)
                        {
                            displayguestdeatils(guestsList.IndexOf(guest));
                            int? roomChoice = ValidateIntInput("Enter the room number to change or X to cancel: ", 0, stay.RoomList.Count - 1, true);
                            if (roomChoice == -1) break;
                            if (roomChoice is null) continue;
                            room = stay.RoomList[Convert.ToInt32(roomChoice)];
                        }
                        else
                            room = stay.RoomList[0];

                        // Display the room details
                        Console.WriteLine("Room details:");
                        Console.WriteLine($"Room number: {room.RoomNumber}");

                        switch (room)
                        {
                            case StandardRoom sr:
                                Console.WriteLine($"Requires Wi-Fi: {(sr.RequireWifi ? "Yes" : "No")}");
                                Console.WriteLine($"Requires breakfast: {(sr.RequireWifi ? "Yes" : "No")}");
                                break;
                            case DeluxeRoom dr:
                                Console.WriteLine($"Additional bed: {(dr.AdditionalBed ? "Yes" : "No")}");
                                break;
                        }

                        ConfigureRoom(room);
                        Console.WriteLine("Room configuration changed successfully!");

                        break;
                    } while (true);

                    break;
                case "0":
                    return;
            }
        }
    }
}

//==========================================================
// INITIALIZATION METHODS
// The functions below are created and called to initialize
// the required variables and objects for the program to
// work.
//==========================================================

void InitializeRooms(List<Room> roomsList)
{
    using StreamReader sr = new("Rooms.csv");
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
        roomsList.Add(room);
    }
}

void InitializeRoom(List<Room> initRoomList, Room? room, Guest guest, string checkedin, string[] requirements, bool optional = false)
{
    try
    {
        if (room != null)
        {
            if (checkedin == "TRUE")
            {
                guest.IsCheckedin = true;
                room.IsAvail = false;
                if (requirements[0] == "TRUE" && room is StandardRoom sWRoom)
                    sWRoom.RequireWifi = true;
                if (requirements[1] == "TRUE" && room is StandardRoom sBRoom)
                    sBRoom.RequireBreakfast = true;
                if (requirements[2] == "TRUE" && room is DeluxeRoom dRoom)
                    dRoom.AdditionalBed = true;
            }

            initRoomList.Add(room);
        }
        else
            throw new ArgumentOutOfRangeException(nameof(initRoomList), "A stay must have at least one room.");
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine(e.Message);
    }
    catch (FormatException)
    {
        if (!optional) Console.WriteLine("Invalid data format. Please check the data file.");
    }
}

void InitializeGuests(List<Guest> g, List<Room> r)
{
    // IMPORTANT: InitializeRooms() must be invoked before this function.

    using StreamReader sr = new("Guests.csv");
    string? line = sr.ReadLine();
    while ((line = sr.ReadLine()) != null)
    {
        string[] data = line.Split(',');
        Guest guest = new();

        // Creates a stay object
        Stay? stay = null;
        using (StreamReader staySR = new("Stays.csv"))
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
        Membership membership = new(data[2], Convert.ToInt32(data[3]));

        // Creates a guest object
        guest.Name = data[0];
        guest.PassportNum = data[1];
        guest.Member = membership;
        guest.HotelStay = stay ?? new Stay();
        g.Add(guest);
    }
}