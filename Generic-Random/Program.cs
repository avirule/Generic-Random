using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Generic_Random
{
    internal class Program
    {
        private static void Main()
        {
            Summary _ = BenchmarkRunner.Run<RandomBenchmark>();
            Console.ReadLine();
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [RPlotExporter]
    public class RandomBenchmark
    {
        private const int _RANDOM_SEED = 783247234;
        private const int _ITERATIONS = 100000;
        private byte[] _Bytes;
        private double[] _Doubles;

        private GenericRandom _GenericRandom;

        private int[] _Ints;
        private long[] _Longs;
        private Random _Random;
        private short[] _Shorts;

        [GlobalSetup]
        public void Setup()
        {
            _GenericRandom = new GenericRandom(_RANDOM_SEED);
            _Random = new Random(_RANDOM_SEED);

            _Ints = new int[_ITERATIONS];
            _Shorts = new short[_ITERATIONS];
            _Bytes = new byte[_ITERATIONS];
            _Doubles = new double[_ITERATIONS];
            _Longs = new long[_ITERATIONS];
        }

        [Benchmark]
        public int[] GenericRandomNextInt()
        {
            for (int index = 0; index < _ITERATIONS; index++) _Ints[index] = _GenericRandom.Next<int>();

            return _Ints;
        }

        [Benchmark]
        public int[] RandomNextInt()
        {
            for (int index = 0; index < _ITERATIONS; index++) _Ints[index] = _Random.Next();

            return _Ints;
        }

        [Benchmark]
        public short[] GenericRandomNextShort()
        {
            for (int index = 0; index < _ITERATIONS; index++) _Shorts[index] = _GenericRandom.Next<short>();

            return _Shorts;
        }

        [Benchmark]
        public short[] RandomNextShort()
        {
            for (int index = 0; index < _ITERATIONS; index++) _Shorts[index] = (short)_Random.Next();

            return _Shorts;
        }

        [Benchmark]
        public byte[] GenericRandomNextByte()
        {
            for (int index = 0; index < _ITERATIONS; index++) _Bytes[index] = _GenericRandom.Next<byte>();

            return _Bytes;
        }

        [Benchmark]
        public byte[] RandomNextByte()
        {
            for (int index = 0; index < _ITERATIONS; index++) _Bytes[index] = (byte)_Random.Next();

            return _Bytes;
        }

        [Benchmark]
        public double[] GenericRandomNextDouble()
        {
            for (int index = 0; index < _ITERATIONS; index++) _Doubles[index] = _GenericRandom.Next<double>();

            return _Doubles;
        }

        [Benchmark]
        public double[] RandomNextDouble()
        {
            for (int index = 0; index < _ITERATIONS; index++) _Doubles[index] = _Random.NextDouble();

            return _Doubles;
        }

        [Benchmark]
        public long[] GenericRandomNextLong()
        {
            for (int index = 0; index < _ITERATIONS; index++) _Longs[index] = _GenericRandom.Next<long>();

            return _Longs;
        }

        [Benchmark]
        public long[] RandomNextLong()
        {
            for (int index = 0; index < _ITERATIONS; index++)
            {
                int low = _Random.Next();
                int high = _Random.Next();
                _Longs[index] = ((long)high << 32) | (uint)low;
            }

            return _Longs;
        }
    }
}
