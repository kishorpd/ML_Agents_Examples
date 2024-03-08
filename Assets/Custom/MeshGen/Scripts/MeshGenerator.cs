using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
  
    Mesh mesh;
    public List<GameObject> verticeGameObjects = new List<GameObject>();

    List<Vector3> vertices = new List<Vector3>();
    //Vector3[] vertices;
    int[] triangles;

    // Use this for initialization
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }



    void UpdateShape()
    {
        for (int i = 0; i < verticeGameObjects.Count; i++)
        {
            vertices[i] = (verticeGameObjects[i].transform.position);
        }

    }

    private void UpdateVertices()
    {
        int i = 0;
        foreach (var obj in verticeGameObjects)
        {
            mesh.vertices[i] = (obj.transform.position);
        }
    }


    void CreateShape()
    {
        /*
        vertices = new Vector3[]
            {

            new Vector3(0,0,0), //0
            new Vector3(0,0,1), //1
            new Vector3(1,0,0), //2
            new Vector3(1,0,1)  //3

            };
         */

        for(int i =0; i < verticeGameObjects.Count; i++) 
        {
            vertices.Add(verticeGameObjects[i].transform.position);
        }

        triangles = new int[]
        {
            0, 1, 2,
            1, 3, 2, 
            2, 3, 4,
            3, 5, 4,
            6, 0, 7,
            0, 2, 7,
            7, 2, 8,
            2, 4, 8
        };

        mesh.vertices = vertices.ToArray();
    }

    void UpdateMesh()
    { 
        mesh.Clear();
        UpdateShape();

        mesh.vertices = vertices.ToArray();

        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void Update()
    {
        UpdateMesh();
    }

}
