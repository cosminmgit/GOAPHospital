using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class GAction : MonoBehaviour
{
    public string ActionName = "Action";
    public float Cost = 1f;
    public GameObject Target;
    public string TargetTag;
    public float Duration;
    public WorldState[] PreConditions;
    public WorldState[] AfterEffects;
    public NavMeshAgent Agent;

    public Dictionary<string, int> Preconditions;
    public Dictionary<string, int> Effects;

    public WorldStates AgentBeliefs;

    public bool running = false;

    public GAction()
    {
        Preconditions = new Dictionary<string, int>();
        Effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
            Agent = GetComponent<NavMeshAgent>();

        if(PreConditions != null)
        {
            foreach(WorldState w in PreConditions)
            {
                Preconditions.Add(w.Key, w.Value);
            }
        }

        if (AfterEffects != null)
        {
            foreach (WorldState w in AfterEffects)
            {
                Effects.Add(w.Key, w.Value);
            }
        }
    }

    public bool IsAchievable()
    {
        return true;
    }
    
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach(KeyValuePair<string, int> p in Preconditions)
        {
            if (!conditions.ContainsKey(p.Key)) return false;
        }

        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();

}
