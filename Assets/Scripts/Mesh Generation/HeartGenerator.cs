using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class HeartGenerator : MonoBehaviour
{
    MeshBuilder builder;
    [Tooltip("Front = negarive numbers")]
    [SerializeField] private Vector3 offsetSmallHeart;
    [SerializeField] private Vector3 offsetBigHeart;

    private Vector3[] smallHeart =
    {
        new Vector3(0,4,0), //top
        new Vector3(0, 0, 0), //bottom
        new Vector3(3, 3, 0),
        new Vector3(3,4,0),
        new Vector3(2,5,0),
        new Vector3(1,5,0)

    };

    private Vector3[] bigHeart =
    {
        new Vector3(0,8,0),
        new Vector3(0,0,0),
        new Vector3(6,6,0),
        new Vector3(6,8,0),
        new Vector3(4,10,0),
        new Vector3(2,10,0),
    };

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
        List<int> frontHeart = CreateSmallHeart(offsetSmallHeart, false);
        List<int> backHeart = CreateSmallHeart(new Vector3(offsetSmallHeart.x, offsetSmallHeart.y, -offsetSmallHeart.z), true);
        List<int> frontBigHeart = CreateBigHeart(offsetBigHeart);
        List<int> backBigHeart = CreateBigHeart(new Vector3(offsetBigHeart.x, offsetBigHeart.y, -offsetBigHeart.z));
        CreateSides(frontHeart, frontBigHeart);
        CreateSides(backBigHeart, backHeart);
        CreateSides(frontBigHeart, backBigHeart);
    }

    private void CreateTriangles(List<int> vertices, bool reverse)
    {
        for (int i = 1; i < vertices.Count - 1; i++)
        {
            if (i == vertices.Count / 2) { continue; }
            if (!reverse)
            {
                builder.AddTriangle(vertices[i], vertices[0], vertices[i + 1]);
            }
            else
            {
                builder.AddTriangle(vertices[i], vertices[i + 1], vertices[0]);
            }
        }
        if (!reverse)
        {
            builder.AddTriangle(vertices[0], vertices[1], vertices[vertices.Count - 1]);
        }
        else
        {
            builder.AddTriangle(vertices[0], vertices[vertices.Count - 1], vertices[1]);
        }
    }
    private List<int> CreateSmallHeart(Vector3 offset, bool reverse)
    {
        List<int> vertices = new List<int>();

        for (int i = 0; i < smallHeart.Length; i++)
        {
            vertices.Add(builder.AddVertex(offset + smallHeart[i]));
        }
        //left side
        for (int i = smallHeart.Length - 1; i >= 2; i--)
        {
            vertices.Add(builder.AddVertex(offset + new Vector3(-smallHeart[i].x, smallHeart[i].y, smallHeart[i].z)));
        }

        //triangles
        CreateTriangles(vertices, reverse);
        return vertices;
    }

    private List<int> CreateBigHeart(Vector3 offset)
    {
        List<int> vertices = new List<int>();

        //right
        for(int i = 0; i < bigHeart.Length; i++)
        {
            vertices.Add(builder.AddVertex(offset + bigHeart[i]));
        }
        //left
        for(int i = bigHeart.Length - 1; i >= 2; i--)
        {
            vertices.Add(builder.AddVertex(offset + new Vector3(-bigHeart[i].x, bigHeart[i].y, bigHeart[i].z)));
        }

        return vertices;
    }

    private void CreateSides(List<int> frontHeart, List<int> backHeart)
    {
        for (int i = 1; i < frontHeart.Count - 1; i++)
        {
            if(i == frontHeart.Count / 2) { continue; }
            builder.AddTriangle(frontHeart[i], backHeart[i + 1], backHeart[i]);
            builder.AddTriangle(frontHeart[i], frontHeart[i + 1], backHeart[i + 1]);
        }
        builder.AddTriangle(frontHeart[0], backHeart[backHeart.Count / 2], frontHeart[frontHeart.Count / 2]);
        builder.AddTriangle(frontHeart[0], backHeart[0], backHeart[backHeart.Count / 2]);
        builder.AddTriangle(frontHeart[0], frontHeart[frontHeart.Count / 2 + 1], backHeart[backHeart.Count / 2 + 1]);
        builder.AddTriangle(frontHeart[0], backHeart[backHeart.Count / 2] + 1, backHeart[0]);
        builder.AddTriangle(frontHeart[1], backHeart[backHeart.Count - 1], frontHeart[frontHeart.Count - 1] );
        builder.AddTriangle(frontHeart[1], backHeart[1], backHeart[backHeart.Count - 1]);

    }
}
