﻿using System;

namespace SweenGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SweenGame())
                game.Run();
        }
    }
}
