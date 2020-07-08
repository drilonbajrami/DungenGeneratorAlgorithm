using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class BreadthFirstPathFinder : PathFinder
{
    List<Node> todoList = new List<Node>();
    List<Node> doneList = new List<Node>();
    List<Node> path = new List<Node>();
    Node currentNode = null;
    
    public BreadthFirstPathFinder(NodeGraph pGraph) : base(pGraph) { }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {    
        todoList.Clear();
        doneList.Clear();
        path.Clear();

        // Add the start node to the todo list
        todoList.Add(pFrom);

        while (true)
        {
            Console.WriteLine("ToDo List " + "(Count = " + todoList.Count() + ")" + ": "+ String.Join(", ", todoList));
            Console.WriteLine("Done list " + "(Count = " + doneList.Count() + ")" + ": "+ String.Join(", ", doneList));
            // If there is no nodes left to check while the end node has not been found
            // then terminate and return no path
            if (todoList.Count() == 0)
            {
                return null;
            }
            
            // Remove the current node from the todo list and add it to the done list
            // then proceed to check its connections
            currentNode = todoList[0];
            todoList.RemoveAt(0);
            doneList.Add(currentNode);
            Console.WriteLine("Current Node ID: " + currentNode.id + "\n");
            
            // If end node is found then start going back to the start node
            // Meanwhile adding the nodes to the path
            if (currentNode == pTo)
            {
                path.Add(currentNode);
                Console.WriteLine("PATH FOUND");
                while(currentNode != pFrom)
                {
                    path.Add(currentNode.parentNode);
                    currentNode = currentNode.parentNode;
                }

                // End node is added first, Start node is added last so path gets reversed
                path.Reverse();
                Console.WriteLine("PATH: " + String.Join(", ", path));   
                return path;
            }
            else
            {
                foreach (Node connection in currentNode.connections)
                {
                    if(todoList.Contains(connection) || doneList.Contains(connection))
                    {
                        continue;
                    }
                    else
                    {
                        // By checking possible connections, mark the current node as a parent of its connection
                        connection.parentNode = currentNode;
                        todoList.Add(connection);
                    } 
                }
            }
        }    
    }
}