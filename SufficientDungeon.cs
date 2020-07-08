using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class SufficientDungeon : Dungeon
{
    private List<Room> todo;
    Random widthR = new Random();
    Random heightR = new Random();

    public SufficientDungeon(Size pSize) : base(pSize) {}

    protected override void generate(int pMinimumRoomSize)
    {
        // First we create a list for rooms that can still be split into two rooms
        todo = new List<Room>();
        // Then we add a room with the size of the dungeon 
        todo.Add(new Room(new Rectangle(0, 0, size.Width, size.Height)));

        // Spliting the rooms and removing them from "todo" list to "rooms" list if they cannot be split any further
        while (todo.Count > 0)
        {
            Split(todo[0], pMinimumRoomSize);
        }

        // Adding doors to connect two neighbouring rooms
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = 0; j < rooms.Count; j++)
            {
                AddDoor(rooms[i], rooms[j]);               
            }
        }
    }

    /// <summary>
    /// Splits a room into two by creating two smaller rooms and removing 
    /// the room on which the method has been called on.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="pMinimumRoomSize"></param>
    public void Split(Room room, int pMinimumRoomSize)
    {       
        if (room.area.Width >= pMinimumRoomSize * 2 && room.area.Width >= room.area.Height)
        {
            // Vertical Split
            int widthRandomLength = widthR.Next(pMinimumRoomSize + 1, room.area.Width - pMinimumRoomSize + 1);
            todo.Add(new Room(new Rectangle(room.area.X, room.area.Y, widthRandomLength + 1, room.area.Height)));          
            todo.Add(new Room(new Rectangle(room.area.X + widthRandomLength, room.area.Y, room.area.Width - widthRandomLength, room.area.Height)));
            todo.Remove(room);
        }
        else if (room.area.Height >= pMinimumRoomSize * 2 && room.area.Width < room.area.Height)
        {
            // Horizontal Split
            int heightRandomLength = heightR.Next(pMinimumRoomSize + 1, room.area.Height - pMinimumRoomSize + 1);
            todo.Add(new Room(new Rectangle(room.area.X, room.area.Y, room.area.Width, heightRandomLength + 1)));
            todo.Add(new Room(new Rectangle(room.area.X, room.area.Y + heightRandomLength, room.area.Width, room.area.Height - heightRandomLength)));
            todo.Remove(room);
        }
        else
        {
            // If it cannot be split anymore, add it to the rooms list and remove it from todo list
            rooms.Add(room);
            todo.Remove(room);
        }
    }

    /// <summary>
    /// Adds a door on the coordinates where two rooms intersect with each other
    /// </summary>
    /// <param name="roomA"></param>
    /// <param name="roomB"></param>
    public void AddDoor(Room roomA, Room roomB)
    {
        if (roomA.area.IntersectsWith(roomB.area))
        {
            //-----------------------------------------------------------------------------------------------------------------------------------------//
            //									   ROOM A TOP = ROOM B BOTTOM || ROOM A BOTTOM = ROOM B TOP                                  
            //-----------------------------------------------------------------------------------------------------------------------------------------//
            if ((roomA.area.Top == roomB.area.Bottom - 1 || roomA.area.Bottom == roomB.area.Top + 1))
            {
                // If B is equal to A
                if (roomA.area.Left == roomB.area.Left && roomA.area.Right == roomB.area.Right)
                {
                    // TOP
                    if (roomA.area.Top == roomB.area.Bottom - 1 && roomA.doorTop == false && roomB.doorBottom == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomA.area.Right - 1), roomA.area.Top), roomA, roomB));
                        roomA.doorTop = true;
                        roomB.doorBottom = true;
                    }
                    // BOTTOM
                    else if (roomA.area.Bottom == roomB.area.Top + 1 && roomA.doorBottom == false && roomB.doorTop == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomA.area.Right - 1), roomB.area.Top), roomA, roomB));
                        roomA.doorBottom = true;
                        roomB.doorTop = true;
                    }
                }
                // If B is bigger than A               
                else if (roomA.area.Left > roomB.area.Left && roomA.area.Right < roomB.area.Right)
                {
                    // TOP
                    if (roomA.area.Top == roomB.area.Bottom - 1 && roomA.doorTop == false && roomB.doorBottom == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomA.area.Right), roomA.area.Top), roomA, roomB));
                        roomA.doorTop = true;
                        roomB.doorBottom = true;
                    }
                    // BOTTOM
                    else if (roomA.area.Bottom == roomB.area.Top + 1 && roomA.doorBottom == false && roomB.doorTop == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomA.area.Right), roomB.area.Top), roomA, roomB));
                        roomA.doorBottom = true;
                        roomB.doorTop = true;
                    }
                }
                // If B is smaller than A and within it
                else if (roomA.area.Left < roomB.area.Left && roomA.area.Right > roomB.area.Right)
                {
                    // TOP
                    if (roomA.area.Top == roomB.area.Bottom - 1 && roomA.doorTop == false && roomB.doorBottom == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomB.area.Left + 1, roomB.area.Right - 1), roomA.area.Top), roomA, roomB));
                        roomA.doorTop = true;
                        roomB.doorBottom = true;
                    }
                    // BOTTOM
                    else if (roomA.area.Bottom == roomB.area.Top + 1 && roomA.doorBottom == false && roomB.doorTop == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomB.area.Left + 1, roomB.area.Right - 1), roomB.area.Top), roomA, roomB));
                        roomA.doorBottom = true;
                        roomB.doorTop = true;
                    }
                }
                // If B is on the left side of A
                else if (roomA.area.Left > roomB.area.Left && roomA.area.Right > roomB.area.Right && roomA.area.Left < roomB.area.Right - 2)
                {
                    // TOP
                    if (roomA.area.Top == roomB.area.Bottom - 1 && roomA.doorTop == false && roomB.doorBottom == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomB.area.Right - 1), roomA.area.Top), roomA, roomB));
                        roomA.doorTop = true;
                        roomB.doorBottom = true;
                    }
                    // BOTTOM
                    else if (roomA.area.Bottom == roomB.area.Top + 1 && roomA.doorBottom == false && roomB.doorTop == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomB.area.Right - 1), roomB.area.Top), roomA, roomB));
                        roomA.doorBottom = true;
                        roomB.doorTop = true;
                    }
                }
                // If B is on the left side of A but the right sides are the same
                else if (roomA.area.Left > roomB.area.Left && roomA.area.Right == roomB.area.Right)
                {
                    // TOP
                    if (roomA.area.Top == roomB.area.Bottom - 1 && roomA.doorTop == false && roomB.doorBottom == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomA.area.Right - 1), roomA.area.Top), roomA, roomB));
                        roomA.doorTop = true;
                        roomB.doorBottom = true;
                    }
                    // BOTTOM
                    else if (roomA.area.Bottom == roomB.area.Top + 1 && roomA.doorBottom == false && roomB.doorTop == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomA.area.Right - 1), roomB.area.Top), roomA, roomB));
                        roomA.doorBottom = true;
                        roomB.doorTop = true;
                    }
                }
                // If B is on the left side of A but the left sides are the same
                else if (roomA.area.Left == roomB.area.Left && roomA.area.Right > roomB.area.Right)
                {
                    // TOP
                    if (roomA.area.Top == roomB.area.Bottom - 1 && roomA.doorTop == false && roomB.doorBottom == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomB.area.Right - 1), roomA.area.Top), roomA, roomB));
                        roomA.doorTop = true;
                        roomB.doorBottom = true;
                    }
                    // BOTTOM
                    else if (roomA.area.Bottom == roomB.area.Top + 1 && roomA.doorBottom == false && roomB.doorTop == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomB.area.Right - 1), roomB.area.Top), roomA, roomB));
                        roomA.doorBottom = true;
                        roomB.doorTop = true;
                    }
                }
                // If B is on the right side of A
                else if (roomA.area.Left < roomB.area.Left && roomA.area.Right < roomB.area.Right && roomA.area.Right > roomB.area.Left + 2)
                {
                    // TOP
                    if (roomA.area.Top == roomB.area.Bottom - 1 && roomA.doorTop == false && roomB.doorBottom == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomB.area.Left + 1, roomA.area.Right - 1), roomA.area.Top), roomA, roomB));
                        roomA.doorTop = true;
                        roomB.doorBottom = true;
                    }
                    // BOTTOM
                    else if (roomA.area.Bottom == roomB.area.Top + 1 && roomA.doorBottom == false && roomB.doorTop == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomB.area.Left + 1, roomA.area.Right - 1), roomB.area.Top), roomA, roomB));
                        roomA.doorBottom = true;
                        roomB.doorTop = true;
                    }
                }
                // If B is on the right side of A but the left sides are the same
                else if (roomA.area.Left == roomB.area.Left && roomA.area.Right < roomB.area.Right)
                {
                    // TOP
                    if (roomA.area.Top == roomB.area.Bottom - 1 && roomA.doorTop == false && roomB.doorBottom == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomA.area.Right - 1), roomA.area.Top), roomA, roomB));
                        roomA.doorTop = true;
                        roomB.doorBottom = true;
                    }
                    // BOTTOM
                    else if (roomA.area.Bottom == roomB.area.Top + 1 && roomA.doorBottom == false && roomB.doorTop == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomA.area.Left + 1, roomA.area.Right - 1), roomB.area.Top), roomA, roomB));
                        roomA.doorBottom = true;
                        roomB.doorTop = true;
                    }
                }
                // If B is on the right side of A but the right sides are the same
                else if (roomA.area.Left < roomB.area.Left && roomA.area.Right == roomB.area.Right)
                {
                    // TOP
                    if (roomA.area.Top == roomB.area.Bottom - 1 && roomA.doorTop == false && roomB.doorBottom == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomB.area.Left + 1, roomA.area.Right - 1), roomA.area.Top), roomA, roomB));
                        roomA.doorTop = true;
                        roomB.doorBottom = true;
                    }
                    // BOTTOM
                    else if (roomA.area.Bottom == roomB.area.Top + 1 && roomA.doorBottom == false && roomB.doorTop == false)
                    {
                        doors.Add(new Door(new Point(Utils.Random(roomB.area.Left + 1, roomA.area.Right - 1), roomB.area.Top), roomA, roomB));
                        roomA.doorBottom = true;
                        roomB.doorTop = true;
                    }
                }
                else if (roomA.area.Right == roomB.area.Left || roomA.area.Left == roomB.area.Right)
                {
                    // do nothing SKIP
                }
            }
            //-----------------------------------------------------------------------------------------------------------------------------------------//
            //									   ROOM A LEFT = ROOM B RIGHT || ROOM A RIGHT = ROOM B LEFT                                  
            //-----------------------------------------------------------------------------------------------------------------------------------------//
            else if ((roomA.area.Right == roomB.area.Left + 1 || roomA.area.Left == roomB.area.Right - 1))
            {
                // If B is the same size as A
                if (roomA.area.Top == roomB.area.Top && roomA.area.Bottom == roomB.area.Bottom)
                {
                    // LEFT
                    if (roomA.area.Left == roomB.area.Right - 1 && roomA.doorLeft == false && roomB.doorRight == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Left, Utils.Random(roomA.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorLeft = true;
                        roomB.doorRight = true;
                    }
                    // RIGHT
                    else if (roomA.area.Right == roomB.area.Left + 1 && roomA.doorRight == false && roomB.doorLeft == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Right - 1, Utils.Random(roomA.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorRight = true;
                        roomB.doorLeft = true;
                    }
                }
                // If B is bigger than A
                else if (roomA.area.Top > roomB.area.Top && roomA.area.Bottom < roomB.area.Bottom)
                {
                    // LEFT
                    if (roomA.area.Left == roomB.area.Right - 1 && roomA.doorLeft == false && roomB.doorRight == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Left, Utils.Random(roomA.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorLeft = true;
                        roomB.doorRight = true;
                    }
                    // RIGHT
                    else if (roomA.area.Right == roomB.area.Left + 1 && roomA.doorRight == false && roomB.doorLeft == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Right - 1, Utils.Random(roomA.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorRight = true;
                        roomB.doorLeft = true;
                    }
                }
                // If B is smaller than A and within it
                else if (roomA.area.Top < roomB.area.Top && roomA.area.Bottom > roomB.area.Bottom)
                {
                    // LEFT
                    if (roomA.area.Left == roomB.area.Right - 1 && roomA.doorLeft == false && roomB.doorRight == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Left, Utils.Random(roomB.area.Top + 1, roomB.area.Bottom)), roomA, roomB));
                        roomA.doorLeft = true;
                        roomB.doorRight = true;
                    }
                    // RIGHT
                    else if (roomA.area.Right == roomB.area.Left + 1 && roomA.doorRight == false && roomB.doorLeft == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Right - 1, Utils.Random(roomB.area.Top + 1, roomB.area.Bottom)), roomA, roomB));
                        roomA.doorRight = true;
                        roomB.doorLeft = true;
                    }
                }
                // If B is on top side of A
                else if (roomA.area.Top > roomB.area.Top && roomA.area.Bottom > roomB.area.Bottom && roomA.area.Top < roomB.area.Bottom - 2)
                {
                    // LEFT
                    if (roomA.area.Left == roomB.area.Right - 1 && roomA.doorLeft == false && roomB.doorRight == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Left, Utils.Random(roomA.area.Top + 1, roomB.area.Bottom - 1)), roomA, roomB));
                        roomA.doorLeft = true;
                        roomB.doorRight = true;
                    }
                    // RIGHT
                    else if (roomA.area.Right == roomB.area.Left + 1 && roomA.doorRight == false && roomB.doorLeft == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Right - 1, Utils.Random(roomA.area.Top + 1, roomB.area.Bottom - 1)), roomA, roomB));
                        roomA.doorRight = true;
                        roomB.doorLeft = true;
                    }
                }
                // If B is on top side of A but bottom sides are the same
                else if (roomA.area.Top > roomB.area.Top && roomA.area.Bottom == roomB.area.Bottom)
                {
                    // LEFT
                    if (roomA.area.Left == roomB.area.Right - 1 && roomA.doorLeft == false && roomB.doorRight == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Left, Utils.Random(roomA.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorLeft = true;
                        roomB.doorRight = true;
                    }
                    // RIGHT
                    else if (roomA.area.Right == roomB.area.Left + 1 && roomA.doorRight == false && roomB.doorLeft == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Right - 1, Utils.Random(roomA.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorRight = true;
                        roomB.doorLeft = true;
                    }
                }
                // If B is on top side of A but top sides are the same
                else if (roomA.area.Top == roomB.area.Top && roomA.area.Bottom > roomB.area.Bottom)
                {
                    // LEFT
                    if (roomA.area.Left == roomB.area.Right - 1 && roomA.doorLeft == false && roomB.doorRight == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Left, Utils.Random(roomA.area.Top + 1, roomB.area.Bottom - 1)), roomA, roomB));
                        roomA.doorLeft = true;
                        roomB.doorRight = true;
                    }
                    // RIGHT
                    else if (roomA.area.Right == roomB.area.Left + 1 && roomA.doorRight == false && roomB.doorLeft == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Right - 1, Utils.Random(roomA.area.Top + 1, roomB.area.Bottom - 1)), roomA, roomB));
                        roomA.doorRight = true;
                        roomB.doorLeft = true;
                    }
                }
                // If B is on bottom side of A
                else if (roomA.area.Top < roomB.area.Top && roomA.area.Bottom < roomB.area.Bottom && roomA.area.Bottom > roomB.area.Top + 2)
                {
                    // LEFT
                    if (roomA.area.Left == roomB.area.Right - 1 && roomA.doorLeft == false && roomB.doorRight == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Left, Utils.Random(roomB.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorLeft = true;
                        roomB.doorRight = true;
                    }
                    // RIGHT
                    else if (roomA.area.Right == roomB.area.Left + 1 && roomA.doorRight == false && roomB.doorLeft == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Right - 1, Utils.Random(roomB.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorRight = true;
                        roomB.doorLeft = true;
                    }
                }
                // If B is on bottom side of A but top sides are the same
                else if (roomA.area.Top == roomB.area.Top && roomA.area.Bottom < roomB.area.Bottom)
                {
                    // LEFT
                    if (roomA.area.Left == roomB.area.Right - 1 && roomA.doorLeft == false && roomB.doorRight == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Left, Utils.Random(roomA.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorLeft = true;
                        roomB.doorRight = true;
                    }
                    // RIGHT
                    else if (roomA.area.Right == roomB.area.Left + 1 && roomA.doorRight == false && roomB.doorLeft == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Right - 1, Utils.Random(roomA.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorRight = true;
                        roomB.doorLeft = true;
                    }
                }
                // If B is on bottom side of A but bottom sides are the same
                else if (roomA.area.Top < roomB.area.Top && roomA.area.Bottom == roomB.area.Bottom)
                {
                    // LEFT
                    if (roomA.area.Left == roomB.area.Right - 1 && roomA.doorLeft == false && roomB.doorRight == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Left, Utils.Random(roomB.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorLeft = true;
                        roomB.doorRight = true;
                    }
                    // RIGHT
                    else if (roomA.area.Right == roomB.area.Left + 1 && roomA.doorRight == false && roomB.doorLeft == false)
                    {
                        doors.Add(new Door(new Point(roomA.area.Right - 1, Utils.Random(roomB.area.Top + 1, roomA.area.Bottom - 1)), roomA, roomB));
                        roomA.doorRight = true;
                        roomB.doorLeft = true;
                    }
                }
                else if (roomA.area.Top == roomB.area.Bottom || roomA.area.Bottom == roomB.area.Top)
                {
                    // do nothing SKIP
                }
            }
        }
    }
}