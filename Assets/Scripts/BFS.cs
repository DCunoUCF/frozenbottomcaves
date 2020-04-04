using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

class qNode
{
    public Point p;
    public qNode prev;
    public int dist;

    public qNode(Point p, int dist, qNode prev)
    {
        this.p = p;
        this.dist = dist;
        this.prev = prev;
    }
}

public static class BFS
{
    static int ROW, COL;
    static int[] rowNum = { -1, 0, 0, 1 };
    static int[] colNum = { 0, -1, 1, 0 };

    static bool isValid(int x, int y)
    {
        return (x >= 0) && (x < ROW) && (y >= 0) && (y < COL);
    }

    static Point returnTarget(qNode finalPoint)
    {
        string path = "";
        path += "(" + finalPoint.p.X + ", " + finalPoint.p.Y + ") ";
        qNode curr = finalPoint.prev;

        if (curr.prev == null)
            return curr.p;

        while (curr.prev != null)
        {
            path += ("(" + curr.p.X + ", " + curr.p.Y + ") ");
            if (curr.prev.prev == null)
                return curr.p;
            curr = curr.prev;
        }

        Debug.Log(path);

        return curr.p;
    }

    public static Point bfs(Cell[,] grid, Point start, Point target, List<Point> offlimits)
    {
        // Set the limits of the grid
        ROW = grid.GetLength(0);
        COL = grid.GetLength(1);

        bool[,] visited = new bool[ROW,COL];

        foreach (Point p in offlimits)
        {
            if (p.X < ROW && p.X >= 0 && p.Y < COL && p.Y >= 0)
            {
                visited[p.X, p.Y] = true;
            }
        }

        visited[start.X, start.Y] = true;

        Debug.Log("starting at x,y" + start.X + ", " + start.Y);

        Queue<qNode> q = new Queue<qNode>();
        qNode s = new qNode(start, 0, null);

        // kick off the bfs
        q.Enqueue(s);
        while (q.Count != 0)
        {
            qNode current = q.Peek();
            Point p = current.p;

            // found target
            if (p.X == target.X && p.Y == target.Y)
            {
                Debug.Log("Distance to target: " + current.dist);
                return returnTarget(current);
            }

            q.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                int row = p.X + rowNum[i];
                int col = p.Y + colNum[i];

                if (isValid(row, col) && grid[row, col].pass && !visited[row,col])
                {
                    visited[row, col] = true;
                    qNode adj = new qNode(new Point(row, col), current.dist + 1, current);
                    q.Enqueue(adj);
                }
            }
        }

        return new Point(-999, -999);
    }
}
