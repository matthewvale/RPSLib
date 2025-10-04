/// ------------------------------
/// Original Author: Matthew Vale
/// ------------------------------

using UnityEngine;

namespace RPSLib
{

    /// <summary>
    /// Physics related helpers.
    /// </summary>
    public class Physics
    {

        /// <summary>
        /// Raycast specific helpers.
        /// </summary>
        public class Raycast
        {

            /// <summary>
            /// Get a reference to the GameObject at the mouse position using a raycast.
            /// </summary>
            public static GameObject? GetGameObjectAtMousePosition(float rayDistance, LayerMask maskToCheck, Camera? sourceCamera = null)
            {
                Ray ray;

                if (sourceCamera == null)
                {
                    if (Camera.main == null)
                    {
                        Debug.Log("No Camera in scene! Set a Main Camera or pass one into this method.", Debug.Style.CriticalError, true);
                        return null;
                    }

                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                }
                else
                {
                    ray = sourceCamera.ScreenPointToRay(Input.mousePosition);
                }

                if (UnityEngine.Physics.Raycast(ray, out RaycastHit rayHit, rayDistance, maskToCheck))
                {
                    return rayHit.transform.gameObject;
                }

                return null;
            }

            /// <summary>
            /// Get a World Space Vector3 point based on the mouse cursor position using a mathematical plane.
            /// </summary>
            /// <param name="originTransform">The reference Transform used to position the mathematical plane.</param>
            public static Vector3 GetMousePositionOnPlane(Vector2 mouseScreenPos, Camera camera, Transform originTransform)
            {
                // Get raw mouse position on screen
                Vector3 mousePosition = new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0f);

                // Set up a ray and a mathematical plane
                Ray mouseRay = camera.ScreenPointToRay(mousePosition);
                Plane p = new Plane(Vector3.up, originTransform.position);

                // If the plane is hit, return the point
                if (p.Raycast(mouseRay, out float hitDist))
                {
                    return mouseRay.GetPoint(hitDist);
                }
                else
                {
                    // Otherwise return standard forward vector
                    return originTransform.forward;
                }
            }

        }

    }

}