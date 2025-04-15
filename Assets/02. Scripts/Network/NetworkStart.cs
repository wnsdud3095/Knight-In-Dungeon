using UnityEngine;
using Fusion;

public class NetworkStart : MonoBehaviour
{
    public NetworkRunner runner_prefab;
    private async void Start()
    {
        var runner = Instantiate(runner_prefab);
        runner.ProvideInput = true;

        SceneRef scene_ref = SceneRef.FromIndex(2);

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "TestRoom",
            Scene = scene_ref,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        }); ;
        

    }

}
