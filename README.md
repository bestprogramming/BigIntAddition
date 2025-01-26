# BigInt Addition
BigInt Addition provides an example for multiplying two 64-bit arrays.
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
| :----------------- | :----------------- | :----------------- | :----------------- | :----- |
|64                  |64                  |2                   |2                   |100%	 |
|64                  |128                 |2                   |1                   |50%	 |
|64                  |256                 |2                   |2                   |100%	 |
|64                  |512                 |3                   |2                   |66%	 |
|64                  |1024                |7                   |7                   |100%	 |
|64                  |2048                |15                  |13                  |86%	 |
|64                  |32768               |215                 |211                 |98%	 |
|64                  |65536               |469                 |450                 |95%	 |
|128                 |64                  |1                   |1                   |100%	 |
|128                 |128                 |2                   |3                   |150%	 |
|128                 |256                 |2                   |1                   |50%	 |
|128                 |512                 |3                   |2                   |66%	 |
|128                 |1024                |7                   |6                   |85%	 |
|128                 |2048                |14                  |12                  |85%	 |
|128                 |32768               |215                 |215                 |100%	 |
|128                 |65536               |464                 |500                 |107%	 |
|256                 |64                  |2                   |1                   |50%	 |
|256                 |128                 |2                   |1                   |50%	 |
|256                 |256                 |4                   |5                   |125%	 |
|256                 |512                 |4                   |2                   |50%	 |
|256                 |1024                |8                   |7                   |87%	 |
|256                 |2048                |15                  |14                  |93%	 |
|256                 |32768               |221                 |225                 |101%	 |
|256                 |65536               |463                 |493                 |106%	 |
|512                 |64                  |2                   |2                   |100%	 |
|512                 |128                 |3                   |2                   |66%	 |
|512                 |256                 |5                   |3                   |60%	 |
|512                 |512                 |8                   |9                   |112%	 |
|512                 |1024                |12                  |8                   |66%	 |
|512                 |2048                |19                  |14                  |73%	 |
|512                 |32768               |217                 |220                 |101%	 |
|512                 |65536               |515                 |511                 |99%	 |
|1024                |64                  |6                   |6                   |100%	 |
|1024                |128                 |7                   |6                   |85%	 |
|1024                |256                 |8                   |7                   |87%	 |
|1024                |512                 |12                  |8                   |66%	 |
|1024                |1024                |19                  |21                  |110%	 |
|1024                |2048                |25                  |16                  |64%	 |
|1024                |32768               |240                 |230                 |95%	 |
|1024                |65536               |498                 |500                 |100%	 |
|2048                |64                  |14                  |12                  |85%	 |
|2048                |128                 |15                  |14                  |93%	 |
|2048                |256                 |16                  |13                  |81%	 |
|2048                |512                 |20                  |15                  |75%	 |
|2048                |1024                |25                  |16                  |64%	 |
|2048                |2048                |37                  |41                  |110%	 |
|2048                |32768               |251                 |235                 |93%	 |
|2048                |65536               |513                 |500                 |97%	 |
|32768               |64                  |226                 |226                 |100%	 |
|32768               |128                 |212                 |216                 |101%	 |
|32768               |256                 |216                 |220                 |101%	 |
|32768               |512                 |223                 |217                 |97%	 |
|32768               |1024                |226                 |218                 |96%	 |
|32768               |2048                |242                 |223                 |92%	 |
|32768               |32768               |615                 |678                 |110%	 |
|32768               |65536               |940                 |670                 |71%	 |
|65536               |64                  |462                 |461                 |99%	 |
|65536               |128                 |488                 |493                 |101%	 |
|65536               |256                 |488                 |500                 |102%	 |
|65536               |512                 |490                 |517                 |105%	 |
|65536               |1024                |490                 |490                 |100%	 |
|65536               |2048                |509                 |497                 |97%	 |
|65536               |32768               |894                 |650                 |72%	 |
|65536               |65536               |1289                |1444                |112%	 |