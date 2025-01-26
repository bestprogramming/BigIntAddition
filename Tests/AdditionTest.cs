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
			64                  64                  2                   2                   100%
			64                  128                 2                   1                   50%
			64                  256                 2                   2                   100%
			64                  512                 3                   2                   66%
			64                  1024                7                   7                   100%
			64                  2048                15                  13                  86%
			64                  32768               215                 211                 98%
			64                  65536               469                 450                 95%
			128                 64                  1                   1                   100%
			128                 128                 2                   3                   150%
			128                 256                 2                   1                   50%
			128                 512                 3                   2                   66%
			128                 1024                7                   6                   85%
			128                 2048                14                  12                  85%
			128                 32768               215                 215                 100%
			128                 65536               464                 500                 107%
			256                 64                  2                   1                   50%
			256                 128                 2                   1                   50%
			256                 256                 4                   5                   125%
			256                 512                 4                   2                   50%
			256                 1024                8                   7                   87%
			256                 2048                15                  14                  93%
			256                 32768               221                 225                 101%
			256                 65536               463                 493                 106%
			512                 64                  2                   2                   100%
			512                 128                 3                   2                   66%
			512                 256                 5                   3                   60%
			512                 512                 8                   9                   112%
			512                 1024                12                  8                   66%
			512                 2048                19                  14                  73%
			512                 32768               217                 220                 101%
			512                 65536               515                 511                 99%
			1024                64                  6                   6                   100%
			1024                128                 7                   6                   85%
			1024                256                 8                   7                   87%
			1024                512                 12                  8                   66%
			1024                1024                19                  21                  110%
			1024                2048                25                  16                  64%
			1024                32768               240                 230                 95%
			1024                65536               498                 500                 100%
			2048                64                  14                  12                  85%
			2048                128                 15                  14                  93%
			2048                256                 16                  13                  81%
			2048                512                 20                  15                  75%
			2048                1024                25                  16                  64%
			2048                2048                37                  41                  110%
			2048                32768               251                 235                 93%
			2048                65536               513                 500                 97%
			32768               64                  226                 226                 100%
			32768               128                 212                 216                 101%
			32768               256                 216                 220                 101%
			32768               512                 223                 217                 97%
			32768               1024                226                 218                 96%
			32768               2048                242                 223                 92%
			32768               32768               615                 678                 110%
			32768               65536               940                 670                 71%
			65536               64                  462                 461                 99%
			65536               128                 488                 493                 101%
			65536               256                 488                 500                 102%
			65536               512                 490                 517                 105%
			65536               1024                490                 490                 100%
			65536               2048                509                 497                 97%
			65536               32768               894                 650                 72%
			65536               65536               1289                1444                112%
             
             */


        }
    }
}
