namespace UseLess.Framework
{
    public abstract class Entity<TId> : IInternalEventHandler
    {
        private readonly Action<object> applier;
        public TId Id { get; protected set; }
        protected Entity(Action<object> applier)
        {
            this.applier = applier ?? throw new ArgumentNullException(nameof(applier));
        }
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
