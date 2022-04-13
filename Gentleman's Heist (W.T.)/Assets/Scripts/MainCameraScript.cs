using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*

----Main Camera Script-----

What does this script do?
- Allows the camera to follow the player
    --> Glides along with player, with a short delay

- Allows the camera to stay within preset boundaries
    --> I currently have it set to what my StandAlone project was, we'll need to change these depending on the
        room sizes we end up choosing for the Store/Banks/Prison/etc.

- Allows us to set a size for the camera
    --> Prevents confusion in Unity when switching around the editor's layout and keeps boundary
        values relevant in all situations

*/


public class MainCameraScript : MonoBehaviour
{

    // Reference to Main Camera's camera
    public Camera cameraMain;

    // The reference to the player
    public PlayerScript player;

    public static MainCameraScript Instance {get; set;}

    // The motion delay for the camera, allows for a smoother follow
    public float delay = 4f;

    // The limits the Camera can go to the right, left, up, and down
    // Changed from 500, 0, 500, 0 respectively 
    private float xBoundRight = 999f;
    private float xBoundLeft = -999f;
    private float yBoundTop = 999f;
    private float yBoundBottom = -999f;


    // Start is called before the first frame update
    void Start()
    {
        // Presetting Camera Settings
        // -- This solidifies the aspect ratio of the Main Camera
        // -- For now, I've set it to be a 16:10 ratio for our game -> We can change this later if need be :)

        // cameraMain.orthographicSize = 5.0f; // Orthographic Size is equal to half the vertical length
        // cameraMain.aspect = (float)  8 / cameraMain.orthographicSize; // The Aspect is a width/height ratio -> 16/10 ratio
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Gathering Player's Location
        //  --> These Coorindates keep track of the player's current x and y positions
        float xPos = player.transform.position.x;
        float yPos = player.transform.position.y;

        // Checking if Player is not within the Camera Boundaries
        // --> If the player's position exceeds camera right/left boundaries, we don't alter the X Position
        if(player.transform.position.x <= xBoundLeft || player.transform.position.x >= xBoundRight)
        {
            xPos = transform.position.x;
        }
        // --> The same follows with the top and bottom boundaries...
        if(player.transform.position.y >= yBoundTop || player.transform.position.y <= yBoundBottom)
        {
            yPos = transform.position.y;
        }


        // Changing the Camera's position
        //  --> If the current Camera position isn't matching the players...
        if(transform.position.x != player.transform.position.x || transform.position.y != player.transform.position.y)
        {
            // Change Camera movement to the new xPos and yPos
            // Lerp method allows us to add the delay
            transform.position = Vector3.Lerp(transform.position, new Vector3(xPos, yPos, transform.position.z), delay);
        }

    }
}

