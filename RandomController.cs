using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celli_Mind
{
    public class RandomController
    {
        /// <summary>
        /// Random with seed to read from
        /// </summary>
        private Random random;
        public RandomController()
        {
            random = new Random();
        }

        /// <summary>
        /// Random chance from a percentage
        /// </summary>
        /// <param name="chance">Percentage of 100</param>
        /// <returns>Whether random was smaller than the chance</returns>
        public bool Chance(int chance = 50)
            => random.Next(100) < chance;

        /// <summary>
        /// Random number
        /// </summary>
        /// <param name="min">Minimum that the result can be</param>
        /// <param name="max">Maximum that the result can be</param>
        /// <returns>A random number between <paramref name="min"/> and <paramref name="max"/></returns>
        public int Number(int min, int max)
            => random.Next(min, max);

        /// <summary>
        /// Random number
        /// </summary>
        /// <param name="max">Maximum that the result can be</param>
        /// <returns>A random number between 0 and <paramref name="max"/></returns>
        public int Number(int max)
            => random.Next(max);
    }
}
