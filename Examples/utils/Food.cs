using System;
using System.Drawing;

public class Food
{
    public string Name { get; set; }
    public int HealthScore { get; set; }

    public Point LocatedAt { get; set; }

    public Food(string name, int score)
    {
        Name = name;
        HealthScore = score;
    }
}

