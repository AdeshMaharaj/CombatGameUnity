using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAttackState : State
{
    public RPatrolState rPatrolState;

    public Transform EvilJeff;        // Evil Jeff's transform
    public float timer;               // Cooldown timer duration
    public float atkDist;             // Attack distance
    private float distToAtk;          // Distance to the player

    [Header("Projectile components")]
    [SerializeField]
    public GameObject projectile;
    public Transform projectilePos;

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
            Debug.LogError("Animator ERROR.");
        }

        initTimer = timer; // Store the initial timer value
    }

    public override State RunCurrentState()
    {
        if (rPatrolState.tar == null)
        {
            rPatrolState.inRange = false;
            Debug.Log("No target found. BAck to Patrolling!");
            return rPatrolState; // Return to ChaseState if there's no target
        }

        // Calculate the distance to the target
        distToAtk = Vector2.Distance(EvilJeff.position, rPatrolState.tar.position);

        // If the player is out of attack range, return to ChaseState
        if (distToAtk > atkDist)
        {
            rPatrolState.inRange = false;
            Debug.Log("Player out of attack range. Patrol time!");
            StpAtk();
            return rPatrolState;
        }

        // If not in cooldown, perform the attack
        if (!coolDown)
        {
            PerformAtk();
        }

        // If in cooldown, decrement the timer
        if (coolDown)
        {
            timer -= Time.deltaTime; // Decrease the timer
            if (timer <= 0)
            {
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
            anim.SetTrigger("Shoot");
            //I switched to a ball shaped projectile so we dont always have to rotate
            Instantiate(projectile, projectilePos.position, Quaternion.identity); //Instantiating the position and rotation of the ball, handling rotation elewhere
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
            anim.SetBool("Shoot", false); // Stop attack animation
            anim.SetBool("canPatrol", true); // Stop attack animation
        }
    }
}
