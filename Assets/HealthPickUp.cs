using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public float health;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().health += health;
            Destroy(gameObject);
        }
    }
}
