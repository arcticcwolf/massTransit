using Quartz;
using System;
using System.Threading.Tasks;
using Core.Models;
using Sender.Workers;

namespace Sender.Jobs
{
    internal class SendMessageJob: IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            string text = "Greetings at Sender 1 " + DateTime.Now.Ticks ;
            await Worker._bus.Publish(new Message { Text = text });
        }
    }
}
