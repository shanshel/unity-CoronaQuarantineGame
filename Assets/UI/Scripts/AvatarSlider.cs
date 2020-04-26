using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AvatarSlider : MonoBehaviour
{
    
    public List<GameObject> doctorAvatarsList;
    public List<GameObject> patientAvatarsList;

    public List<GameObject> doctorsAvatarsInstats;
    public List<GameObject> patientAvatarsInstats;
    public GameObject avatarParent;
    bool isDoctorAvatars = false;
    int currentIndex = 0;
    int prevIndex = 0;
    int nextIndex = 0;
    float timer = 1f;
    // Start is called before the first frame update
    void Start()
    {

        for (var x = 0; x < doctorAvatarsList.Count; x++)
        {
            var avatar = Instantiate(doctorAvatarsList[x], Vector2.zero, Quaternion.identity, avatarParent.transform);
            RectTransform rectTransform = avatar.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            avatar.SetActive(false);
            doctorsAvatarsInstats.Add(avatar);
            if (x == 0)
            {
                avatar.SetActive(true);
            }

        }
        SetDoctorsAvatars();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
    }
    void SetDoctorsAvatars()
    {
        currentIndex = 0;
        prevIndex = doctorAvatarsList.Count - 1;
        nextIndex = 1;
        isDoctorAvatars = true;
        loadStarterAvater();
    }

    void SetPatientAvatars()
    {
    
    }
    public void Prev()
    {
        if (timer > 0)
        {
            return;
        }
        timer = 1f;
        int localNextIndex, localPrevIndex, localCurrentIndex;
        localCurrentIndex = currentIndex - 1;
        localNextIndex = currentIndex;
        localPrevIndex = currentIndex - 2;

        if (isDoctorAvatars)
        {
            if (localCurrentIndex < 0)
            {
                localCurrentIndex = doctorsAvatarsInstats.Count - 1;
                localPrevIndex = localCurrentIndex - 1;
            }
            // move current to right
            var currentAvatar = doctorsAvatarsInstats[currentIndex];
            var currentAvatarRect = currentAvatar.GetComponent<RectTransform>();
            currentAvatarRect.DOAnchorPos(new Vector2(1000f, 0f), .6f);

            //move prev to zero
            var nextAvatar = doctorsAvatarsInstats[prevIndex];
            var nextAvatarRect = nextAvatar.GetComponent<RectTransform>();
            nextAvatarRect.anchoredPosition = new Vector2(-1000f, 0);
            nextAvatar.SetActive(true);
            nextAvatarRect.DOAnchorPos(new Vector2(0f, 0f), .6f);
        }

        currentIndex = localCurrentIndex;
        prevIndex = localPrevIndex;
        nextIndex = localNextIndex;

    }
    public void next()
    {
        if (timer > 0)
        {
            return;
        }
        timer = 1f;
        int localNextIndex, localPrevIndex, localCurrentIndex;
        localCurrentIndex = nextIndex;
        localNextIndex = nextIndex + 1;
        localPrevIndex = currentIndex;
       
        if (isDoctorAvatars)
        {
            if (localNextIndex >= doctorsAvatarsInstats.Count)
            {
                localNextIndex = 0;
            }
            Debug.Log("dddvvv");
            // move current to left
            var currentAvatar = doctorsAvatarsInstats[currentIndex];
            var currentAvatarRect = currentAvatar.GetComponent<RectTransform>();
            currentAvatarRect.DOAnchorPos(new Vector2(-1000f, 0f), .6f);

            //move next to zero 
            var nextAvatar = doctorsAvatarsInstats[nextIndex];
            var nextAvatarRect = nextAvatar.GetComponent<RectTransform>();
            nextAvatarRect.anchoredPosition = new Vector2(1000f, 0);
            nextAvatar.SetActive(true);
            nextAvatarRect.DOAnchorPos(new Vector2(0f, 0f), .6f);
        }

        currentIndex = localCurrentIndex;
        prevIndex = localPrevIndex;
        nextIndex = localNextIndex;

        /*
        var nextIndex = currentIndex + 1;
        if (isDoctorAvatars)
        {
            if (nextIndex >= doctorAvatarsList.Count)
            {
                nextIndex = 0;
            }
        }

        currentIndex = nextIndex;
        loadAvatar();
        */
    }

    void prev()
    {

    }

    void loadStarterAvater()
    {
        if (isDoctorAvatars)
        {
 
            var neededAvater = doctorAvatarsList[currentIndex];
     
            var rectTrans = neededAvater.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = new Vector2(1000f, 0f);
            neededAvater.SetActive(true);
            rectTrans.DOAnchorPos(Vector2.zero, .6f);
        }
    }
    void loadAvatar()
    {
        //move old to the left
        if (isDoctorAvatars)
        {

            if (prevIndex != currentIndex)
            {
                var prevAvatar = doctorAvatarsList[prevIndex];
                var prevRect = prevAvatar.GetComponent<RectTransform>();
                prevRect.DOAnchorPos(new Vector2(-1000f, 0f), .6f);
            }

            var neededAvater = doctorAvatarsList[currentIndex];
            var rectTrans = neededAvater.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = new Vector2(1000f, 0f);
            neededAvater.SetActive(true);
            rectTrans.DOAnchorPos(Vector2.zero, .6f);
        }
    }
}
