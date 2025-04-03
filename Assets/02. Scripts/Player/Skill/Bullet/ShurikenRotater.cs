using UnityEngine;

public class ShurikenRotater : MonoBehaviour
{
    public float SpinningSpeed { get; set; }

    void Update()
    {
        if (GameManager.Instance.GameState != GameEventType.Playing) return;

        if (ChildrenCheck())
        {
            transform.Rotate(Vector3.back * SpinningSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private bool ChildrenCheck()
    {
        Animator[] animators = GameManager.Instance.Player.transform.GetComponentsInChildren<Animator>();
        
        return animators != null ? true : false;
    }

}
