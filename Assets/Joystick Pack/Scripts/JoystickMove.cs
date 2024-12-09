using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick mj; 
    public float velocidade;
    private Rigidbody2D rb;
    private Vector2 direcao = Vector2.down;

    public AnimatedSpriteRender spriteRendererUp;
    public AnimatedSpriteRender spriteRendererDown;
    public AnimatedSpriteRender spriteRendererLeft;
    public AnimatedSpriteRender spriteRendererRight;
    public AnimatedSpriteRender spriteRendererDeath;
    private AnimatedSpriteRender activespriteRenderer;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        activespriteRenderer = spriteRendererDown;
        Debug.Log("Script Inicializado");
    }

    public void FixedUpdate()
    {
        if (mj.Direction != Vector2.zero)
        {
            Vector2 posicao = rb.position;
            Vector2 resultado = velocidade * mj.Direction * Time.fixedDeltaTime; // Use a direção do joystick
            rb.MovePosition(posicao + resultado);

            // Atualize a animação e direção enquanto se move
            Atualiza();

            Debug.Log("Movendo: " + resultado);
        }
        else
        {
            rb.velocity = Vector2.zero;

            // Atualize para o estado idle quando parado
            Atualiza();

            Debug.Log("Parado");
        }
    }



    public void Atualiza() // esse método verifica qual distância é maior, com base nisso realiza o movimento 
    {
        float valorX = mj.Direction.x, valorY = mj.Direction.y;
        float moduloX = Mathf.Abs(valorX), moduloY = Mathf.Abs(valorY);

        if (moduloX > moduloY)
        {
            if (valorX < 0)
                setDirecao(Vector2.left, spriteRendererLeft);
            else
                setDirecao(Vector2.right, spriteRendererRight);
        }
        else if (moduloY > moduloX)
        {
            if (valorY < 0)
                setDirecao(Vector2.down, spriteRendererDown);
            else
                setDirecao(Vector2.up, spriteRendererUp);
        }
        else
        {
            setDirecao(Vector2.zero, activespriteRenderer);
        }
        Debug.Log("Direção: " + direcao);
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
        Debug.Log("OnTriggerEnter2D chamado. Colidiu com: " + other.gameObject.name);

        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Debug.Log("Colisão com camada Explosion detectada!");
            DeathSequence();
        }
    }




    public void DeathSequence()
    {
        enabled = false;
        GetComponent<NewBehaviourScript>().enabled = false; 

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
