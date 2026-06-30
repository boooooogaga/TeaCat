using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameTrigger : DefaultInteract
{
    public int CurrentMiniGameIndex; //Позиция, на которую нужно перейти при входе в триггер (можно использовать для разных мини-игр)
    public GameObject UsageSprite; //Панелька которая выводится при входе в триггер (типа иконка кнопки взаимодействия или типа того)
    void Start() 
    {
        UsageSprite.SetActive(false); //Скрываем иконку взаимодействия при старте
    }


    public override void onFocus()
    {
        Debug.Log("Player is in focus of the mini-game trigger.");
        UsageSprite.SetActive(true);
    }
    public override void onDefocus()
    {
        Debug.Log("Player is out of focus of the mini-game trigger.");
        UsageSprite.SetActive(false);
    }

    public override void Interact()
    {
        Debug.Log("Player has interacted with the mini-game trigger.");

        SceneManager.LoadScene(CurrentMiniGameIndex);
    }

}
