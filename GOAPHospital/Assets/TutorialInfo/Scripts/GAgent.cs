using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Search;

public class SubGoal
{
    public Dictionary<string, int> Sgoals;
    public bool Remove;


    public SubGoal(string s, int i, bool r)
    {
        Sgoals = new Dictionary<string, int>();
        Sgoals.Add(s, i);
        Remove = r;
    } 

}
public class GAgent : MonoBehaviour
{
    public List<GAction> Actions = new List<GAction>();
    public Dictionary<SubGoal, int> Goals = new Dictionary<SubGoal, int>();

    private GPlanner planner;
    private Queue<GAction> actionQueue;

    public GAction currentAction;
    private SubGoal currentGoal;

    private void Start()
    {
        GAction[] acts = GetComponents<GAction>();
        foreach (GAction a in acts)
        {
            Actions.Add(a);
        }
    }

    private void LateUpdate()
    {
        
    }
}
