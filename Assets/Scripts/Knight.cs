using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class Knight
{
    public static List<Point> basicAttack = new List<Point>() { new Point(1, 0), new Point(-1, 0), new Point(0, 1) , new Point(0, -1),
                                                new Point(1, 1), new Point(1, -1), new Point(-1, 1), new Point(-1, -1)};

    public static List<Point> basicMove = new List<Point>() { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };
}
