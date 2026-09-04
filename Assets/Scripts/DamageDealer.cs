using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 10;
    public int kr = -1;
    public int iFrames = -1;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (kr == -1)
            {
                PlayerHealth.TakeDamage(damageAmount);
            } else
            {
                PlayerHealth.TakeDamage(damageAmount, kr, iFrames);
            }
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (kr == -1)
            {
                PlayerHealth.TakeDamage(damageAmount);
            }
            else
            {
                PlayerHealth.TakeDamage(damageAmount, kr, iFrames);
            }
        }
    }
}
