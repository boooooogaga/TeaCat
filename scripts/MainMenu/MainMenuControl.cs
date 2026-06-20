using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuControl : MonoBehaviour
{
    public void Enter()
    {
        SceneManager.LoadScene(1); 
    }

    public void Leave()
    {
        Application.Quit();
    }
    public void Defs()
    {
        
    }
}
