using System;

namespace RailFenceCipher
{
    class Program
    {
        static void Main(string[] args)
        {
            var processor = new RailFenceProcessor();
            string text = "ahoj_lidiahoj_lidiahoj_lidiahoj_lidiahoj_lidiahoj_lidiahoj_lidi";
            var encode = processor.Encode(text,30);
            var decode = processor.Decode(encode, 30);
            Console.WriteLine(text);
            Console.WriteLine(encode);
            Console.WriteLine(decode);
        }
    }
}