﻿
namespace Redzen.Random
{
    public interface IUniformDistribution<T> : IContinuousDistribution<T>
        where T : struct
    {
        /// <summary>
        /// Get a sample from the uniform distribution with interval [0, scale).
        /// </summary>
        double Sample(double scale);

        /// <summary>
        /// Get a sample from the uniform distribution with interval [0, scale), or [-scale, scale) if 'signed' is true.
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="signed"></param>
        /// <returns></returns>
        double Sample(double scale, bool signed);

        /// <summary>
        /// Get a sample from the unit uniform distribution, i.e. with interval [0,1).
        /// </summary>
        double SampleUnit();

        /// <summary>
        /// Get a sample from the signed unit uniform distribution, i.e. with interval [-1,1).
        /// </summary>
        double SampleUnitSigned();
    }
}
