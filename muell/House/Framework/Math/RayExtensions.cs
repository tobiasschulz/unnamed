/*
 * Copyright (c) 2013-2014 Tobias Schulz, Maximilian Reuter, Pascal Knodel,
 *                         Gerd Augsburg, Christina Erler, Daniel Warzel
 *
 * This source code file is part of Knot3. Copying, redistribution and
 * use of the source code in this file in source and binary forms,
 * with or without modification, are permitted provided that the conditions
 * of the MIT license are met:
 *
 *   Permission is hereby granted, free of charge, to any person obtaining a copy
 *   of this software and associated documentation files (the "Software"), to deal
 *   in the Software without restriction, including without limitation the rights
 *   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *   copies of the Software, and to permit persons to whom the Software is
 *   furnished to do so, subject to the following conditions:
 *
 *   The above copyright notice and this permission notice shall be included in all
 *   copies or substantial portions of the Software.
 *
 *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *   SOFTWARE.
 *
 * See the LICENSE file for full license details of the Knot3 project.
 */

using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;

namespace Knot3.Framework.Math
{
    public static class RayExtensions
    {
        public static float? Intersects (this Ray ray, BoundingCylinder cylinder)
        {
            Vector3 dirAB = cylinder.SideB - cylinder.SideA;
            // Raystart innerhalb des Zylinders
            if (Vector3.Cross ((ray.Position - cylinder.SideA), ray.Direction).Length () < cylinder.Radius && !(Vector3.Dot (dirAB, ray.Position - cylinder.SideA) < 0 || Vector3.Dot (dirAB, cylinder.SideB - ray.Position) < 0)) {
                return 0.0f;
            }
            Vector3 perpendicular = Vector3.Cross (dirAB, ray.Direction);
            // if !(Ray Parallel zum Zylinder)
            if (perpendicular.Length () > 0.0000001f) {
                perpendicular.Normalize ();
                if (Vector3.Dot (perpendicular, ray.Direction) > 0) {
                    perpendicular = -perpendicular;
                }
                Vector3 perpendicular2 = Vector3.Cross (dirAB, perpendicular);
                // If (Ray Senkrecht zum Zylinder)
                if (perpendicular2.Length () < 0.0000001f) {
                    if (Vector3.Dot (dirAB, ray.Position - cylinder.SideA) < 0 || Vector3.Dot (dirAB, cylinder.SideB - ray.Position) < 0) {
                        return null;
                    }
                    float? result = Vector3.Cross ((ray.Position - cylinder.SideA), ray.Direction).Length () - cylinder.Radius;
                    if (result < 0) {
                        result = 0.0f;
                    }
                    return result;
                }
                if (Vector3.Dot (perpendicular2, ray.Direction) > 0) {
                    perpendicular2 = -perpendicular2;
                }
                perpendicular2.Normalize ();
                float minDist = System.Math.Abs (Vector3.Dot (cylinder.SideA - ray.Position, perpendicular));
                if (minDist > cylinder.Radius) {
                    return null;
                }
                Vector3 plainNorm = perpendicular * minDist + (float)System.Math.Sqrt (cylinder.Radius * cylinder.Radius - minDist * minDist) * perpendicular2;
                plainNorm.Normalize ();
                float? other_result = ray.Intersects (new Plane (plainNorm, Vector3.Dot (plainNorm, cylinder.SideA + plainNorm * cylinder.Radius)));
                if (other_result == null) {
                    return null;
                }
                Vector3 cutA = ray.Position + ray.Direction * (float)other_result - cylinder.SideA;
                Vector3 cutB = ray.Position + ray.Direction * (float)other_result - cylinder.SideB;
                if (Vector3.Dot (dirAB, cutA) > 0 && Vector3.Dot (-dirAB, cutB) > 0) {
                    return other_result;
                }
            }
            if (Vector3.Distance (ray.Position, cylinder.SideA) < Vector3.Distance (ray.Position, cylinder.SideB)) {
                dirAB.Normalize ();
                float? result = ray.Intersects (new Plane (dirAB, Vector3.Dot (dirAB, cylinder.SideA)));
                if (result == null || Vector3.Distance (ray.Position + ray.Direction * (float)result, cylinder.SideA) > cylinder.Radius) {
                    return null;
                }
                return result;
            }
            else {
                dirAB.Normalize ();
                dirAB = -dirAB;
                float? result = ray.Intersects (new Plane (dirAB, Vector3.Dot (dirAB, cylinder.SideB)));
                if (result == null || Vector3.Distance (ray.Position + ray.Direction * (float)result, cylinder.SideB) > cylinder.Radius) {
                    return null;
                }
                return result;
            }
            /*
            Vector3 diffA = capsule.CornerA - ray.Position;
            Vector3 diffB = capsule.CornerB - ray.Position;
            float diffASquared = diffA.LengthSquared ();
            float diffBSquared = diffB.LengthSquared ();
            float radiusSquared = capsule.Radius * capsule.Radius;
            // Startpunkt innerhalb der Eckkugeln
            if (diffASquared < radiusSquared || diffBSquared < radiusSquared)
            {
                return 0.0f;
            }
            Vector3 dirBA = (capsule.CornerA - capsule.CornerB);
            float distAlongAB = Vector3.Dot (diffA, dirBA) / dirBA.Length ();
            // Startpunkt innerhalb des Zylinders
            if (distAlongAB > 0 && distAlongAB < dirBA.Length () && (distAlongAB * distAlongAB + radiusSquared - diffA.LengthSquared () < 0))
            {
                return 0.0f;
            }
            float distAlongRayA = Vector3.Dot (ray.Direction, diffA);
            float distAlongRayB = Vector3.Dot (ray.Direction, diffB);
            // Richtung geht weg von der Kapsel
            if (distAlongRayA < 0 && distAlongRayB < 0)
                return null;
            Vector3 perpendicular = Vector3.Cross (ray.Direction, dirBA);
            perpendicular.Normalize ();
            float minDistance = Math.Abs (Vector3.Dot (diffA, perpendicular));
            // Kommt selbst der Geraden nie nahe genug.
            if (minDistance > capsule.Radius)
            {
                return null;
            }
            Vector3 normDirAB = -dirBA;
            normDirAB.Normalize ();
            Vector3 extensionToBase = Vector3.Cross (normDirAB, perpendicular);
            extensionToBase.Normalize ();
            Matrix transformation = new Matrix (normDirAB.X, normDirAB.Y, normDirAB.Z, 0, perpendicular.X, perpendicular.Y, perpendicular.Z, 0, extensionToBase.X, extensionToBase.Y, extensionToBase.Z, 0, capsule.CornerA.X, capsule.CornerA.Y, capsule.CornerA.Z, 1);
            transformation = Matrix.Invert (transformation);
             */
        }
    }
}
