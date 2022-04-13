using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardGraphics : MonoBehaviour
{

    
    public Animator animator;
    private float xMove;
    private float yMove;
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {

    }

    void rotationStablizer()
    {
        float zed = transform.parent.gameObject.transform.rotation.z;
        transform.rotation = Quaternion.Euler(0, 0, zed);
    }

    void animationChangerDirection()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
        float distToPlayer = Vector2.Distance(this.transform.parent.position, target.transform.position);
        
        // Is he walking?
        animator.SetBool("walking", true);

        // Figuring out Direction:
        if (distToPlayer >= 2)
        {
            if (transform.localRotation.eulerAngles.z >= 45 && transform.localRotation.eulerAngles.z <= 135) // left
            {
                // this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                xMove = -1;
                yMove = 0;
            }
            else if (transform.localRotation.eulerAngles.z > 135 && transform.localRotation.eulerAngles.z <= 225)// up
            {
            // this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                xMove = 0;
                yMove = 1;
            }
            else if (transform.localRotation.eulerAngles.z > 225 && transform.localRotation.eulerAngles.z <= 315)// right
            {
                // this.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                xMove = 1;
                yMove = 0;
            }
            else if (transform.localRotation.eulerAngles.z > 315) // down
            {
                // this.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                xMove = 0;
                yMove = -1;
            }
        }
        else{
            // animator.SetBool("walking", false); nevermind this whoops
        }
        changeAnimation();
    }



    //The animation stuff is a mess and needs fixing




    void changeAnimation()
    {
        animator.SetFloat("xChange", xMove);
        animator.SetFloat("yChange", yMove);
    }

    private void FixedUpdate()
    {
        // Sorry Ben I commented all this out, Input.GetAxisRaw only works with WASD controls :( which sucks
        // xMove = Input.GetAxisRaw("Horizontal");
        // yMove = Input.GetAxisRaw("Vertical");
        // if (xMove != 0 || yMove != 0)
        // {
        //     //this.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);//debug
        //     changeAnimation();
        //     animator.SetBool("walking", true); // set walking to true
        //     //walkingSound.UnPause();

        // }
        // // If no movement, no walking animation is needed
        // else
        // {
        //     //this.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);//debug
        //     animator.SetBool("walking", false);
        //     //walkingSound.Pause();
        // }
    }
    // Update is called once per frame
    void Update()
    {
        rotationStablizer();
        animationChangerDirection();
    }
}
