using System;

namespace RailFenceCipher
{
    class Program
    {
        static void Main(string[] args)
        {
            var processor = new RailFenceProcessor();
            string text = "ahoj_pepo_a";
            int key = 4;
             var encode = processor.GraphicsEncode(text,key);
            processor.PrintArray();
              //var decode = processor.GraphicsDecode(encode, key);
             //Console.WriteLine(text);
             Console.WriteLine(encode);
             //Console.WriteLine(decode);

             var mtencode = processor.MathEncode(text, key);
             //Console.WriteLine(text);
             Console.WriteLine(mtencode);
        }
    }
}