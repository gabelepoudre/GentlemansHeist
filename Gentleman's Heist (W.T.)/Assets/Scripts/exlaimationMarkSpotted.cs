using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exlaimationMarkSpotted : MonoBehaviour
{
    bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
    }


    void HideMark()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerScript.detected == true && done != true)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            Invoke("HideMark",3f);
            done = true;
        }
    }
}
