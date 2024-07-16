using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoints : MonoBehaviour
{
    public static PathPoints instance;

    public Transform floor1, floor2;
    [HideInInspector]
    public List<Transform> floor1Points, floor2Points = new List<Transform>();

    public bool isFirstFloor;
    private void Awake()
    {
        instance = this;

        foreach (Transform point in floor1)
        {
            floor1Points.Add(point);
        }
        foreach (Transform point in floor2)
        {
            floor2Points.Add(point);
        }
    }

    public List<Transform> GetPathPints()
    {
        if (isFirstFloor)
        {
            isFirstFloor = !isFirstFloor;
            return floor2Points;
        }
        else
        {
            isFirstFloor = !isFirstFloor;
            return floor1Points;
        }
    }


}
