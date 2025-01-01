using System.Collections;
using UnityEngine;

public class TreeBurning : MonoBehaviour
{
    // Sprites para os diferentes estágios
    public Sprite initialStage; // Sprite da árvore no estágio inicial
    public Sprite fireStage1;   // Primeiro estágio da animação de fogo
    public Sprite fireStage2;   // Segundo estágio da animação de fogo

    // Referência ao componente SpriteRenderer
    private SpriteRenderer spriteRenderer;

    // Intervalo de tempo entre os quadros da animação
    private float animationSpeed = 1f / 6f;

    private void Start()
    {
        // Obtém o SpriteRenderer do objeto
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Certifique-se de que o sprite inicial seja exibido na cena
        spriteRenderer.sprite = initialStage;

        // Inicia a animação de fogo automaticamente ao iniciar o jogo
        StartCoroutine(BurningAnimation());
    }

    /// <summary>
    /// Animação de fogo em loop, alternando entre os sprites.
    /// </summary>
    private IEnumerator BurningAnimation()
    {
        while (true)
        {
            // Primeiro quadro: estágio inicial
            spriteRenderer.sprite = initialStage;
            yield return new WaitForSeconds(animationSpeed);

            // Segundo quadro: primeiro estágio de fogo
            spriteRenderer.sprite = fireStage1;
            yield return new WaitForSeconds(animationSpeed);

            // Terceiro quadro: segundo estágio de fogo
            spriteRenderer.sprite = fireStage2;
            yield return new WaitForSeconds(animationSpeed);

            // Volta ao estágio inicial (último sprite exibido)
            spriteRenderer.sprite = initialStage;
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
