using FubuCore;
using FubuCore.Reflection;
using FubuValidation.Fields;
using FubuValidation.Strategies;
using FubuValidation.Tests.Models;
using NUnit.Framework;
using System.Linq;

namespace FubuValidation.Tests.Fields
{
    [TestFixture]
    public class when_evaluating_greater_or_equal_to_zero_rule
    {
        private SimpleModel theModel;
        private GreaterOrEqualToZeroRule theRule;

        [SetUp]
        public void BeforeEach()
        {
            theRule = new GreaterOrEqualToZeroRule();
            theModel = new SimpleModel();

        }

        [Test]
        public void should_register_message_if_value_is_less_than_zero()
        {
            theModel.GreaterOrEqualToZero = -1;
            theRule.Validate(theModel, x => x.GreaterOrEqualToZero)
                .MessagesFor<SimpleModel>(x => x.GreaterOrEqualToZero).Messages.Select(x => x.StringToken)
                .ShouldHaveTheSameElementsAs(ValidationKeys.GREATER_OR_EQUAL_TO_ZERO);
        }

        [Test]
        public void should_not_register_a_message_if_value_is_zero()
        {
            theModel.GreaterOrEqualToZero = 0;
            theRule.Validate(theModel, x => x.GreaterOrEqualToZero)
                .MessagesFor<SimpleModel>(x => x.GreaterOrEqualToZero).Messages.ShouldBeEmpty();
        }

        [Test]
        public void should_not_register_a_message_if_value_is_greater_than_zero()
        {
            theModel.GreaterOrEqualToZero = 10;
            theRule.Validate(theModel, x => x.GreaterOrEqualToZero)
                .MessagesFor<SimpleModel>(x => x.GreaterOrEqualToZero).Messages.ShouldBeEmpty();
        }
    }
}