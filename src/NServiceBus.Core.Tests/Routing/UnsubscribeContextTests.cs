﻿namespace NServiceBus.Core.Tests.Routing
{
    using Extensibility;
    using NUnit.Framework;

    [TestFixture]
    public class UnsubscribeContextTests
    {
        [Test]
        public void ShouldShallowCloneContext()
        {
            var context = new ContextBag();
            context.Set("someKey", "someValue");
            
            var testee = new UnsubscribeContext(new RootContext(null, null, null), typeof(object), context);
            testee.Extensions.Set("someKey", "updatedValue");
            testee.Extensions.Set("anotherKey", "anotherValue");

            string value;
            string anotherValue;
            context.TryGet("someKey", out value);
            Assert.AreEqual("someValue", value);
            Assert.IsFalse(context.TryGet("anotherKey", out anotherValue));
            string updatedValue;
            string anotherValue2;
            testee.Extensions.TryGet("someKey", out updatedValue);
            testee.Extensions.TryGet("anotherKey", out anotherValue2);
            Assert.AreEqual("updatedValue", updatedValue);
            Assert.AreEqual("anotherValue", anotherValue2);
        }

        [Test]
        public void ShouldNotMergeOptionsToParentContext()
        {
            var context = new ContextBag();
            context.Set("someKey", "someValue");

            var parentContext = new RootContext(null, null, null);

            new UnsubscribeContext(parentContext, typeof(object), context);

            string parentContextValue;
            var valueFound = parentContext.TryGet("someKey", out parentContextValue);

            Assert.IsFalse(valueFound);
        }
    }
}