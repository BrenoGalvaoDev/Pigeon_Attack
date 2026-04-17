using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeTeste : MonoBehaviour
{
    public float timer = 5f;
    public float speed = 10f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }

        transform.position += transform.right * Time.deltaTime * speed;
    }
}
