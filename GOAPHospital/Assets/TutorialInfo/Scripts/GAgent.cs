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

    public void Start()
    {
        GAction[] acts = GetComponents<GAction>();
        foreach (GAction a in acts)
        {
            Actions.Add(a);
        }
    }

    bool invoked = false;
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }
    private void LateUpdate()
    {
        if(currentAction != null && currentAction.running)
        {
            float distanceToTarget = Vector3.Distance(currentAction.Target.transform.position, transform.position);
            if(currentAction.Agent.hasPath && distanceToTarget < 2f)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.Duration);
                    invoked = true;
                }
            
            }
            return;
        }

        if(planner == null || actionQueue == null)
        {
            planner = new GPlanner();

            var sortedGoals = from entry in Goals orderby entry.Value descending select entry;
       
            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals){
                actionQueue = planner.Plan(Actions, sg.Key.Sgoals, null);
                if(actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if(actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.Remove)
            {
                Goals.Remove(currentGoal);
            }
            planner = null;
        }

        if(actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if(currentAction.Target == null && currentAction.TargetTag != "")
                        currentAction.Target = GameObject.FindWithTag(currentAction.TargetTag);
            
                if(currentAction.Target != null)
                {
                    currentAction.running = true;
                    currentAction.Agent.SetDestination(currentAction.Target.transform.position);
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}
