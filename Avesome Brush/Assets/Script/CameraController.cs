using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;  // Ціль, за якою слідує камера
    public float smoothSpeed = 0.5f;  // Плавність руху камери

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
