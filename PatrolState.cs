using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PatrolState : State
{
    //public ChaseState chaseState;
    //public bool inRange; // Player detection flag

    //public Transform P1;
    //public Transform P2;
    //public float jeffSpeed;
    //public Transform jeff;

    //private Transform tar;

    //private void Start()
    //{
    //    ChooseTarget(); // Start patrolling toward the closest point
    //}

    //public override State RunCurrentState()
    //{
    //    if (!InPatrolArea() && !inRange)
    //    {
    //        ChooseTarget(); // Reassign the patrol target if Jeff is outside patrol bounds
    //        Debug.Log(inRange);
    //    }

    //    Patrol();
    //    //Debug.Log(inRange);
    //    if (inRange) // If player is detected, start chase
    //    {
    //        return chaseState;
    //    }

    //    return this;
    //}

    //void OnTriggerEnter2D(Collider2D enter)
    //{
    //    Debug.Log($"Trigger entered by: {enter.gameObject.name}"); // Debug log for detection
    //    if (enter.gameObject.tag == "Player")
    //    {
    //        tar = enter.transform; // Store the player as the target
    //        inRange = true;        // Set inRange to true
    //        Flip();                // Flip to face the player
    //    }
    //}

    //private void Patrol()
    //{
    //    if (tar == null || Vector2.Distance(jeff.position, tar.position) < 0.1f)
    //    {
    //        tar = tar == P1 ? P2 : P1; // Switch between P1 and P2
    //    }

    //    Vector2 targetPosition = new Vector2(tar.position.x, jeff.position.y);
    //    jeff.position = Vector2.MoveTowards(jeff.position, targetPosition, jeffSpeed * Time.deltaTime);
    //}

    //private void ChooseTarget()
    //{
    //    float distP1 = Vector2.Distance(transform.position, P1.position); // assigns distance from Evil Jeff current position to the postion of P1
    //    float distP2 = Vector2.Distance(transform.position, P2.position);
    //    if (distP1 > distP2)
    //    {
    //        tar = P1;
    //    }
    //    else
    //    {
    //        tar = P2;
    //    }
    //    Flip();

    //}

    //private void Flip()
    //{
    //    if (jeff.position.x > tar.position.x)
    //    {
    //        jeff.localScale = new Vector2(1, 1);
    //    }
    //    else
    //    {
    //        jeff.localScale = new Vector2(-1, 1);
    //    }
    //}

    //private bool InPatrolArea()
    //{
    //    return jeff.position.x > P1.position.x && jeff.position.x < P2.position.x;
    //}

    public ChaseState chaseState;

    public Transform P1;          // Patrol point 1
    public Transform P2;          // Patrol point 2
    public float jeffSpeed;       // Movement speed
    public Transform EvilJeff;        // Evil Jeff's transform


    private Transform tar;        // Current patrol target
    private Animator anim;

    [SerializeField]
    public bool inRange; // Whether the player is detected

    private void Start()
    {
        ChooseTarget(); // Start by patrolling toward the closest point
        inRange = false;
        anim = EvilJeff.GetComponent<Animator>();
    }

    public override State RunCurrentState()
    {
        anim.SetBool("Attack", false); // Stop attack animation
        anim.SetBool("canWalk", true); // Stop attack animation
        if (inRange) // Transition to ChaseState if the player is detected
        {
            Debug.Log("Player detected, switching to ChaseState.");
            chaseState.lost=false;
            return chaseState;
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

    

