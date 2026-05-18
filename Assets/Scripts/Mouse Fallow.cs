using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure it stays on the 2D plane
        transform.position = mousePos;
    }
}