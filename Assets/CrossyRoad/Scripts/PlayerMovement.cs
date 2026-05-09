using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float stepSize = 1f;
    [SerializeField] private float rayDistance = 1f;


    private void Move(Vector3 direction)
    {
        if (canMove(direction))
        {
            transform.position += direction * stepSize;
        }
    }

    private bool canMove(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, rayDistance))
        {
            return false; // Obstacle detected, cannot move
        }
        return true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && canMove(Vector3.forward))
        {
            Move(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.S) && canMove(Vector3.back))
        {
            Move(Vector3.back);
        }
        else if (Input.GetKeyDown(KeyCode.A) && canMove(Vector3.left))
        {
            Move(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) && canMove(Vector3.right))
        {
            Move(Vector3.right);
        }
    }
}