using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float destructionTime = 1f;
    [Range(0f, 1f)]
    public float itemSpawnChance = 0.2f;
    public GameObject[] spawnableItemns;

    private void Start()
    {
        Destroy(gameObject, destructionTime);
    }
    private void OnDestroy()
    {
        if (spawnableItemns.Length > 0 && Random.value < itemSpawnChance)
        {
            int randonIndex = Random.Range(0, spawnableItemns.Length);
            Instantiate(spawnableItemns[randonIndex], transform.position, Quaternion.identity);
        }
    }
}