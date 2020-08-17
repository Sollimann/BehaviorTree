
using BehaviorTreeLibrary;
using System.Collections.Generic;
using System.Drawing;

public class Dwarf
{
    public List<Behavior> Behaviors = new List<Behavior>();
    public float MaxSpeed { get; set; }
    public float MaxAcceleration { get; set; }

    public Point Location { get; set; }

    // Use this for initialization
    public Dwarf()
    {
        Location = new Point(0, 0);
        MaxSpeed = 10f;
        MaxAcceleration = 30f;
        Behaviors.Add(new Hunger(this));
    }

    // Update is called once per frame
    public void Update()
    {
        foreach(var behavior in Behaviors)
        {
            behavior.Tick();
        }
    }
}

