using FreshGuard.ColdTrack.Platform.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace FreshGuard.ColdTrack.Platform.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}