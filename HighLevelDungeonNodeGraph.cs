using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class HighLevelDungeonNodeGraph : SampleDungeonNodeGraph
{

    public HighLevelDungeonNodeGraph(Dungeon pDungeon) : base(pDungeon) {}

    protected override void generate()
    {
        // Adding nodes to all the rooms 
        foreach (Room room in _dungeon.rooms)
        {
            Node node = new Node(getRoomCenter(room), room, null);
            room.node = node;
            nodes.Add(node);
        }

        // Adding nodes to all the doors and connecting them to the adjacent room nodes
        foreach(Door door in _dungeon.doors)
        {
            Node node = new Node(getDoorCenter(door), null, door);
            door.node = node;
            nodes.Add(node);
            AddConnection(node, door.roomA.node);
            AddConnection(node, door.roomB.node);
        }

        // I make things more complicated than they should be (IGNORE THIS)
        #region
        //// Draw all connections only between door nodes and room nodes
        //for (int i = 0; i < _dungeon.doors.Count; i++)
        //{
        //    for (int j = 0; j < _dungeon.rooms.Count; j++)
        //    {
        //        if (_dungeon.doors[i].roomA == _dungeon.rooms[j])
        //        {
        //            for (int h = 0; h < nodes.Count; h++)
        //            {
        //                for (int k = 0; k < nodes.Count; k++)
        //                {
        //                    if (nodes[h].location == getDoorCenter(_dungeon.doors[i]) && nodes[k].location == getRoomCenter(_dungeon.rooms[j]))
        //                    {
        //                        AddConnection(nodes[h], nodes[k]);
        //                    }
        //                }
        //            }
        //        }
        //        else if (_dungeon.doors[i].roomB == _dungeon.rooms[j])
        //        {
        //            for (int h = 0; h < nodes.Count; h++)
        //            {
        //                for (int k = 0; k < nodes.Count; k++)
        //                {
        //                    if (nodes[h].location == getDoorCenter(_dungeon.doors[i]) && nodes[k].location == getRoomCenter(_dungeon.rooms[j]))
        //                    {
        //                        AddConnection(nodes[h], nodes[k]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion
    }
}

