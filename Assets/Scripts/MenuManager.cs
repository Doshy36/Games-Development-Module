using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject[] levels;
    public string[] levelNames;
    private int level;

    public AudioSource source;

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    public void SetLevel(int i)
    {
        levels[level].SetActive(false);
        levels[i].SetActive(true);

        this.level = i;
    }

    IEnumerator StartGameCoroutine()
    {
        source.Play();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelNames[level], LoadSceneMode.Single);
    }

}
