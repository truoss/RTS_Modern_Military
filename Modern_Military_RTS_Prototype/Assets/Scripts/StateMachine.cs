using UnityEngine;
using System.Collections.Generic;

public interface IGameState
{
    void Init();
    IGameState Update();
    void Finished();
}

public class StateMachine
{
    // calling SetState and Abort from the Update function state itself is a bit dodgy in terms of behaviour
    // we need to take care that the init and finished functions are not called while another state is currently being updated
    bool isCurrentlyUpdating = false;
    List<IGameState> buffer = new List<IGameState>();

    IGameState state = null;

    public IGameState CurrentState
    {
        get { return state; }
    }

    public void Update()
    {
        if (state == null)
        {
            Debug.LogError("No state set, make sure SetState is called at least once with the initial state");
            return;
        }

        try
        {
            ProcessStack();

            isCurrentlyUpdating = true;
            var newState = state.Update();

            if (newState != state)
            {
                DoFinish(state);
                DoInit(newState);
                state = newState;
            }

            ProcessStack();
        }
        finally
        {
            isCurrentlyUpdating = false;
        }
    }

    void ProcessStack()
    {
        if (buffer.Count > 0)
        {
            for (int i = 0; i < buffer.Count; i++)
            {
                DoFinish(state);
                state = buffer[i];
                DoInit(state);
            }

            buffer.Clear();
        }
    }

    public void SetState(IGameState newState)
    {
        if (isCurrentlyUpdating)
        {
            buffer.Add(newState);
        }
        else
        {
            DoFinish(state);
            DoInit(newState);
            state = newState;
        }
    }

    public void Abort()
    {
        if (isCurrentlyUpdating)
        {
            buffer.Add(null);
        }
        else
        {
            DoFinish(state);
            state = null;
        }
    }

    static void DoInit(IGameState state)
    {
        if (state != null)
        {
            //Debug.Log("STATE INIT: " + state.GetType().Name);
            state.Init();
        }
    }

    static void DoFinish(IGameState state)
    {
        if (state != null)
        {
            //Debug.Log("STATE FINISHED: " + state.GetType().Name);
            state.Finished();
        }
    }
}