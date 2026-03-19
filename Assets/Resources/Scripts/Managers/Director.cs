using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard
    }
    public enum DirectorState
    {
        Relax,      // jogador está em perigo > reduz pressão
        BuildUp,    // tensão está subindo
        Peak,       // máxima pressão
        Cooldown    // recuperação após pico
    }

    [Header("Components")]
    [SerializeField] GameManager gameManager;
    [SerializeField] PigeonsSpawnManager pigeonsSpawn;
    [SerializeField] CollectiblesScpawnManager collectiblesSpawn;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] PlayerLife playerLife;


    #region DATA    
    int CurrentScore, CurrentLife, MaxLife, CoinAmount, PigeonsAmountInScene;

    public GameDifficulty gameDifficulty;
    public DirectorState state;

    bool directorActive = true;
    #endregion

    public static Director Instance { get; private set; }
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

    private void Start()
    {
        InvokeRepeating(nameof(ParaTeste), 5f, 5f);
    }

    private void Update()
    {
        if(!directorActive) return;

        CurrentScore = gameManager.Score;


    }

    void ParaTeste()
    {
        collectiblesSpawn.SpawnCollectible();
    }
    #region EVENTS
    private void OnEnable()
    {
        playerManager.OnPauseGame += (bool value) => directorActive = value;
        playerManager.OnCollectedCoin += (int coinAmount) => CoinAmount = coinAmount;

        playerLife.OnHealthChanged += (int currentHealth, int maxHealth) => 
        {
            CurrentLife = currentHealth;
            MaxLife = maxHealth;
        };
    }
    private void OnDisable()
    {
        playerManager.OnPauseGame -= (bool value) => directorActive = value;
        playerManager.OnCollectedCoin -= (int coinAmount) => CoinAmount = coinAmount;

        playerLife.OnHealthChanged -= (int currentHealth, int maxHealth) =>
        {
            CurrentLife = currentHealth;
            MaxLife = maxHealth;
        };
    }
    #endregion
}
