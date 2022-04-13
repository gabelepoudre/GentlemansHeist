using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardConeGone : MonoBehaviour
{
    bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerScript.detected == true && done != true)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            done = true;
        }
    }
}
