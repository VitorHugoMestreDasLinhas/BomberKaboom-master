using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{   
    [Header("Bomba")]
    public GameObject bombPrefab;  
    public int quantidadeBomba = 1;
    public int bombasRestantes; 
    public Button botaoBomba; 
    public float tempoParaExplosao = 2f;
    public LayerMask explosionLayerMask;
    [Header("Explosao")]
    public Explosion explosionPrefab;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    private void Start()
    {
        Debug.Log("NewBehaviourScript Start() chamado"); // Adicionado para debug
        botaoBomba.onClick.AddListener(DropBomb);
    }

    private void OnEnable() 
    { 
        bombasRestantes = quantidadeBomba;
    }

    public void DropBomb()
    {
        Debug.Log("DropBomb() chamado"); // Adicionado para debug
        if (bombasRestantes > 0)
        {
            StartCoroutine(PlaceBomb());
        }
        else
        {
            Debug.LogWarning("Não há bombas restantes.");
        }
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 posicao = transform.position; 
        posicao.x = Mathf.Round(posicao.x);
        posicao.y = Mathf.Round(posicao.y);

        if (bombPrefab != null)
        {
            GameObject bomba = Instantiate(bombPrefab, posicao, Quaternion.identity);
            bombasRestantes--;
            yield return new WaitForSeconds(tempoParaExplosao);
            
            posicao = bomba.transform.position;
            posicao.x = Mathf.Round(posicao.x);
            posicao.y = Mathf.Round(posicao.y);

            Debug.Log("Instanciando explosão na posição: " + posicao);

            if (explosionPrefab != null)
            {
                Explosion explosion = Instantiate(explosionPrefab, posicao, Quaternion.identity);
                explosion.SetActiveRenderer(explosion.start);
                explosion.SetDirection(Vector2.up); // Adicionei chamada para teste
                Destroy(explosion.gameObject, explosionDuration);

                Explode(posicao, Vector2.up, explosionRadius);
                Explode(posicao, Vector2.down, explosionRadius);
                Explode(posicao, Vector2.left, explosionRadius);
                Explode(posicao, Vector2.right, explosionRadius);
            }
            else
            {
                Debug.LogError("explosionPrefab é nulo. Certifique-se de que o prefab da explosão está atribuído no Inspector.");
            }

            Destroy(bomba);
            bombasRestantes++;
        }
        else
        {
            Debug.LogError("bombPrefab é nulo. Certifique-se de que o prefab da bomba está atribuído no Inspector.");
        }
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }
        
        position += direction;
       
        if(Physics2D.OverlapBox(position, Vector2.one /2f, 0f, explosionLayerMask))
        {
            return;
        }
            Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
            explosion.SetDirection(direction);
            Destroy(explosion.gameObject, explosionDuration);

            Explode(position, direction, length - 1);
        
    }
}
