using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class HeartGenerator : MonoBehaviour
{
    MeshBuilder builder;
    [SerializeField] private Vector3 offset;

    private Vector3[] smallHeart =
    {
        new Vector3(0,4,0),
        new Vector3(0, 0, 0),
        new Vector3(3, 3, 0),
        new Vector3(3,4,0),
        new Vector3(2,5,0),
        new Vector3(1,5,0)

    }; 
    private List<int> vertices = new List<int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        builder = new MeshBuilder();
        CreateShape();
        GetComponent<MeshFilter>().mesh = builder.CreateMesh(true);
    }

    void CreateShape()
    {
        builder.Clear();

        //int bottom = builder.AddVertex(offset + new Vector3(0, 0, 0));
        //int rightBottom = builder.AddVertex(offset + new Vector3(3, 3, 0));
        //int rightTop = builder.AddVertex(offset + new Vector3(3,4,0));
        //int topRight = builder.AddVertex(offset + new Vector3(2,5,0));
        //int topLeft = builder.AddVertex(offset + new Vector3(1,5,0));
        //int top = builder.AddVertex(offset + new Vector3(0,4,0));

        //builder.AddTriangle(bottom, top, rightBottom);
        //builder.AddTriangle(rightBottom,top, rightTop);
        //builder.AddTriangle(rightTop,top, topRight);
        //builder.AddTriangle(topRight,top, topLeft);
        //rightSide
        for (int i = 0; i < smallHeart.Length; i++) 
        {
            vertices.Add(builder.AddVertex(offset + smallHeart[i]));
        }
        //left side
        for (int i = smallHeart.Length - 1; i >= 2; i--)
        {
            vertices.Add(builder.AddVertex(offset + new Vector3(-1 * smallHeart[i].x, smallHeart[i].y, smallHeart[i].z)));
        }

        //triangles
        for(int i = 1; i < vertices.Count - 1; i++)
        {
            if (i == vertices.Count / 2) {continue;}
            builder.AddTriangle(vertices[i], vertices[0], vertices[i + 1]);
        }
        builder.AddTriangle(vertices[0], vertices[1], vertices[vertices.Count - 1]);

    }

}
