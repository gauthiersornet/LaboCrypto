using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace laboCrypto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ElipticCurve ec = ElipticCurve.secp256k1;
            /*
            //BigInteger n = BigIntPoint.StringToBigUInteger("FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFE FFFFFC2F");
            BigInteger n = BigIntPoint.StringToBigUInteger("AFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFE FFFFFC2F");
            BigInteger bi = -BigIntPoint.StringToBigUInteger("79BE667E F9DCBBAC 55A06295 CE870B07 029BFCDB 2DCE28D9 59F2815B 16F81798");
            BigInteger t = bi % n;
            BigInteger bi = BigIntPoint.StringToBigUInteger("8") / BigIntPoint.StringToBigUInteger("3");*/
            /*BigInteger bi = new BigInteger(128);
            BigInteger n = new BigInteger(257);
            BigInteger invmodn = new BigInteger(4).modInverse(n);
            BigInteger r = (bi * invmodn) % n;*/

            /*BigInteger k = new BigInteger(3);
            BigIntPoint kP1 = ec.MutiplyRec(ec.G, k); Console.WriteLine(kP1);
            bool isOnECP1 = ec.IsPointOfCurve(kP1);
            BigIntPoint kP2 = ec.Mutiply(ec.G, k); Console.WriteLine(kP2);
            bool isOnECP2 = ec.IsPointOfCurve(kP2);
            BigIntPoint kP3 = ec.MutiplyLin(ec.G, k); Console.WriteLine(kP3);
            bool isOnECP3 = ec.IsPointOfCurve(kP3);*/

            /*Console.WriteLine(ec.G);
            BigIntPoint kP3 = ec.Add(ec.G); Console.WriteLine(kP3);
            kP3 = ec.Add(kP3); Console.WriteLine(kP3);
            kP3 = ec.Add(kP3); Console.WriteLine(kP3);
            kP3 = ec.Add(kP3); Console.WriteLine(kP3);
            kP3 = ec.Add(ec.G, kP3); Console.WriteLine(kP3);
            bool isOnECP3 = ec.IsPointOfCurve(kP3);*/

            /*BigInteger n = 257;
            BigInteger a = -128;
            BigInteger q = 4;
            Console.WriteLine( (n + (a * q.modInverse(n)) % n) % n );*/

            /*BigInteger k = 256;
            for (k = 1; k < 1000; ++k)
            {
                Console.WriteLine("====k:"+k+"====");
                BigIntPoint kPRec = ec.MutiplyRec(ec.G, k); Console.WriteLine("kPRec=" + kPRec);
                BigIntPoint kPSeq = ec.Mutiply(ec.G, k); Console.WriteLine("kPSeq=" + kPSeq);
                if (kPRec != kPSeq) break;
            }*/
            
            Console.WriteLine("G=" + ec.G);
            BigInteger k = new BigInteger("1E99423A4ED27608A15A2616A2B0E9E52CED330AC530EDCC32C8FFC6A526AEDD", 16);
            Console.WriteLine("k=" + k);
            BigIntPoint kG;
            //kG = ec.MutiplyLin(ec.G, k); Console.WriteLine("kG=" + kG); Console.WriteLine("kG est sur la couvre ? " + ec.IsPointOfCurve(kG));
            //kG = ec.MutiplyRec(ec.G, k); Console.WriteLine("kG=" + kG); Console.WriteLine("kG est sur la couvre ? " + ec.IsPointOfCurve(kG));
            kG = ec.Mutiply(ec.G, k); Console.WriteLine("Normal kG=" + kG); Console.WriteLine("kG est sur la couvre ? " + ec.IsPointOfCurve(kG));
            kG = ec.Fast_kG(k); Console.WriteLine("Fast kG=" + kG); Console.WriteLine("kG est sur la couvre ? " + ec.IsPointOfCurve(kG));
            //kG = ec.UltraFast_kG(k); Console.WriteLine("UltraFast kG=" + kG); Console.WriteLine("kG est sur la couvre ? " + ec.IsPointOfCurve(kG));
            //BigIntPoint tG = new BigIntPoint(BigIntPoint.StringToBigInteger("F028892BAD7ED57D2FB57BF33081D5CFCF6F9ED3D3D7F159C2E2FFF579DC341A"), BigIntPoint.StringToBigInteger("07CF33DA18BD734C600B96A72BBC4749D5141C90EC8AC328AE52DDFE2E505BDB"));
            BigIntPoint tG = new BigIntPoint("F028892BAD7ED57D2FB57BF33081D5CFCF6F9ED3D3D7F159C2E2FFF579DC341A", "07CF33DA18BD734C600B96A72BBC4749D5141C90EC8AC328AE52DDFE2E505BDB");
            Console.WriteLine("tG=" + tG);
            Console.WriteLine("tG est sur la couvre ? " + ec.IsPointOfCurve(tG));

            BigIntPoint km1G = ec.Mutiply(kG, k.modInverse(ec.p)); Console.WriteLine("Fast km1G=" + km1G); Console.WriteLine("km1G est sur la couvre ? " + ec.IsPointOfCurve(km1G));

            DateTime ds;
            ds = DateTime.Now;
            for(int i=0;i<100;++i)
                kG = ec.Mutiply(ec.G, k);
            DateTime de = DateTime.Now;
            Console.WriteLine("Mutiply_kG(ec.G, k) : " + (de-ds));

            ds = DateTime.Now;
            for (int i = 0; i < 100; ++i)
                kG = ec.Fast_kG(k);
            de = DateTime.Now;
            Console.WriteLine("Fast_kG(k) : " + (de - ds));

            ds = DateTime.Now;
            for (int i = 0; i < 100; ++i)
                kG = ec.UltraFast_kG(k);
            de = DateTime.Now;
            Console.WriteLine("UltraFast_kG(k) : " + (de - ds));
        }

        private void btAction_Click(object sender, EventArgs e)
        {
            btAction.Text = "En cours...";
            btAction.Refresh();
            int nbZeroBits = int.Parse(txtNbZero.Text);
            byte[] datas = Convert.FromBase64String(txtData.Text);
            int nonce = 0;
            bool hasSolved = false;
            SHA256 sha256;
            byte[] nonceBytes = new byte[4];
            do
            {
                nonceBytes[0] = (byte)((nonce >> 0) & 0xFF);
                nonceBytes[1] = (byte)((nonce >> 8) & 0xFF);
                nonceBytes[2] = (byte)((nonce >> 16) & 0xFF);
                nonceBytes[3] = (byte)((nonce >> 24) & 0xFF);
                sha256 = SHA256.Create();
                sha256.ComputeHash(datas);
                sha256.ComputeHash(nonceBytes);

                byte[] hash = sha256.Hash;
                int nbZ = nbZeroBits;
                foreach (byte b in hash)
                {
                    if (nbZ > 0)
                    {
                        if(nbZ>=8)
                        {
                            if (b == 0) nbZ -= 8;
                            else break;
                        }
                        else
                        {
                            if ((b & ~(0xFF<<nbZ)) == 0) nbZ = 0;
                            break;
                        }
                    }
                    else break;
                }
                if ((nbZ == 0))hasSolved = true;
                else ++nonce;
            }
            while (!hasSolved);
            txtRes.Text = "nonce = " + nonce + "\r\n" + "hash = " + sha256.Hash.Select(b => b.ToString("X")).Aggregate((a,b) => a+b);
            btAction.Text = "action";
        }
    }
}
