using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LaserArray : MonoBehaviour
{
    private Transform gameObjectTransform;
    public float length;
    private List<(LineRenderer, Vector2)> lines = new List<(LineRenderer, Vector2)>();
    // Start is called before the first frame update
    void Start()
    {
        gameObjectTransform = gameObject.transform;
        Physics2D.queriesHitTriggers = false;
        List<Vector2> angles = GetAngles(45, -90, 90);
        foreach (var angle in angles)
        {
            LineRenderer line = GenLine();
            line.SetPosition(0, gameObject.transform.position);
            lines.Add((line, angle));
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var bit in lines)
        {
            var line = bit.Item1;
            var dir = bit.Item2;
            Debug.DrawRay(gameObjectTransform.position, dir);
            RaycastHit2D ray = Physics2D.Raycast(gameObjectTransform.position, dir, length);
            Debug.Log("Point: " + ray.point);
            Debug.Log("Direction: " + dir);
            // if (ray.collider != null)
            // {
                line.SetPosition(0, gameObjectTransform.position);
                line.SetPosition(1, ray.point);
            // }
            // else
            // {
            //     line.SetPosition(0, gameObjectTransform.position);
            //     line.SetPosition(1, ray.point);
            // }
        }
    }

    private LineRenderer GenLine()
    {
        LineRenderer lineRend = new GameObject("Line").AddComponent<LineRenderer>();
        lineRend.startColor = new Color(0, 0, 0, 10);
        lineRend.endColor = new Color(0, 0, 0, 50);
        lineRend.startWidth = 0.01f;
        lineRend.endWidth = 0.03f;
        lineRend.positionCount = 2;
        lineRend.useWorldSpace = true;
        return lineRend;
    }

    private List<Vector2> GetAngles(float start, float end, int divs)
    {
        List<Vector2> angies = new List<Vector2>();
        float steps = (Math.Abs(start) + Math.Abs(end)) / divs;
        float angler = start;
        while (!(0.995 < (angler / end) && (angler / end) < 1.005))
        {
            angies.Add(AngleToVector2(angler));
            angler -= steps;
        }

        return angies;
    }

    private Vector2 AngleToVector2(float angle)
    {
        return new Vector2((float) Math.Cos(angle*(3.1415/180)), (float) Math.Cos(angle*(3.1415/180)));
    } 
}
