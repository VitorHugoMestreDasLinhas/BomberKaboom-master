using System.Collections;
using UnityEngine;

public class TreeBurning : MonoBehaviour
{
    // Sprites para os diferentes estágios
    public Sprite initialStage; // Sprite da árvore no estágio inicial (sem fogo)
    public Sprite fireStage1;   // Primeiro estágio da animação de fogo
    public Sprite fireStage2;   // Segundo estágio da animação de fogo

    // Referência ao componente SpriteRenderer
    private SpriteRenderer spriteRenderer;

    // Controle da animação de fogo
    private bool isBurning = true; // Controla se a árvore está queimando ou não

    private void Start()
    {
        // Obtém o SpriteRenderer do objeto
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Certifique-se de que a árvore começa no estágio inicial (sem fogo)
        spriteRenderer.sprite = initialStage;

        // Inicia a animação de fogo automaticamente ao iniciar o jogo
        StartCoroutine(BurningAnimation());
    }

    /// <summary>
    /// Animação de fogo em loop, alternando entre os sprites, se a árvore estiver queimando.
    /// </summary>
    private IEnumerator BurningAnimation()
    {
        while (isBurning) // A animação só continua se a árvore estiver queimando
        {
            // Primeiro quadro: estágio inicial
            spriteRenderer.sprite = initialStage;
            yield return new WaitForSeconds(0.18f); // Ajuste o tempo conforme necessário

            // Segundo quadro: primeiro estágio de fogo
            spriteRenderer.sprite = fireStage1;
            yield return new WaitForSeconds(0.18f);

            // Terceiro quadro: segundo estágio de fogo
            spriteRenderer.sprite = fireStage2;
            yield return new WaitForSeconds(0.18f);

            // Volta ao estágio inicial (último sprite exibido)
            spriteRenderer.sprite = initialStage;
            yield return new WaitForSeconds(0.18f);
        }
    }

    // Método para detectar a colisão com a explosão
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a colisão foi com a explosão
        if (collision.gameObject.CompareTag("Explosion"))
        {
            // Se a árvore for atingida pela explosão, apaga o fogo
            ApagarFogo();
        }
    }

    // Função para apagar o fogo da árvore
    public void ApagarFogo()
    {
        // Para a animação de fogo
        isBurning = false;

        // Define o sprite da árvore como o estágio inicial (sem fogo)
        spriteRenderer.sprite = initialStage;

        // Desabilita qualquer animação ou lógica relacionada ao fogo
        Debug.Log("Fogo apagado!");
    }

    // Método para verificar se a árvore ainda está queimando
    public bool IsBurning()
    {
        return isBurning; // Retorna true se a árvore ainda está queimando
    }
}
