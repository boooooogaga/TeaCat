using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControll : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Cursor cursor;
    private Rigidbody2D rb;
    [Header("Cursor Sprites")]
    [SerializeField] private Sprite[] sprites;
    private Vector3 MousePos;

    private Vector3 lastPosition;
    public float rotationSpeed = 10f;

    public Vector3 customVelocity { get; private set; }
    void Start()
    {
        Cursor.visible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void GetMouseData()
    {
        // Получаем позицию мыши в пикселях
        Vector3 mousePos = Input.mousePosition;

        // Переводим пиксели в мировые координаты (метры)
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Для 2D зануляем Z, чтобы объект не улетел за плоскость камеры
        worldPos.z = 0f;
        MousePos = worldPos;
    }
    public void SetCursorPos()
    {
         GetMouseData();

        // Присваиваем объекту правильные координаты
        transform.position = MousePos;
    }
    public void SetCursorRot()
    {


        // 2. Рассчитываем вектор "скорости" (разницу между кадрами)
        customVelocity = (transform.position - lastPosition) / Time.deltaTime;

        // 3. Поворачиваем курсор на основе рассчитанной скорости
        if (customVelocity.sqrMagnitude > 0.01f)
        {
            // Считаем угол наклона по оси X (как в предыдущем примере)
            float targetAngle = customVelocity.x * -2f; // Коэффициент наклона
            targetAngle = Mathf.Clamp(targetAngle, -45f, 45f);

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Плавное возвращение в исходное положение, если мышь замерла
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        }

        // Сохраняем позицию для следующего кадра
        lastPosition = transform.position;
    }
    void Update()
    {
        GetMouseData();
        SetCursorPos();
        SetCursorRot();
        if(Input.GetMouseButtonDown(0))
        {
            SetStateSprite(1);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            SetStateSprite(0);
        }
        
    }

    public void SetStateSprite(int state = 0)
    {
        if(state >= 0 && state < sprites.Length)
        {
            spriteRenderer.sprite = sprites[state];
        }
    }
    
}
