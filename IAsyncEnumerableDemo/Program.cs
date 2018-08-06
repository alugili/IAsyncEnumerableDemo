using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace IAsyncEnumerableDemo
{
  class TaskExample
  {
    static int SumFromOneToCount(int count)
    {
      ConsoleExt.WriteLine("SumFromOneToCount called!");

      var sum = 1;
      for (var i = 0; i < count; i++)
      {
        sum = sum + i;
      }
      return sum;
    }

    static IEnumerable<int> SumFromOneToCountYield(int count)
    {
      ConsoleExt.WriteLine("SumFromOneToCountYield called!");

      var sum = 1;
      for (var i = 0; i < count; i++)
      {
        sum = sum + i;

        yield return sum;
      }
    }

    static async Task<int> SumFromOneToCountAsync(int count)
    {
      ConsoleExt.WriteLine("SumFromOneToCountAsync called!");

      var result = await Task.Run(() =>
      {
        var sum = 1;

        for (var i = 0; i < count; i++)
        {
          sum = sum + i;
        }
        return sum;
      });

      return result;
    }

    static async Task<IEnumerable<int>> SumFromOneToCountTaskIEnumerable(int count)
    {
      ConsoleExt.WriteLine("SumFromOneToCountAsyncIEnumerable called!");
      var collection = new Collection<int>();

      var result = await Task.Run(() =>
      {
        var sum = 1;

        for (var i = 0; i < count; i++)
        {
          sum = sum + i;
          collection.Add(sum);
        }
        return collection;
      });

      return result;
    }

    static async Task ConsumeAsyncSumSeqeunc(IAsyncEnumerable<int> sequence)
    {
      ConsoleExt.WriteLineAsync("ConsumeAsyncSumSeqeunc Called");

      await sequence.ForEachAsync(value =>
      {
        ConsoleExt.WriteLineAsync($"Consuming the value: {value}");

        // Simulate some delay!
        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
      });
    }

    static IEnumerable<int> ProduceAsyncSumSeqeunc(int count)
    {
      ConsoleExt.WriteLineAsync("ProduceAsyncSumSeqeunc Called");
      var sum = 1;

      for (var i = 0; i < count; i++)
      {
        sum = sum + i;

        //ConsoleExt.WriteLineAsync("Producing the value:" + i);

        // Simulate some delay!
        Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();

        yield return sum;
      }
    }

    static async Task Main()
    {
      const int count = 5;
      //ConsoleExt.WriteLine($"Starting the application with count: {count}!");

      //ConsoleExt.WriteLine("Classic sum starting.");
      //ConsoleExt.WriteLine($"Classic sum result: {SumFromOneToCount(count)}");
      //ConsoleExt.WriteLine("Classic sum completed.");

      //ConsoleExt.WriteLine("################################################");
      //ConsoleExt.WriteLine(Environment.NewLine);

      //ConsoleExt.WriteLine("Sum with yield starting.");
      //foreach (var i in SumFromOneToCountYield(count))
      //{
      //ConsoleExt.WriteLine($"Yield sum: {i}");
      //}
      //ConsoleExt.WriteLine("Sum with yield completed.");

      //ConsoleExt.WriteLine("################################################");
      //ConsoleExt.WriteLine(Environment.NewLine);

      //ConsoleExt.WriteLine("async example starting.");
      //// Sum runs asynchronously! Not enough. We need sum to be async with lazy behavior.
      //var result = await SumFromOneToCountAsync(count);
      //ConsoleExt.WriteLine("async Result: " + result);
      //ConsoleExt.WriteLine("async completed.");

      //ConsoleExt.WriteLine("################################################");
      //ConsoleExt.WriteLine(Environment.NewLine);

      //ConsoleExt.WriteLine("SumFromOneToCountAsyncIEnumerable started!");
      //var scs = await SumFromOneToCountTaskIEnumerable(count);
      //ConsoleExt.WriteLine("SumFromOneToCountAsyncIEnumerable done!");

      //foreach (var sc in scs)
      //{
      //// !!!This is not what we need we become the result in one block!!!!
      //ConsoleExt.WriteLine($"AsyncIEnumerable Result: {sc}");
      //}

      //ConsoleExt.WriteLine("################################################");
      //ConsoleExt.WriteLine(Environment.NewLine);

      ConsoleExt.WriteLine("Starting Async Streams Demo!");

      // Start a new task. Used to produce async sequence of data!
      IAsyncEnumerable<int> pullBasedAsyncSequence = ProduceAsyncSumSeqeunc(count).ToAsyncEnumerable();

      ConsoleExt.WriteLineAsync("X#X#X#X#X#X#X#X#X#X# Doing some other work X#X#X#X#X#X#X#X#X#X#");

      // Start another task; Used to consume the async data sequence!
      var consuingTask = Task.Run(() => ConsumeAsyncSumSeqeunc(pullBasedAsyncSequence));

      // Just for demo! Wait until the task is finished!
      consuingTask.Wait();
      ConsoleExt.WriteLineAsync("Async Streams Demo Done!");

      Console.ReadLine();
    }
  }
}
