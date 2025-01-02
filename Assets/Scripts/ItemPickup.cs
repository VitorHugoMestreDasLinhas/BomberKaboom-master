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
        ItemPickupFire, // Novo item para apagar o fogo da �rvore
    }

    public ItemType type;

    private void OnItemPickup(GameObject player)
    {
        var playerScript = player.GetComponent<NewBehaviourScript>();
        if (playerScript == null)
        {
            Debug.LogError("NewBehaviourScript n�o encontrado no jogador!");
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
                Debug.Log($"Novo raio de explos�o: {playerScript.explosionRadius}");
                break;

            case ItemType.SpeedIncrease:
                var joystickMove = player.GetComponent<JoystickMove>();
                if (joystickMove != null)
                {
                    Debug.Log("Ajustar a l�gica da velocidade aqui.");
                }
                else
                {
                    Debug.LogWarning("JoystickMove n�o encontrado no jogador!");
                }
                break;

            case ItemType.ItemPickupFire:
                // Encontrar a primeira �rvore que ainda est� queimando
                var trees = FindObjectsOfType<TreeBurning>();
                foreach (var tree in trees)
                {
                    if (tree.IsBurning()) // Verifica se a �rvore ainda est� queimando
                    {
                        tree.ApagarFogo(); // Apaga o fogo da primeira �rvore encontrada
                        Debug.Log("Fogo de uma �rvore apagado!");
                        break; // Sai do loop ap�s apagar o fogo de uma �rvore
                    }
                }
                break;
        }

        Destroy(gameObject); // Destr�i o item ap�s ser coletado
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject); // Quando o jogador entra em contato com o item
        }
    }
}
