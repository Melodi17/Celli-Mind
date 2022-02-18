using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celli_Mind
{
    public class RandomController
    {
        private Random random;
        public RandomController()
        {
            random = new Random();
        }

        public bool Chance(int chance = 50)
            => random.Next(100) < chance;

        public int Number(int min, int max)
            => random.Next(min, max);

        public int Number(int max)
            => random.Next(max);
    }
}
