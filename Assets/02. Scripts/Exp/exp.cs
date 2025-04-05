using UnityEngine;

public class exp : MonoBehaviour
{
    private int expAmount;

    public void SetExpAmount(int amount)
    {
        expAmount = amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerExperience playerExp = collision.GetComponent<PlayerExperience>();

            if (playerExp != null)
            {
                playerExp.GainExperience(expAmount);
            }

            ExpPool.Instance.ReturnExp(gameObject); 
        }
    }
}
