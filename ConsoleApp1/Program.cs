﻿// See https://aka.ms/new-console-template for more information'
using System;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            DBClient dbc = new DBClient();
            dbc.Start();
        }
    }
}