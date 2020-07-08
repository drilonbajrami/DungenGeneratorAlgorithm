using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class RecursivePathFinder : PathFinder
{
    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph) { }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        return findPath(new List<Node>(), new List<Node>(), pFrom, pTo);
    }

    protected List<Node> findPath(List<Node> pHistory, List<Node> shortestPath, Node pFrom, Node pTo, int pDepth = 0)
    {
        string indent = new string('\t', pDepth);
        // Add first node to the history list
        pHistory.Add(pFrom);
        Console.WriteLine(indent + pFrom.id + ":=> Current path: " + String.Join(", ", pHistory));

        // If end node is found then proceed with return the path
        if (pFrom == pTo)
        {
            // If there is no shortest path found yet add the current history path to the shortest path
            // Otherwise if the current history path is shorter than the current shortest path, replace it.
            Console.WriteLine(indent + pFrom.id + ":<== PATH FOUND: " + String.Join(", ", pHistory));
            if (shortestPath.Count() == 0 || pHistory.Count() < shortestPath.Count())
            {
                Console.WriteLine(indent + "New shorter path found! ");
                Console.WriteLine(indent + "Replacing old path with new path");
                shortestPath = new List<Node>(pHistory);
            }
            Console.WriteLine(indent + pFrom.id + ":<== Returned shortest path: " + String.Join(", ", shortestPath));
            return shortestPath;
        }

        // Check all possibilites of each node's connections to find a path
        Console.WriteLine(indent + pFrom.id + ":=> Don't know, asking children ");
        foreach (Node connection in pFrom.connections)
        {
            Console.WriteLine(indent + pFrom.id + ":==> Processing child: " + connection.id);
            if (!pHistory.Contains(connection))
            {
                shortestPath = findPath(new List<Node>(pHistory), shortestPath, connection, pTo, pDepth + 1);
            }
            else
            {
                Console.WriteLine(indent + pFrom.id + ":==> Already visited, skipping: " + connection.id);
            }
        }
        Console.WriteLine(indent + "Returning this path: " + String.Join(", ", shortestPath));
        return shortestPath;
    }
}
