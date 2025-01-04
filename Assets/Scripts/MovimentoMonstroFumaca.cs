using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoMonstroFumaca : MonoBehaviour
{
    public float velocidade = 4f;
    public float raioDeteccao = 5f; // Raio de detec��o para perseguir o jogador
    public Transform jogador; // Refer�ncia ao jogador

    private Rigidbody2D rb;
    private Vector2 direcao = Vector2.down;

    public AnimatedSpriteRender spriteRendererUp;
    public AnimatedSpriteRender spriteRendererDown;
    public AnimatedSpriteRender spriteRendererLeft;
    public AnimatedSpriteRender spriteRendererRight;
    public AnimatedSpriteRender spriteRendererDeath;
    public AnimatedSpriteRender spriteRendererInvulneravel; // Sprite para invulnerabilidade
    private AnimatedSpriteRender activespriteRenderer;

    private float tempoTrocaDirecao = 1f; // Troca de dire��o a cada 1 segundo
    private float tempoAtualTroca = 0f;

    private bool isImortal = false; // Indica se o monstro est� imortal

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        activespriteRenderer = spriteRendererDown;
        Debug.Log("Monstro Fuma�a inicializado");

        StartCoroutine(AlternarImortalidade()); // Inicia a altern�ncia de imortalidade
    }

    private void FixedUpdate()
    {
        // Verifica se o jogador est� dentro do raio de detec��o
        if (jogador != null && Vector2.Distance(transform.position, jogador.position) <= raioDeteccao)
        {
            PerseguirJogador();
        }
        else
        {
            MovimentoAleatorio();
        }

        Mover();
    }

    private void MovimentoAleatorio()
    {
        // Incrementa o tempo desde a �ltima troca de dire��o
        tempoAtualTroca += Time.fixedDeltaTime;
        if (tempoAtualTroca >= tempoTrocaDirecao)
        {
            MudarDirecao(); // Muda a dire��o aleatoriamente a cada 1 segundo
            tempoAtualTroca = 0f;
        }
    }

    private void PerseguirJogador()
    {
        // Define a dire��o para perseguir o jogador
        direcao = (jogador.position - transform.position).normalized;
    }

    private void Mover()
    {
        Vector2 posicao = rb.position;
        Vector2 resultado = velocidade * direcao * Time.fixedDeltaTime;
        rb.MovePosition(posicao + resultado);

        Atualiza();
    }

    private void MudarDirecao()
    {
        // Escolhe uma dire��o aleat�ria
        int escolha = Random.Range(0, 4);
        switch (escolha)
        {
            case 0: direcao = Vector2.up; break;
            case 1: direcao = Vector2.down; break;
            case 2: direcao = Vector2.left; break;
            case 3: direcao = Vector2.right; break;
        }

        Debug.Log("Nova dire��o escolhida: " + direcao);
    }

    private void Atualiza()
    {
        // Atualiza os sprites com base na dire��o e estado de imortalidade
        if (isImortal)
        {
            AtivarSpriteInvulneravel();
        }
        else
        {
            float valorX = direcao.x, valorY = direcao.y;

            if (valorX < 0)
                setDirecao(Vector2.left, spriteRendererLeft);
            else if (valorX > 0)
                setDirecao(Vector2.right, spriteRendererRight);
            else if (valorY < 0)
                setDirecao(Vector2.down, spriteRendererDown);
            else if (valorY > 0)
                setDirecao(Vector2.up, spriteRendererUp);
        }
    }

    public void setDirecao(Vector2 novaDirecao, AnimatedSpriteRender spriteRenderer)
    {
        direcao = novaDirecao;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activespriteRenderer = spriteRenderer;
        activespriteRenderer.idle = direcao == Vector2.zero;
    }

    private IEnumerator AlternarImortalidade()
    {
        while (true)
        {
            // Estado imortal
            isImortal = true;
            AtivarSpriteInvulneravel();
            Debug.Log("Monstro Fuma�a est� imortal!");
            yield return new WaitForSeconds(5f); // 5 segundos imortal

            // Estado vulner�vel
            isImortal = false;
            DesativarSpriteInvulneravel();
            Debug.Log("Monstro Fuma�a n�o est� mais imortal!");
            yield return new WaitForSeconds(2f); // 2 segundos normal
        }
    }

    private void AtivarSpriteInvulneravel()
    {
        // Ativa a anima��o de invulnerabilidade
        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererInvulneravel.enabled = true;
    }

    private void DesativarSpriteInvulneravel()
    {
        // Retorna para os sprites normais
        spriteRendererInvulneravel.enabled = false;
        Atualiza(); // Atualiza com base na dire��o
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colidiu com: " + other.gameObject.name);

        if (!isImortal && other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Debug.Log("Colis�o com explos�o detectada!");
            DeathSequence();
        }
    }

    public void DeathSequence()
    {
        enabled = false;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;
        spriteRendererInvulneravel.enabled = false;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
    }
}
