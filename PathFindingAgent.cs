using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class PathFindingAgent : OffGraphWayPointAgent
{
    private Node _target = null;
    private Node currentNode = null;
    private List<Node> pathToFollow = new List<Node>();
    PathFinder pathFinder;

    public PathFindingAgent(NodeGraph pNodeGraph, PathFinder pPathFinder) : base(pNodeGraph)
    {
        SetOrigin(width / 2, height / 2);
        pathFinder = pPathFinder;

        // Position ourselves on a random node
        if (pNodeGraph.nodes.Count > 0)
        {
            currentNode = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
            jumpToNode(currentNode);
        }
        // Listen to nodeclicks
        pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
    }

    protected override void onNodeClickHandler(Node pNode)
    {
        // Generate the shortest path when the target node is clicked
        pathToFollow = pathFinder.Generate(currentNode, pNode);
    }

    protected override void Update()
    {
        if (pathToFollow.Count > 0)
        {
            currentNode = pathToFollow[0];
            _target = pathToFollow[0];
        }

        // No target? Stop moving.
        if (_target == null) return;

        //Move towards the target node, if we reached it, clear the target, if not then keep going
        if (moveTowardsNode(_target) && pathToFollow.Count() != 0)
        {
            pathToFollow.RemoveAt(0);
        }
    }
}

