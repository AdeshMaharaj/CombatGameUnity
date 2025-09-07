using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectilePos;

    private GameObject tar;
    private float timer; //Use it to control the spaen rate of le ballz
    // Start is called before the first frame update
    void Start()
    {
        tar = GameObject.FindGameObjectWithTag("Player"); //Assigning the player as the target
    }

    // Update is called once per frame
    void Update()
    {
       

        float distToTarget = Vector2.Distance(transform.position, tar.transform.position);
        Debug.Log(distToTarget);

        if (distToTarget < 7)
        {
            timer += Time.deltaTime; //make timer go up in seconds
            if (timer > 2) // if 2 seconds have passed
            {
                timer = 0; //restting the timer, shoot ever 2 seconds
                shoot();
            }
        }
        
    }

    void shoot()
    {
        //I switched to a ball shaped projectile so we dont always have to rotate
        Instantiate(projectile, projectilePos.position, Quaternion.identity); //Instantiating the position and rotation of the ball, handling rotation elewhere
    }
}
