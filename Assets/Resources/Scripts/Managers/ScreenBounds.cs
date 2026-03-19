using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    float screenLeft;
    float screenRight;
    float screenBottom;
    public float screenTop { get; set; }

    void Start()
    {
        Camera cam = Camera.main;
        screenLeft = cam.ScreenToWorldPoint(Vector3.zero).x;
        screenRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        screenBottom = cam.ScreenToWorldPoint(Vector3.zero).y;
        screenTop = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

        CreateCollider("Left", new Vector2(screenLeft, 0), new Vector2(0.1f, cam.orthographicSize * 2));
        CreateCollider("Right", new Vector2(screenRight, 0), new Vector2(0.1f, cam.orthographicSize * 2));
        CreateCollider("Top", new Vector2(0, screenTop), new Vector2(cam.aspect * cam.orthographicSize * 2, 0.1f));        
        CreateCollider("Bottom", new Vector2(0, screenBottom), new Vector2(cam.aspect * cam.orthographicSize * 2, 0.1f));        
    }

    void CreateCollider(string name, Vector2 position, Vector2 size)
    {
        GameObject colObj = new GameObject(name);
        colObj.transform.position = position;
        colObj.transform.parent = transform;
        colObj.tag = "ScreenBounds";
        BoxCollider2D collider = colObj.AddComponent<BoxCollider2D>();
        collider.size = size;
    }
}
