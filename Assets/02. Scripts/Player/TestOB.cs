using UnityEngine;
using Fusion;
public class TestOB : NetworkBehaviour
{
    public override void Spawned()
    {
        Invoke("Die", 10f);
    }

    public void Die()
    {
        GameManager.Instance.NowRunner.Despawn(GetComponent<NetworkObject>());
    }
}
