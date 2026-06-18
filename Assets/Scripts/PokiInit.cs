using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PokiInit : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PokiUnitySDK.Instance.init();
    }

    private IEnumerator Start()
    {
        yield return null; // espera 1 frame
        PokiUnitySDK.Instance.gameLoadingFinished();

        SceneManager.LoadScene(1);
    }
}