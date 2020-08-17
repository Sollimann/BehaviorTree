using BehaviorTreeLibrary;
using BehaviorTreeLibrary.leaf;
using Serilog;
using System;
using System.Drawing;

public class Hunger : Sequence
{
    public int hunger = 0;
    private Dwarf _dwarf;
    private Food _targetFood;

    public Hunger(Dwarf dwarf)
    {
        _dwarf = dwarf;

        Add<Behavior>().Update = IncreaseHunger;
        Add<Condition>().CanRun = IsHungry;
        Add<Behavior>().Update = LocateFood;

        var selector = Add<Selector>();
        var sequence = selector.Add<Sequence>();

        sequence.Add<Condition>().CanRun = NearFood;
        sequence.Add<Behavior>().Update = () =>
        {
            Log.Debug("Eating");
            Console.WriteLine("Eating");
            hunger = 0;
            _targetFood = null;
            return Status.BhSuccess;
        };
    }

    public Status IncreaseHunger()
    {
        hunger++;
        return Status.BhSuccess;
    }

    public bool IsHungry()
    {
        return hunger > 100;
    }

    public Status LocateFood()
    {
        // if dwarf has not found food
        if(_targetFood == null)
        {
            _targetFood.LocatedAt = new Point(6, 6);
        }
        // if he has found food
        return Status.BhSuccess;
    }

    public bool NearFood()
    {
        if((_dwarf.Location.X - _targetFood.LocatedAt.X) < 0.4f && (_dwarf.Location.Y - _targetFood.LocatedAt.Y) < 0.4f) 
        {
            return true;
        }
        return false;
    }
}

