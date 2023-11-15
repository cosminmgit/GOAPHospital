using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldState
{
    public string Key;
    public int Value;

}

public class WorldStates
{
   public Dictionary<string, int> States;

    public WorldStates()
    {
        States = new Dictionary<string, int>();
    }

    public bool HasState(string Key)
    {
        return States.ContainsKey(Key);

    }

    private void AddState(string Key, int Value)
    {
        States.Add(Key, Value);
    }

    public void ModifyState(string Key, int Value)
    {
        if(States.ContainsKey(Key))
        {
            States[Key] += Value;
            if (States[Key] <= 0) RemoveState(Key);
        }
        else States.Add(Key, Value);
    }

    public void RemoveState(string Key)
    {
       if(States.ContainsKey(Key)) States.Remove(Key);
    }

    public void SetState(string Key, int Value)
    {
        if (States.ContainsKey(Key)) States[Key] = Value;
        else States.Add(Key, Value);

    }

    public Dictionary<string, int> GetStates()
    {
        return States;
    }

}
