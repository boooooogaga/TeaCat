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
    public void MoveToObject(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * PlayerSpeed;
    }
    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }
    // В FixedUpdate применяем физику (работает с фиксированным шагом времени)
    void FixedUpdate()
    {
        
        
        
    }
}
