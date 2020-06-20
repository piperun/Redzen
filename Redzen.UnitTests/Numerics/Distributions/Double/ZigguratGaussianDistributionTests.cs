﻿using Redzen.Numerics.Distributions.Double;
using Xunit;

namespace Redzen.UnitTests.Numerics.Distributions.Double
{
    public class ZigguratGaussianDistributionTests
    {
        #region Test Methods

        [Fact]
        public void SimpleStats()
        {
            var sampler = new ZigguratGaussianSampler(0.0, 1.0);
            GaussianDistributionTestUtils.TestSimpleStats(sampler);
        }

        [Theory]
        [InlineData(0.0, 1.0)]      // Standard normal.
        [InlineData(10.0, 1.0)]     // Non-zero mean tests.
        [InlineData(-100.0, 1.0)]   // 
        [InlineData(0.0, 0.2)]      // Non-1.0 standard deviations
        [InlineData(0.0, 5.0)]      //
        [InlineData(10.0, 2.0)]     // Non-zero mean and non-1.0 standard deviation.
        [InlineData(-10.0, 3.0)]
        public void TestCumulativeDistribution(double mean, double stdDev)
        {
            var sampler = new ZigguratGaussianSampler(mean, stdDev);
            GaussianDistributionTestUtils.TestDistribution(sampler, mean, stdDev);
        }

        #endregion
    }
}
