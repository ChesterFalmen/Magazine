using Microsoft.EntityFrameworkCore;
using Magazine.EF;
using Magazine.Models;
using System;
using System.Linq;
using System.Threading;

namespace Magazine
{
    internal class Program
    {
        ///union, except, intersect, join, distinct, group by, агрегатних функцій.
        static AutoResetEvent waitHandler = new AutoResetEvent(true);

        static void Main(string[] args)
        {

            var example = new MultithreadingToDatabase();
            
            example.monitor();
            example.AutoResetEvent();
            example.Mutex();
            example.Lock();
        }
    }
}