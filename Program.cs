using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static System.Console;

namespace tpl_presentation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bb = new BroadcastBlock<int>(c => c);
            var log = new ActionBlock<int>(x => WriteLine($"Log: {x}"));
            var save = new ActionBlock<int>(x => WriteLine($"Save: {x}"));
            bb.LinkTo(log);
            bb.LinkTo(save);

            await bb.SendAsync(2);

            await Task.Delay(3000);
        }
    }
}
