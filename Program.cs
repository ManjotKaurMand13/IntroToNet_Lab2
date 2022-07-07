using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Room
    {
        // data members
        public string Number;
        public int Capacity;
        public Boolean occupied;
        public List<Reservation> Reservations;
        public string Rating;

        // constructor
        public Room(string number, int capacity, string rating)
        {
            Number = number;
            Capacity = capacity;
            occupied = false;
            Reservations = new List<Reservation>();
            Rating = rating;
        }
    }

    class Reservation
    {
        // data members
        public DateTime Date;
        public int Occupants;
        public bool IsCurrent;
        public Client Client;
        public Room Room;

        // constructor
        public Reservation(DateTime date, int occupants, bool isCurrent, Client client, Room room)
        {
            Date = date;
            Occupants = occupants;
            IsCurrent = isCurrent;
            Client = client;
            Room = room;
        }


        // method to create a new reservation
        public void CreateReservation(DateTime startDate, int occupants, Client client, Room room)
        {
            // create date object
            DateTime created = DateTime.Now;
            // create reservation object
            Reservation reservation = new Reservation(startDate, occupants, true, client, room);
            // add reservation to list
            room.Reservations.Add(reservation);
            // set room to occupied
            client.Reservations.Add(reservation);
        }

        // method to remove a room reservation
        public void ReserveRoom(DateTime startDate, int occupants, Client client, Room room)
        {
            DateTime created = DateTime.Now;
            Reservation reservation = new Reservation(startDate, occupants, true, client, room);
            room.Reservations.Add(reservation);
            client.Reservations.Add(reservation);
        }


    }

    class Client
    {
        // data members
        public string Name;
        public int CreditCard;
        public List<Reservation> Reservations;

        // Constructor
        public Client(string name, int creditCard)
        {
            Name = name;
            CreditCard = creditCard;
            Reservations = new List<Reservation>();
        }
    }

    class Hotel
    {
        // data members and getters/setters
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<Client> Clients { get; set; }

        // method to get client by client id
        public Client GetClient(int clientID)
        {
            foreach (Client c in Clients)
            {
                if (c.CreditCard == clientID)
                {
                    return c;
                }
            }
            return null;
        }

        // method to get reservation by reservation id
        public Reservation GetReservation(int ID)
        {
            foreach (Reservation r in Reservations)
            {
                if (r.Occupants == ID)
                {
                    return r;
                }
            }
            return null;
        }

        // method to get room by room number
        public Room GetRoom(string roomNumber)
        {
            foreach (Room r in Rooms)
            {
                if (r.Number == roomNumber)
                {
                    return r;
                }
            }
            return null;
        }

        // method to get list of vacant rooms
        public List<Room> GetVacantRooms()
        {
            List<Room> vacantRooms = new List<Room>();
            foreach (Room r in Rooms)
            {
                if (!r.occupied)
                {
                    vacantRooms.Add(r);
                }
            }
            return vacantRooms;
        }

        //  method to get top three clients by number of reservations
        public List<Client> TopThreeClients()
        {
            // create list of clients
            List<Client> topThreeClients = new List<Client>();
            // create list of reservations
            List<int> topThreeClientIDs = new List<int>();
            // create list of client ids
            List<int> topThreeClientCount = new List<int>();
            // loop through clients
            foreach (Client c in Clients)
            {
                int count = 0;
                foreach (Reservation r in Reservations)
                {
                    if (r.Client.CreditCard == c.CreditCard)
                    {
                        count++;
                    }
                }
                topThreeClientIDs.Add(c.CreditCard);
                topThreeClientCount.Add(count);
            }
            for (int i = 0; i < 3; i++)
            {
                int max = 0;
                int maxIndex = 0;
                for (int j = 0; j < topThreeClientCount.Count; j++)
                {
                    if (topThreeClientCount[j] > max)
                    {
                        max = topThreeClientCount[j];
                        maxIndex = j;
                    }
                }
                topThreeClients.Add(GetClient(topThreeClientIDs[maxIndex]));
                topThreeClientCount.RemoveAt(maxIndex);
                topThreeClientIDs.RemoveAt(maxIndex);
            }
            return topThreeClients;
        }

        // method to automatically reserve a room for a client
        public Reservation AutomaticReservation(int clientID, int occupants)
        {
            foreach (Room r in Rooms)
            {
                if (!r.occupied && r.Capacity >= occupants)
                {
                    Reservation res = new Reservation(DateTime.Now, occupants, true, GetClient(clientID), r);
                    r.Reservations.Add(res);
                    r.occupied = true;
                    return res;
                }
            }
            return null;
        }

        // method to checkin a client by name
        public void Checkin(string clientName)
        {
            foreach (Client c in Clients)
            {
                if (c.Name == clientName)
                {
                    foreach (Reservation r in c.Reservations)
                    {
                        r.IsCurrent = true;
                    }
                }
            }
        }

        // method to checkout a client by room number
        public void CheckoutRoom(int RoomNumber)
        {
            foreach (Room r in Rooms)
            {
                if (r.Number == RoomNumber.ToString())
                {
                    r.occupied = false;
                }
            }
        }

        // method to checkout a client by client name

        public void CheckoutRoom(string clientName)
        {
            foreach (Client c in Clients)
            {
                if (c.Name == clientName)
                {
                    foreach (Reservation r in c.Reservations)
                    {
                        r.IsCurrent = false;
                    }
                }
            }
        }

        // get total capacity of all rooms
        public int TotalCapacityRemaining()
        {

            int totalCapacity = 0;
            foreach (Room r in Rooms)
            {
                totalCapacity += r.Capacity;
            }

            int totalOccupants = 0;
            foreach (Room r in Rooms)
            {
                foreach (Reservation res in r.Reservations)
                {
                    if (res.IsCurrent)
                    {
                        totalOccupants += res.Occupants;
                    }
                }
            }
            return totalCapacity - totalOccupants;


        }

        // get average occupancy of all rooms
        public int AverageOccupancyPercentage()
        {
            int totalOccupants = 0;
            int totalCapacity = 0;
            foreach (Room r in Rooms)
            {
                foreach (Reservation res in r.Reservations)
                {
                    if (res.IsCurrent)
                    {
                        totalOccupants += res.Occupants;
                    }
                }
            }
            foreach (Room r in Rooms)
            {
                totalCapacity += r.Capacity;
            }
            return (totalOccupants / totalCapacity) * 100;
        }

        // Bonus question: get average occupancy of all rooms by client name
        public List<Reservation> FutureBookings()
        {
            List<Reservation> futureBookings = new List<Reservation>();
            foreach (Reservation r in Reservations)
            {
                if (r.Date > DateTime.Now)
                {
                    futureBookings.Add(r);
                }
            }
            return futureBookings;
        }



        public static void Main(string[] args)
        {
            Hotel hotel = new Hotel();
            hotel.Name = "Hotel California";
            hotel.Address = "123 Main St";
            hotel.Rooms = new List<Room>();
            hotel.Reservations = new List<Reservation>();
            hotel.Clients = new List<Client>();

            // create rooms
            Room room1 = new Room("101", 2, "A");
            Room room2 = new Room("102", 2, "A");
            Room room3 = new Room("103", 2, "A");
            Room room4 = new Room("104", 2, "A");
            Room room5 = new Room("105", 2, "A");

            // add rooms to hotel
            hotel.Rooms.Add(room1);
            hotel.Rooms.Add(room2);
            hotel.Rooms.Add(room3);
            hotel.Rooms.Add(room4);
            hotel.Rooms.Add(room5);


            // create clients
            Client client1 = new Client("John Smith", 123456789);
            Client client2 = new Client("Jane Doe", 987654321);
            Client client3 = new Client("Jack Black", 98765432);

            // add clients to hotel
            hotel.Clients.Add(client1);
            hotel.Clients.Add(client2);
            hotel.Clients.Add(client3);

            // create reservations
            Reservation reservation1 = new Reservation(DateTime.Now, 2, true, client1, room1);
            Reservation reservation2 = new Reservation(DateTime.Now, 2, true, client2, room2);
            Reservation reservation3 = new Reservation(DateTime.Now, 2, true, client3, room3);

            // add reservations to hotel
            hotel.Reservations.Add(reservation1);
            hotel.Reservations.Add(reservation2);
            hotel.Reservations.Add(reservation3);

            // print hotel info
            Console.WriteLine("Hotel Name: " + hotel.Name);
            Console.WriteLine("Hotel Address: " + hotel.Address);
            Console.WriteLine("Rooms:");
            foreach (Room room in hotel.Rooms)
            {
                Console.WriteLine("Room Number: " + room.Number);
                Console.WriteLine("Room Capacity: " + room.Capacity);
                Console.WriteLine("Room Occupied: " + room.occupied);
                Console.WriteLine();
            }

            // print reservations
            Console.WriteLine("Reservations:");
            foreach (Reservation reservation in hotel.Reservations)
            {
                Console.WriteLine("Reservation Date: " + reservation.Date);
                Console.WriteLine("Reservation Occupants: " + reservation.Occupants);
                Console.WriteLine("Reservation IsCurrent: " + reservation.IsCurrent);
                Console.WriteLine("Reservation Client: " + reservation.Client.Name);
                Console.WriteLine("Reservation Room: " + reservation.Room.Number);
                Console.WriteLine();
            }

            // call checkin method and print results
            Console.WriteLine("Checking in client " + client1.Name + " to room " + room1.Number);
            hotel.Checkin(client1.Name);

            // print capacity remaining
            Console.WriteLine("Total Capacity Remaining: " + hotel.TotalCapacityRemaining());
            Console.WriteLine("Average Occupancy Percentage: " + hotel.AverageOccupancyPercentage());
            // print reservation for room 103
            Console.WriteLine("\n\n     *******Reservation Details for room 103********     ");
            foreach (Reservation reservation in hotel.Reservations)
            {
                if (reservation.Room.Number == "103")
                {
                    Console.WriteLine("Reservation Date: " + reservation.Date);
                    Console.WriteLine("Reservation Occupants: " + reservation.Occupants);
                    Console.WriteLine("Reservation IsCurrent: " + reservation.IsCurrent);
                    Console.WriteLine("Reservation Client: " + reservation.Client.Name);
                    Console.WriteLine("Reservation Room: " + reservation.Room.Number);
                    Console.WriteLine();
                }
            }
        }

    }
}


