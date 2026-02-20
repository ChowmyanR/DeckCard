using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class StartGame : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "GameScene";
    public float connectionTimeout = 3f;

    [Header("UI")]
    public TMP_Text statusText;

    public void StartAutoConnect()                      // STARTS THE AUTO CONNECT COROUTINE
    {
        StartCoroutine(AutoConnect());
    }

    public IEnumerator AutoConnect()                                // AUTO CONNECT:
                                                                        // TRIES AS A CLIENT FIRST
    {                                                                   // IF FAILED, BECOMES HOST
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager missing!");
            yield break;
        }

        statusText.text = "Connecting as Client...";

        NetworkManager.Singleton.StartClient();

        float timer = 0f;

        while (!NetworkManager.Singleton.IsConnectedClient &&
               timer < connectionTimeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // ✅ Connected as client
        if (NetworkManager.Singleton.IsConnectedClient)
        {
            statusText.text = "Connected!";
            yield break; // Host will load scene
        }

        // No server found → become host
        statusText.text = "No server found. Starting Host...";

        if (NetworkManager.Singleton.IsClient)
            NetworkManager.Singleton.Shutdown();

        yield return new WaitForSeconds(0.5f);

        NetworkManager.Singleton.StartHost();

        statusText.text = "Hosting game...";

        yield return new WaitForSeconds(1f);

        //  Only Host loads scene
        NetworkManager.Singleton.SceneManager.LoadScene(
            gameSceneName,
            LoadSceneMode.Single);
    }
}
