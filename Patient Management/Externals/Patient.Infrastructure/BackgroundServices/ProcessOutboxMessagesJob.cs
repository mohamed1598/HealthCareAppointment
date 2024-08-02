using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Patient.Persistence.DataSource;
using Patient.Persistence.Outbox;
using Quartz;
using Shared.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Infrastructure.BackgroundServices;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(PatientDbContext _context, ISender _sender) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _context
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync();
        
        foreach(var outboxMessage in messages)
        {
            try
            {
                var domainEvent = JsonConvert
                .DeserializeObject(outboxMessage.Content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                if (domainEvent is null) continue;

                await _sender.Send(domainEvent, context.CancellationToken);

                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception)
            {

            }
            
        }

        await _context.SaveChangesAsync();
    }
}
