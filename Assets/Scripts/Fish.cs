using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Fish : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] Material material;
    [SerializeField] private float speed = 1;
    [SerializeField] private List<GameObject> movePoints = new();
    [SerializeField] private GameObject middlePoint;
    [SerializeField] float amountToNextPoint = 0.25f;

    private Vector3 cameraPosition;
    private Vector3 cameraDirection;
    private Vector3 fragmentPosition;
    private float intensity;

    Vector3 rightVector;
    Vector3 upVector;

    int currentpoint = 0;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        cameraPosition = cam.transform.position;
        cameraDirection = cam.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        upVector = Vector3.Normalize(Vector3.Cross(transform.position - movePoints[currentpoint].transform.position , transform.position - middlePoint.transform.position));

        if ((transform.position - movePoints[currentpoint].transform.position).magnitude <= amountToNextPoint)
        {
            currentpoint++;
            if (currentpoint == movePoints.Count) { currentpoint = 0; }
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movePoints[currentpoint].transform.position - transform.position, upVector), 30 * Time.deltaTime);
        transform.Translate(0, Mathf.Sin(Time.time) * upVector.y * 0.001f, 0);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        

        Vector3 fragmentDirection = fragmentPosition - cameraPosition;
        intensity = Vector3.Dot(cameraDirection, fragmentDirection);

        material.SetFloat("Base Color", intensity);
        
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
