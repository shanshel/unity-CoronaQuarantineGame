using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTestManager : MonoBehaviour
{
    public GameObject aiprefab;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("spawnTest", 2f);
    }

    void spawnTest()
    {
        for (var x = 0; x < 100; x++)
        {
            //Instantiate(aiprefab, PathPointMap._inst.pathPoints[Random.Range(0, PathPointMap._inst.pathPoints.Count)].transform.position, Quaternion.identity);
        }
    }
  
}
