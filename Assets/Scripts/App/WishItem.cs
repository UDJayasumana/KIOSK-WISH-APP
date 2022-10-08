using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishItem : MonoBehaviour
{

    [HideInInspector]
    public Vector3 PointA, PointB;

    public bool DestroyAtTheEnd = false;

    private float timeElapsed = 0;
    private float lerpDuration = 3;

    private bool _isUpdateMesh = false;

    private Vector3 targetPosition;
    private Vector3 targetScale;

    private bool canUpdate = false;

    private Vector3 startScale = Vector3.zero;
    private Vector3 endScale = Vector3.zero;

 

    public void Initialize(Vector3 startPosition, Vector3 endPosition, bool destroyAtTheEnd)
    {
        PointA = startPosition;
        PointB = endPosition;
        DestroyAtTheEnd = destroyAtTheEnd;

        float randomStartScale = Random.Range(2, 5);
        startScale = new Vector3(randomStartScale, randomStartScale, randomStartScale);

        endScale = transform.localScale;

        canUpdate = true;
    }

    void Update()
    {
        if (!canUpdate)
            return;
       
        
        if(timeElapsed < lerpDuration)
        {
            targetPosition = Vector3.Lerp(PointA, PointB, timeElapsed/lerpDuration);
            targetScale = Vector3.Lerp(startScale, endScale, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;

            transform.position = targetPosition;
            transform.localScale = targetScale;
        }
        
        if(timeElapsed >= lerpDuration && DestroyAtTheEnd)
        {
            Debug.Log("Destroy Extra Wish");
            DestroyImmediate(gameObject);
        }

       /*
        if(Vector3.Distance(transform.position, PointB) < 0.1f && !_isUpdateMesh)
        {
            Debug.Log("ME Updated!");
            MeshData<GameObject, Vector3, Vector3> meshData = new MeshData<GameObject, Vector3, Vector3>();
            meshData.GameObject = gameObject;
            meshData.StartPoint = PointA;
            meshData.EndPoint = PointB;
            FindObjectOfType<MeshDataManager>().MeshInstancer.UpdateMeshInstant(meshData, FindObjectOfType<MeshDataManager>().RandomMeshPointList);
            _isUpdateMesh = true;
        }
       */
      
    }


}
