using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPointMap : MonoBehaviour
{
    public static PathPointMap _inst;
    public List<PathPoint> pathPoints = new List<PathPoint>();

    private void Awake()
    {
        _inst = this;
    }


    
}
