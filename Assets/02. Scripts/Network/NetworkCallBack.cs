using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using static Unity.Collections.Unicode;

public class NetworkCallBack : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkObjectManager m_network_object_manager;
    public GameObject m_player_prefab;
    public JoyStickCtrl m_joy_stick_ctrl;

    public List<PlayerRef> PlayerRefs = new();

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData input_data = new NetworkInputData();

        if(m_joy_stick_ctrl != null)
        {
            input_data.MoveDirection = m_joy_stick_ctrl.GetInputVector();
        }
        input.Set(input_data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        m_network_object_manager = FindFirstObjectByType<NetworkObjectManager>();

        PlayerRefs.Add(player);
        if (player == runner.LocalPlayer)
        {
            runner.Spawn(m_player_prefab, Vector3.zero, Quaternion.identity, player);

            m_joy_stick_ctrl = GameObject.Find("TouchPanel").GetComponent<JoyStickCtrl>();
        }

        GameManager.Instance.StartCoroutine(GameManager.Instance.InitPlayers());
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
