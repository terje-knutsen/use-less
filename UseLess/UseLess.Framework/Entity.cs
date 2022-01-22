namespace UseLess.Framework
{
    public abstract class Entity<TId> : IInternalEventHandler
    {
        private Action<object> applier;
        public TId Id { get; protected set; }
        public void SetApplier(IInternalEventHandler handler) => applier = handler.Handle;
        protected Entity(Action<object> applier) => this.applier = applier;
        protected abstract void When(object @event);
        protected void Apply(object @event) 
        {
            When(@event);
            applier(@event);
        }
        public void Handle(object @event)
        => When(@event);
    }
}
