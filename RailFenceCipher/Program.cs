using System;

namespace RailFenceCipher
{
    class Program
    {
        static void Main(string[] args)
        {
            var processor = new RailFenceProcessor();
            string text = "ahoj_pepooo";
            int key = 2;
            // for (int i = 1; i < 10; i++)
            // {
            //     Console.WriteLine(i+" - "+processor.GetFinalCount(10,i,5));
            // }
            //Console.WriteLine(text);
            var encode = processor.GraphicsEncode(text, key);
            //processor.PrintArray();
            var decode = processor.GraphicsDecode(encode, key);
            var mtencode = processor.MathEncode(text, key);
            var mtdecode = processor.MathDecode(mtencode, key);

            Console.WriteLine("text:" +text);
            Console.WriteLine();
            Console.WriteLine("encode  :" +encode);
            Console.WriteLine("mtencode:" +mtencode);
            Console.WriteLine();
            Console.WriteLine("decode  :" +decode);
            Console.WriteLine("mtdecode:" +mtdecode);
            
        }
    }
}