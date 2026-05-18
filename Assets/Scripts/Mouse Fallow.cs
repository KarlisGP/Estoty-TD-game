using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    void Update()
    {
        // 1. Get the mouse position in pixels
        Vector3 mousePos = Input.mousePosition;

        // 2. Tell Unity how far from the camera the "world point" should be. 
        // For 2D, we just want to translate the screen pixels to world coordinates.
        mousePos.z = 20f; 

        // 3. Convert
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // 4. Force Z to 0 so it stays on the 2D plane with your sprites
        worldPos.z = 0f;

        // 5. Apply
        transform.position = worldPos;
    }
}