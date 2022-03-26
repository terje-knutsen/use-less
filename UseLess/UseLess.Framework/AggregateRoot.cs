﻿namespace UseLess.Framework
{
    public abstract class AggregateRoot<TId> : IInternalEventHandler
    {
        private readonly List<object> changes;
        protected AggregateRoot() => changes = new List<object>();
        protected void TryApply(Action testCase, object @event)
        {
            testCase();
            Apply(@event);
        }
        protected void Apply(object @event) 
        {
            When(@event);
            EnsureValidState();
            changes.Add(@event);
        }
        protected abstract void When(object @event);
        protected abstract void EnsureValidState();
        protected void ApplyToEntity(IInternalEventHandler? entity, object @event)
            => entity?.Handle(@event);
        protected void ApplyToEntities(IEnumerable<IInternalEventHandler> entities, object @event)
            => entities.ToList().ForEach(x => x.Handle(@event));
        public void Load(IEnumerable<object> history) 
        {
            foreach(var e in history) 
            {
                When(e);
                Version++;
            }
        }
        public TId Id { get; protected set; }
        public int Version { get; private set; } = -1;
        public IEnumerable<object> GetChanges() => changes.AsEnumerable();
        public void Clear() => changes.Clear();

        public void Handle(object @event) => Apply(@event);
    }
}
