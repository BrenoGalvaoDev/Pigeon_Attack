using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pigeon : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Collider2D collider2;
    [SerializeField] float speed = 5f;

    public Pooling pooling;

    [Header("Attack Settings")]
    [SerializeField] float attackTime = 5f;

    bool isPaused, canAttack;

    List<GameObject> droppings = new List<GameObject>();

    Vector2 targetPoint;

    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        GameManager.Instance._playerManager.OnPauseGame += (bool value) => isPaused = value;
        GameManager.Instance._playerManager.OnGameOver += () => isPaused = true;
    }
    private void OnDisable()
    {
        GameManager.Instance._playerManager.OnPauseGame -= (bool value) => isPaused = value;
        GameManager.Instance._playerManager.OnGameOver -= () => isPaused = true;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        GetDroppings();
        targetPoint = GetRandomPoint();
        canAttack = true;
    }

    void Update()
    {
        if(isPaused) return;
        Move();
        Attack();
    }

    #region Movement
    void Move()
    {
        if(Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            targetPoint = GetRandomPoint();
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
        spriteRenderer.flipX = targetPoint.x < transform.position.x;
    }

    Vector2 GetRandomPoint()
    {
        return new Vector2(Random.Range(collider2.bounds.min.x, collider2.bounds.max.x),
                           Random.Range(collider2.bounds.min.y, collider2.bounds.max.y));
    }
    #endregion

    #region Attack System

    void Attack()
    {
        if(!canAttack) return;

        canAttack = false;
        
        GameObject drop = Droppings();
        drop.transform.position = transform.position;
        drop.SetActive(true);

        StartCoroutine(AttackCooldown());
    }

    void GetDroppings()
    {
        droppings.Clear();

        droppings = pooling.ObjectPool("droppings");
    }

    GameObject Droppings()
    {
        return droppings.Find(d => !d.activeInHierarchy);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackTime);
        canAttack = true;
    }
    #endregion
}
