using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoConnect : MonoBehaviour
{
    public string gameScene = "GameScene";
    public float timeout = 15f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        // Try to connect as client first
        NetworkManager.Singleton.StartClient();

        float timer = 0f;

        while (!NetworkManager.Singleton.IsConnectedClient &&
               timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!NetworkManager.Singleton.IsConnectedClient)
        {
            // No server found → become host
            NetworkManager.Singleton.Shutdown();
            yield return new WaitForSeconds(0.5f);

            NetworkManager.Singleton.StartHost();
        }

        // Load game scene through network
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager
                .LoadScene(gameScene,
                           LoadSceneMode.Single);
        }
    }
}
