using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlipMesh : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }


}
