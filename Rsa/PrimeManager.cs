using System.Collections.Generic;
using System.Linq;

namespace Rsa
{
    internal class PrimeManager
    {
        private const int Final = (int)1e7;
        private readonly IList<int> _primes;

        public PrimeManager()
        {
            _primes = GetPrimeNumbers();
        }

        public bool CheckNumber(int number)
        {
            return _primes.Contains(number);
        }

        public int GetPrimeHighOrEquals(int number)
        {
            var index = BinarySearch(_primes, number);

            return _primes[index] == number ? _primes[index] : _primes[index + 1];
        }

        private static int BinarySearch(IList<int> numbers, int sought)
        {
            var left = -1;
            var right = numbers.Count;

            while (left < right - 1)
            {
                var middle = (left + right) / 2;

                if (numbers[middle] < sought)
                    left = middle;
                else
                    right = middle;
            }

            return right;
        }

        private static void FillPrimeList(out List<int> primeNumbers)
        {
            var minDivider = new int[Final + 1];
            primeNumbers = new List<int>();

            for (var i = 2; i <= Final; i++)
            {
                if (minDivider[i] == 0)
                {
                    minDivider[i] = i;
                    primeNumbers.Add(i);
                }

                for (var j = 0; j < primeNumbers.Count && primeNumbers[j] <= minDivider[i] && i * primeNumbers[j] <= Final; j++)
                {
                    minDivider[i * primeNumbers[j]] = primeNumbers[j];
                }
            }
        }

        private static IList<int> GetPrimeNumbers()
        {
            FillPrimeList(out var primeNumbers);

            return primeNumbers.ToList();
        }
    }
}