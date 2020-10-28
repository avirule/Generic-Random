using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Generic_Random
{
    public class GenericRandom
    {
        //
        // Private Constants
        //
        private const int _MBIG = int.MaxValue;
        private const int _MSEED = 161803398;
        private const int _MZ = 0;

        //
        // Member Variables
        //
        private int _NextInt;
        private int _NextIntP;
        private int[] _SeedArray = new int[56];

        public GenericRandom(int seed)
        {
            int ii = 0;
            int mj, mk = 1;

            //Initialize our Seed array.
            int subtraction = seed == int.MinValue ? int.MaxValue : Math.Abs(seed);
            _SeedArray[55] = mj = _MSEED - subtraction;

            for (int i = 1; i < 55; i++)
            {
                //Apparently the range [1..55] is special (Knuth) and so we're wasting the 0'th position.
                if ((ii += 21) >= 55) ii -= 55;
                _SeedArray[ii] = mk;
                mk = mj - mk;
                if (mk < 0) mk += _MBIG;
                mj = _SeedArray[ii];
            }

            for (int k = 1; k < 5; k++)
            {
                for (int i = 1; i < 56; i++)
                {
                    int n = i + 30;
                    if (n >= 55) n -= 55;
                    _SeedArray[i] -= _SeedArray[1 + n];
                    if (_SeedArray[i] < 0) _SeedArray[i] += _MBIG;
                }
            }

            _NextInt = 0;
            _NextIntP = 21;
        }

        public unsafe T Next<T>() where T : unmanaged
        {
            if (typeof(T) == typeof(float))
            {
                float sample = InternalSample() * (1f / _MBIG);
                return (T)(object)sample;
            }
            else if (typeof(T) == typeof(double))
            {
                double sample = InternalSample() * (1d / _MBIG);
                return (T)(object)sample;
            }
            else if (typeof(T) == typeof(decimal))
            {
                decimal sample = InternalSample() * (1.0m / _MBIG);
                return (T)(object)sample;
            }
            else if (typeof(T) == typeof(long))
            {
                // cast low to uint to avoid losing sign
                uint sampleLow = (uint)InternalSample();
                int sampleHigh = InternalSample();

                long sample = ((long)sampleHigh << 32) | sampleLow;
                return (T)(object)sample;
            }
            else if (typeof(T) == typeof(ulong))
            {
                // cast low to uint to avoid losing sign
                uint sampleLow = (uint)InternalSample();
                int sampleHigh = InternalSample();

                ulong sample = ((ulong)sampleHigh << 32) | sampleLow;
                return (T)(object)sample;
            }
            else if ((typeof(T) == typeof(int))
                     || (typeof(T) == typeof(uint))
                     || (typeof(T) == typeof(short))
                     || (typeof(T) == typeof(ushort))
                     || (typeof(T) == typeof(byte))
                     || (typeof(T) == typeof(sbyte)))
            {
                int sample = InternalSample();
                return Unsafe.As<int, T>(ref sample);
            }
            else
            {
                Span<byte> data = stackalloc byte[sizeof(T)];
                Next(data);

                return MemoryMarshal.Read<T>(data);
            }

            // -or-
            // else throw new NotSupportedException("T is of unsupported type.");
        }

        public void Next<T>(Span<T> fill) where T : unmanaged
        {
            if (fill.Length == 0) return;

            for (int index = 0; index < fill.Length; index++) fill[index] = Next<T>();
        }

        private int InternalSample()
        {
            int retVal;
            int locINext = _NextInt;
            int locINextp = _NextIntP;

            if (++locINext >= 56) locINext = 1;
            if (++locINextp >= 56) locINextp = 1;

            retVal = _SeedArray[locINext] - _SeedArray[locINextp];

            if (retVal == _MBIG) retVal--;
            if (retVal < 0) retVal += _MBIG;

            _SeedArray[locINext] = retVal;

            _NextInt = locINext;
            _NextIntP = locINextp;

            return retVal;
        }
    }
}
