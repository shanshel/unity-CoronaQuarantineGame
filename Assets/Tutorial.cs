using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;
using DG.Tweening;
public class Tutorial : MonoBehaviour
{
    private static Tutorial _instance;
    public static Tutorial _inst { get { return _instance; } }



    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public GameObject doctorTutorial, patientTutorial;

    public void hideTutorial(Team team)
    {
        if (team == Team.Doctors)
        {
            doctorTutorial.SetActive(false);
        }
        else
        {
            patientTutorial.SetActive(false);

        }
    }

    public void showTutorial(Team team)
    {
        if (team == Team.Doctors)
        {
            doctorTutorial.SetActive(true);
        }
        else if (team == Team.Patients)
        {
            patientTutorial.SetActive(true);
        }

    }
}
