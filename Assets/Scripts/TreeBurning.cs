using System.Collections;
using UnityEngine;

public class TreeBurning : MonoBehaviour
{
    // Sprites para os diferentes est�gios
    public Sprite initialStage; // Sprite da �rvore no est�gio inicial
    public Sprite fireStage1;   // Primeiro est�gio da anima��o de fogo
    public Sprite fireStage2;   // Segundo est�gio da anima��o de fogo

    // Refer�ncia ao componente SpriteRenderer
    private SpriteRenderer spriteRenderer;

    // Intervalo de tempo entre os quadros da anima��o
    private float animationSpeed = 1f / 6f;

    private void Start()
    {
        // Obt�m o SpriteRenderer do objeto
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Certifique-se de que o sprite inicial seja exibido na cena
        spriteRenderer.sprite = initialStage;

        // Inicia a anima��o de fogo automaticamente ao iniciar o jogo
        StartCoroutine(BurningAnimation());
    }

    /// <summary>
    /// Anima��o de fogo em loop, alternando entre os sprites.
    /// </summary>
    private IEnumerator BurningAnimation()
    {
        while (true)
        {
            // Primeiro quadro: est�gio inicial
            spriteRenderer.sprite = initialStage;
            yield return new WaitForSeconds(animationSpeed);

            // Segundo quadro: primeiro est�gio de fogo
            spriteRenderer.sprite = fireStage1;
            yield return new WaitForSeconds(animationSpeed);

            // Terceiro quadro: segundo est�gio de fogo
            spriteRenderer.sprite = fireStage2;
            yield return new WaitForSeconds(animationSpeed);

            // Volta ao est�gio inicial (�ltimo sprite exibido)
            spriteRenderer.sprite = initialStage;
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
