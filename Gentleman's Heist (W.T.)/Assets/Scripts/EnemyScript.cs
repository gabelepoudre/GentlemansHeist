using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //[SerializeField]
    //Transform player;

    //[SerializeField]
    //float agroRange;

    //[SerializeField]
    //float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*float distToPlayer = Vector2.Distance(transform.position, player.position);
        //print(distToPlayer);


        if (player.GetComponentInChildren<CircleCollider2D>().IsTouching(this.gameObject.GetComponent<CircleCollider2D>()))
        {
            PlayerFound();
        }
        else
        {
            PlayerNotFound();
        }*/

    }

    void PlayerFound()
    {
        this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }

    void PlayerNotFound()
    {
        this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }
}
