using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    //[SerializeField]
    
    bool detected = false;
    //[SerializeField]
    


    [SerializeField]
    public Transform Waypoint1;
    [SerializeField]
    public Transform Waypoint2;


    //public float speed = 200f;
    public float nextWaypointDistance = 3f;
    bool reachedEndOfPath = false;


    Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;

    GameObject target;
    GameObject sound;


    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;



    public HealthBar healthBar;
    private int health = GuardData.getGuardHealth();

    public AudioSource musicToStop;
    public AudioSource musicToPlay;


    // Start is called before the first frame update
    void Start()
    {
        healthBar.setMaxHealth(health);
        target = GameObject.FindGameObjectsWithTag("Player")[0];
        sound = GameObject.FindGameObjectsWithTag("Sound")[0];
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        musicToStop = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[0];
        musicToPlay = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[1];

        musicToPlay.Play();
        musicToPlay.Pause();

        InvokeRepeating("Shoot", 0f, 0.5f);
        InvokeRepeating("UpdatePathWaypoint1", 0f, .5f);
        
    }



    private void UpdatePathWaypoint1()
    {
        float distToW1 = Vector2.Distance(transform.position, Waypoint1.transform.position);
        if (PlayerScript.detected == false)
        {
            if (distToW1 < 2)
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
        if (PlayerScript.detected == false)
        {
            if (distToW2 < 2)
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
    private void UpdatePath()
    {
        if (PlayerScript.detected == true)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
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

    bool CanSeePlayer(float distance)
    {
        float castDist = distance;

        Vector2 endPos = transform.position + -this.gameObject.transform.up * distance;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if(hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            { 
                return true;
            }
        }
        Debug.DrawLine(transform.position, endPos, Color.red);
        return false;
    }

    void CanHearPlayer()
    {
        if (sound.GetComponentInChildren<CircleCollider2D>().IsTouching(this.gameObject.GetComponentInChildren<CircleCollider2D>()))
        {
            PlayerScript.detected = true;
            musicToStop.Pause();
            musicToPlay.UnPause();
        }
    }

    void facePlayer()
    {
        if (PlayerScript.detected == true)
        {
            var offset = 90f;
            Vector2 direction = target.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }
        else
        {
            Vector2 moveDirection = rb.velocity;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
        }
    }

    void Shoot()
    {
        if (PlayerScript.detected == true)
        {
            if (CanSeePlayer(30) == true)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D bullBody = bullet.GetComponent<Rigidbody2D>();
                bullBody.AddForce(-firePoint.up * bulletForce, ForceMode2D.Impulse);
            }
        }
    }


    public void gotShot()
    {
        health -= 5;
        //GuardData.setGuardHealth(GuardData.getGuardHealth() - 5);
        healthBar.setHealth(health);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (health == 0)
        {
            Destroy(this.gameObject);
        }
        float distToPlayer = Vector2.Distance(transform.position, target.transform.position);
        //print(distToPlayer);
        CanHearPlayer();
        if (CanSeePlayer(9) == true)
        {
            PlayerScript.detected = true;
            musicToStop.Pause();
            musicToPlay.UnPause();
        }
        if (true)
        {
            if (CanSeePlayer(10)==true && distToPlayer <= 10)
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
            facePlayer();
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * GuardData.getGuardSpeed() * Time.deltaTime;

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
    }
}
