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
    Rigidbody2D rb;
    Vector2 moveDirection;

    public bool canWalk = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        pManager.OnPauseGame += PauseGame;
        pManager.OnGameOver += GameOver;
    }
    private void OnDisable()
    {
        pManager.OnPauseGame -= PauseGame;
        pManager.OnGameOver -= GameOver;
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

    #endregion
}
