using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoMonstroCaveira : MonoBehaviour
{
    public float velocidade = 4f;
    public float raioDeteccao = 5f; // Raio de detec��o para perseguir o jogador
    public Transform jogador; // Refer�ncia ao jogador
    public float distanciaRaycast = 1f; // Dist�ncia do raycast para verificar obst�culos

    private Rigidbody2D rb;
    private Vector2 direcao = Vector2.down;

    public AnimatedSpriteRender spriteRendererUp;
    public AnimatedSpriteRender spriteRendererDown;
    public AnimatedSpriteRender spriteRendererLeft;
    public AnimatedSpriteRender spriteRendererRight;
    public AnimatedSpriteRender spriteRendererDeath;
    private AnimatedSpriteRender activespriteRenderer;

    private float tempoTrocaDirecao = 1f; // Troca de dire��o a cada 1 segundo
    private float tempoAtualTroca = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        activespriteRenderer = spriteRendererDown;
        Debug.Log("Monstro Caveira inicializado");
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

        // Verifica se a dire��o escolhida est� bloqueada por um obst�culo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direcao, distanciaRaycast);

        if (hit.collider != null) // Se houver um obst�culo na frente
        {
            // Tentamos mudar para uma dire��o alternativa
            TentarDirecaoAlternativa(direcao);
        }
    }

    private void TentarDirecaoAlternativa(Vector2 direcaoOriginal)
    {
        // Tentamos mover para a esquerda, direita, para cima ou para baixo, em uma ordem fixa, para evitar que o monstro mude de dire��o aleatoriamente.
        Vector2[] direcoesAlternativas = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        foreach (var novaDirecao in direcoesAlternativas)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, novaDirecao, distanciaRaycast);
            if (hit.collider == null) // Se o caminho estiver livre nessa dire��o
            {
                direcao = novaDirecao; // Muda para essa dire��o
                break;
            }
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colidiu com: " + other.gameObject.name);

        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
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

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
    }
}
