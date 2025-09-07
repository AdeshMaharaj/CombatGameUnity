using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallBehaviour : MonoBehaviour
{
    private GameObject tar;
    private Rigidbody2D rb;
    private float timer;

    public float force;
    public float damage = 30;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tar = GameObject.FindGameObjectWithTag("Player"); //Assigning the player as the target 

        Vector3 dir = tar.transform.position - transform.position; //player pos - ball pos, to create a direction towards the player.
        rb.velocity = new Vector2(dir.x, dir.y).normalized * force;

        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 6)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<PlayerHealth>().health -= damage;
        }
    }
}
