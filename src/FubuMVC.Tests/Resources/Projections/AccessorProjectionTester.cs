using FubuMVC.Core.Resources.Media;
using FubuMVC.Core.Resources.Media.Projections;
using FubuMVC.Core.Resources.Media.Xml;
using FubuMVC.Core.Urls;
using FubuTestingSupport;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Resources.Projections
{
    [TestFixture]
    public class AccessorProjectionTester
    {
        private AccessorProjection<ValueTarget, int> theAccessorProjection;
        private SimpleValues<ValueTarget> _theValues;
        private XmlAttCentricMediaNode theMediaNode;

        [SetUp]
        public void SetUp()
        {
            theAccessorProjection = AccessorProjection<ValueTarget, int>.For(x => x.Age);
            _theValues = new SimpleValues<ValueTarget>(new ValueTarget
            {
                Age = 37
            });

            theMediaNode = XmlAttCentricMediaNode.ForRoot("root");
        }

        [Test]
        public void project_the_property_with_default_node_name()
        {
            theAccessorProjection.WriteValue(new ProjectionContext<ValueTarget>(null, _theValues), theMediaNode);

            theMediaNode.Element.GetAttribute("Age").ShouldEqual("37");
        }

        [Test]
        public void project_the_property_with_a_different_node_name()
        {
            theAccessorProjection.Name("CurrentAge");

            theAccessorProjection.WriteValue(new ProjectionContext<ValueTarget>(null, _theValues), theMediaNode);

            theMediaNode.Element.GetAttribute("CurrentAge").ShouldEqual("37");
        }

        [Test]
        public void project_the_property_with_formatting()
        {
            theAccessorProjection.FormattedBy(age => "*" + age + "*");
            theAccessorProjection.WriteValue(new ProjectionContext<ValueTarget>(null, _theValues), theMediaNode);

            theMediaNode.Element.GetAttribute("Age").ShouldEqual("*37*");
        }

        [Test]
        public void write_url_for_model()
        {
            var urls = new StubUrlRegistry();
            var services = MockRepository.GenerateMock<IServiceLocator>();
            services.Stub(x => x.GetInstance<IUrlRegistry>()).Return(urls);

            theAccessorProjection.WriteUrlFor(age => new CreateValueTarget{
                Name = age.ToString()
            });

            var expectedUrl = urls.UrlFor(new CreateValueTarget{
                Name = "37"
            });

            theAccessorProjection.WriteValue(new ProjectionContext<ValueTarget>(services, _theValues), theMediaNode);

            theMediaNode.Element.GetAttribute("Age").ShouldEqual(expectedUrl);
        }
    }

    public class ValueTarget
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class CreateValueTarget
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("CreateValueTarget: {0}", Name);
        }
    }
}