namespace Alpha.Main.Tests.Helpers
{
    using System;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    public static class FakeDataHelper
    {
        public static T Create<T>(string paramName, dynamic paramValue)
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new FixedBuilder(paramValue),
                    new ParameterSpecification(
                        paramValue.GetType(), paramName)));

            return fixture.Create<T>();
        }
    }
}