using Eveneum;
using Moq;
using NUnit.Framework;
using SpecsFor.Core;
using SpecsFor.StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Framework;
using UseLess.Messages;
using Useless.AzureStore;
using UseLess.Domain.Values;
using Should;
using Nito.AsyncEx;

namespace Useless.Services.Tests
{
    internal class AggregateStoreSpecs
    {
        public class When_load_budget_given_event_stream_exist : SpecsFor<AggregateStore>
        {
            private Budget budget;

            protected override void Given()
            {
                Given<EventStreamExist>();
                base.Given();
            }
            protected override void When()
            {
                AsyncContext.Run(async () => budget = await SUT.Load<Budget, BudgetId>(BudgetId.From(Guid.NewGuid())));
            }
            [Test]
            public void Then_budget_should_be_loaded() 
            {
                budget.ShouldNotBeNull();
            }
        }
        public class When_load_budget_given_events_are_not_set : SpecsFor<AggregateStore>
        {
            protected override void Given()
            {
                Given<EventCollectionIsNotSet>();
                base.Given();
            }
            [Test]
            public void Then_default_budget_should_return() 
            {
                Assert.Throws<InvalidDataException>(() => AsyncContext.Run(async () =>  await SUT.Load<Budget, BudgetId>(BudgetId.From(Guid.NewGuid()))));
            }
        }
        public class When_load_budget_given_event_collection_are_empty : SpecsFor<AggregateStore>
        {
            protected override void Given()
            {
                Given<EventCollectionAreEmpty>();
                base.Given();
            }
            [Test]
            public void Then_invalid_data_exception_should_be_thrown() 
            {
                Assert.Throws<InvalidDataException>(()=> AsyncContext.Run(async ()=> await SUT.Load<Budget,BudgetId>(BudgetId.From(Guid.NewGuid()))));
            }
        }
        public class When_load_budget_given_events_are_of_wrong_format : SpecsFor<AggregateStore>
        {
            protected override void Given()
            {
                Given<EventsAreOfWrongFormat>();
                base.Given();
            }
            [Test]
            public void Then_exception_should_be_thrown() 
            {
                Assert.Throws<InvalidDataException>(() => AsyncContext.Run(async () => await SUT.Load<Budget, BudgetId>(BudgetId.From(Guid.NewGuid()))));
            }
        }
        public class When_load_budget_given_loaded_with_null : SpecsFor<AggregateStore>
        {
            [Test]
            public void Then_should_throw_invalid_argument_exception ()
            {
                Assert.Throws<ArgumentException>(() => AsyncContext.Run(async () => await SUT.Load<Budget, BudgetId>(default(BudgetId))));
            }
        }
        private class EventStreamExist : IContext<AggregateStore>
        {
            public void Initialize(ISpecs<AggregateStore> state)
            {
                state.GetMockFor<IReadStream>().Setup(x => x.ReadStream(It.IsAny<string>(), It.IsAny<ReadStreamOptions>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new StreamResponse(eventStream,false,0f)));
            }
            private static Guid BudgetId = Guid.NewGuid();
            private static Eveneum.Stream eventStream = new Eveneum.Stream()
            {
                Events = new EventData[]
                {
                    new EventData(
                        BudgetId.ToString(),
                        new Events.BudgetCreated(BudgetId,"Name",BudgetState.Active.Name,new DateTime(2022,2,2)),
                        null, 0,"a time stamp",false),

                }
            };
        }
        private class EventCollectionIsNotSet : IContext<AggregateStore>
        {
            public void Initialize(ISpecs<AggregateStore> state)
            {
                state.GetMockFor<IReadStream>().Setup(x => x.ReadStream(It.IsAny<string>(), It.IsAny<ReadStreamOptions>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new StreamResponse(new Eveneum.Stream(), true, 0f)));
            }
        }
        private class EventCollectionAreEmpty : IContext<AggregateStore>
        {
            public void Initialize(ISpecs<AggregateStore> state)
            {
                state.GetMockFor<IReadStream>().Setup(x => x.ReadStream(It.IsAny<string>(), It.IsAny<ReadStreamOptions>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new StreamResponse(new Eveneum.Stream()
                    {
                        Events = new EventData[0]
                    }, true, 0f)));
            }
        }
        private class EventsAreOfWrongFormat : IContext<AggregateStore>
        {
            public void Initialize(ISpecs<AggregateStore> state)
            {
                state.GetMockFor<IReadStream>().Setup(x => x.ReadStream(It.IsAny<string>(), It.IsAny<ReadStreamOptions>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new StreamResponse(new Eveneum.Stream()
                    {
                        Events = new EventData[] {new EventData() { Body = new object()} }
                    }, true, 0f)));
            }
        }
    }
}
