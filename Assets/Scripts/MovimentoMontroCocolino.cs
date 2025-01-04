using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoMonstroCocolino : MonoBehaviour
{
    public float velocidade = 4f;
    public float raioDeteccao = 5f; // Raio de detecção para perseguir o jogador
    public Transform jogador; // Referência ao jogador
    public GameObject monstroSimplesPrefab; // Prefab do monstro simples a ser spawnado
    public Transform spawnPoint; // Posição onde os monstros simples serão spawnados

    private Rigidbody2D rb;
    private Vector2 direcao = Vector2.down;

    public AnimatedSpriteRender spriteRendererUp;
    public AnimatedSpriteRender spriteRendererDown;
    public AnimatedSpriteRender spriteRendererLeft;
    public AnimatedSpriteRender spriteRendererRight;
    public AnimatedSpriteRender spriteRendererDeath;
    private AnimatedSpriteRender activespriteRenderer;

    private float tempoTrocaDirecao = 1f; // Troca de direção a cada 1 segundo
    private float tempoAtualTroca = 0f;
    private int monstrosCriados = 0; // Contador de monstros spawnados
    private bool podeSpawnar = true; // Controla se ainda pode spawnar monstros

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        activespriteRenderer = spriteRendererDown;
        Debug.Log("Monstro Cocolino inicializado");

        StartCoroutine(SpawnarMonstros());
    }

    private void FixedUpdate()
    {
        // Verifica se o jogador está dentro do raio de detecção
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
        // Incrementa o tempo desde a última troca de direção
        tempoAtualTroca += Time.fixedDeltaTime;
        if (tempoAtualTroca >= tempoTrocaDirecao)
        {
            MudarDirecao(); // Muda a direção aleatoriamente a cada 1 segundo
            tempoAtualTroca = 0f;
        }
    }

    private void PerseguirJogador()
    {
        // Define a direção para perseguir o jogador
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
        // Escolhe uma direção aleatória
        int escolha = Random.Range(0, 4);
        switch (escolha)
        {
            case 0: direcao = Vector2.up; break;
            case 1: direcao = Vector2.down; break;
            case 2: direcao = Vector2.left; break;
            case 3: direcao = Vector2.right; break;
        }

        Debug.Log("Nova direção escolhida: " + direcao);
    }

    private void Atualiza()
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

    private IEnumerator SpawnarMonstros()
    {
        while (monstrosCriados < 3 && podeSpawnar)
        {
            yield return new WaitForSeconds(15f); // Espera 15 segundos para spawnar

            Instantiate(monstroSimplesPrefab, spawnPoint.position, Quaternion.identity);
            monstrosCriados++;
            Debug.Log("Monstro simples spawnado. Total: " + monstrosCriados);

            if (monstrosCriados >= 3)
            {
                podeSpawnar = false;
                Debug.Log("Monstro Cocolino não pode mais spawnar monstros.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colidiu com: " + other.gameObject.name);

        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Debug.Log("Colisão com explosão detectada!");
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

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
    }
}
