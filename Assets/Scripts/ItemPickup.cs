using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        ExtraBomba,
        BlastRadius,
        SpeedIncrease,
        ItemPickupFire, // Novo item para apagar o fogo da árvore
    }

    public ItemType type;

    private void OnItemPickup(GameObject player)
    {
        var playerScript = player.GetComponent<NewBehaviourScript>();
        if (playerScript == null)
        {
            Debug.LogError("NewBehaviourScript não encontrado no jogador!");
            return;
        }

        switch (type)
        {
            case ItemType.ExtraBomba:
                playerScript.quantidadeBomba++;
                playerScript.bombasRestantes++;
                break;

            case ItemType.BlastRadius:
                playerScript.explosionRadius++;
                Debug.Log($"Novo raio de explosão: {playerScript.explosionRadius}");
                break;

            case ItemType.SpeedIncrease:
                var joystickMove = player.GetComponent<JoystickMove>();
                if (joystickMove != null)
                {
                    Debug.Log("Ajustar a lógica da velocidade aqui.");
                }
                else
                {
                    Debug.LogWarning("JoystickMove não encontrado no jogador!");
                }
                break;

            case ItemType.ItemPickupFire:
                // Encontrar a primeira árvore que ainda está queimando
                var trees = FindObjectsOfType<TreeBurning>();
                foreach (var tree in trees)
                {
                    if (tree.IsBurning()) // Verifica se a árvore ainda está queimando
                    {
                        tree.ApagarFogo(); // Apaga o fogo da primeira árvore encontrada
                        Debug.Log("Fogo de uma árvore apagado!");
                        break; // Sai do loop após apagar o fogo de uma árvore
                    }
                }
                break;
        }

        Destroy(gameObject); // Destrói o item após ser coletado
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject); // Quando o jogador entra em contato com o item
        }
    }
}
