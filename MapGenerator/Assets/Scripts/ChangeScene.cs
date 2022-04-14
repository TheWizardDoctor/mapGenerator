using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingText;
    public void nextScene(string sceneName)
    {
        loadingText.SetActive(true);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
