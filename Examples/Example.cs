using BehaviorTreeLibrary;
using System;
using System.Collections.Generic;

namespace Examples
{
    internal class Example
    {

        static void Main(string[] args)
        {
            Dwarf dwarf1 = new Dwarf();
            Hunger findFood = new Hunger(dwarf1);

            findFood.Tick();
            findFood.IncreaseHunger();

        }
    }
}
