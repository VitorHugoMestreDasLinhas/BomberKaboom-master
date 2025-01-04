using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoMonstroDevorador : MonoBehaviour
{
    public int vidas = 3; // Monstro deve ser acertado tr�s vezes antes de morrer
    public float velocidade = 4f;
    public float raioDeteccao = 5f; // Raio de detec��o para perseguir o jogador
    public Transform jogador; // Refer�ncia ao jogador
    public GameObject monstroCocoPrefab; // Prefab do Monstro Coco
    public Transform pontoSpawn; // Ponto onde o Monstro Coco ser� gerado

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
        Debug.Log("Monstro Devorador inicializado com " + vidas + " vidas.");

        // Inicia o spawning do Monstro Coco a cada 40 segundos
        InvokeRepeating("SpawnMonstroCoco", 40f, 40f);
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
        tempoAtualTroca += Time.fixedDeltaTime;
        if (tempoAtualTroca >= tempoTrocaDirecao)
        {
            MudarDirecao(); // Muda a dire��o aleatoriamente a cada 1 segundo
            tempoAtualTroca = 0f;
        }
    }

    private void PerseguirJogador()
    {
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
            vidas--;
            Debug.Log("Monstro Devorador foi atingido! Vidas restantes: " + vidas);

            if (vidas <= 0)
            {
                DeathSequence();
            }
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

        Debug.Log("Monstro Devorador morreu!");
        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
    }

    // Fun��o para gerar o Monstro Coco
    private void SpawnMonstroCoco()
    {
        if (monstroCocoPrefab != null && pontoSpawn != null)
        {
            Instantiate(monstroCocoPrefab, pontoSpawn.position, Quaternion.identity);
            Debug.Log("Monstro Coco gerado!");
        }
    }
}
