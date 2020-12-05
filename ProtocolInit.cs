using System;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace RGR
{
    public class ProtocolInit
    {
        public Int64 N { get; set; }

        public ProtocolInit()
        {
            var rand = new Random();
            Int64 p = rand.Next(8192, 16384);
            Int64 q = rand.Next(8192, 16384);
            while (!isPrime(p))
            {
                p = rand.Next(8192, 16384);
            }
            while (!isPrime(q))
            {
                q = rand.Next(8192, 16384);
            }
            N = p * q;
        }

        public bool isPrime(Int64 n)
        {
            var rand = new Random();
            var k = 5;
            if (n == 2 || n == 3)
                return true;
            if (n < 2 || n % 2 == 0)
                return false;
            var t = n - 1;
            var s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }
            for (int i = 0; i < k; i++)
            {
                var a = rand.Next(2, (int)n - 2);
                var x = pow_m(a, t, n);
                if (x == 1 || x == n - 1)
                    continue;
                for (int j = 0; j < s - 1; j++)
                {
                    x = pow_m(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;
            }
            return true;
        }
        public Int64 pow_m(Int64 a, Int64 n, Int64 p)
        {
            var powBin = new BitArray(BitConverter.GetBytes(n));
            var len = powBin.Count;
            for(var i = len - 1; i >= 0; i--)
            {
                if(powBin[i] == true)
                {
                    len = i + 1;
                    break;
                }
            }
            var bitArr = new List<bool>();
            for (var i = 0; i < len; i++)
                bitArr.Add(powBin[i]);
            var remainds = new List<Int64>();
            remainds.Add(a % p);
            for (int i = 1; i < len; i++)
            {
                remainds.Add(remainds[i - 1] * remainds[i - 1] % p);
            }
            Int64 sum = 1;
            for (int i = 0; i < len; i++)
            {
                if (bitArr[i] == true)
                    sum *= remainds[i];
            }
            return sum % p;
        }

        public Int64 mult_m(Int64 a, Int64 b, Int64 c)
        {
            var A = new BigInteger(a);
            var B = new BigInteger(b);
            var C = new BigInteger(c);
            var bytes = (A * B % C).ToByteArray();
            Int64 res = 0;
            for (var i = 0; i < bytes.Length && i < 8; i++)
                res += bytes[i] * (Int64)Math.Pow(256, i);
            return res;
        }
    }
}
