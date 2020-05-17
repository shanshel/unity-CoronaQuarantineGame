using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

public class VisibilitySystemActor : MonoBehaviour
{
    public GameObject[] objectToActivateOnSee;
    public SpriteRenderer[] spriteToEnableOnSee;

    public CircleCollider2D ColliderSeeArea;
    public Rigidbody2D _rigidBodyKi;
    public bool forPatients, forDoctors;

    public Team visiblityTeam = Team.Both;

 
    bool isShowed;
    private void Start()
    {
        hide();
    }

    private void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (visiblityTeam == NetworkPlayers._inst._localCPlayer._thisPlayerTeam)
        {
            show();
            return;
        }


        if (collision.tag == "Patient" && forPatients)
        {

            CPlayer player = null;
            if (visiblityTeam == Team.Doctors)
            {
                player = transform.parent.GetComponent<CPlayer>();
            }
            hide();
            if (player)
                player._currentWeaponObject.VisiablePartContainer.SetActive(false);
        }

        if (collision.tag == "Doctor" && forDoctors)
        {
            hide();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (NetworkPlayers._inst == null || NetworkPlayers._inst._localCPlayer == null) return;
        if (visiblityTeam == NetworkPlayers._inst._localCPlayer._thisPlayerTeam)
        {
         
            show();
            return;
        }


        if (collision.tag == "Patient" && forPatients)
        {
            
            var hitRay = Physics2D.Raycast(transform.position, collision.transform.position - transform.position, 50f, LayerMask.GetMask("PatientPlayerLayer") | LayerMask.GetMask("Wall"));
            CPlayer player = null;
            if (visiblityTeam == Team.Doctors)
            {
                player = transform.parent.GetComponent<CPlayer>();
            }
            if (hitRay.transform.tag == "Patient")
            {
                CPlayer patientCPlayer = hitRay.transform.GetComponent<CPlayer>();

                show();
                CPlayer seenBy = transform.parent.GetComponent<CPlayer>();
                if (seenBy != null && seenBy.cStatus != playerStatus.dead)
                {
                    patientCPlayer.suprise();
                }
                
                if (player)
                    player._currentWeaponObject.VisiablePartContainer.SetActive(true);
            }
            else
            {
                hide();
                if(player)
                    player._currentWeaponObject.VisiablePartContainer.SetActive(false);
            }
        }
       
        if (collision.tag == "Doctor" && forDoctors)
        {
            var hitRay = Physics2D.Raycast(transform.position, collision.transform.position - transform.position, 50f, LayerMask.GetMask("DoctorPlayerLayer") | LayerMask.GetMask("Wall"));

   
            if (hitRay.transform.tag == "Doctor")
            {
                var doctorScript = hitRay.transform.GetComponent<CPlayer>();
                var dir = (transform.position - doctorScript.transform.position);
                var eluerAngle = doctorScript.aimContainerObject.transform.rotation.eulerAngles;
                var lookAngle = eluerAngle.z + 90f;
                //var lookAngle = doctorScript.lookQuaternion.eulerAngles.z;

                Quaternion rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
                float doctorToEnemyAngle = rotation.eulerAngles.z;


                if (Mathf.DeltaAngle(lookAngle, doctorToEnemyAngle) < 45f && Mathf.DeltaAngle(lookAngle, doctorToEnemyAngle) > -45f)
                {
                    CPlayer seenBy = transform.parent.GetComponent<CPlayer>();
                    if (seenBy != null && seenBy.cStatus != playerStatus.dead)
                    {
                        doctorScript.suprise();
                    }
                   
                    show();
                }
                else
                {
                    hide();
                }


            }
            else
            {
                hide();
            }

        }

    }


    void hide()
    {
        isShowed = false;
        foreach (var objToActive in objectToActivateOnSee)
        {
            objToActive.SetActive(false);
        }
        foreach (var spToEnable in spriteToEnableOnSee)
        {
            spToEnable.enabled = false;
        }
    }

    void show()
    {
        isShowed = true;
        foreach (var objToActive in objectToActivateOnSee)
        {
            objToActive.SetActive(true);
        }
        foreach (var spToEnable in spriteToEnableOnSee)
        {
            spToEnable.enabled = true;
        }
    }
}
