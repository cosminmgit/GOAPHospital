using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{
    public Node Parent;
    public float Cost;
    public Dictionary<string, int> State;
    public GAction Action;

    public Node(Node parent, float cost, Dictionary<string, int> allstates, GAction action)
    {
        this.Parent = parent;
        this.Cost = cost;

        // create a copy of the dictionary
        this.State = new Dictionary<string, int>(allstates);
        
        this.Action = action;
    }

}

public class GPlanner
{
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach (GAction a in actions)
        {
            if (a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        // first graph leaf
        List<Node> leaves = new List<Node>();

        Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        Node cheapest = null;
        foreach(Node leaf in leaves)
        {
            if (cheapest == null) cheapest = leaf;
            else
            {
                if (leaf.Cost < cheapest.Cost) cheapest = leaf;
            }

        }

        List<GAction> result = new List<GAction>();
        Node n = cheapest;

        while(n != null)
        { 
            if(n.Action != null)
            {
                result.Insert(0, n.Action);
            }

            n = n.Parent;
        }

        Queue<GAction> queue = new Queue<GAction>();    
        foreach(GAction a in result)
        {
            queue.Enqueue(a);
        }

        Debug.Log("The Plan is: ");
        foreach(GAction a in queue)
        {
            Debug.Log("Q: " + a.ActionName);
        }

        return queue;

    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usuableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;

        foreach(GAction action in usuableActions) 
        {
                if (action.IsAchievableGiven(parent.State))
                {
                    Dictionary<string, int> currentState = new Dictionary<string, int>(parent.State);
                
                    foreach(KeyValuePair<string, int>eff in action.Effects)
                    {
                        if(!currentState.ContainsKey(eff.Key))
                        {
                            currentState.Add(eff.Key, eff.Value);
                        }

                        Node node = new Node(parent, parent.Cost + action.Cost, currentState, action);
                    
                        if(GoalAchieved(goal, currentState))
                        {
                            leaves.Add(node);
                            foundPath = true;
                        }
                        else
                        {
                            List<GAction> subset = ActionSubset(usuableActions, action);
                            bool found = BuildGraph(node, leaves, subset, goal);
                            if(found)
                            {
                                foundPath= true;
                            }
                        }
                    }
               
            }
            
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> State) 
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if (!State.ContainsKey(g.Key))
                return false;
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach (GAction a in actions)
        {
            if(!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }
}
