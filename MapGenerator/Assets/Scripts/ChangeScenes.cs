using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public GameObject loadingTextGO;

    public void LoadScene(string name)
    {
        if (loadingTextGO != null)
        {
            loadingTextGO.SetActive(true);
        }

        StartCoroutine(LoadYourAsyncScene(name));
    }

    private IEnumerator LoadYourAsyncScene(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
