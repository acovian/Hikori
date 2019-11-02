using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelExit : Interactable
{
    [SerializeField] float LevelLoadDelay = 2f;
    public Image black;
    public Animator myAnimator;

    public override void Interact()
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {

        myAnimator.SetBool("Fade", true);
        //yield return new WaitForSecondsRealtime(LevelLoadDelay);
        yield return new WaitUntil(() => black.color.a == 1);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        FindObjectOfType<GameSession>().ResetHealth();
    }
}
