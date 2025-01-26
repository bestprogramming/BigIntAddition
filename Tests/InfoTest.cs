using System.Diagnostics;
using System.Numerics;
using Xunit.Abstractions;

namespace Tests
{
    public class InfoTest(ITestOutputHelper output) : BaseTest(output)
    {
        [Fact]
        public void UlongDigitsT1()
        {
            int length;
            ulong[] bits;
            BigInteger value;
            int digits;

            length = 64;
            bits = new ulong[length];
            for (var n = 0; n < length; n++) bits[n] = ulong.MaxValue;
            value = ToBigInteger(bits);
            digits = value.ToString().Length;
            Assert.Equal(1234, digits);

            length = 128;
            bits = new ulong[length];
            for (var n = 0; n < length; n++) bits[n] = ulong.MaxValue;
            value = ToBigInteger(bits);
            digits = value.ToString().Length;
            Assert.Equal(2467, digits);

            length = 256;
            bits = new ulong[length];
            for (var n = 0; n < length; n++) bits[n] = ulong.MaxValue;
            value = ToBigInteger(bits);
            digits = value.ToString().Length;
            Assert.Equal(4933, digits);

            length = 512;
            bits = new ulong[length];
            for (var n = 0; n < length; n++) bits[n] = ulong.MaxValue;
            value = ToBigInteger(bits);
            digits = value.ToString().Length;
            Assert.Equal(9865, digits);

            length = 1024;
            bits = new ulong[length];
            for (var n = 0; n < length; n++) bits[n] = uint.MaxValue;
            value = ToBigInteger(bits);
            digits = value.ToString().Length;
            Assert.Equal(19719, digits);

            length = 2048;
            bits = new ulong[length];
            for (var n = 0; n < length; n++) bits[n] = uint.MaxValue;
            value = ToBigInteger(bits);
            digits = value.ToString().Length;
            Assert.Equal(39447, digits);

            //length = 32768;
            //bits = new ulong[length];
            //for (var n = 0; n < length; n++) bits[n] = uint.MaxValue;
            //value = ToBigInteger(bits);
            //digits = value.ToString().Length;
            //Assert.Equal(631297, digits);

            //length = 65536;
            //bits = new ulong[length];
            //for (var n = 0; n < length; n++) bits[n] = uint.MaxValue;
            //value = ToBigInteger(bits);
            //digits = value.ToString().Length;
            //Assert.Equal(1262602, digits);
        }
    }
}
