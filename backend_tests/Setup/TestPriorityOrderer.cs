using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace backend_tests.Setup
{
    //! Test Priority is having zero effect on the execution of integration tests. Fix ASAP
    /// <summary>
    /// PriorityOrderer class to establish an order for tests to be run in.
    /// Based on this solution <a href = https://logcorner.com/asp-net-web-api-core-integration-testing-using-inmemory-entityframeworkcore-sqlite-or-localdb-and-xunit2/></a>
    /// </summary>
    public class TestPriorityOrderer : ITestCaseOrderer
    {

        /// <summary>
        /// Constant representing the TestPriorityOrderer's type name.
        /// </summary>
        public const string TYPE_NAME = "backend_tests.Setup.TestPriorityOrderer";

        /// <summary>
        /// Constant representing the TestPriorityOrderer's assembly name.
        /// </summary>
        public const string ASSEMBLY_NAME = "backend_tests";

        /// <summary>
        /// Organizes tests in a specific order
        /// </summary>
        /// <param name="testCases">list that contains all of the tests to be run</param>
        /// <typeparam name="TTestCase">type of the test</typeparam>
        /// <returns>IEnumerable of all TestCases ordered by priority</returns>
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var sortedTestMethods = new SortedDictionary<int, TTestCase>(new DuplicateKeyComparer<int>());

            foreach (TTestCase testCase in testCases)
            {
                IAttributeInfo attribute = testCase.TestMethod.Method
                .GetCustomAttributes(typeof(TestPriorityAttribute)
                .AssemblyQualifiedName)
                .FirstOrDefault();

                var priority = attribute.GetNamedArgument<int>("Priority");
                sortedTestMethods.Add(priority, testCase);
            }

            return sortedTestMethods.Values;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    /// <summary>
    /// Represents the priority of a test
    /// </summary>
    public class TestPriorityAttribute : Attribute
    {
        private int priority { get; }

        public TestPriorityAttribute(int priority)
        {
            this.priority = priority;
        }
    }

    /// <summary>
    /// DuplicateKeyComparer class to compare test order priorities
    /// </summary>
    /// <typeparam name="TKey">type of the key that is being compared</typeparam>
    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
    {
        public int Compare(TKey tkeyX, TKey tkeyY)
        {

            int result = tkeyX.CompareTo(tkeyY);

            if (result == 0)
            {
                return 1;
            }
            else
            {
                return result;
            }
        }
    }
}