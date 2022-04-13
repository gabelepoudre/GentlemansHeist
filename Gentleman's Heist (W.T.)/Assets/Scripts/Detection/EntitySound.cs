using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Decided to make this a script you attach to some 2d circle, which is attached to an entity that is needed
 * Will modify the size of the circle
 */
public class EntitySound : MonoBehaviour
{
    // The ratio at which an objects velocity impacts it's sound. Could be dynamically changed
    public double speedToRadiusRatio;
    
    // number frames transition is split over. More frames = 
    public int transitionDivs = 60;

    // making it pretty stuff
    private double _oldRadius;
    private double _newRadius;
    private double incremenet;
    
    // The current sound radius that will be displayed
    private double _curRadius;

    // The amount of time currentRadius will not be updated with velocity (ie after SetSoundEvent)
    private int _eventFrames;
    
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _eventFrames = 0;
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_eventFrames > 0)
        {
            _eventFrames -= 1;
        }
        else
        {
            if (speedToRadiusRatio < 0)
            {
                speedToRadiusRatio = 0.0001f;
            }
            // most of this is for making the transition between states look nice
            _oldRadius = _curRadius;
            _newRadius = (rb.velocity.magnitude) * speedToRadiusRatio;
            if (_oldRadius > _newRadius)
            {
                incremenet = -((_oldRadius - _newRadius) / transitionDivs);
            }
            else
            {
                incremenet = ((_newRadius - _oldRadius) / transitionDivs);
            }
        }
        
        // all of this is to make the transitions look nice
        if (_curRadius == 0)
        {
            _curRadius = 0.001;
        }
        else if (_newRadius == 0)
        {
            _newRadius = 0.001;
        }
        if (_curRadius / _newRadius < 0.99 || _curRadius / _newRadius > 1.01)
        {
            _curRadius += incremenet;
        }
        else
        {
            _curRadius = _newRadius;
        }
        
        gameObject.transform.localScale = new Vector3((float) _curRadius / 2.0f, (float) _curRadius / 2.0f, 1f);
    }

    /*
     * Given some in game event (ex: tripping) allows outside setting of a custom radius for limited time (frames)
     * Hint: The game should run at 60fps
     * TODO: Untested
     */
    public void SetSoundEvent(double newRadius, int numFrames)
    {
        _curRadius = newRadius;
        _eventFrames = numFrames;
    }

    // gets the radius goal, neglects the transition period
    public double GetCurrentRadius()
    {
        return _newRadius;
    }
}
