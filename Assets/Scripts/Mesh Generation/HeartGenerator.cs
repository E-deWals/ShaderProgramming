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

    private Vertex[] smallHeart =
    {
        new Vertex (new Vector3(0,4,0), new Vector2(0.5f, 0.7f)), //top
        new Vertex (new Vector3(0,0,0), new Vector2(0.5f, 0.3f)), //bottom
        new Vertex(new Vector3(3,3,0), new Vector2(3/4f, 0.6f)),
        new Vertex(new Vector3(3,4,0), new Vector2(3/4f, 0.7f)),
        new Vertex(new Vector3(2,5,0), new Vector2(2/3f, 0.8f)),
        new Vertex(new Vector3(1,5,0), new Vector2(7/12f, 0.8f))

    };

    private Vertex[] bigHeart =
    {
        new Vertex(new Vector3(0,8,0), new Vector2(0.5f, 0.8f)),
        new Vertex (new Vector3(0,0,0), new Vector2(0.5f, 0)),
        new Vertex(new Vector3(6,6,0), new Vector2(1, 0.6f)),
        new Vertex(new Vector3(6,8,0), new Vector2(1, 0.8f)),
        new Vertex(new Vector3(4,10,0), new Vector2(5/6f, 1)),
        new Vertex(new Vector3(2,10,0), new Vector2(2/3f, 1))
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
            vertices.Add(builder.AddVertex(offset + smallHeart[i].position, smallHeart[i].UV));
        }
        //left side
        for (int i = smallHeart.Length - 1; i >= 2; i--)
        {
            vertices.Add(builder.AddVertex(offset + new Vector3(-smallHeart[i].position.x, smallHeart[i].position.y, smallHeart[i].position.z), new Vector2(1 - smallHeart[i].UV.x, smallHeart[i].UV.y)));
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
            vertices.Add(builder.AddVertex(offset + bigHeart[i].position, bigHeart[i].UV));
        }
        //left
        for(int i = bigHeart.Length - 1; i >= 2; i--)
        {
            vertices.Add(builder.AddVertex(offset + new Vector3(-bigHeart[i].position.x, bigHeart[i].position.y, bigHeart[i].position.z), new Vector2(1 - bigHeart[i].UV.x, bigHeart[i].UV.y)));
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

public class Vertex
{
    public Vector3 position;
    public Vector2 UV;

    public Vertex(Vector3 position, Vector2 UV)
    {
        this.position = position;
        this.UV = UV;
    }
}
