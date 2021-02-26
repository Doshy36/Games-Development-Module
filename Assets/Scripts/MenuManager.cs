using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public AudioSource source;

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        source.Play();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }

}
