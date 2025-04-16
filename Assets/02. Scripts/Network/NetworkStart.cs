using UnityEngine;
using Fusion;

public class NetworkStart : NetworkBehaviour
{
    public NetworkRunner runner_prefab;
    private async void Start()
    {
        var runner = Instantiate(runner_prefab);
        runner.ProvideInput = true;

        var pooledProvider = FindFirstObjectByType<NetworkObjectManager>();

        SceneRef scene_ref = SceneRef.FromIndex(4);
    
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "TestRoom",
            Scene = scene_ref,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            ObjectProvider = pooledProvider
        }); ;
        GameManager.Instance.NowRunner = runner;


    }

}
