using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float PlayerSpeed = 5f;
    private Rigidbody2D rb;
    
    private float moveHorizontal;
    private float moveVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // В Update собираем ввод от игрока (каждый кадр)
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
    }

    // В FixedUpdate применяем физику (работает с фиксированным шагом времени)
    void FixedUpdate()
    {
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        
        rb.velocity = movement.normalized * PlayerSpeed;
    }
}
