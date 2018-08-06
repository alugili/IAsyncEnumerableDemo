using System;
using System.Threading;
using System.Threading.Tasks;

namespace IAsyncEnumerableDemo
{
  public static class ConsoleExt
  {
    public static void WriteLine(object message)
    {
      Console.WriteLine($"(Time: {DateTime.Now.ToShortTimeString()},  Thread {Thread.CurrentThread.ManagedThreadId}): {message} ");
    }

    public static async void WriteLineAsync(object data)
    {
      await Task.Run(() => Console.WriteLine($"(Time: {DateTime.Now.ToShortTimeString()},  Thread {Thread.CurrentThread.ManagedThreadId}): {data} "));
    }
  }
}
