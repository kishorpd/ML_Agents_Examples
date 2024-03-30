using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 30;
    public int zSize = 30;
    public float height = 1;
    public float heightIncremented = 1;
    public float repeatingDelay = 0.3f;

    // Use this for initialization
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        InvokeRepeating("CreateShape", repeatingDelay, repeatingDelay);
    }

    void Update () 
    {
        UpdateMesh();
    }


    /// <summary>
    /// Create shape, in this case a grid
    /// </summary>
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        heightIncremented = height;
        int stairWidth = 0;
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            stairWidth= (z % 2 == 0) ? z : z - 1;
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, heightIncremented, stairWidth);
                i++;
            }
            if(z % 2 == 0)
            {
                //stairWidth++;
                heightIncremented++;
            }
        }



        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

               // yield return new WaitForSeconds(.1f);
            }
            vert++;
        }


    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
