using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class OffGraphWayPointAgent : NodeGraphAgent
{
    private Node _target = null;
    private Node currentNode = null;
    List<Node> nodesClicked = new List<Node>();

    public OffGraphWayPointAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
    {
        // Position ourselves on a random node
        if (pNodeGraph.nodes.Count > 0)
        {
            currentNode = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
            nodesClicked.Add(currentNode);
            jumpToNode(currentNode);
        }
        // Listen to nodeclicks
        pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
    }

    protected virtual void onNodeClickHandler(Node pNode)
    {
        // If the next clicked node is connected to the current node then add to nodesClicked list
        if (IsNodeConnectedTo(pNode, nodesClicked[nodesClicked.Count - 1]))
        {
            nodesClicked.Add(pNode);
            Console.WriteLine(nodesClicked.Count);
        }    
    }

    protected override void Update()
    {
        if (nodesClicked.Count > 1)
        {
            _target = nodesClicked[1];
        }

        // No target? Stop moving.
        if (_target == null) return;

        //Move towards the target node, if we reached it, clear the target, if not then keep going
        if (moveTowardsNode(_target))
        {
              currentNode = _target;
              _target = null;
              nodesClicked.Remove(nodesClicked[0]);
        }
    }

    /// <summary>
    /// Returns true if two of the selcted nodes are connected, otherwise false.
    /// </summary>
    /// <param name="pNode"></param>
    /// <param name="pNodeOther"></param>
    /// <returns></returns>
    protected virtual bool IsNodeConnectedTo(Node pNode, Node pNodeOther)
    {
        if (pNode.room == null && pNodeOther.door == null && (pNode.door.roomA == pNodeOther.room || pNode.door.roomB == pNodeOther.room))
        { return true; }
        else if (pNode.door == null && pNodeOther.room == null && (pNode.room == pNodeOther.door.roomA || pNode.room == pNodeOther.door.roomB))
        { return true; }
        else { return false; }
    }
}

