using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct MeshPoints
{
    public enum MeshOffsetDirection { X, Y, Z }

    public GameObject meshObject;
    private List<Vector3> meshPointList;

    public List<Vector3> MeshPointList
    {
        get { return meshPointList; }
    }

    public MeshPoints(GameObject meshObject)
    {
        this.meshObject = meshObject;
        this.meshPointList = null;
    }

    public List<Vector3> GenerateRandomPointsOnMesh(int pointsPerTriangle)
    {
        this.meshPointList = new List<Vector3>();

        Mesh mesh = this.meshObject.GetComponent<MeshFilter>().mesh;
        int triangleCount = mesh.triangles.Length / 3;
        int triangleIndex = 0;

        for(int i = 0; i < triangleCount; i++)
        {
            Vector3 trianglePoint1 = this.meshObject.transform.TransformPoint(mesh.vertices[mesh.triangles[triangleIndex + 0]]);
            Vector3 trianglePoint2 = this.meshObject.transform.TransformPoint(mesh.vertices[mesh.triangles[triangleIndex + 1]]);
            Vector3 trianglePoint3 = this.meshObject.transform.TransformPoint(mesh.vertices[mesh.triangles[triangleIndex + 2]]);

            Vector3[] trianglePoints = new Vector3[3] { trianglePoint1, trianglePoint2, trianglePoint3 };

            for(int j = 0; j < pointsPerTriangle; j++)
            {
                var r1 = Mathf.Sqrt(Random.Range(0f, 1f));
                var r2 = Random.Range(0f, 1f);
                var m1 = 1 - r1;
                var m2 = r1 * (1 - r2);
                var m3 = r2 * r1;

                Vector3 result = (m1 * trianglePoints[0]) + (m2 * trianglePoints[1]) + (m3 * trianglePoints[2]);

                this.meshPointList.Add(result);
            }

            triangleIndex += 3;

        }

        return this.meshPointList;

    }

    public List<Vector3> GenerateRandomPointsOnMesh(int pointsPerTriangle, float minPointOffset, float maxPointOffset, MeshOffsetDirection meshOffsetDirection)
    {
        this.meshPointList = new List<Vector3>();

        Mesh mesh = this.meshObject.GetComponent<MeshFilter>().mesh;
        int triangleCount = mesh.triangles.Length / 3;
        int triangleIndex = 0;

        for (int i = 0; i < triangleCount; i++)
        {
            Vector3 trianglePoint1 = this.meshObject.transform.TransformPoint(mesh.vertices[mesh.triangles[triangleIndex + 0]]);
            Vector3 trianglePoint2 = this.meshObject.transform.TransformPoint(mesh.vertices[mesh.triangles[triangleIndex + 1]]);
            Vector3 trianglePoint3 = this.meshObject.transform.TransformPoint(mesh.vertices[mesh.triangles[triangleIndex + 2]]);

            Vector3[] trianglePoints = new Vector3[3] { trianglePoint1, trianglePoint2, trianglePoint3 };

            for (int j = 0; j < pointsPerTriangle; j++)
            {
                var r1 = Mathf.Sqrt(Random.Range(0f, 1f));
                var r2 = Random.Range(0f, 1f);
                var m1 = 1 - r1;
                var m2 = r1 * (1 - r2);
                var m3 = r2 * r1;

                Vector3 result = (m1 * trianglePoints[0]) + (m2 * trianglePoints[1]) + (m3 * trianglePoints[2]);

                float randomOffset = Random.Range(minPointOffset, maxPointOffset);

                switch(meshOffsetDirection)
                {
                    case MeshOffsetDirection.X:
                        result.x = randomOffset;
                        break;

                    case MeshOffsetDirection.Y:
                        result.y = randomOffset;
                        break;

                    case MeshOffsetDirection.Z:
                        result.z = randomOffset;
                        break;
                }


                this.meshPointList.Add(result);
            }

            triangleIndex += 3;

        }

        return this.meshPointList;
    }


    public static List<Vector3> operator+ (MeshPoints a, MeshPoints b)
    {
        if((a.MeshPointList.Count == 0 && b.MeshPointList.Count == 0) || b.MeshPointList.Count == 0)
            return new List<Vector3>();


         List<Vector3> meshPoints = new List<Vector3>();

        foreach(Vector3 pointA in a.MeshPointList)
        {
            meshPoints.Add(pointA);
        }

        foreach (Vector3 pointB in b.MeshPointList)
        {
            meshPoints.Add(pointB);
        }


        return meshPoints;
    }


    
   
}
