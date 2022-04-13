using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGuard : MonoBehaviour
{
    public int rangeFrames = 360;
    GameObject[] guard;

    public AudioSource gunShot;

    // Reference to hit effect game object
    public GameObject hitEffect;
    // Reference to Guard damage value
    public int damageVal = 30;

    private void Start()
    {
        //gunShot = gameObject.GetComponent<AudioSource>();
        guard = GameObject.FindGameObjectsWithTag("Guard");
        foreach (GameObject x in guard)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), x.GetComponentInChildren<Collider2D>());
        }
    }

    void Update()
    {
        if (rangeFrames > 0)
        {
            rangeFrames -= 1;
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        /*        if (collision.gameObject.CompareTag("ENTER TAG HERE"))
                {
                    // Behavior on that specific tag
                    // ex: collision.gameObject.GetComponent<HealthBar>().setHealth(0);
                }*/
        if (collision.gameObject.CompareTag("Player")) //When player gets shot
        {
            //add player health reduction or whatever should go here
            PlayerData.TakeDamage(damageVal);
            Destroy(gameObject);
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(hit, 1f);
        }
        else if (collision.gameObject.CompareTag("Guard"))
        {
            Debug.LogWarning("Guard is suicidal, stop that");
        }
        else if (collision.collider)
        {
            Destroy(gameObject);
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(hit, 1f);
        }
        //Destroy(gameObject,5f);
    }
        
}
