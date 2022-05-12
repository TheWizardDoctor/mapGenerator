using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public GameObject loadingTextGO;

    public void LoadScene(string name)
    {
        if(loadingTextGO!=null)
        {
            loadingTextGO.SetActive(true);
        }
        SceneManager.LoadScene(name);
    }
}
