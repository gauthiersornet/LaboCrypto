using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboCrypto
{
    class BigIntPoint
    {
        public BigInteger x;
        public BigInteger y;

        public static BigInteger StringToBigUInteger(string snum)
        {
            snum = snum.Replace(" ", "");
            return new BigInteger(snum.Replace(" ", ""), 16);
        }

        /*public static BigInteger StringToBigInteger(string snum)
        {
            snum = snum.Replace(" ","");
            BigInteger bi;
            if (Convert.ToByte(snum[0].ToString(), 16) >= 8)
            {
                string nsnum = "";
                foreach (char c in snum) nsnum += ((~Convert.ToByte(c.ToString(), 16)) & 0xF).ToString("X");
                bi = -(new BigInteger(nsnum, 16) + 1);
            }
            else bi = new BigInteger(snum, 16);
            return bi;
        }*/

        public BigIntPoint(BigInteger _x, BigInteger _y)
        {
            x = _x;
            y = _y;
        }

        public BigIntPoint(string _x, string _y)
        {
            x = new BigInteger(_x.Replace(" ", ""), 16);
            y = new BigInteger(_y.Replace(" ", ""), 16);
        }

        public BigIntPoint(string h16p)
        {
            h16p = h16p.Replace(" ", "");
            string format = h16p.Substring(0,2);
            h16p = h16p.Substring(2);
            if (format == "04" && h16p.Length%2 == 0)
            {
                int clen = h16p.Length / 2;
                x = StringToBigUInteger(h16p.Substring(0, clen));
                y = StringToBigUInteger(h16p.Substring(clen));
            }
            else throw new Exception("Unmanaged string BigIntPoint format");
        }

        public override string ToString()
        {
            return "("+x+";"+y+")";
        }

        public override bool Equals(object obj)
        {
            BigIntPoint bip = obj as BigIntPoint;

            if(bip != null)
                return (bip.x == x && bip.y == y);
            else
                return base.Equals(obj);
        }

        public static bool operator ==(BigIntPoint bipA, BigIntPoint bipB) => (Object.ReferenceEquals(bipA, bipB) || (!Object.ReferenceEquals(bipA, null) && !Object.ReferenceEquals(bipB, null) && (bipA.x == bipB.x && bipA.y == bipB.y)));
        public static bool operator !=(BigIntPoint bipA, BigIntPoint bipB) => !(Object.ReferenceEquals(bipA, bipB) || (!Object.ReferenceEquals(bipA, null) && !Object.ReferenceEquals(bipB, null) && (bipA.x == bipB.x && bipA.y == bipB.y)));
    }

    class ElipticCurve
    {
        public static ElipticCurve secp256k1 = new ElipticCurve(
                "FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFE FFFFFC2F",
                "00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000",
                "00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000007",
                "FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFE BAAEDCE6 AF48A03B BFD25E8C D0364141",
                "04 79BE667E F9DCBBAC 55A06295 CE870B07 029BFCDB 2DCE28D9 59F2815B 16F81798 483ADA77 26A3C465 5DA4FBFC 0E1108A8 FD17B448 A6855419 9C47D08F FB10D4B8"
            );

        public BigInteger p;
        public BigInteger a;
        public BigInteger b;
        public BigInteger n;
        public BigIntPoint G; // base point

        private BigIntPoint[] GWeight;
        private BigIntPoint[] GWeight16b;


        /*
            int modInverse(int a, int n) 
            {
                int i = n, v = 0, d = 1;
                while (a>0) {
                    int t = i/a, x = a;
                    a = i % x;
                    i = x;
                    x = d;
                    d = v - t*x;
                    v = x;
                }
                v %= n;
                if (v<0) v = (v+n)%n;
                return v;
            }
        */

        public static BigInteger DivPByQModN(BigInteger P, BigInteger Q, BigInteger N)
        {
            if (Q == 0) return null;
            else if(Q > 0) return modN((modN(P, N) * Q.modInverse(N)), N);
            else return modN(-(modN(P, N) * (-Q).modInverse(N)), N);
        }

        public BigInteger CalculSlopeModN(BigIntPoint P1, BigIntPoint P2)
        {
            if(P1.x == P2.x)
            {
                if (P1.y == P2.y)
                    return CalculSlopeModN(P1);
                else return null;
            }
            else if (P1.y == P2.y && P1.y == 0) return null;
            return DivPByQModN((P2.y - P1.y), (P2.x - P1.x), p);
        }

        public BigInteger CalculSlopeModN(BigIntPoint P)
        {
            if (P.y == 0) return null;
            return DivPByQModN((3 * P.x * P.x + a), (2 * P.y), p);
        }

        public BigInteger CalculVFactorModN(BigIntPoint P1, BigIntPoint P2)
        {
            if (P1.x == P2.x)
            {
                if (P1.y == P2.y)
                    return CalculVFactorModN(P1);
                else return null;
            }
            else if(P1.y == P2.y && P1.y == 0)return null;
            return DivPByQModN((P1.y * P2.x - P2.y * P1.x), (P2.x - P1.x), p);
        }

        public BigInteger CalculVFactorModN(BigIntPoint P)
        {
            return DivPByQModN((-P.x * P.x * P.x + a * P.x + 2 * b), (2 * P.y), p);
        }

        static private BigInteger modN(BigInteger bi, BigInteger p)
        {
            if (bi < 0) return (p + (bi % p));
            else return (bi % p);
        }

        private BigInteger modN(BigInteger bi)
        {
            if (bi < 0) return (p + (bi % p));
            else return (bi % p);
        }

        public BigIntPoint Add(BigIntPoint P1, BigIntPoint P2)
        {
            if (object.ReferenceEquals(P1, null)) return P2;
            if (object.ReferenceEquals(P2, null)) return P1;

            BigInteger lambda = CalculSlopeModN(P1, P2);
            BigInteger v = CalculVFactorModN(P1, P2);

            return new BigIntPoint(
                    modN(lambda * lambda - P1.x - P2.x),
                    modN(-lambda * lambda * lambda + lambda * (P1.x + P2.x) - v)
                );
        }

        public BigIntPoint Add(BigIntPoint P)
        {
            if (object.ReferenceEquals(P, null)) return null;

            BigInteger lambda = CalculSlopeModN(P);
            BigInteger v = CalculVFactorModN(P);

            return new BigIntPoint(
                    modN(lambda * lambda - 2 * P.x),
                    modN(-lambda * lambda * lambda + lambda * 2 * P.x - v)
                );
        }

        static public int nbBits(BigInteger v)
        {
            if (v == 0) return 0;

            byte[] vBytes = v.getBytes();

            int nuOct;
            for (nuOct = 0; vBytes[nuOct] == 0; ++nuOct) ;

            byte bits = vBytes[nuOct];
            int numBit;
            for (numBit = 7; ((bits & (1 << numBit)) == 0); --numBit) ;

            return (vBytes.Length-1-nuOct)*8+ numBit+1;
        }

        public BigIntPoint[] gWeigth(BigIntPoint g)
        {
            int nbbits = nbBits(p);
            int nbOct = nbbits / 8;
            BigIntPoint[] res = new BigIntPoint[nbOct * 8];

            BigIntPoint bi = g;
            for(int j = res.Length-8; j>=8; j-=8)
            {
                for(int i=0; i<8; ++i)
                {
                    res[j+i] = bi;
                    bi = Add(bi,bi);
                }
            }

            int limInf = 8 - (nbbits % 8);

            for (int i = 0; i < limInf; ++i)
            {
                res[i] = bi;
                bi = Add(bi, bi);
            }

            return res;
        }

        public BigIntPoint[] gWeight16b(BigIntPoint g)
        {
            BigIntPoint[] res = new BigIntPoint[1<<16];
            BigIntPoint bi = g;
            for (int i=0;i<(1<<16);++i)
            {
                res[i] = bi;
                bi = Add(bi, g);
            }
            return res;
        }

        public BigIntPoint Mutiply(BigIntPoint P, BigInteger k)
        {
            if (k == 0) return null;

            BigIntPoint res = P;
            byte[] kBytes = k.getBytes();

            int nuOct;
            for (nuOct = 0; kBytes[nuOct] == 0; ++nuOct);

            byte bits = kBytes[nuOct];
            int numBit;
            for (numBit = 7; ((bits & (1 << numBit)) == 0); --numBit);
            for (--numBit; numBit>=0; --numBit)
            {
                res = Add(res); //Mutiply by 2
                if ((bits & (0b1 << numBit)) != 0)
                    res = Add(P, res);//Add + P
            }

            for (++nuOct; nuOct < kBytes.Length; ++nuOct)
            {
                bits = kBytes[nuOct];
                for (numBit=7; numBit >= 0; --numBit)
                {
                    res = Add(res); //Mutiply by 2
                    if ((bits & (0b1 << numBit)) != 0)
                        res = Add(P, res);//Add + P
                }
            }

            return res;
        }

        public BigIntPoint Fast_kG(BigInteger k)
        {
            if (k == 0) return null;

            BigIntPoint res = null;

            byte[] kBytes = k.getBytes();
            for(int j=0;j< kBytes.Length; ++j)
            {
                byte b = kBytes[j];
                for (int i = 0; i < 8; ++i)
                    if((b & (1<<i)) != 0)res = Add(res, GWeight[j*8+i]);
            }

            return res;
        }

        public BigIntPoint UltraFast_kG(BigInteger k)
        {
            if (k == 0) return null;

            BigIntPoint res = null;

            byte[] kBytes = k.getBytes();
            int j;
            if(kBytes.Length % 2 == 0)
            {
                res = GWeight16b[kBytes[0]<<8 | kBytes[1]];
                j = 2;
            }
            else
            {
                res = GWeight16b[kBytes[0]];
                j = 1;
            }

            for (; j < kBytes.Length; j+=2)
                res = Add(Add(res, res), GWeight16b[kBytes[j] << 8 | kBytes[j+1]]);

            return res;
        }

        public BigIntPoint MutiplyRec(BigIntPoint P, BigInteger k)
        {
            if (k == 0) return null;
            if (k == 1) return P;

            BigIntPoint Pm = Add(MutiplyRec(P, k / 2));
            if (k % 2 == 0)return Pm;
            else return Add(P, Pm);
        }

        public BigIntPoint MutiplyLin(BigIntPoint P, BigInteger k)
        {
            if (k == 0) return null;
            if (k == 1) return P;

            BigIntPoint res = Add(P);
            for(BigInteger i = k-2; i>0 ; --i)
                res = Add(P, res);
            return res;
        }

        public ElipticCurve(BigInteger _p, BigInteger _a, BigInteger _b, BigInteger _n, BigIntPoint _g)
        {
            p = _p;
            a = _a;
            b = _b;
            n = _n;
            G = _g;
            GWeight = gWeigth(G);
            GWeight16b = gWeight16b(G);
        }

        public ElipticCurve(string _p, string _a, string _b, string _n, string _g)
        {
            p = BigIntPoint.StringToBigUInteger(_p);
            a = BigIntPoint.StringToBigUInteger(_a);
            b = BigIntPoint.StringToBigUInteger(_b);
            n = BigIntPoint.StringToBigUInteger(_n);
            G = new BigIntPoint(_g);
            if (IsPointOfCurve(G) == false) throw new Exception("The point G is not a point of the curve");
            GWeight = gWeigth(G);
            GWeight16b = gWeight16b(G);
        }

        public bool IsPointOfCurve(BigIntPoint bp)
        {
            //return ((bp.y * bp.y) % p == ((bp.x * bp.x * bp.x) + a * bp.x + b) % p);
            return (((bp.y * bp.y) - ((bp.x * bp.x * bp.x) + a * bp.x + b)) % p) == 0;
        }
    }
}
