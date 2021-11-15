using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Types
{
    Circle,
    Triangle,
    FanShaped,
    Rectangle,
    Sector,
    Ring,
}

[ExecuteInEditMode]
public class CollisionDetector : MonoBehaviour
{
    public Types type = Types.Circle;
    public Transform playerTransform;
    public Transform targetTransform;
    
    private void Update()
    {
        if (null != playerTransform)
        {
            Debug.DrawLine(playerTransform.position, playerTransform.position + playerTransform.forward * 8, Color.yellow);
        }

        switch (type)
        {
            case Types.Circle:
            {
                CircleCheck(playerTransform, targetTransform, 10);
                break;
            }
        }
    }

    public bool CircleCheck(Transform self, Transform target, float distance)
    {
        if (null == self || null == target)
            return false;
        
        //--------------------- Draw Debug Line ---------------------
        Vector3 selfPosition = NormalizePosition(self.position, self.position.y);
        Vector3 targetPosition = NormalizePosition(target.position, self.position.y);
 
        int nCircleSlices = 180;
        
        Vector3 beginPoint = selfPosition;
        Vector3 endPoint = Vector3.zero;
        
        float tempStep = 2 * Mathf.PI / nCircleSlices;
        
        bool bFirst = true;
        for (float step = 0; step < 2 * Mathf.PI; step += tempStep)
        {
            float x = distance * Mathf.Cos(step);
            float z = distance * Mathf.Sin(step);
            endPoint.x = selfPosition.x + x;
            endPoint.z = selfPosition.z + z;
 
            if (bFirst)
                bFirst = false;
            else
                Debug.DrawLine(beginPoint, endPoint, Color.red);
            
            beginPoint = endPoint;
        }
        //--------------------- Draw Debug Line ---------------------

        //----------------------- Calculation -----------------------
        float currDistance = Vector3.Distance(selfPosition, targetPosition);
        if (currDistance <= distance)
            return true;
        //----------------------- Calculation -----------------------
 
        return false;
    }
    
    // Remove height for better accuracy 
    private Vector3 NormalizePosition(Vector3 position,float height = 0.0f)
    {
        Vector3 normalizePosition = Vector3.zero;
        normalizePosition.x = position.x;
        normalizePosition.y = height;
        normalizePosition.z = position.z;
 
        return normalizePosition;
    }
    
}
