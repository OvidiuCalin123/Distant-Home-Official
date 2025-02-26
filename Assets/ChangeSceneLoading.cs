using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneLoading : MonoBehaviour
{
    public Animator anim;
    public GameObject spinner;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startLoadingScene2()
    {
        anim.SetBool("goToLoad", true);
    }

    public void makeSpinnerAndTipsVisible()
    {
        spinner.SetActive(true);

        // Start coroutine to wait and then load scene
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Act2");
    }
}
