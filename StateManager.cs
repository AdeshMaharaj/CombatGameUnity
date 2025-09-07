using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public State currentState; // The state that will be played in our state machine

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        //Taking the state and assign it to whatever the current state returns 
        State nextState = currentState?.RunCurrentState(); //if the var is not null then get the current state 

        if (nextState != null)
        {
            SwitchToNextState(nextState); //if its not null then switch to the next state 
        }

    }
    void OnTriggerEnter2D(Collider2D enter)
    {
        currentState.HandleTriggerEnter2D(enter);
        Debug.Log("VIRTUAL THINGY WORKED!");
    }

    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }
}
