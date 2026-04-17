using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMove : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float timeToLive = 5f;

    public PlayerManager playerManager;

    float t;

    bool isPaused;

    private void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        playerManager.OnPauseGame += (bool value) => isPaused = value;
        playerManager.OnGameOver += () => isPaused = true;
    }
    private void OnDisable()
    {
        playerManager.OnPauseGame -= (bool value) => isPaused = value;
        playerManager.OnGameOver -= () => isPaused = true;
    }

    void Update()
    {
        if (isPaused) return;

        t += Time.deltaTime;

        transform.position += Vector3.down * speed * Time.deltaTime;

        if (t >= timeToLive)
        {
            DesactiveObj();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            DesactiveObj();
        }
    }

    private void OnBecameInvisible()
    {
        DesactiveObj();
    }

    void DesactiveObj()
    {
        t = 0f;

        gameObject.SetActive(false);
    }
}
