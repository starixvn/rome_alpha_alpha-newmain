namespace Alpha.Main.Tests.TestData.Helper.ConstructorQuery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ploeh.AutoFixture.Kernel;

    public class ParametersAmountConstructorQuery : IMethodQuery
    {
        private readonly int paramsAmount;

        public ParametersAmountConstructorQuery(int paramsAmount)
        {
            this.paramsAmount = paramsAmount;
        }

        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return from constructorInfo in type.GetConstructors()
                   where constructorInfo.GetParameters().Length == paramsAmount
                   select new ConstructorMethod(constructorInfo) as IMethod;
        }
    }
}