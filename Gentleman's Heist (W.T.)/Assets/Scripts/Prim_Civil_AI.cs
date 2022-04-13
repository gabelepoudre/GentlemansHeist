using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Prim_Civil_AI : MonoBehaviour
{
    // Pathfinding references
    Seeker seeker;
    Rigidbody2D rb;
    bool detected = false;

    public Transform Waypoint1;
    
    public Transform Waypoint2;

    public Transform Waypoint3;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    bool reachedEndOfPath = false;

    private float prevX;
    private float prevY;

    public Animator animator;
    private float xMove;
    private float yMove;
    
    GameObject sound;
    
    GameObject player;


    Path path;
    int currentWaypoint = 0;
    private void Start(){

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        sound = GameObject.FindGameObjectsWithTag("Sound")[0];
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        InvokeRepeating("UpdatePathWaypoint1", 0f, .5f);

        prevX = 0;
        prevY = 0;

    }
    private void UpdatePathWaypoint1()
    {
        float distToW1 = Vector2.Distance(transform.position, Waypoint1.transform.position);
        if (detected == false)
        {
            if (distToW1 < 1)
            {
                InvokeRepeating("UpdatePathWaypoint2", 0f, .5f);
                CancelInvoke("UpdatePathWaypoint1");
            }
            if (seeker.IsDone())
                seeker.StartPath(rb.position, Waypoint1.transform.position, OnPathComplete);

        }
        else
        {
            InvokeRepeating("UpdatePath", 0f, .5f);
        }
    }

    private void UpdatePathWaypoint2()
    {
        float distToW2 = Vector2.Distance(transform.position, Waypoint2.transform.position);
        if (detected == false)
        {
            if (distToW2 < 1)
            {
                InvokeRepeating("UpdatePathWaypoint1", 0f, .5f);
                CancelInvoke("UpdatePathWaypoint2");
            }
            if (seeker.IsDone())
                seeker.StartPath(rb.position, Waypoint2.transform.position, OnPathComplete);

        }
        else
        {
            InvokeRepeating("UpdatePath", 0f, .5f);
        }
    }

    private void UpdatePathWaypoint3()
    {
        float distToW2 = Vector2.Distance(transform.position, Waypoint3.transform.position);
        if (detected == false)
        {
            if (distToW2 < 1)
            {
                InvokeRepeating("UpdatePathWaypoint2", 0f, .5f);
                CancelInvoke("UpdatePathWaypoint3");
            }
            if (seeker.IsDone())
                seeker.StartPath(rb.position, Waypoint3.transform.position, OnPathComplete);

        }
        else
        {
            InvokeRepeating("UpdatePath", 0f, .5f);
        }
    }

    private void UpdatePath()
    {
        if (detected == true)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, Waypoint1.transform.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void CanHearPlayer()
    {
        if (sound.GetComponentInChildren<CircleCollider2D>().IsTouching(this.gameObject.GetComponentInChildren<CircleCollider2D>()))
        {
            detected = true;
        }
    }

    void PlayerFound(){
        if(PlayerScript.detected == true){
            UpdatePathWaypoint3();
        }
    }

    void FixedUpdate()
    {

        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);

        CanHearPlayer();
        if (CanSeePlayer(9) == true)
        {
            detected = true;
        }
        if (true)
        {
            if (CanSeePlayer(10) == true && distToPlayer <= 10)
            {
                return;
            }
            if (path == null)
                return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            return;
        }

        findDirection();
        prevY = transform.position.y;
        prevX = transform.position.x;
    }

    bool CanSeePlayer(float distance)
    {
        float castDist = distance;

        Vector2 endPos = transform.position + -this.gameObject.transform.up * distance;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        Debug.DrawLine(transform.position, endPos, Color.red);
        return false;
    }
    
    void findDirection(){
     // doesn't work perfectly just yet, but it's a step in the right direction (pun intended)
     if(prevY - transform.position.y < 0){
        // moving down
        xMove = 0;
        yMove = -1;
     }
     if(prevY - transform.position.y > 0){
        // moving up
        xMove = 0;
        yMove = 1;
     }
     if(prevX - transform.position.x > 0){
         // moving right
        xMove = 1;
        yMove = 0;
     }
     if(prevX - transform.position.x > 0){
         // moving left
        xMove = -1;
        yMove = 0;
     }
     changeAnimation();

    }

    void changeAnimation()
    {
        animator.SetBool("walking", true);
        animator.SetFloat("xChange", xMove);
        animator.SetFloat("yChange", yMove);
    }
   
}
