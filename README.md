# BigInt Addition
BigInt Addition provides an example for adding two 64-bit arrays.
If LeftLength is greater than RightLength, AddByGreaterLength.asm is executed, if equal, AddByEqualLength.asm is executed.
AddByEqualLength requires Length additions. 
If there is a carry value at the end of the addition, the final value of result is 1.
AddByEqualLength requires RightLength additions.
If there is a carry value at the end of the addition, 1 is added until the carry value becomes zero.
If there is any unprocessed digit left from Left, all of these digits are copied to the result.

## Decimal digits count of 64-bit arrays
When performing mathematical operations, we may need operations with higher precision than numbers such as double or float. 
For example, if we want to find the roots of a quartic function as a double, the input of the function must be greater than the double precision.

| Length | Byte Count | Decimal Count |
| :----- | :--------- | :------------ |
|  128   |  1024      |  2467         |
|  256   |  2048      |  4933         |
|  512   |  4096      |  9865         |
|  1024  |  8192      |  19719        |
|  2048  |  16384     |  39447        |
|  32768 |  262144    |  631297       |
|  65536 |  524288    |  1262602      |

## Performance Results
In the performance table below, two random 64-bit numbers of length leftLength and rightLength were multiplied. 
Each line was run 300 times and the minimum ElapsedTicks values were written with StopWatch(.NET). 
ExpectedTick(max) belongs to BigInteger(.NET).


|leftLength          |rightLength         |expectedTick(max)   |actualTick          |Percent |
| :------------------| :------------------| :------------------| :------------------| :----- |
|64                  |64                  |2                   |1                   |50%     |
|64                  |128                 |2                   |1                   |50%     |
|64                  |256                 |2                   |2                   |100%    |
|64                  |512                 |3                   |3                   |100%    |
|64                  |1024                |7                   |6                   |85%     |
|64                  |2048                |14                  |14                  |100%    |
|64                  |32768               |208                 |214                 |102%    |
|64                  |65536               |451                 |474                 |105%    |
|128                 |64                  |1                   |1                   |100%    |
|128                 |128                 |2                   |1                   |50%     |
|128                 |256                 |2                   |2                   |100%    |
|128                 |512                 |3                   |2                   |66%     |
|128                 |1024                |7                   |6                   |85%     |
|128                 |2048                |14                  |13                  |92%     |
|128                 |32768               |212                 |220                 |103%    |
|128                 |65536               |471                 |458                 |97%     |
|256                 |64                  |1                   |1                   |100%    |
|256                 |128                 |2                   |1                   |50%     |
|256                 |256                 |4                   |2                   |50%     |
|256                 |512                 |5                   |3                   |60%     |
|256                 |1024                |9                   |7                   |77%     |
|256                 |2048                |16                  |14                  |87%     |
|256                 |32768               |218                 |217                 |99%     |
|256                 |65536               |461                 |464                 |100%    |
|512                 |64                  |2                   |2                   |100%    |
|512                 |128                 |3                   |2                   |66%     |
|512                 |256                 |5                   |3                   |60%     |
|512                 |512                 |8                   |3                   |37%     |
|512                 |1024                |12                  |8                   |66%     |
|512                 |2048                |19                  |15                  |78%     |
|512                 |32768               |220                 |219                 |99%     |
|512                 |65536               |455                 |486                 |106%    |
|1024                |64                  |6                   |6                   |100%    |
|1024                |128                 |6                   |6                   |100%    |
|1024                |256                 |8                   |7                   |87%     |
|1024                |512                 |12                  |7                   |58%     |
|1024                |1024                |19                  |9                   |47%     |
|1024                |2048                |25                  |16                  |64%     |
|1024                |32768               |224                 |219                 |97%     |
|1024                |65536               |478                 |490                 |102%    |
|2048                |64                  |14                  |13                  |92%     |
|2048                |128                 |14                  |13                  |92%     |
|2048                |256                 |16                  |13                  |81%     |
|2048                |512                 |19                  |14                  |73%     |
|2048                |1024                |25                  |15                  |60%     |
|2048                |2048                |37                  |18                  |48%     |
|2048                |32768               |236                 |224                 |94%     |
|2048                |65536               |488                 |464                 |95%     |
|32768               |64                  |217                 |221                 |101%    |
|32768               |128                 |215                 |220                 |102%    |
|32768               |256                 |216                 |218                 |100%    |
|32768               |512                 |222                 |217                 |97%     |
|32768               |1024                |229                 |218                 |95%     |
|32768               |2048                |244                 |227                 |93%     |
|32768               |32768               |607                 |327                 |53%     |
|32768               |65536               |897                 |635                 |70%     |
|65536               |64                  |467                 |473                 |101%    |
|65536               |128                 |463                 |487                 |105%    |
|65536               |256                 |454                 |492                 |108%    |
|65536               |512                 |469                 |472                 |100%    |
|65536               |1024                |472                 |457                 |96%     |
|65536               |2048                |485                 |448                 |92%     |
|65536               |32768               |863                 |618                 |71%     |
|65536               |65536               |1255                |781                 |62%     |
