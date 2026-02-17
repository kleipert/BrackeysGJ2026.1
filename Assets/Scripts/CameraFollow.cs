using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float xOffset = 0f;
    [SerializeField] float smoothTime = 0.15f;

    float vx;

    void LateUpdate()
    {
        float desiredX = target.position.x + xOffset;
        float x = Mathf.SmoothDamp(transform.position.x, desiredX, ref vx, smoothTime);

        var p = transform.position;
        p.x = x;
        transform.position = p;
    }
}