using System;
using System.Collections.Generic;
using Ukiyo.Common.Singleton;
using UnityEngine;

namespace Ukiyo.Player
{
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
    public class CollisionDetector : MonoSingleton<CollisionDetector>
    {
        public Types type = Types.Circle;
        public bool debug;
        public Transform playerTransform;
        public Transform targetTransform;
    
        private void Update()
        {
            if (null != playerTransform && debug)
            {
                bool result = false;
            
                Debug.DrawLine(playerTransform.position, playerTransform.position + playerTransform.forward * 6, Color.yellow);
            
                switch (type)
                {
                    case Types.Circle:
                    {
                        result = CircleCheck(playerTransform, targetTransform, 6);
                        break;
                    }
                    case Types.Triangle:
                    {
                        result = TriangleCheck(playerTransform, targetTransform, 3, 6);
                        break;
                    }
                    case Types.FanShaped:
                    {
                        result = FanShapedCheck(playerTransform, targetTransform, 80, 2);
                        break;
                    }
                    case Types.Rectangle:
                    {
                        result = RectangleCheck(playerTransform, targetTransform, 3, 6);
                        break;
                    }
                    case Types.Sector:
                    {
                        result = SectorCheck(playerTransform, targetTransform, 30, 3, 6, 6);
                        break;
                    }
                    case Types.Ring:
                    {
                        result = RingCheck(playerTransform, targetTransform, 3, 6);
                        break;
                    }
                }

                //Debug.Log(type + ": " + result);
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

        public bool TriangleCheck(Transform self, Transform target, float halfWidth, float distance, float allowedHeightDifference = 5.0f)
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

            return IsPointInsideTriangle(leftPoint, forwardPoint, rightPoint, targetPosition) 
                   && Mathf.Abs(self.position.y - target.position.y) < allowedHeightDifference;
        }
    
        // Determine if a point is inside an angle
        // References #1: https://stackoverflow.com/questions/1167022/2d-geometry-how-to-check-if-a-point-is-inside-an-angle
        public bool FanShapedCheck(Transform self, Transform target, float halfAngle, float distance, float allowedHeightDifference = 5.0f)
        {
            if (null == self || null == target)
                return false;
        
            Vector3 selfPosition = NormalizePosition(self.position, self.position.y);
            Vector3 targetPosition = NormalizePosition(target.position, self.position.y);
        
            Quaternion rotation = self.rotation;
        
            //--------------------- Draw Debug Line ---------------------
            if (type == Types.FanShaped && debug)
            {
                Vector3 firstPoint = Vector3.zero;
                firstPoint.y = selfPosition.y;
                Vector3 beginPoint = selfPosition;
                Vector3 endPoint = Vector3.zero;
                endPoint.y = selfPosition.y;

                float y = self.eulerAngles.y;
            
                bool bFirst = true;
                for (float i = -halfAngle + y; i <= halfAngle + y; i += 1.0f)
                {
                    float x = Mathf.Sin(i * Mathf.Deg2Rad) * distance;
                    float z = Mathf.Cos(i * Mathf.Deg2Rad) * distance;
                    endPoint.x = selfPosition.x + x;
                    endPoint.z = selfPosition.z + z;
                
                    if (bFirst) {
                        firstPoint = endPoint;
                        bFirst = false;
                    }
                    Debug.DrawLine(beginPoint, endPoint, Color.red);
                    beginPoint = endPoint;
                }
                Debug.DrawLine(selfPosition, firstPoint, Color.red);
                Debug.DrawLine(selfPosition, endPoint, Color.red);
            }
            //--------------------- Draw Debug Line ---------------------
        
        
            //----------------------- Calculation -----------------------
            // Calculate Distance
            float currentDistance = Vector3.Distance(selfPosition, targetPosition);
            if (currentDistance > distance)
                return false;
 
            // Vector direction from self to target
            Vector3 direction = targetPosition - selfPosition;
 
            // Dor product of direction vector and forward vector
            float dotForward = Vector3.Dot(direction.normalized, (rotation * Vector3.forward).normalized);
 
            // Get the angle radians and convert to angles by the arccos
            float radian = Mathf.Acos(dotForward);
            float currentAngle = Mathf.Rad2Deg * radian;
 
            if (Mathf.Abs(currentAngle) <= halfAngle && Mathf.Abs(self.position.y - target.position.y) < allowedHeightDifference)
                return true;
            //----------------------- Calculation -----------------------
 
            return false;
        }
    
        public bool RectangleCheck(Transform self, Transform target, float halfWidth, float distance, float allowedHeightDifference = 5.0f)
        {
            if (null == self || null == target)
                return false;
        
            Vector3 selfPosition = NormalizePosition(self.position, self.position.y);
            Vector3 targetPosition = NormalizePosition(target.position, self.position.y);
        
            Quaternion rotation = self.rotation;
        
            // Rectangle's four points
            Vector3 leftPoint = selfPosition + rotation * Vector3.left * halfWidth;
            Vector3 rightPoint = selfPosition + rotation * Vector3.right * halfWidth;
            Vector3 forwardLeftPoint = leftPoint + rotation * Vector3.forward * distance;
            Vector3 forwardRightPoint = rightPoint + rotation * Vector3.forward * distance;

            //--------------------- Draw Debug Line ---------------------
            if (type == Types.Rectangle && debug)
            {
                Debug.DrawLine(leftPoint, forwardLeftPoint, Color.red);
                Debug.DrawLine(rightPoint, forwardRightPoint, Color.red);
                Debug.DrawLine(leftPoint, rightPoint, Color.red);
                Debug.DrawLine(forwardRightPoint, forwardLeftPoint, Color.red);
            }
            //--------------------- Draw Debug Line ---------------------
        
            //----------------------- Calculation -----------------------
            return IsPointInsideRectangle(V3ToV2(forwardLeftPoint), V3ToV2(forwardRightPoint), 
                       V3ToV2(leftPoint), V3ToV2(rightPoint), V3ToV2(targetPosition)) 
                   && Mathf.Abs(self.position.y - target.position.y) < allowedHeightDifference;
            //----------------------- Calculation -----------------------
        }

        // Similar to fan shaped, but with one more distance check
        public bool SectorCheck(Transform self, Transform target, float halfAngle, float nearDistance, float farDistance, float allowedHeightDifference = 5.0f)
        {
            if (null == self || null == target)
                return false;

            if (nearDistance > farDistance)
                (nearDistance, farDistance) = (farDistance, nearDistance);
        
            Vector3 selfPosition = NormalizePosition(self.position, self.position.y);
            Vector3 targetPosition = NormalizePosition(target.position, self.position.y);
        
            Quaternion rotation = self.rotation;
        
            //--------------------- Draw Debug Line ---------------------
            if (type == Types.Sector && debug)
            {
                Vector3 nearFirstPoint = Vector3.zero;
                Vector3 nearBeginPoint = selfPosition;
                Vector3 nearEndPoint = Vector3.zero;
                nearFirstPoint.y = selfPosition.y;
                nearEndPoint.y = selfPosition.y;
            
                Vector3 farFirstPoint = Vector3.zero;
                Vector3 farBeginPoint = selfPosition;
                Vector3 farEndPoint = Vector3.zero;
                farFirstPoint.y = selfPosition.y;
                farEndPoint.y = selfPosition.y;

                float y = self.eulerAngles.y;
            
                bool bFirst = true;
                for (float i = -halfAngle + y; i <= halfAngle + y; i += 1.0f)
                {
                    // Draw Near
                    float nearX = Mathf.Sin(i * Mathf.Deg2Rad) * nearDistance;
                    float nearZ = Mathf.Cos(i * Mathf.Deg2Rad) * nearDistance;
                    nearEndPoint.x = selfPosition.x + nearX;
                    nearEndPoint.z = selfPosition.z + nearZ;
                
                    // Draw Far
                    float farX = Mathf.Sin(i * Mathf.Deg2Rad) * farDistance;
                    float farZ = Mathf.Cos(i * Mathf.Deg2Rad) * farDistance;
                    farEndPoint.x = selfPosition.x + farX;
                    farEndPoint.z = selfPosition.z + farZ;
                
                    if (bFirst) {
                        nearFirstPoint = nearEndPoint;
                        farFirstPoint = farEndPoint;
                        bFirst = false;
                    }
                    Debug.DrawLine(nearBeginPoint, nearEndPoint, Color.red);
                    Debug.DrawLine(farBeginPoint, farEndPoint, Color.red);
                    nearBeginPoint = nearEndPoint;
                    farBeginPoint = farEndPoint;
                }
                Debug.DrawLine(nearFirstPoint, farFirstPoint, Color.red);
                Debug.DrawLine(nearEndPoint, farEndPoint, Color.red);
                Debug.DrawLine(selfPosition, nearFirstPoint, Color.blue);
                Debug.DrawLine(selfPosition, nearEndPoint, Color.blue);
            }
            //--------------------- Draw Debug Line ---------------------
        
        
            //----------------------- Calculation -----------------------
            // Calculate Distance
            float currentDistance = Vector3.Distance(selfPosition, targetPosition);
            if (currentDistance < nearDistance ||  currentDistance > farDistance)
                return false;
 
            // Vector direction from self to target
            Vector3 direction = targetPosition - selfPosition;
 
            // Dor product of direction vector and forward vector
            float dotForward = Vector3.Dot(direction.normalized, (rotation * Vector3.forward).normalized);
 
            // Get the angle radians and convert to angles by the arccos
            float radian = Mathf.Acos(dotForward);
            float currentAngle = Mathf.Rad2Deg * radian;
 
            if (Mathf.Abs(currentAngle) <= halfAngle && Mathf.Abs(self.position.y - target.position.y) < allowedHeightDifference)
                return true;
            //----------------------- Calculation -----------------------
 
            return false;
        }
    
        public bool RingCheck(Transform self, Transform target, float nearDistance, float farDistance, float allowedHeightDifference = 5.0f)
        {
            if (null == self || null == target)
                return false;
 
            if (nearDistance > farDistance)
                (nearDistance, farDistance) = (farDistance, nearDistance);
        
            Vector3 selfPosition = NormalizePosition(self.position, self.position.y);
            Vector3 targetPosition = NormalizePosition(target.position, self.position.y);
        
            //--------------------- Draw Debug Line ---------------------
            if (type == Types.Ring && debug)
            {
                int nCircleSlices = 180;
        
                Vector3 nearBeginPoint = selfPosition;
                Vector3 nearEndPoint = Vector3.zero;
                nearEndPoint.y = selfPosition.y;
        
                Vector3 farBeginPoint = selfPosition;
                Vector3 farEndPoint = Vector3.zero;
                farEndPoint.y = selfPosition.y;
        
                float tempStep = 2 * Mathf.PI / nCircleSlices;
        
                bool bFirst = true;
                for (float step = 0; step < 2 * Mathf.PI; step += tempStep)
                {
                    float nearX = nearDistance * Mathf.Cos(step);
                    float nearZ = nearDistance * Mathf.Sin(step);
                    nearEndPoint.x = selfPosition.x + nearX;
                    nearEndPoint.z = selfPosition.z + nearZ;
 
                    float farX = farDistance * Mathf.Cos(step);
                    float farZ = farDistance * Mathf.Sin(step);
                    farEndPoint.x = selfPosition.x + farX;
                    farEndPoint.z = selfPosition.z + farZ;
 
                    if (bFirst)
                        bFirst = false;
                    else
                    {
                        Debug.DrawLine(nearBeginPoint, nearEndPoint, Color.red);
                        Debug.DrawLine(farBeginPoint, farEndPoint, Color.red);
                    }
 
                    nearBeginPoint = nearEndPoint;
                    farBeginPoint = farEndPoint;
                }
            }
            //--------------------- Draw Debug Line ---------------------
 
            //----------------------- Calculation -----------------------
            float currDistance = Vector3.Distance(selfPosition, targetPosition);
            if (currDistance >= nearDistance && currDistance <= farDistance && Mathf.Abs(self.position.y - target.position.y) < allowedHeightDifference)
                return true;
            //----------------------- Calculation -----------------------
 
            return false;
        }

        // Check tp is inside Rec(p1, p2, p3, p4)
        // Using Cross Product to determine the point inside the rectangle
        // References #1: https://math.stackexchange.com/questions/190111/how-to-check-if-a-point-is-inside-a-rectangle
        //            #2: https://stackoverflow.com/questions/2752725/finding-whether-a-point-lies-inside-a-rectangle-or-not
        private bool IsPointInsideRectangle(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 tp)
        {
            return GetCross(p1, p2, tp) * GetCross(p3, p4, tp) >= 0 && 
                   GetCross(p2, p3, tp) * GetCross(p4, p1, tp) >= 0;
        }

        // Calculate |p1 p2| * |p1 p3|
        private float GetCross(Vector2 p1, Vector2 p2, Vector2 tp)
        {
            return (p2.x - p1.x) * (tp.y - p1.y) - (tp.x - p1.x) * (p2.y - p1.y);
        }

        private Vector2 V3ToV2(Vector3 vector3)
        {
            Vector2 vector2 = Vector2.zero;
            vector2.x = vector3.x;
            vector2.y = vector3.z;
            return vector2;
        }

        // Math Barycentric Technique algorithm
        // References #1: https://blackpawn.com/texts/pointinpoly/default.html
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
}