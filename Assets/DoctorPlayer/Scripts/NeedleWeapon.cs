using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleWeapon : MonoBehaviour
{
    public GameObject Needle;
    public GameObject doctorLight;
    public Transform shotPoint;
    private float shotTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shotTime -= Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            if (shotTime <= 0)
            {
                Instantiate(Needle, shotPoint.position, doctorLight.transform.rotation);
                shotTime = 1f;
            }
        } 
    }
}
