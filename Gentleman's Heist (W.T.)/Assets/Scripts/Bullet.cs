using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int rangeFrames = 360;
    public GameObject hitEffect;
    GameObject player;

    public AudioSource gunShot;

    private void Start()
    {
        //gunShot = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>() );
    }
    void FixedUpdate()
    {
        if (rangeFrames > 0)
        {
            rangeFrames -= 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Player")
        {
            Debug.LogWarning("Bullet hit player, which isn't good!");
        }
/*        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Behavior on that specific tag
            // ex: collision.gameObject.SetActive(false);
            // ex: collision.gameObject.GetComponent<HealthBar>().setHealth(0);
            Destroy(gameObject, 0.25f);
        }*/
        else if (collision.gameObject.CompareTag("Guard")) //When guard gets shot
        {
            collision.gameObject.GetComponent<EnemyAI>().gotShot();
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity );
            Destroy(gameObject);
            Destroy(hit, 1f);
        }
        if (collision.collider)
        {
            Destroy(gameObject, 0f);
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(hit, 1f);
        }
        Destroy(gameObject,2f);
    }
        
}
