using UnityEngine;

public class GhostScript : MonoBehaviour
{
    private SpriteRenderer sr;
    public float fadeSpeed = 3f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 4; 
    }

    void Update()
    {
        Color color = sr.color;
        color.a -= fadeSpeed * Time.deltaTime;
        sr.color = color;

        if (color.a <= 0) Destroy(gameObject);
    }
}
