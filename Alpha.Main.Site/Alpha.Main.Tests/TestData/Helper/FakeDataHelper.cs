namespace Alpha.Main.Tests.TestData.Helper
{
    using ConstructorQuery;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;
    using System.Collections.Generic;

    public static class FakeDataHelper
    {
        public static T Create<T>(int ctorParamsAmount = 0)
        {
            var fixture = new Fixture();

            fixture = CustomizeConstructorInvoker<T>(fixture, ctorParamsAmount);

            return fixture.Create<T>();
        }

        public static T Create<T>(string paramName, dynamic paramValue, int ctorParamsAmount = 0)
        {
            var fixture = new Fixture();

            fixture = CustomizeConstructorInvoker<T>(fixture, ctorParamsAmount);

            fixture = CustomizeParamerter(fixture, paramName, paramValue);

            return fixture.Create<T>();
        }

        public static T Create<T>(IDictionary<string, dynamic> parameters, int ctorParamsAmount = 0)
        {
            var fixture = new Fixture();

            fixture = CustomizeConstructorInvoker<T>(fixture, ctorParamsAmount);

            foreach (var param in parameters)
            {
                fixture = CustomizeParamerter(fixture, param.Key, param.Value);
            }

            return fixture.Create<T>();
        }

        public static IEnumerable<T> CreateMany<T>(int ctorParamsAmount = 0)
        {
            var fixture = new Fixture();

            fixture = CustomizeConstructorInvoker<T>(fixture, ctorParamsAmount);

            return fixture.CreateMany<T>();
        }

        private static Fixture CustomizeConstructorInvoker<T>(Fixture fixture, int ctorParamsAmount)
        {
            fixture.Customize<T>(c => c.FromFactory(GetConstructorInvoker(ctorParamsAmount)));

            return fixture;
        }

        private static Fixture CustomizeParamerter(Fixture fixture, string paramName, dynamic paramValue)
        {
            fixture.Customizations.Add(new FilteringSpecimenBuilder(
                new FixedBuilder(paramValue),
                new ParameterSpecification(
                    paramValue.GetType(), paramName)));

            return fixture;
        }

        private static MethodInvoker GetConstructorInvoker(int ctorParamsAmount)
        {
            return ctorParamsAmount > 0
                ? new MethodInvoker(new ParametersAmountConstructorQuery(ctorParamsAmount))
                : new MethodInvoker(new GreedyConstructorQuery());
        }
    }
}
