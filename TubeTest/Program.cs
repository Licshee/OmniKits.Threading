using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TubeTest
{
    using OmniKits.Threading;
    using OmniKits.Threading.Tasks;

    class Program
    {
        enum DataStage
        {
            Init = 1,
            Intermediate = 2,
            End = 3,
        }

        class Data
        {
            public int ThreadIndex { get; set; }
            public Guid Guid { get; set; }
            public int Number { get; set; }
            public DataStage Stage { get; set; }

            byte[] _Payload = new byte[10000000];
        }


        static async void AsyncProc()
        {
            await TaskEx.Yield();

            var tube = new TaskPulseTube<Data>();
            var pc = Environment.ProcessorCount;
            pc /= 2;

            for (var j = 0; j < pc; j++)
            {
                var idx = j;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    var rnd = new Random();

                    while (true)
                    {
                        var from = rnd.Next(2000000000);
                        //var to = from + rnd.Next(65536);
                        var to = from + 4096;
                        var guid = Guid.NewGuid();


                        tube.Push(new Data
                        {
                            ThreadIndex = idx,
                            Guid = guid,
                            Number = from,
                            Stage = DataStage.Init,
                        });

                        for (var i = from + 1; i < to; i++)
                        {
                            switch (i % 65536)
                            {
                                case 0:
                                case 4:
                                case 16:
                                case 64:
                                case 256:
                                case 1024:
                                    continue;
                            }

                            tube.Push(new Data
                            {
                                ThreadIndex = idx,
                                Guid = guid,
                                Number = i,
                                Stage = DataStage.Intermediate,
                            });
                        }

                        tube.Push(new Data
                        {
                            ThreadIndex = idx,
                            Guid = guid,
                            Number = to,
                            Stage = DataStage.End,
                        });
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
                        var ti = data.ThreadIndex;
                        var stage = data.Stage;
                        var number = data.Number;

                        if (stage == DataStage.End)
                        {
                            Console.WriteLine($"[{stage}]\t{idx} - {ti} : {data.Guid} - {number}");
                            dict.Remove(ti);
                            continue;
                        }

                        try
                        {
                            Data old;
                            if (dict.TryGetValue(ti, out old))
                            {
                                var oldNumber = old.Number;

                                if (stage == DataStage.Intermediate && number == oldNumber + 1)
                                    continue;

                                Console.WriteLine($"[Error]\t[{stage}]\t{idx} - {ti}: {data.Guid} - {oldNumber} -> {number}");
                            }
                            else
                            {
                                Console.WriteLine($"[{stage}]\t{idx} - {ti} : {data.Guid} - {number}");
                            }
                        }
                        finally
                        {
                            dict[ti] = data;
                        }
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
