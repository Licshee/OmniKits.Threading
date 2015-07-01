using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TubeTest
{
    using OmniKits.Threading;

    class Program
    {
        class Data
        {
            public int ThreadIndex { get; set; }
            public Guid Guid { get; set; }
            public int Number { get; set; }

            byte[] _Payload = new byte[10000000];
        }


        static async void AsyncProc()
        {
            await TaskEx.Yield();

            var tube = new NanaTubeAsync<Data>();
            var pc = Environment.ProcessorCount / 2;

            for (var j = 0; j < pc; j++)
            {
                var idx = j;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    var rnd = new Random();

                    while (true)
                    {
                        var from = rnd.Next(2000000000);
                        var to = from + rnd.Next(65536);
                        var guid = Guid.NewGuid();

                        for (var i = from; i < to; i++)
                        {
                            //var r = rnd.Next(65536);
                            //if (r == 0)
                            //    i++;
                            //else if (r == 1)
                            //    continue;

                            tube.Push(new Data
                            {
                                ThreadIndex = idx,
                                Guid = guid,
                                Number = i,
                            });
                        }
                    }
                });
            };

            for (var j = 0; j < pc; j++)
            {
                var idx = j;
                ThreadPool.QueueUserWorkItem(async _ =>
                {
                    await TaskEx.Yield();

                    var dict = new Dictionary<int, Data>();

                    foreach (var data in tube.AsEnumerable<Data>())
                    {
                        Data old;
                        if (dict.TryGetValue(data.ThreadIndex, out old) && data.Guid == old.Guid)
                        {
                            if (data.Number != old.Number + 1)
                            {
                                Console.WriteLine("[Error] {0} - {1}: {2} - {3} -> {4}", idx, data.ThreadIndex, data.Guid, old.Number, data.Number);
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} - {1}: {2} - {3}", idx, data.ThreadIndex, data.Guid, data.Number);
                        }

                        dict[data.ThreadIndex] = data;
                    }
                });
            };
        }

        static void Main(string[] args)
        {
            AsyncProc();

            Console.ReadKey();
        }
    }
}
