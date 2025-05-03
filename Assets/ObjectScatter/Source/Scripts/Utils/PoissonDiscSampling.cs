using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PointSamplingGridPoint
{
    public Vector2 point;
    public Vector2Int index;
}

public struct PointsSamplingData
{
    public int[,] grid;
    public List<Vector2> points;
    public List<PointSamplingGridPoint> gridPointsData;
}

public static class PoissonDiscSampling 
{

    public static PointsSamplingData GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
    {
        PointsSamplingData data = new PointsSamplingData();

        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();
        List<PointSamplingGridPoint> gridPointsData = new List<PointSamplingGridPoint>();

        spawnPoints.Add(sampleRegionSize / 2);
        while(spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);

                if(IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);

                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;

                    PointSamplingGridPoint psg = new PointSamplingGridPoint();
                    psg.index = new Vector2Int((int)(candidate.x / cellSize), (int)(candidate.y / cellSize));
                    psg.point = candidate;

                    gridPointsData.Add(psg);

                    break;
                }

            }

            if(!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        data.points = points;
        data.gridPointsData = gridPointsData;
        data.grid = grid;

        return data;

    }

    public static List<Vector2> RegeneratePoints(float radius, Vector2 sampleRegionSize, List<PointSamplingGridPoint> ignoredGridPoints, int numSamplesBeforeRejection = 30)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        //for (int i = 0; i < ignoredGridPoints.Count; i++)
        //{
        //    points.Add(ignoredGridPoints[i].point);
        //    spawnPoints.Add(ignoredGridPoints[i].point);
        //    grid[ignoredGridPoints[i].index.x, ignoredGridPoints[i].index.y] = points.Count;

        //}

        spawnPoints.Add(sampleRegionSize / 2);
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);

                int xt = (int)(candidate.x / cellSize);
                int yt = (int)(candidate.y / cellSize);

                int f = ignoredGridPoints.FindIndex(it => it.index.x == xt && it.index.y == yt);
                if(f >= 0)
                {
                    continue;
                }

                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);

                    grid[xt, yt] = points.Count;
                    candidateAccepted = true;

                    break;
                }

            }

            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;

    }

    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if(candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if(pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if(sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        return false;
    }
}
