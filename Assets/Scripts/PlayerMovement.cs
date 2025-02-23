using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    private NavMeshAgent agent;
    private Camera myCam;
    public LayerMask ground;
    private Animator myAnimator;
    private Rigidbody2D rb;
    private Vector2 destination;

    public Vector3 offset;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        myCam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis= false;

        myAnimator= GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            destination = myCam.ScreenToWorldPoint(Input.mousePosition);
            myAnimator.SetBool("IsRunning", true);
            agent.SetDestination(myCam.ScreenToWorldPoint(Input.mousePosition));
        }
        myCam.transform.position = new Vector3(this.gameObject.transform.position.x + offset.x, this.gameObject.transform.position.y + offset.y, offset.z);

        // Check if we've reached the destination
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    myAnimator.SetBool("IsRunning", false);
                }
            }
        }
        
        if(agent.desiredVelocity.x < 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(agent.desiredVelocity.x > 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Fireball")
        {
            GameManager.Instance.Defeat();
        }
    }
}
