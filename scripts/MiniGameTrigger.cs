using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameTrigger : MonoBehaviour
{
    public int CurrentMiniGameIndex; //Индекс сцены на которую нужно перейти при входе в триггер (можно использовать для разных мини-игр)
    public GameObject UsageSprite; //Панелька которая выводится при входе в триггер (типа иконка кнопки взаимодействия или типа того)
    void Start() 
    {
        UsageSprite.SetActive(false); // Скрываем панельку при старте
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger area.");
            UsageSprite.SetActive(true);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Теперь Unity гарантированно поймает нажатие в любой момент
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                Debug.Log("Player pressed E to interact with the mini-game trigger.");
                SceneManager.LoadScene(CurrentMiniGameIndex);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger area.");
            UsageSprite.SetActive(false); // Скрываем панельку при выходе из триггера
        }
    }
}
