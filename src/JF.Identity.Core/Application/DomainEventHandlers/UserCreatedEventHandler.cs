using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JF.Domain.Event;
using JF.Identity.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JF.Identity.Core.Application.DomainEventHandlers
{
    public class UserCreatedEventHandler : DomainEventHandler<UserCreatedEvent>
    {
        private readonly ILogger _logger;
        public UserCreatedEventHandler(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            //_logger = _provider
        }

        public override Task RecieveAsync(UserCreatedEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
