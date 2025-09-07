/// ------------------------------
/// Original Author: Matthew Vale
/// ------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace RPSLib
{

    /// <summary>
    /// Various mathematic functions.
    /// </summary>
    public class Maths
    {

        /// <summary>
        /// Distance calculations.
        /// </summary>
        public class Distances
        {

            /// <summary>
            /// Get the rough distance between 2 points using Sqr Mangitude.
            /// <br></br>Use Vector3.Distance if you need the actual distance between 2 points.
            /// <br></br>
            /// <br></br>Example: Which object out of N objects is closer to a point, without caring about actual distance?
            /// </summary>
            public static float SqrMagDistance(Vector3 from, Vector3 to) { return (to - from).sqrMagnitude; }

            /// <summary>
            /// Get the closest GameObject to a point from a List of GameObjects.
            /// </summary>
            /// <param name="from">Point of origin.</param>
            /// <param name="targets">List of GameObjects to compare.</param>
            public static GameObject GetClosestGameObject(Vector3 from, List<GameObject> targets)
            {
                byte closestIndex = 0;
                float minDist = Mathf.Infinity;
                for (int i = 0; i < targets.Count; i++)
                {
                    float distanceToTarget = Vector3.Distance(targets[i].transform.position, from);
                    if (distanceToTarget < minDist)
                    {
                        minDist = distanceToTarget;
                        closestIndex = (byte)i;
                    }
                }

                return targets[closestIndex];
            }

        }

        /// <summary>
        /// Gradient functions.
        /// </summary>
        public class Gradients
        {

            /// <summary>
            /// Useful for smooth animations. Equation is: 
            /// <code>x / x + 1</code>
            /// See <a href="LINK">https://www.desmos.com/calculator/0mxrzgwqgt</a>
            /// </summary>
            public static float SoftLimit(float x)
            {
                x = Mathf.Max(x, 0);
                return x / (x + 1);
            }

        }

        /// <summary>
        /// Weighted chances functions.
        /// </summary>
        public class WeightedChances
        {

            /// <summary>
            /// Selects a random index from the given array of weights, where the probability of each index being
            /// selected is proportional to its weight.
            /// </summary>
            public static int GetRandomWeightedIndex(float[] weights)
            {

                if (weights == null || weights.Length == 0)
                {
                    return -1;
                }

                float w;
                float t = 0;
                int i; // Use the same float for both loops

                for (i = 0; i < weights.Length; i++)
                {
                    w = weights[i];

                    if (float.IsPositiveInfinity(w))
                    {
                        return i;
                    }
                    else if (w >= 0f && !float.IsNaN(w))
                    {
                        t += weights[i];
                    }
                }

                float r = Random.value;
                float s = 0f;

                for (i = 0; i < weights.Length; i++)
                {
                    w = weights[i];
                    if (float.IsNaN(w) || w <= 0f)
                        continue;

                    s += w / t;
                    if (s >= r)
                        return i;
                }

                return -1;
            }

        }

    }

}