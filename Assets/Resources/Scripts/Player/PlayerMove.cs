using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Move Settings")]

    [SerializeField] private float moveSpeed = 5f;

    //Components
    PlayerManager pManager;
    PlayerLife pLife;
    Rigidbody2D rb;
    Vector2 moveDirection;
    SpriteRenderer spriteRenderer;

    public bool canWalk = true;

    public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pManager = GetComponent<PlayerManager>();
        pLife = GetComponent<PlayerLife>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        pManager.OnPauseGame += PauseGame;
        pManager.OnGameOver += GameOver;
        pLife.OnPlayerTakeHit += () => StartCoroutine(TakeHit());
    }
    private void OnDisable()
    {
        pManager.OnPauseGame -= PauseGame;
        pManager.OnGameOver -= GameOver;
        pLife.OnPlayerTakeHit -= () => StartCoroutine(TakeHit());
    }

    void FixedUpdate()
    {
        if (canWalk)
        {
            Move();
        }
    }

    void Move()
    {
        if(moveDirection.x != 0)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        spriteRenderer.flipX = moveDirection.x < 0 ? true : moveDirection.x > 0 ? false : spriteRenderer.flipX;
    }

    public void SetMoveInputDirection(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Vector2 m = value.ReadValue<Vector2>();

            moveDirection.x = m.x;
        }
        if(value.canceled)
        {
            moveDirection.x = 0;
        }
    }

    #region EXTERNAL

    void PauseGame(bool value) { canWalk = !value; }
    void GameOver() { canWalk = false; rb.velocity = Vector2.zero; }

    public void PlayerMovement(float value) { moveSpeed = value; }

    IEnumerator TakeHit()
    {
        canWalk = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(1f);
        canWalk = true; 
    }

    #endregion
}
