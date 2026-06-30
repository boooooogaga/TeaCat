using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuControl : MonoBehaviour
{
    public GameObject transPan; //панель для анимации перехода
    public void Enter()
    {
        StartCoroutine(sceneTransition(1));
    }

    public void Leave()
    {
        Application.Quit();
    }
    public void Defs()
    {
        
    }

    public IEnumerator sceneTransition(int sceneNum)
    {
        transPan.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(1);
    }
}
