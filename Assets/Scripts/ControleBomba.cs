using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Bomba")]
    public GameObject bombPrefab;
    public int quantidadeBomba = 1;
    public int bombasRestantes;
    public Button botaoBomba;
    public float tempoParaExplosao = 2f;
    public LayerMask explosionLayerMask;

    [Header("Explosão")]
    public Explosion explosionPrefab;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;
    internal bool hasFireExtinguisher;

    private void Start()
    {
        if (botaoBomba != null)
        {
            botaoBomba.onClick.AddListener(DropBomb);
        }
        else
        {
            Debug.LogWarning("botaoBomba não foi atribuído no Inspector.");
        }
    }

    private void OnEnable()
    {
        bombasRestantes = quantidadeBomba;
    }

    public void DropBomb()
    {
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
        float cellSize = 1f; // Tamanho da célula do grid
        posicao.x = Mathf.Round(posicao.x / cellSize) * cellSize;
        posicao.y = Mathf.Round(posicao.y / cellSize) * cellSize;

        if (bombPrefab != null)
        {
            GameObject bomba = Instantiate(bombPrefab, posicao, Quaternion.identity);
            bombasRestantes--;
            yield return new WaitForSeconds(tempoParaExplosao);

            Debug.Log("Instanciando explosão na posição: " + posicao);

            if (explosionPrefab != null)
            {
                Explosion explosion = Instantiate(explosionPrefab, posicao, Quaternion.identity);
                explosion.SetActiveRenderer(explosion.start);
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

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }

        if (explosionPrefab != null)
        {
            Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
            explosion.SetDirection(direction);
            Destroy(explosion.gameObject, explosionDuration);

            Explode(position, direction, length - 1);
        }
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        // Incrementa a quantidade de bombas restantes
        bombasRestantes++;

        // Incrementa a quantidade máxima de bombas
        quantidadeBomba++;

        // Opcional: Atualize o botão se necessário (exemplo, texto ou estado)
        if (botaoBomba != null)
        {
            botaoBomba.interactable = bombasRestantes > 0; // Habilitar ou desabilitar o botão com base na quantidade de bombas
        }

        Debug.Log("Bombas adicionadas. Total: " + bombasRestantes);
    }
}