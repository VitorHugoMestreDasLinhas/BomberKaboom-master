using UnityEngine;

public class AnimatedSpriteRenderer : MonoBehaviour
{
    public Sprite[] animationSprites;
    private SpriteRenderer spriteRenderer;
    private int currentFrame;

    public float animationSpeed = 0.25f;
    private float animationTimer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        animationTimer += Time.deltaTime;
        if (animationTimer > animationSpeed)
        {
            animationTimer -= animationSpeed;
            currentFrame = (currentFrame + 1) % animationSprites.Length;
            spriteRenderer.sprite = animationSprites[currentFrame];
        }
    }
}
