using HotelApp;

List<Room> rooms = new List<Room>();

rooms.Add(new StandardRoom(101, "Single", 90, true));
Console.WriteLine(rooms[0].ToString());