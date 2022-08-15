using UnityEngine;

public class SpriteStretch : MonoBehaviour
{
    void Start() => ResizeSpriteToScreen();

    void ResizeSpriteToScreen()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        var width = sr.sprite.bounds.size.x;
        var height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        this.transform.localScale = new Vector3(worldScreenWidth / width,  worldScreenHeight / height, this.transform.localScale.z);
    }
}
