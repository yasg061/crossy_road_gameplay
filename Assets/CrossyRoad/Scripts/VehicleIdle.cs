using UnityEngine;

public class VehicleIdle : MonoBehaviour
{
    Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float pulse = 1f + Mathf.Sin(Time.time * 8f) * 0.03f;
        transform.localScale = startScale * pulse;
    }
}