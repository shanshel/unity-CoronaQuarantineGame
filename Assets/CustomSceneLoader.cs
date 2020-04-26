using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;
using UnityEngine.SceneManagement;

public class CustomSceneLoader : MonoBehaviour
{
    public Animator _anim;
    // Start is called before the first frame update
    private static CustomSceneLoader _instance;
    public int prevScene;
    public static CustomSceneLoader _inst { get { return _instance; } }



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

    public void loadScene(SceneEnum scene)
    {
      
        _anim.SetTrigger("End");
        if (scene == SceneEnum.Lobby)
        {
            StartCoroutine(loadSceneEnumerator(1));
        }
        else if (scene == SceneEnum.InGame)
        {
            StartCoroutine(loadSceneEnumerator(3));
        }
        else if (scene == SceneEnum.MainMenu)
        {
            StartCoroutine(loadSceneEnumerator(0));
        }
        else if (scene == SceneEnum.Room)
        {
            StartCoroutine(loadSceneEnumerator(2));
        }
        else if (scene == SceneEnum.PrevScene)
        {
            StartCoroutine(loadSceneEnumerator(prevScene));
        }
       
        prevScene = SceneManager.GetActiveScene().buildIndex;
    }



 

    IEnumerator loadSceneEnumerator(int sceneIndex)
    {
        SoundManager._inst.playSoundOnce(SoundEnum.SceneSwipe);
        yield return new WaitForSeconds(1f);
        UIWindow.beforeTransLoadNewScene();
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneIndex);
        while(!asyncOp.isDone)
        {
            yield return new WaitForSeconds(.1f);
        }
        _anim.SetTrigger("Start");
        yield return new WaitForSeconds(.2f);
        SoundManager._inst.playSoundOnce(SoundEnum.SceneSwipe);
        UIWindow.transWaitForNewSceneComplete();
    }
}
