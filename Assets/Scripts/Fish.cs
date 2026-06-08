using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] Material material;
    private Vector3 cameraPosition;
    private Vector3 cameraDirection;
    private Vector3 fragmentPosition;
    private float intensity;

    Vector3 rightVector;
    Vector3 upVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        cameraPosition = cam.transform.position;
        cameraDirection = cam.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        rightVector = Vector3.Normalize(Vector3.Cross(Vector3.up, transform.forward));
        upVector = Vector3.Normalize(Vector3.Cross(transform.forward, rightVector));
        
        Vector3 fragmentDirection = fragmentPosition - cameraPosition;
        intensity = Vector3.Dot(cameraDirection, fragmentDirection);

        //material.SetFloat(name, intensity);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + rightVector * 3f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + upVector * 3f);
    }
}
