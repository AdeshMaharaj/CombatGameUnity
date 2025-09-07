using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;


public class EvilJeffBehaviour : MonoBehaviour
{
    //Ray cast vars
    public Transform rc; //Ray cast 
    public LayerMask rcMask;
    public float rcLen;

    public float atkDist; //To adjust the attack area
    public float jeffSpeed;
    public float timer;

    //Patrolling vars
    public Transform P1;
    public Transform P2;

    private RaycastHit2D hit;
    private Transform tar;
    private Animator anim;

    private float distToAtk; //Distance to the player 
    
    //bools
    private bool attackMode;
    private bool inRange;
    private bool coolDown;
    private float initTimer;

    private void Awake()
    {
        ChooseTarget(); //Make sure evil jeff have a target
        initTimer = timer; //Store init value of the timer
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if(!InPatrolArea() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Mummy_attack"))
        {
            ChooseTarget();
        }

        if (inRange)
        {
            //If it is in range then send out and store the raycast in the hit variable
            //1st param is where the raycast will be orginated 
            //2nd param is the direction the ray cast will be shot 
            //3rd param is the lenth of the raycast 
            //4th param is the layer mask to detect the objects on the selected layer
            hit = Physics2D.Raycast(rc.position, transform.right, rcLen, rcMask);
            VisualiseRayCast();
        }

        //Do this is the player is detectedby checking if the raycast is not null
        if (hit.collider != null) 
        {
            EvilJeffLogic();
        }
        else if (hit.collider == null)
        {
            inRange = false;    
        }

        if (inRange == false)
        {
            //anim.SetBool("canWalk", false);
            StpAtk();
        }
    }

    

   void OnTriggerEnter2D(Collider2D enter)
    {
        Debug.Log(enter.gameObject.tag);
        if (enter.gameObject.tag == "Player") //Checking if an object with the Player tag enters the attack range boxy
        {
            tar = enter.transform; //If a player does enter, store them as the target 
            inRange = true;
            Flip();
        }
    }
    void EvilJeffLogic()
    {
        distToAtk = Vector2.Distance(transform.position, tar.position); //Gets and stores the distance between enemy and player

        if (distToAtk > atkDist)
        {
            StpAtk();
        }
        else if (atkDist >= distToAtk && coolDown == false)
        {
            PerformAtk();
        }
        if (coolDown)
        {
            anim.SetBool("Attack", false);
        }
    }


    void Move() // I have a feeling that this can be done with/in the patroling State 
    {
        anim.SetBool("canWalk", true);
        //Check if the attack animation is playing 
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Mummy_attack"))
        {
            Vector2 tarPos = new Vector2(tar.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, tarPos, jeffSpeed * Time.deltaTime); // Move towards Player
        }

    }
    void PerformAtk()
    {
        //reset the timer
        timer = initTimer;
        //Change bool to enter attack mode
        attackMode = true;

        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);
    }

    void StpAtk()
    {
        coolDown = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    void VisualiseRayCast()
    {
        if (distToAtk > atkDist)
        {
            Debug.DrawRay(rc.position, transform.right * rcLen, Color.red); //Will draw a line to show the raycast and will be red if player is out of range
        }
        else if (atkDist > distToAtk)
        {
            Debug.DrawRay(rc.position, transform.right * rcLen, Color.green);//Changes the colour of the raycast to green when in range 
        }
    }

    private bool InPatrolArea()
    {
        return transform.position.x > P1.position.x && transform.position.x < P2.position.x; //Looking if Evil Jeff is inside patrol points, true if he is 
    }

    private void ChooseTarget()
    {
        float distP1 = Vector2.Distance(transform.position, P1.position); // assigns distance from Evil Jeff current position to the postion of P1
        float distP2 = Vector2.Distance(transform.position, P2.position);

        if (distP1 > distP2)
        {
            tar = P1;
        }
        else
        {
            tar = P2;
        }
        Flip();

    }

    private void Flip()
    {
        if(transform.position.x > tar.position.x)
        {
            transform.localScale = new Vector2(1, 1); // Face right
        }
        else
        {
            transform.localScale = new Vector2(-1, 1); // Face left
        }
    }


}
