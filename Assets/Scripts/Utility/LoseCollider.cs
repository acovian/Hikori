using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessHit();

    }

    private void ProcessHit()
    {
        StartCoroutine(Wait());
    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        Die();
    }

    private void Die()
    {
        Destroy(gameObject);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
