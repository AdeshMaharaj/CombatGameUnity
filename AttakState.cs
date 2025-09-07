using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : State
{
    public ChaseState chaseState;     // Reference to ChaseState

    public Transform EvilJeff;        // Evil Jeff's transform
    public float timer;               // Cooldown timer duration
    public float atkDist;             // Attack distance
    private float distToAtk;          // Distance to the player

    private Animator anim;            // Animator for Evil Jeff
    private float initTimer;          // Initial timer value
    private bool coolDown = false;    // Whether Evil Jeff is in cooldown

    private void Start()
    {
        if (EvilJeff == null)
        {
            EvilJeff = transform; // Assign Evil Jeff's transform if not set
        }

        // Get the Animator from the parent object
        anim = EvilJeff.GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator not found on parent object! Please ensure FSMEvilJeff has an Animator component.");
        }

        initTimer = timer; // Store the initial timer value
    }

    public override State RunCurrentState()
    {
        if (chaseState.tar == null)
        {
            Debug.LogWarning("No target found. Returning to ChaseState.");
            return chaseState; // Return to ChaseState if there's no target
        }

        // Calculate the distance to the target
        distToAtk = Vector2.Distance(EvilJeff.position, chaseState.tar.position);

        // If the player is out of attack range, return to ChaseState
        if (distToAtk > atkDist)
        {
            Debug.Log("Player out of attack range. Returning to ChaseState.");
            StpAtk();
            return chaseState;
        }

        // If not in cooldown, perform the attack
        if (!coolDown)
        {
            PerformAtk();
        }

        // If in cooldown, decrement the timer
        if (coolDown)
        {
            anim.SetBool("Attack", false);
            timer -= Time.deltaTime; // Decrease the timer
            if (timer <= 0)
            {
                anim.SetBool("Attack", true);
                coolDown = false;   // End cooldown
                timer = initTimer; // Reset the timer
                Debug.Log("Cooldown complete. Ready for next attack.");
            }
        }

        return this; // Stay in AttackState
    }

    private void PerformAtk()
    {
        Debug.Log("Performing Attack!");

        // Trigger attack animation
        if (anim != null)
        {
            anim.SetBool("Attack", true);
            anim.SetTrigger("Attack");
        }

        // Enter cooldown
        coolDown = true;
    }

    private void StpAtk()
    {
        Debug.Log("Stopping Attack.");
        coolDown = false; // Ensure cooldown resets
        if (anim != null)
        {
            anim.SetBool("Attack", false); // Stop attack animation
            anim.SetBool("canWalk", true); // Stop attack animation
        }
    }
}
