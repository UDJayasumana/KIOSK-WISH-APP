using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishItem : MonoBehaviour
{
    public Camera MainCamera;

    public Transform PointA, PointB;

    public GameObject SpawnPF;

    public Transform MousePoint;

    private float timeElapsed = 0;
    private float lerpDuration = 3;

    
    void Start()
    {
        transform.position = PointA.position;
    }


    void Update()
    {
        /*
        if(timeElapsed < lerpDuration)
        {
            Vector3 targetPosition = Vector3.Lerp(PointA.position, PointB.position, timeElapsed/lerpDuration);
            timeElapsed += Time.deltaTime;

            transform.position = targetPosition;
        }
        */
        //Debug.Log(string.Format("Object Position : {0} Point B Position : {1}", transform.position, PointB.position));

        if(Input.GetMouseButtonDown(0))
        {
            GetRandomScreenPoint();
        }

    }

    void GetRandomScreenPoint()
    {
        /*
        Vector3 spawnPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);


        spawnPoint.z = 0;

        MousePoint.position = spawnPoint;
        */
        Vector2 Bounds = MainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        float minX = -Bounds.x;
        float maxX =  Bounds.x;
        float minY = -Bounds.y;
        float maxY =  Bounds.y;
   
        Vector2 pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        MousePoint.position = pos;

    }

}
