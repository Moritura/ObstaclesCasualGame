using System;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public event Action ExplosionEvent;

    private List<Obstacle> _obstacles;

    public Map()
    {
        _obstacles = new List<Obstacle>();
    }

    public void Registration(Obstacle obstacle)
    {
        _obstacles.Add(obstacle);
        obstacle.ExplosionEvent += OnExplosion;
    }

    public List<(float, Obstacle)> GetObstaclesInSphere(Vector3 position, float radius)
    {
        var obstaclesInSphere = new List<(float, Obstacle)>();

        foreach (var obstacle in _obstacles)
        {
            if (obstacle != null)
            {
                float distance = Vector3.Distance(position, obstacle.GetPosition);
                if (distance <= radius)
                {
                    obstaclesInSphere.Add((distance, obstacle));
                }
            }
        }

        return obstaclesInSphere;
    }

    private void OnExplosion(Obstacle obstacle)
    {
        obstacle.ExplosionEvent -= OnExplosion;
        _obstacles.Remove(obstacle);
        ExplosionEvent?.Invoke();
    }

    public float GetAdjustmentDistance(Vector3 startPoint, Vector3 endPoint, float width)
    {
        width += _obstacles[0].GetRadius;

        float minDistance = float.MaxValue;

        Vector3 direction = (endPoint - startPoint).normalized;

        Vector3 perpendicular = new Vector3(-direction.z, 0, direction.x).normalized;

        float halfWidth = width / 2;

        Vector3 lowerLeft = startPoint - perpendicular * halfWidth;
        Vector3 lowerRight = startPoint + perpendicular * halfWidth;

        foreach (var obstacle in _obstacles)
        {
            Vector3 objPosition = new Vector3(obstacle.GetPosition.x, 0, obstacle.GetPosition.z);

            Vector3 projectedPoint = ProjectPointOnLine(startPoint, direction, objPosition);
            if (IsPointWithinWidth(objPosition, lowerLeft, lowerRight))
            {
                float distance = Vector3.Distance(startPoint, projectedPoint);

                if (Vector3.Dot(projectedPoint - startPoint, direction) > 0 && distance < minDistance)
                {
                    minDistance = distance;
                }
            }
        }

        return minDistance;
    }

    private bool IsPointWithinWidth(Vector3 point, Vector3 lowerLeft, Vector3 lowerRight)
    {
        Vector3 AB = lowerRight - lowerLeft;
        Vector3 AM = point - lowerLeft;
        float dotABAM = Vector3.Dot(AB, AM);
        float dotABAB = Vector3.Dot(AB, AB);

        return 0 <= dotABAM && dotABAM <= dotABAB;
    }

    private Vector3 ProjectPointOnLine(Vector3 lineStart, Vector3 lineDir, Vector3 point)
    {
        return lineStart + Vector3.Dot(point - lineStart, lineDir) * lineDir;
    }
}
