using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("シーン移行");
        SceneManager.LoadScene(sceneName);
    }
}
