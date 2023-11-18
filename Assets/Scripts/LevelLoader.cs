using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class LevelLoader : MonoBehaviour
{
    // [SerializeField] Image crossfadeImage;
    // public float SceneTransTime = 1f;
    MMF_Player sceneLoaderFeedback;
    private void Awake()
    {
        sceneLoaderFeedback = GetComponent<MMF_Player>();
    }
    public void LoadScene(string scene)
    {
        if (!sceneLoaderFeedback)
            return;
        sceneLoaderFeedback.GetFeedbackOfType<MMF_LoadScene>().DestinationSceneName = scene;
        sceneLoaderFeedback.PlayFeedbacks();
        // StartCoroutine(LoadLevelCoroutine(scene));
    }
    
    // IEnumerator LoadLevelCoroutine(string scene)
    // {
    //     crossfadeImage.gameObject.SetActive(true);
    //     crossfadeImage.DOColor(Color.black, SceneTransTime);
    //     yield return new WaitForSeconds(SceneTransTime);
    //     SceneManager.LoadScene(scene);
    //     crossfadeImage.DOColor(Color.clear, SceneTransTime).OnComplete(
    //         () => crossfadeImage.gameObject.SetActive(false)
    //     );
    // }




}
