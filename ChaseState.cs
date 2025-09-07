using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : State
{
    public PatrolState patrolState;
    public AttackState AttackState;
    public Transform EvilJeff;

    public float jeffSpeed;

    public Transform rc; //Ray cast 
    public LayerMask rcMask;
    public float rcLen;

    public bool lost = false;

    public float atkDist;  //To adjust the attack area
    public float distToAtk; //Distance to the player

    public Transform tar;
    private RaycastHit2D hit;

    private void Start()
    {
        if (EvilJeff == null)
        {
            EvilJeff = transform;

        }
    }

    public override State RunCurrentState()
    {
        if (lost)
        {
            Debug.Log("Player lost, switching to PatrolState.");
            patrolState.inRange = false;
            return patrolState;
        }
        //If it is in range then send out and store the raycast in the hit variable
        //1st param is where the raycast will be orginated 
        //2nd param is the direction the ray cast will be shot 
        //3rd param is the lenth of the raycast 
        //4th param is the layer mask to detect the objects on the selected layer
        hit = Physics2D.Raycast(rc.position, transform.right, rcLen, rcMask);
        VisualiseRayCast();

        if(hit.collider != null && hit.collider.tag.Equals("Player"))
        {
            tar = hit.collider.transform;
            ChaseLogic();

            if (distToAtk < atkDist)
            {
                Debug.Log("GOING ATTAK MODE!");
                return AttackState;
            }
        } else //If the other conditions arent met then we lost Good jeff
        {
            Debug.Log("Player not detected, setting lost flag.");
            lost = true;
        }

        
        return this;
    }

    private void ChaseLogic()
    {
        if (tar == null)
        {
            Debug.LogWarning("No target to chase.");
            return;
        }

        // Calculate the distance to the player
        distToAtk = Vector2.Distance(EvilJeff.position, tar.position);

        // Move toward the player
        Vector2 targetPosition = new Vector2(tar.position.x, EvilJeff.position.y);
        EvilJeff.position = Vector2.MoveTowards(EvilJeff.position, targetPosition, jeffSpeed * Time.deltaTime);

        // Flip Evil Jeff to face the player
        patrolState.Flip();
    }

    void VisualiseRayCast()
    {
        if (distToAtk > atkDist)
        {
            Debug.DrawRay(rc.position, EvilJeff.right * rcLen, Color.red); //Will draw a line to show the raycast and will be red if player is out of range
        }
        else if (atkDist > distToAtk)
        {
            Debug.DrawRay(rc.position, EvilJeff.right * rcLen, Color.green);//Changes the colour of the raycast to green when in range 
        }
    }
}
