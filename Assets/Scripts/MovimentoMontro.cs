using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoMonstro : MonoBehaviour
{
    public float velocidade;
    private Rigidbody2D rb;
    private Vector2 direcao = Vector2.down;

    public AnimatedSpriteRender spriteRendererUp;
    public AnimatedSpriteRender spriteRendererDown;
    public AnimatedSpriteRender spriteRendererLeft;
    public AnimatedSpriteRender spriteRendererRight;
    public AnimatedSpriteRender spriteRendererDeath;
    private AnimatedSpriteRender activespriteRenderer;

    private float tempoTrocaDirecao = 1f; // Troca de dire��o a cada 2 segundos
    private float tempoAtualTroca = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        activespriteRenderer = spriteRendererDown;
        Debug.Log("Monstro inicializado");
    }

    private void FixedUpdate()
    {
        Mover();

        // Incrementa o tempo desde a �ltima troca de dire��o
        tempoAtualTroca += Time.fixedDeltaTime;
        if (tempoAtualTroca >= tempoTrocaDirecao)
        {
            MudarDirecao(); // Muda a dire��o aleatoriamente a cada 2 segundos
            tempoAtualTroca = 0f;
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

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstaculo"))
        {
            Debug.Log("Colis�o com obst�culo detectada. Mudando dire��o!");
            MudarDirecao();
        }

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
