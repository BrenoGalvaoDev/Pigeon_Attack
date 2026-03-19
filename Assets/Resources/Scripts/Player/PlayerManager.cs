using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PlayerMove), typeof(PlayerLife))]

public class PlayerManager : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int   inicialLife = 5;

    public int coinAmount { get; private set; }

    #region Components

    PlayerMove pMove;
    PlayerLife pLife;


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
    }
    private void OnDisable()
    {
        pLife.OnPlayerDeath -= GameOver;
    }
    #endregion

    #region UNITY

    private void Awake()
    {
        pMove = GetComponent<PlayerMove>();
        pLife = GetComponent<PlayerLife>();
    }

    private void Start()
    {
        SetMoveSpeed();
        SetMaxLife();
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
}