using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    List<PathPoint> linkedPoints = new List<PathPoint>();
    float radius = 30f;

    private void Start()
    {
        PathPointMap._inst.pathPoints.Add(this);
        Invoke("buildPointLinks", 1f);

    }

    void buildPointLinks()
    {
        var objColliders = Physics2D.OverlapCircleAll(transform.position, radius, 1 << (int)15);
        for (var x = 0; x < objColliders.Length; x++)
        {
      
            var hit = Physics2D.Raycast(transform.position, (objColliders[x].transform.position - transform.position).normalized, Vector2.Distance(transform.position, objColliders[x].gameObject.transform.position) + 1f, 1 << 13);
            if (hit.collider == null)
            {
                PathPoint pointObj = objColliders[x].gameObject.GetComponent<PathPoint>();
                if (transform.position != pointObj.transform.position)
                {
                    linkedPoints.Add(pointObj);

                }
            }
        }

     
    }

    public PathPoint getRandomLinkedPoint()
    {
        for (var x = 0; x < linkedPoints.Count; x++)
        {
            Debug.Log(linkedPoints[x].name);
        }
        //Debug.Log(linkedPoints.Count);
        return linkedPoints[Random.Range(0, linkedPoints.Count)];
    }

    public PathPoint getNewLinkedPoint(List<PathPoint> usedPoints)
    {
        var newPoints = new List<PathPoint>();
        for (var x = 0; x < linkedPoints.Count; x++)
        {
            if (!usedPoints.Contains(linkedPoints[x]))
            {
                newPoints.Add(linkedPoints[x]);
            }
          
        }
        if (newPoints.Count == 0)
        {
            return linkedPoints[Random.Range(0, linkedPoints.Count)];
        }

        return newPoints[Random.Range(0, newPoints.Count)];
        //Debug.Log(linkedPoints.Count);
    }

    private void Update()
    {
        /*
        var objColliders = Physics2D.OverlapCircleAll(transform.position, radius, 1 << (int)15);

        for (var x = 0; x < objColliders.Length; x++)
        {
            var hit = Physics2D.Raycast(transform.position, (objColliders[x].transform.position - transform.position).normalized, Vector2.Distance(transform.position, objColliders[x].gameObject.transform.position), 1 << 13);
            if (hit.collider == null)
            {
                PathPoint pointObj = objColliders[x].gameObject.GetComponent<PathPoint>();
                if (transform.position == pointObj.transform.position) continue;
          
                Debug.DrawRay(transform.position, objColliders[x].transform.position - transform.position, Color.green);

            }
        }
        */
    }

    private void OnDrawGizmosSelected()
    {
         Gizmos.color = Color.red;
         //Gizmos.DrawWireSphere(transform.position, radius);
    }
}
