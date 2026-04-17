using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Moving,
    Hurt,
    Dead
}

[RequireComponent(typeof(PlayerMove), typeof(PlayerLife), typeof(PlayerAnimator))]

public class PlayerManager : MonoBehaviour
{
    PlayerState currentState;

    [Header("Player Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int   inicialLife = 5;

    public int coinAmount { get; private set; }

    #region Components

    PlayerMove     pMove;
    PlayerLife     pLife;
    PlayerAnimator pAnim;

    //BOOLS 
    bool isGameOver, isPaused;

    #endregion

    #region Events

    public Action<bool> OnPauseGame;
    public Action       OnGameOver;
    public Action<int>  OnCollectedCoin;


    private void OnEnable()
    {
        pLife.OnPlayerDeath += GameOver;
        pLife.OnPlayerTakeHit += TakeHit;
    }
    private void OnDisable()
    {
        pLife.OnPlayerDeath -= GameOver;
        pLife.OnPlayerTakeHit -= TakeHit;
    }
    #endregion

    #region UNITY

    private void Awake()
    {
        pMove = GetComponent<PlayerMove>();
        pLife = GetComponent<PlayerLife>();
        pAnim = GetComponent<PlayerAnimator>();

        currentState = PlayerState.Idle;
    }

    private void Start()
    {
        SetMoveSpeed();
        SetMaxLife();
    }

    private void Update()
    {
        if (currentState == PlayerState.Dead) return;

        UpdateState();
    }

    #endregion

    #region Set Infos

    void SetMoveSpeed()
    {
        pMove.PlayerMovement(moveSpeed);
    }

    void SetMaxLife()
    {
        pLife.SetMaxHeath(inicialLife);
    }
    #endregion

    #region External

    public void PauseGame()
    {
        isPaused = !isPaused;

        OnPauseGame?.Invoke(isPaused);
    }

    void GameOver()
    {
        isGameOver = true;
        ChangeState(PlayerState.Dead);
        OnGameOver?.Invoke();
    }

    public void CollectionObject(CollectableType type, int amount)
    {
        switch (type)
        {
            case CollectableType.MaxHealth:
                pLife.SetMaxHeath(amount);
                break;
            case CollectableType.Heal:
                pLife.Heal(amount);
                break;
            case CollectableType.Coin:
                coinAmount += amount;
                OnCollectedCoin?.Invoke(coinAmount);
                break;
        }
    }

    #endregion

    #region STATE MANAGER

    void UpdateState()
    {
        if (currentState == PlayerState.Hurt) return;

        bool isMoving = pMove.MoveDirection.x != 0;
        PlayerState newState = isMoving ? PlayerState.Moving : PlayerState.Idle;

        if (newState != currentState)
        {
            ChangeState(newState);
        }
    }

    void ChangeState(PlayerState state)
    {
        currentState = state;
        pAnim.SetAnimation(currentState);
    }

    void TakeHit()
    {
        ChangeState(PlayerState.Hurt);

        StartCoroutine(ResetState(1f));
    }

    IEnumerator ResetState(float timer)
    {
        yield return new WaitForSeconds(timer);

        currentState = PlayerState.Idle;
        ChangeState(currentState);
    }

    #endregion

}