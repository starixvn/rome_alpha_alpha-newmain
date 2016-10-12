namespace Alpha.Main.Tests.TestData.Helper
{
    using ConstructorQuery;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    public static class FakeDataHelper
    {
        public static T Create<T>(string paramName, dynamic paramValue, int ctorParamsAmount = 0)
        {
            var fixture = new Fixture();

            fixture.Customize<T>(c => c.FromFactory(GetConstructorInvoker(ctorParamsAmount)));

            fixture.Customizations.Add(new FilteringSpecimenBuilder(
                new FixedBuilder(paramValue),
                new ParameterSpecification(
                    paramValue.GetType(), paramName)));

            return fixture.Create<T>();
        }

        private static MethodInvoker GetConstructorInvoker(int ctorParamsAmount)
        {
            return ctorParamsAmount > 0
                ? new MethodInvoker(new ParametersAmountConstructorQuery(ctorParamsAmount))
                : new MethodInvoker(new GreedyConstructorQuery());
        }
    }
}