using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Conponents")]
    [SerializeField] PlayerManager playerManager;
    [SerializeField] UIManager uiManager;

    #region GAME DATA
    int score = 0;
    int highScore = 0;
    float time = 0f;

    public int Score => score;
    public int HighScore => highScore;
    #endregion

    #region INFOS
    bool isGameOver = false;
    bool isPause = false;
    #endregion

    public static GameManager Instance { get; private set; }
    public PlayerManager _playerManager { get => playerManager; set => playerManager = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(!isGameOver && !isPause)
        {
            UpdateScore();
            UpdateUI(score, highScore);
        }
    }

    #region EVENTS

    private void OnEnable()
    {
        playerManager.OnPauseGame += (bool value) => isPause = value;
        playerManager.OnGameOver += () => isGameOver = true;
        playerManager.OnCollectedCoin += IncrementScore;
    }

    private void OnDisable()
    {
        playerManager.OnPauseGame -= (bool value) => isPause = value;
        playerManager.OnGameOver -= () => isGameOver = true;
        playerManager.OnCollectedCoin -= IncrementScore;
    }
    #endregion

    #region CALLED BY EVENTS

    #endregion

    #region SAVE&LOAD SYSTEM

    #endregion

    #region SCORE SYSTEM

    void UpdateScore()
    {
        time += Time.deltaTime;
        score = (int)time;
        UpdateHighScore();
    }

    void UpdateHighScore()
    {
        if(score > highScore)
        {
            highScore = score;
        }
    }

    void ResetScore()
    {
        score = 0;
        time = 0f;
    }

    void IncrementScore(int increment)
    {
        score += (increment * 10);
    }

    void UpdateUI(float score, float maxScore)
    {
        uiManager.UpdateScoreTxt(score, maxScore);
    }

    #endregion
}
