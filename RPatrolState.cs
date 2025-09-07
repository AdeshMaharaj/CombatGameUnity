using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPatrolState : State
{
    public RAttackState rAttackState;

    public Transform P1;          // Patrol point 1
    public Transform P2;          // Patrol point 2
    public float jeffSpeed;       // Movement speed
    public Transform EvilJeff;        // Evil Jeff's transform


    public Transform tar;        // Current patrol target
    private Animator anim;

    public bool inRange; // Whether the player is detected

    private void Start()
    {
        ChooseTarget(); // Start by patrolling toward the closest point
        inRange = false;
        anim = EvilJeff.GetComponent<Animator>();
    }

    public override State RunCurrentState()
    {
        anim.SetBool("Shoot", false); // Stop attack animation
        anim.SetBool("canPatrol", true); // Stop attack animation
        if (inRange) // Transition to ChaseState if the player is detected
        {
            Debug.Log("Player in shooting range, swithcing to RAttackState!");
            //chaseState.lost = false;
            return rAttackState;
        }

        Patrol(); // Perform patrolling behavior

        if (!InPatrolArea() && !inRange)
        {
            ChooseTarget(); // Reassign the patrol target if Jeff is outside patrol bounds
        }

        return this;
    }

    public override void HandleTriggerEnter2D(Collider2D enter)
    {
        Debug.Log(enter.gameObject.tag);
        if (enter.gameObject.tag.Equals("Player")) //Checking if an object with the Player tag enters the attack range boxy
        {
            tar = enter.transform; //If a player does enter, store them as the target 
            inRange = true;
            Flip();
        }
    }

    private void Patrol()
    {
        if (tar == null || Vector2.Distance(EvilJeff.position, tar.position) < 0.1f)
        {
            tar = tar == P1 ? P2 : P1; // Switch between P1 and P2
        }

        Vector2 targetPosition = new Vector2(tar.position.x, EvilJeff.position.y);
        EvilJeff.position = Vector2.MoveTowards(EvilJeff.position, targetPosition, jeffSpeed * Time.deltaTime);
    }

    private void ChooseTarget()
    {
        float distP1 = Vector2.Distance(EvilJeff.position, P1.position); // Distance to P1
        float distP2 = Vector2.Distance(EvilJeff.position, P2.position); // Distance to P2
        tar = distP1 > distP2 ? P1 : P2; // Choose the closer point
        Flip();
    }

    public void Flip()
    {
        if (EvilJeff.position.x > tar.position.x)
        {
            EvilJeff.localScale = new Vector2(1, 1); // Face left
        }
        else
        {
            EvilJeff.localScale = new Vector2(-1, 1); // Face right
        }
    }

    private bool InPatrolArea()
    {
        return EvilJeff.position.x > P1.position.x && EvilJeff.position.x < P2.position.x;
    }
}

