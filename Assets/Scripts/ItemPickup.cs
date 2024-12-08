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
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject);
        }
    }
}