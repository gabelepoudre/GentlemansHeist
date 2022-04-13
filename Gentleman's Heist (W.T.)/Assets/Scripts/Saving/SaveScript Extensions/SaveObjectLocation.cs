using UnityEngine;

public class SaveObjectLocation : SaveScript
{
    
    void Start()
    {
        SaveMaster.KeepTrackOf(this);
    }

    public override void SaveMe()
    {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        SaveMaster.SaveVariables(uid, floats:new float[2]{x, y});
    }

    public override void LoadMe()
    {
        var load = SaveMaster.LoadVariables(gameObject, uid);
        bool loadedProperly = load.Item1;
        var data = load.Item2;
        if (loadedProperly)
        {
            Vector3 newPos = new Vector3(data.Item3[0], data.Item3[1], 0);
            gameObject.transform.position = newPos;
            // it looked bad when the bucket was still moving
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
        else
        {
            Debug.Log("failed to load properly");
        }
    }
}
