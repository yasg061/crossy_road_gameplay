using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int xMax = 15;


    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x > xMax)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle player collision, e.g., reset player position or end game
            Debug.Log("Player hit by car!");
            Time.timeScale = 0f; // Pause the game
        }
    }


}
