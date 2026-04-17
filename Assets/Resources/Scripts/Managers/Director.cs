using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    [Header("Spawn Settings")]
    [SerializeField] float relaxCooldown = 10f;    // Mais frequente em relax
    [SerializeField] float buildUpCooldown = 15f;
    [SerializeField] float peakCooldown = 30f;     // Raro em pico
    [SerializeField] float cooldownCooldown = 20f;
    private float lastCollectibleSpawnTime;

    [Header("Pigeon Settings")]
    [SerializeField] int maxPigeonsEasy = 5;
    [SerializeField] int maxPigeonsMedium = 8;
    [SerializeField] int maxPigeonsHard = 12;
    [SerializeField] float pigeonCheckInterval = 1f;  // Verifica a cada 1s
    private float lastPigeonCheckTime;

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

    private void Update()
    {
        if(!directorActive) return;

        CurrentScore = gameManager.Score;

        if (Time.time - lastPigeonCheckTime >= pigeonCheckInterval)
        {
            HandlePigeonPopulation();
            lastPigeonCheckTime = Time.time;
        }

        HandleCollectibleSpawn();
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

        pigeonsSpawn.OnPigeonsAmountChanged += (int pigeonsAmount) => PigeonsAmountInScene = pigeonsAmount;
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

        pigeonsSpawn.OnPigeonsAmountChanged -= (int pigeonsAmount) => PigeonsAmountInScene = pigeonsAmount;
    }
    #endregion

    #region STATE

    #endregion

    #region COLLECTIBLES
    void HandleCollectibleSpawn()
    {
        if (Time.time - lastCollectibleSpawnTime < GetStateCooldown()) return;

        float lifePercent = (float)CurrentLife / MaxLife;

        float spawnChance = 1f - lifePercent;

        float stateModifier = state switch
        {
            DirectorState.Relax => 0.5f,    
            DirectorState.BuildUp => 1.1f,
            DirectorState.Peak => 1.5f,     
            DirectorState.Cooldown => 1.2f,
            _ => 1f
        };

        if (Random.value < spawnChance * stateModifier)
        {
            collectiblesSpawn.SpawnCollectible();
            lastCollectibleSpawnTime = Time.time;
        }
    }

    float GetStateCooldown()
    {
        return state switch
        {
            DirectorState.Relax => relaxCooldown,
            DirectorState.BuildUp => buildUpCooldown,
            DirectorState.Peak => peakCooldown,
            DirectorState.Cooldown => cooldownCooldown,
            _ => 20f
        };
    }

    #endregion

    #region PIGEONS

    int GetMaxPigeons() => gameDifficulty switch
    {
        GameDifficulty.Easy => maxPigeonsEasy,
        GameDifficulty.Medium => maxPigeonsMedium,
        GameDifficulty.Hard => maxPigeonsHard,
        _ => maxPigeonsMedium
    };

    int GetTargetPigeons()
    {
        return state switch
        {
            DirectorState.Relax => Mathf.FloorToInt(GetMaxPigeons() * 0.4f),     // 40%
            DirectorState.BuildUp => Mathf.FloorToInt(GetMaxPigeons() * 0.6f),  // 60%
            DirectorState.Peak => GetMaxPigeons(),                               // 100%
            DirectorState.Cooldown => Mathf.FloorToInt(GetMaxPigeons() * 0.5f), // 50%
            _ => GetMaxPigeons() / 2
        };
    }

    void HandlePigeonPopulation()
    {
        int targetAmount = GetTargetPigeons();
        int currentAmount = PigeonsAmountInScene;

        //Debug.Log($"Estado: {state}, Alvo: {targetAmount}, Atual: {currentAmount}");

        // SPAWN se abaixo do alvo
        while (currentAmount < targetAmount && pigeonsSpawn.CanSpawnPigeon())
        {
            pigeonsSpawn.SpawnPigeon();
            currentAmount++;
        }

        // DESPAWN se acima do alvo (mantém no mínimo 1-2 pombos)
        while (currentAmount > targetAmount + 1)
        {
            pigeonsSpawn.DespawnRandomPigeon();
            currentAmount--;
        }
    }
    #endregion
}
