using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;

namespace FrontEnd.Worker
{
    public class BackgroundJob
    {
        private static BackgroundJob instance = new BackgroundJob();

        private ConcurrentQueue<string> queue;

        private BackgroundJob()
        {
            queue = new ConcurrentQueue<string>();
        }

        public static BackgroundJob Current
        {
            get
            {
                return instance;
            }
        }

        public void Schedule(string value)
        {
            queue.Enqueue(value);
        }

        private Task worker = null;
        private CancellationTokenSource token = null;

        public void Start()
        {
            lock(this)
            {
                if (token == null)
                {
                    token = new CancellationTokenSource();
                    worker = Task.Run(async () => await RunLoop(), token.Token);
                }
            }
        }

        public void Stop()
        {
            lock(this)
            {
                if (token != null)
                {
                    token.Cancel();
                    worker.Wait();

                    token = null;
                    worker = null;
                }
            }
        }

        private async Task RunLoop()
        {
            while(true)
            {
                if (token.IsCancellationRequested)
                    break;

                if (queue.IsEmpty)
                {
                    await Task.Delay(200);
                    continue;
                }

                string value;
                if(queue.TryDequeue(out value))
                {
                    try
                    {
                        DoWork(value);
                    }
                    catch { }
                }
            }
        }

        private void DoWork(string value)
        {
            var c = new WebClient();
            c.DownloadString(value);
        }
    }
}