using System.Collections;
using UnityEngine;

public class TreeBurning : MonoBehaviour
{
    // Sprites para os diferentes est�gios
    public Sprite initialStage; // Sprite da �rvore no est�gio inicial (sem fogo)
    public Sprite fireStage1;   // Primeiro est�gio da anima��o de fogo
    public Sprite fireStage2;   // Segundo est�gio da anima��o de fogo

    // Refer�ncia ao componente SpriteRenderer
    private SpriteRenderer spriteRenderer;

    // Controle da anima��o de fogo
    private bool isBurning = true; // Controla se a �rvore est� queimando ou n�o

    private void Start()
    {
        // Obt�m o SpriteRenderer do objeto
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Certifique-se de que a �rvore come�a no est�gio inicial (sem fogo)
        spriteRenderer.sprite = initialStage;

        // Inicia a anima��o de fogo automaticamente ao iniciar o jogo
        StartCoroutine(BurningAnimation());
    }

    /// <summary>
    /// Anima��o de fogo em loop, alternando entre os sprites, se a �rvore estiver queimando.
    /// </summary>
    private IEnumerator BurningAnimation()
    {
        while (isBurning) // A anima��o s� continua se a �rvore estiver queimando
        {
            // Primeiro quadro: est�gio inicial
            spriteRenderer.sprite = initialStage;
            yield return new WaitForSeconds(0.18f); // Ajuste o tempo conforme necess�rio

            // Segundo quadro: primeiro est�gio de fogo
            spriteRenderer.sprite = fireStage1;
            yield return new WaitForSeconds(0.18f);

            // Terceiro quadro: segundo est�gio de fogo
            spriteRenderer.sprite = fireStage2;
            yield return new WaitForSeconds(0.18f);

            // Volta ao est�gio inicial (�ltimo sprite exibido)
            spriteRenderer.sprite = initialStage;
            yield return new WaitForSeconds(0.18f);
        }
    }

    // M�todo para detectar a colis�o com a explos�o
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a colis�o foi com a explos�o
        if (collision.gameObject.CompareTag("Explosion"))
        {
            // Se a �rvore for atingida pela explos�o, apaga o fogo
            ApagarFogo();
        }
    }

    // Fun��o para apagar o fogo da �rvore
    public void ApagarFogo()
    {
        // Para a anima��o de fogo
        isBurning = false;

        // Define o sprite da �rvore como o est�gio inicial (sem fogo)
        spriteRenderer.sprite = initialStage;

        // Desabilita qualquer anima��o ou l�gica relacionada ao fogo
        Debug.Log("Fogo apagado!");
    }

    // M�todo para verificar se a �rvore ainda est� queimando
    public bool IsBurning()
    {
        return isBurning; // Retorna true se a �rvore ainda est� queimando
    }
}
