using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMeshPointGenerate
{

    /// <summary>
    /// Mesh Array which used to generate random points 
    /// on the each polygons of the mesh
    /// </summary>
    GameObject[] MeshObjects { get; set; }
    /// <summary>
    /// Maximum random points per polygon
    /// </summary>
    int PointsPerTriangle { get; set; }
    /// <summary>
    /// Minimum offset in current offset direction
    /// </summary>
    float PointMinOffset { get; set; }
    /// <summary>
    /// Maximum offset in current offset direction
    /// </summary>
    float PointMaxOffset { get; set; }
    /// <summary>
    /// Mesh offset direction for the generating points
    /// </summary>
    MeshPoints.MeshOffsetDirection MeshesOffsetDirection { get; set; }

    /// <summary>
    /// MeshPoints class array for each mesh
    /// </summary>
    [HideInInspector]
    MeshPoints[] MeshPointSet { get; set; }

    /// <summary>
    /// Random points of all the meshes
    /// </summary>
    List<Vector3> RandomMeshPointList { get; set; }


    /// <summary>
    /// Generate Random points on the Triangles in all the seelected meshes
    /// </summary>
    void GenerateMeshPoints();

}
