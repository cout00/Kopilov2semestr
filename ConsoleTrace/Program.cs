﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1ThreshHold.Infrastructure.Captures;

namespace ConsoleTrace
{
    class Program
    {
        static void Main(string[] args)
        {
            Lab3Grip lab3Grip = new Lab3Grip(new short[1, 1]);
            lab3Grip.DoProcessAsync();
        }
    }
}
