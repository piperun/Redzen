﻿using Redzen.Random;

namespace Redzen.UnitTests.Random
{
    public class Xoshiro512StarStarRandomTests : RandomSourceTests
    {
        protected override IRandomSource CreateRandomSource()
        {
            return new Xoshiro512StarStarRandom(1);
        }
    }
}
