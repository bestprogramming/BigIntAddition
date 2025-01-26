using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using Xunit.Abstractions;

namespace Tests.HowToCalculate
{
    public class AdditionTest(ITestOutputHelper output) : BaseTest(output)
    {
        [DllImport(dllPath)]
        private static unsafe extern int AddByEqualLength(int length, ulong* left, ulong* right, ulong* result);

        [DllImport(dllPath)]
        private static unsafe extern int AddByGreaterLength(int leftLength, int rightLength, ulong* left, ulong* right, ulong* result);

        private unsafe static ulong[] Add(ulong[] left, ulong[] right)
        {
            int leftLength = left.Length;
            int rightLength = right.Length;

            ulong[] result;
            ulong[] ret;

            if (leftLength == rightLength)
            {
                int size = leftLength + 1;
                result = ArrayPool<ulong>.Shared.Rent(size);

                fixed (ulong* p1 = left, p2 = right, p = result)
                {
                    size = AddByEqualLength(leftLength, p1, p2, p);
                    ret = result[..size];
                }
            }
            else if (leftLength > rightLength)
            {
                int size = leftLength + 1;
                result = ArrayPool<ulong>.Shared.Rent(size);

                fixed (ulong* p1 = left, p2 = right, p = result)
                {
                    size = AddByGreaterLength(leftLength, rightLength, p1, p2, p);
                    ret = result[..size];
                }
            }
            else
            {
                int size = rightLength + 1;
                result = ArrayPool<ulong>.Shared.Rent(size);

                fixed (ulong* p1 = left, p2 = right, p = result)
                {
                    size = AddByGreaterLength(rightLength, leftLength, p2, p1, p);
                    ret = result[..size];
                }
            }

            ArrayPool<ulong>.Shared.Return(result);

            return ret;
        }


        [Fact]
        public void AddByEqualLengthT1()
        {
            ulong[] left;
            ulong[] right;
            BigInteger expected;
            ulong[] actual;


            left = [3, 1];
            right = [5, 2];
            expected = ToBigInteger(left) + ToBigInteger(right);
            actual = Add(left, right);
            Assert.True(Eq(expected, actual));

            left = [ulong.MaxValue, ulong.MaxValue];
            right = [ulong.MaxValue, ulong.MaxValue];
            expected = ToBigInteger(left) + ToBigInteger(right);
            actual = Add(left, right);
            Assert.True(Eq(expected, actual));
        }

        [Fact]
        public void AddByGreaterLengthT1()
        {
            ulong[] left;
            ulong[] right;
            BigInteger expected;
            ulong[] actual;


            left = [5, 3];
            right = [2];
            expected = ToBigInteger(left) + ToBigInteger(right);
            actual = Add(left, right);
            Assert.True(Eq(expected, actual));

            left = [ulong.MaxValue, ulong.MaxValue];
            right = [ulong.MaxValue];
            expected = ToBigInteger(left) + ToBigInteger(right);
            actual = Add(left, right);
            Assert.True(Eq(expected, actual));
        }

        [Fact]
        public void AddByGreaterLengthT2()
        {
            ulong[] left;
            ulong[] right;
            BigInteger expected;
            ulong[] actual;


            left = [2];
            right = [5, 3];
            expected = ToBigInteger(left) + ToBigInteger(right);
            actual = Add(left, right);
            Assert.True(Eq(expected, actual));

            left = [ulong.MaxValue];
            right = [ulong.MaxValue, ulong.MaxValue];
            expected = ToBigInteger(left) + ToBigInteger(right);
            actual = Add(left, right);
            Assert.True(Eq(expected, actual));
        }

        [Fact]
        public void VsBigIntegerT1()
        {
            var actualTicks = new List<long>();
            var expectedTicks = new List<long>();

            var testCount = 300;

            var leftLength = 65536;
            var rightLength = 256;

            for (var test = 0; test < testCount; test++)
            {
                var left = new ulong[leftLength];
                var right = new ulong[rightLength];
                for (var n = 0; n < leftLength; n++)
                {
                    left[n] = RandomUlong();
                }

                for (var n = 0; n < rightLength; n++)
                {
                    right[n] = RandomUlong();
                }

                //var index = -1; output.WriteLine(left.Aggregate("", (c, n) => { index++; return c + (c != "" ? "\r\n" : "") + $"left[{index}] = {n};"; }));
                //index = -1; output.WriteLine(right.Aggregate("", (c, n) => { index++; return c + (c != "" ? "\r\n" : "") + $"right[{index}] = {n};"; }));

                var b1 = ToBigInteger(left);
                var b2 = ToBigInteger(right);

                var swExpected = Stopwatch.StartNew();
                var expected = b1 + b2;
                swExpected.Stop();
                expectedTicks.Add(swExpected.ElapsedTicks);

                var swActual = Stopwatch.StartNew();
                var actual = Add(left, right);
                swActual.Stop();
                actualTicks.Add(swActual.ElapsedTicks);

                Assert.True(Eq(expected, actual));
            }

            expectedTicks.Sort();
            actualTicks.Sort();

            output.WriteLine($"leftLength:{leftLength}, rightLength:{rightLength}");
            output.WriteLine($"{"expected(max)",-20}{"actual",-20}");

            for (var n = 0; n < 5; n++)
            {
                output.WriteLine($"{expectedTicks.ElementAt(n),-20}{actualTicks.ElementAt(n),-20}");
                //Assert.True(actualTicks.ElementAt(n) <= expectedTicks.ElementAt(n));
            }
        }

        [Fact]
        public void VsBigIntegerT2()
        {
            var actualTicks = new List<long>();
            var expectedTicks = new List<long>();

            var testCount = 300;

            var leftLengths = new[] { 64, 128, 256, 512, 1024, 2048, 32768, 65536 };
            var rightLengths = new[] { 64, 128, 256, 512, 1024, 2048, 32768, 65536 };

            var table = $"\t\t\t{"leftLength",-20}{"rightLength",-20}{"expectedTick(max)",-20}{"actualTick",-20}{"Percent"}\r\n";
            foreach (var leftLength in leftLengths)
            {
                foreach (var rightLength in rightLengths)
                {
                    actualTicks.Clear();
                    expectedTicks.Clear();

                    for (var test = 0; test < testCount; test++)
                    {
                        var left = new ulong[leftLength];
                        var right = new ulong[rightLength];
                        for (var n = 0; n < leftLength; n++)
                        {
                            left[n] = RandomUlong();
                        }

                        for (var n = 0; n < rightLength; n++)
                        {
                            right[n] = RandomUlong();
                        }

                        var b1 = ToBigInteger(left);
                        var b2 = ToBigInteger(right);

                        var swExpected = Stopwatch.StartNew();
                        var expected = b1 + b2;
                        swExpected.Stop();
                        expectedTicks.Add(swExpected.ElapsedTicks);

                        var swActual = Stopwatch.StartNew();
                        var actual = Add(left, right);
                        swActual.Stop();
                        actualTicks.Add(swActual.ElapsedTicks);
                             
                        Assert.True(Eq(expected, actual));
                    }

                    expectedTicks.Sort();
                    actualTicks.Sort();


                    var expectedTick = expectedTicks.First();
                    var actualTick = actualTicks.First();
                    table += $"\t\t\t{leftLength,-20}{rightLength,-20}{expectedTick,-20}{actualTick,-20}{Percent(expectedTick, actualTick)}\r\n";
                }
            }

            output.WriteLine(table);
            File.WriteAllText($"{appDir}/AdditionTest.txt", table);

            /* 

			leftLength          rightLength         expectedTick(max)   actualTick          Percent
			64                  64                  2                   1                   50%
			64                  128                 2                   1                   50%
			64                  256                 2                   2                   100%
			64                  512                 3                   3                   100%
			64                  1024                7                   6                   85%
			64                  2048                14                  13                  92%
			64                  32768               213                 216                 101%
			64                  65536               435                 453                 104%
			128                 64                  1                   1                   100%
			128                 128                 2                   1                   50%
			128                 256                 2                   1                   50%
			128                 512                 3                   2                   66%
			128                 1024                6                   6                   100%
			128                 2048                14                  13                  92%
			128                 32768               214                 214                 100%
			128                 65536               452                 457                 101%
			256                 64                  2                   2                   100%
			256                 128                 3                   2                   66%
			256                 256                 5                   2                   40%
			256                 512                 5                   3                   60%
			256                 1024                11                  9                   81%
			256                 2048                16                  14                  87%
			256                 32768               216                 223                 103%
			256                 65536               464                 474                 102%
			512                 64                  2                   2                   100%
			512                 128                 3                   2                   66%
			512                 256                 5                   3                   60%
			512                 512                 8                   3                   37%
			512                 1024                12                  7                   58%
			512                 2048                19                  14                  73%
			512                 32768               217                 215                 99%
			512                 65536               451                 495                 109%
			1024                64                  6                   6                   100%
			1024                128                 7                   6                   85%
			1024                256                 8                   7                   87%
			1024                512                 12                  8                   66%
			1024                1024                19                  9                   47%
			1024                2048                24                  15                  62%
			1024                32768               222                 218                 98%
			1024                65536               457                 445                 97%
			2048                64                  13                  13                  100%
			2048                128                 14                  13                  92%
			2048                256                 16                  13                  81%
			2048                512                 19                  14                  73%
			2048                1024                25                  16                  64%
			2048                2048                37                  18                  48%
			2048                32768               236                 219                 92%
			2048                65536               480                 470                 97%
			32768               64                  213                 216                 101%
			32768               128                 213                 214                 100%
			32768               256                 218                 219                 100%
			32768               512                 221                 216                 97%
			32768               1024                227                 217                 95%
			32768               2048                239                 224                 93%
			32768               32768               616                 332                 53%
			32768               65536               864                 659                 76%
			65536               64                  484                 465                 96%
			65536               128                 453                 446                 98%
			65536               256                 464                 464                 100%
			65536               512                 477                 502                 105%
			65536               1024                476                 459                 96%
			65536               2048                492                 495                 100%
			65536               32768               893                 641                 71%
			65536               65536               1297                812                 62%
             
             */


        }
    }
}
