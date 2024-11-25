using System;
using System.Reflection;

namespace MyApp
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Version? version = Assembly.GetExecutingAssembly().GetName().Version;
      Console.WriteLine($"Hello World! {version}");
    }
  }
}