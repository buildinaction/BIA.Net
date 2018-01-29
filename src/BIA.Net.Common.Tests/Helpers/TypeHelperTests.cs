using BIA.Net.Common.Helpers;
using NUnit.Framework;

namespace BIA.Net.Common.Tests.Helpers
{
    /// <summary>
    /// Unit Test class for type helper.
    /// </summary>
    [TestFixture]
    public static class TypeHelperTests
    {
        /// <summary>
        /// Unit test for NameOf method.
        /// </summary>
        [Test]
        public static void TestNameOf()
        {
            // Member Access
            Assert.AreEqual("StaticProperty", TypeHelper.NameOf(() => LocalTestType.StaticProperty));
            Assert.AreEqual("StaticProperty", TypeHelper.NameOf(() => LocalTestType.StaticProperty, false));
            Assert.AreEqual("MainProperty", TypeHelper<LocalTestType>.NameOf(ltt => ltt.MainProperty));
            Assert.AreEqual("MainProperty", TypeHelper<LocalTestType>.NameOf(ltt => ltt.MainProperty, false));
            Assert.AreEqual("SecondLevel.IsLevel2", TypeHelper<LocalTestType>.NameOf(ltt => ltt.SecondLevel.IsLevel2));
            Assert.AreEqual("IsLevel2", TypeHelper<LocalTestType>.NameOf(ltt => ltt.SecondLevel.IsLevel2, false));
            Assert.AreEqual("SecondLevel.ThirdLevel.IsLevel3", TypeHelper<LocalTestType>.NameOf(ltt => ltt.SecondLevel.ThirdLevel.IsLevel3));
            Assert.AreEqual("IsLevel3", TypeHelper<LocalTestType>.NameOf(ltt => ltt.SecondLevel.ThirdLevel.IsLevel3, false));

            // Method Access
            Assert.AreEqual("Foo", TypeHelper.NameOf(() => LocalTestType.Foo()));
            Assert.AreEqual("Foo", TypeHelper.NameOf(() => LocalTestType.Foo(), false));
            Assert.AreEqual("Foo", TypeHelper<LocalTestType>.NameOf(ltt => LocalTestType.Foo()));
            Assert.AreEqual("Foo", TypeHelper<LocalTestType>.NameOf(ltt => LocalTestType.Foo(), false));
            Assert.AreEqual("SecondLevel.Bar", TypeHelper<LocalTestType>.NameOf(ltt => ltt.SecondLevel.Bar()));
            Assert.AreEqual("Bar", TypeHelper<LocalTestType>.NameOf(ltt => ltt.SecondLevel.Bar(), false));
            Assert.AreEqual("SecondLevel.ThirdLevel.FooBar", TypeHelper<LocalTestType>.NameOf(ltt => ltt.SecondLevel.ThirdLevel.FooBar()));
            Assert.AreEqual("FooBar", TypeHelper<LocalTestType>.NameOf(ltt => ltt.SecondLevel.ThirdLevel.FooBar(), false));
        }
    }

#pragma warning disable SA1402 // File may only contain a single class

    /// <summary>
    /// Test class
    /// </summary>
    internal class LocalTestType
    {
        /// <summary>
        /// Gets or sets the static property.
        /// </summary>
        public static int StaticProperty { get; set; }

        /// <summary>
        /// Gets or sets the main property.
        /// </summary>
        public int MainProperty { get; set; }

        /// <summary>
        /// Gets or sets the second level.
        /// </summary>
        public Level2LocalTestType SecondLevel { get; set; }

        /// <summary>
        /// Foo method
        /// </summary>
        /// <returns>Hello World!</returns>
        public static string Foo()
        {
            return "Hello World!";
        }
    }

    /// <summary>
    ///  Test class of Level 2
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Unit test data.")]
    internal class Level2LocalTestType
    {
        /// <summary>
        /// Gets or sets a value indicating whether level 2
        /// </summary>
        public bool IsLevel2 { get; set; }

        /// <summary>
        /// Gets or sets the third level.
        /// </summary>
        public Level3LocalTestType ThirdLevel { get; set; }

        /// <summary>
        /// Bar method
        /// </summary>
        /// <returns>Hello World! from Level 2</returns>
        public string Bar()
        {
            return this.IsLevel2 ? "Hello World! from Level 2" : "Not Level 2";
        }
    }

    /// <summary>
    ///  Test class of Level 3
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Unit test data.")]
    internal class Level3LocalTestType
    {
        /// <summary>
        /// Gets or sets a value indicating whether level 3
        /// </summary>
        public bool IsLevel3 { get; set; }

        /// <summary>
        /// FooBar method
        /// </summary>
        /// <returns>Hello World! from Level 3</returns>
        public string FooBar()
        {
            return this.IsLevel3 ? "Hello World! from Level 3" : "Not Level 3";
        }
    }

#pragma warning restore SA1402 // File may only contain a single class
}