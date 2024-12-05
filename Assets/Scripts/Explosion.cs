using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;

    private void Awake()
{
    Debug.Log("Explosions Awake() chamado");
    Debug.Log("Start: " + (start != null ? start.name : "null"));
    Debug.Log("Middle: " + (middle != null ? middle.name : "null"));
    Debug.Log("End: " + (end != null ? end.name : "null"));
}


    public void SetActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        Debug.Log("SetActiveRenderer chamado com: " + renderer);
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    public void SetDirection(Vector2 direction)
    {
        Debug.Log("SetDirection chamado com: " + direction);
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
