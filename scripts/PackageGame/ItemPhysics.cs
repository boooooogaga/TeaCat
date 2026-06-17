using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPhysics : DefaultInteract
{
    public CursorControll cursor;
    public Rigidbody2D rb;
    public CircleCollider2D Collider;

    public SpriteRenderer Shadow;
    void Start()
    {   
        Shadow = GetComponentInChildren<SpriteRenderer>();
        Collider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        cursor = GameObject.Find("hand_0").GetComponent<CursorControll>();
    }

    public override void onFocus()
    {
        Debug.Log("Focus");
        cursor.SetStateSprite(2);
        Collider.enabled = true;
    }
    public override void onDefocus()
    {
        Debug.Log("Defocus");
        cursor.SetStateSprite(0);
        Collider.enabled = false;
    }
    public override void MouseProcess()
    {
        Debug.Log("MouseProcess");
        rb.gravityScale = 0;

    // 1. Получаем мировые координаты из пикселей экрана
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // 2. КРИТИЧЕСКИ ВАЖНО: Зануляем Z, чтобы объект оставался в плоскости 2D-игры
        worldPos.z = 0f;

    // 3. Передаем готовую позицию объекту
        transform.position = worldPos;
    }
    public override void MouseUp()
    {
        Debug.Log("MouseUp");
        
        rb.gravityScale = 1;
        rb.velocity = cursor.GetComponent<Rigidbody2D>().velocity;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            Shadow.enabled = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            Shadow.enabled = false;
        }
    }
}
