// <copyright file="AbstractUnitTest.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>
namespace Safran.BIADemo.Test.Tests
{
    using BIA.Net.Core.Test;
    using Microsoft.Extensions.DependencyInjection;
    using Safran.BIADemo.Infrastructure.Data;
    using Safran.BIADemo.Test.Data;
    using Safran.BIADemo.Test.IoC;

    /// <summary>
    /// Base class for all unit tests of the project.
    /// </summary>
    /// <seealso cref="BIAAbstractUnitTest{TMockEF, TDbContext}" />
    public abstract class AbstractUnitTest : BIAAbstractUnitTest<MockEntityFrameworkInMemory, DataContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractUnitTest"/> class.
        /// </summary>
        /// <param name="isInitDB">Shall we initialize the database with some default values?</param>
        protected AbstractUnitTest(bool isInitDB)
            : base(isInitDB)
        {
            // Initialize AbstractUnitTest.isInitDB, which is used to either start each test of this test suite:
            // - with an empty DB
            // - or with some default data in the DB (inserted through IMockEntityFramework.InitDefaultData())
        }

        /// <inheritdoc cref="BIAAbstractUnitTest{TMockEF, TDbContext}.InitServiceCollection"/>
        protected override void InitServiceCollection()
        {
            this.servicesCollection = new ServiceCollection();

            // Add IoC for components specific to your project in this method.
            IocContainerTest.ConfigureContainerTest(this.servicesCollection);
        }
    }
}
