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
    public bool debug;
    public Transform playerTransform;
    public Transform targetTransform;
    
    private void Update()
    {
        if (null != playerTransform)
        {
            Debug.DrawLine(playerTransform.position, playerTransform.position + playerTransform.forward * 6, Color.yellow);
        }

        switch (type)
        {
            case Types.Circle:
            {
                CircleCheck(playerTransform, targetTransform, 6);
                break;
            }
            case Types.Triangle:
            {
                TriangleCheck(playerTransform, targetTransform, 3, 6);
                break;
            }
            case Types.FanShaped:
            {
                TriangleCheck(playerTransform, targetTransform, 3, 6);
                break;
            }
            case Types.Rectangle:
            {
                TriangleCheck(playerTransform, targetTransform, 3, 6);
                break;
            }
            case Types.Sector:
            {
                TriangleCheck(playerTransform, targetTransform, 3, 6);
                break;
            }
            case Types.Ring:
            {
                TriangleCheck(playerTransform, targetTransform, 3, 6);
                break;
            }
        }
    }

    public bool CircleCheck(Transform self, Transform target, float distance, float allowedHeightDifference = 5.0f)
    {
        if (null == self || null == target)
            return false;
        
        Vector3 selfPosition = NormalizePosition(self.position, self.position.y);
        Vector3 targetPosition = NormalizePosition(target.position, self.position.y);

        //--------------------- Draw Debug Line ---------------------
        if (type == Types.Circle && debug)
        {
            int nCircleSlices = 180;
        
            Vector3 beginPoint = selfPosition;
            Vector3 endPoint = Vector3.zero;
            endPoint.y = selfPosition.y;
        
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
        }
        //--------------------- Draw Debug Line ---------------------

        //----------------------- Calculation -----------------------
        float currDistance = Vector3.Distance(selfPosition, targetPosition);
        if (currDistance <= distance && Mathf.Abs(self.position.y - target.position.y) < allowedHeightDifference)
            return true;
        //----------------------- Calculation -----------------------
 
        return false;
    }

    public bool TriangleCheck(Transform self, Transform target, float halfWidth, float distance)
    {
        if (null == self || null == target)
            return false;
        
        Vector3 selfPosition = NormalizePosition(self.position, self.position.y);
        Vector3 targetPosition = NormalizePosition(target.position, self.position.y);
        
        Quaternion rotation = self.rotation;

        // Triangle's three point
        Vector3 leftPoint = selfPosition + rotation * Vector3.left * halfWidth;
        Vector3 rightPoint = selfPosition + rotation * Vector3.right * halfWidth;
        Vector3 forwardPoint = selfPosition + rotation * Vector3.forward * distance;
        
        //--------------------- Draw Debug Line ---------------------
        if (type == Types.Triangle && debug)
        {
            Debug.DrawLine(leftPoint, forwardPoint, Color.red);
            Debug.DrawLine(rightPoint, forwardPoint, Color.red);
            Debug.DrawLine(leftPoint, rightPoint, Color.red);
        }
        //--------------------- Draw Debug Line ---------------------

        return IsPointInsideTriangle(leftPoint, forwardPoint, rightPoint, targetPosition);
    }

    private bool IsPointInsideTriangle(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 targetPoint)
    {
        Vector3 v0 = point2 - point1;
        Vector3 v1 = point3 - point1;
        Vector3 v2 = targetPoint - point1;

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float inverDeno = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * inverDeno;

        if (u < 0 || u > 1)
            return false;

        float v = (dot00 * dot12 - dot01 * dot02) * inverDeno;
        if (v < 0 || v > 1)
            return false;

        return true;
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
