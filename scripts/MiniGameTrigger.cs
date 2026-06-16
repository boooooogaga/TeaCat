using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameTrigger : DefaultInteract
{
    public int CurrentMiniGameIndex;
    public GameObject player;
    public Rigidbody2D rb;
    public Transform movePosition; //Позиция, на которую нужно перейти при входе в триггер (можно использовать для разных мини-игр)
    public GameObject UsageSprite; //Панелька которая выводится при входе в триггер (типа иконка кнопки взаимодействия или типа того)
    void Start() 
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody2D>();
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
    
        StartCoroutine(MoveAndSwapScene());
    }

        private IEnumerator MoveAndSwapScene()
    {
        // Пока расстояние до целевой точки больше, чем микро-порог (0.05 единиц)
        while (Vector2.Distance(player.transform.position, movePosition.position) > 0.05f)
        {
            // Говорим скрипту игрока двигаться к цели
            player.GetComponent<CharacterControl>().MoveToObject(movePosition);
            
            // КРИТИЧЕСКИ ВАЖНО: Ждем следующего кадра, чтобы физика успела переместить игрока!
            yield return null; 
        }

        // Когда дошли — принудительно останавливаем игрока
        player.GetComponent<CharacterControl>().StopMoving();

        // Загрузка мини-игры
        SceneManager.LoadScene(CurrentMiniGameIndex); 
    }

}
