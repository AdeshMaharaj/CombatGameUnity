using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public PatrolState patrolState;
    public ChaseState chaseState;

    public bool inRange; // Player detection flag

    public override State RunCurrentState()
    {
        if (inRange)
        {
            return chaseState; // Transition to ChaseState if the player is detected
        }
        else
        {
            return patrolState; // Transition to PatrolState if no player is detected
        }
    }

    private void OnTriggerEnter2D(Collider2D enter)
    {
        if (enter.gameObject.tag == "Player")
        {
            inRange = true; // Set inRange to true when the player enters the detection zone
        }
    }

    private void OnTriggerExit2D(Collider2D exit)
    {
        if (exit.gameObject.tag == "Player")
        {
            inRange = false; // Reset inRange when the player leaves the detection zone
        }
    }
}
