using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBeh : DefaultInteract
{
    private Animator anim;
    public Transform spot;
    public Transform interactPoint;
    public Rigidbody2D rb;
    public CharacterControl player;
    public int Id;
    public bool CanInteract;
    void Awake()
    {
        player = GetComponent<CharacterControl>();
    }

    void Start()
    {
        StartCoroutine(MoveToSpot(spot));
    }
    void Update()
    {
        if(OrderManager.activeOrders[Id].redTeaRequired == 0 && OrderManager.activeOrders[Id].redTeaRequired == 0 && OrderManager.activeOrders[Id].redTeaRequired == 0)
        {
            
        }
    }
    public override void onFocus()
    {
        anim.SetBool("Focus", true);
    }

    public override void onDefocus()
    {
        anim.SetBool("Focus", false);
    }

    public override void Interact()
    {
        if(CanInteract)
        {
            OrderManager.Instance.AddOrder(Id, Random.Range(0,4),Random.Range(0,4),Random.Range(0,4));
        }
    }

        private IEnumerator MoveToSpot(Transform point)
    {
        // Пока расстояние до целевой точки больше, чем микро-порог (0.05 единиц)
        while (Vector2.Distance(transform.position, point.position) > 0.05f)
        {
            Vector2 direction = (point.position - transform.position).normalized;
            rb.velocity = direction;
            
            // КРИТИЧЕСКИ ВАЖНО: Ждем следующего кадра, чтобы физика успела переместить игрока!
            yield return null; 
        }

        // Когда дошли — принудительно останавливаем игрока
        player.GetComponent<CharacterControl>().StopMoving();

    }
}
