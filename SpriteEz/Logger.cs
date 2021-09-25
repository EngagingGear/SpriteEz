using System;
using System.Diagnostics;

namespace SpriteEzNs
{
    public class Logger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }
    }
}