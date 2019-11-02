using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactRange = 2;

    private void Update()
    {
        if (GameManager.instance.player)
        {
            if (Vector2.Distance(gameObject.transform.position, GameManager.instance.player.position) < interactRange)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Interact();
                }
            }

        }
    }

    public virtual void Interact()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
