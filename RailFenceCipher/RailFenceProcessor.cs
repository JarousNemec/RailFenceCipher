using System;
using System.Drawing;

namespace RailFenceCipher
{
    public class RailFenceProcessor
    {
        #region Graphics solution

        private char[,] _railFence;
        private Point _cursor;
        private Direction _direction;

        public string GraphicsEncode(string toEncode, int key)
        {
            if (key <= 1 || key >= toEncode.Length)
                return "Wrong key";

            toEncode = Initialize(toEncode, key);

            PutEachCharToArrayByDiagonal(toEncode);

            return ReadCipherFromArrayByRailFenceRules();
        }

        private string ReadCipherFromArrayByRailFenceRules()
        {
            var output = string.Empty;
            for (int y = 0; y < _railFence.GetLength(0); y++)
            {
                for (int x = 0; x < _railFence.GetLength(1); x++)
                {
                    if (_railFence[y, x] != Char.MinValue)
                        output += _railFence[y, x];
                }
            }

            return output;
        }

        private void PutEachCharToArrayByDiagonal(string toEncode)
        {
            for (int x = 0; x < _railFence.GetLength(1); x++)
            {
                _cursor.X = x;
                _railFence[_cursor.Y, _cursor.X] = toEncode[x];
                NextStep();
            }
        }

        public void PrintArray()
        {
            Console.Write("  ");
            for (int i = 0; i < _railFence.GetLength(1); i++)
            {
                Console.Write(i);
            }

            Console.WriteLine();
            for (int y = 0; y < _railFence.GetLength(0); y++)
            {
                Console.Write(y+" ");
                for (int x = 0; x < _railFence.GetLength(1); x++)
                {
                    Console.Write(_railFence[y, x]);
                }

                Console.WriteLine();
                for (int x = 0; x < _railFence.GetLength(1)+2; x++)
                {
                    Console.Write('-');
                }

                Console.WriteLine();
            }
        }

        private string Initialize(string toEncode, int key)
        {
            int x = (int)toEncode.Length / key;
            int y = Equation(key, toEncode.Length, x);
            if (y > 0)
                toEncode += new string('X', y);

            _railFence = new char[key, toEncode.Length];
            for (int i = 0; i < _railFence.GetLength(0); i++)
            {
                for (int j = 0; j < _railFence.GetLength(1); j++)
                {
                    _railFence[i, j] = Char.MinValue;
                }
            }

            _cursor = new Point(0, 0);
            _direction = Direction.RightDown;

            return toEncode;
        }

        private void NextStep()
        {
            switch (_direction)
            {
                case Direction.RightDown:
                    StepRightDown();
                    break;
                case Direction.RightUp:
                    StepRightUp();
                    break;
            }
        }

        private void StepRightDown()
        {
            if (_cursor.Y < _railFence.GetLength(0) - 1)
            {
                _cursor.Y++;
            }
            else if (_cursor.Y == _railFence.GetLength(0) - 1)
            {
                _cursor.Y--;
                _direction = Direction.RightUp;
            }
        }

        private void StepRightUp()
        {
            if (_cursor.Y > 0)
            {
                _cursor.Y--;
            }
            else if (_cursor.Y == 0)
            {
                _cursor.Y++;
                _direction = Direction.RightDown;
            }
        }

        public string GraphicsDecode(string toDecode, int key)
        {
            if (key <= 1 || key >= toDecode.Length)
                return "Wrong key";

            //put substitute chars to highlight the places where will be chars from encoded text
            PutSubstituteChars(toDecode, key);

            //put chars from encoded text to highlight places by lines to read to decode by RailFence rules
            ReplaceSubstituteChars(toDecode);

            //read chars by diagonal to decode the cipher
            return ReadCharsByDiagonal();
        }

        private string ReadCharsByDiagonal()
        {
            var output = string.Empty;
            _cursor.X = 0;
            _cursor.Y = 0;
            _direction = Direction.RightDown;
            for (int x = 0; x < _railFence.GetLength(1); x++)
            {
                _cursor.X = x;
                output += _railFence[_cursor.Y, _cursor.X];
                NextStep();
            }

            return output;
        }

        private void ReplaceSubstituteChars(string toDecode)
        {
            var index = 0;
            for (var y = 0; y < _railFence.GetLength(0); y++)
            {
                for (var x = 0; x < _railFence.GetLength(1); x++)
                {
                    if (_railFence[y, x] == 'X')
                    {
                        _railFence[y, x] = toDecode[index];
                        index++;
                    }
                }
            }
        }

        private void PutSubstituteChars(string toDecode, int key)
        {
            string substituteChars = new string('X', toDecode.Length);
            Initialize(substituteChars, key);
            PutEachCharToArrayByDiagonal(substituteChars);
        }

        #endregion

        #region Math solution

        private OrderedPair _orderedPair;

        public string MathEncode(string text, int key)
        {
            var x = text.Length / (key);
            if (text.Length % (key) != 0) x++;
            var y = Equation(key, text.Length, x);
            text += new string('x', y);
            

            if (key >= text.Length)
                return "Wrong key";

            _orderedPair = new OrderedPair();
            var output = string.Empty;

            for (var i = 0; i < key; i++)
            {
                _orderedPair.SetInterval(
                    new[] { _orderedPair.GetSpace(key, i, 0), _orderedPair.GetSpace(key, i, 1) });

                var pos = i;
                var finalCount = GetFinalCount(key, x, i);
                //Console.WriteLine(i + " - " + finalCount);
                for (var count = 0; count < finalCount; count++)
                {
                    var orderedPair = _orderedPair.GetOrderedPair();
                    //Console.WriteLine( $"{pos} - interval+1:{orderedPair+1} interval:{orderedPair}");
                    //Console.WriteLine(pos+" = "+text[pos]+" - "+orderedPair);
                    if (orderedPair == 0)
                        orderedPair = _orderedPair.GetOrderedPair();
                    if (x == 2)
                    {
                        orderedPair = _orderedPair.Intervals[0];
                    }

                    output += text[pos];
                    pos += orderedPair;
                }
            }

            return output;
        }

        private int GetFinalCount(int lenght, int diagonalsCount, int index)
        {
            if (index > 0 && index < lenght - 1)
            {
                return diagonalsCount;
            }

            if (index == 0)
            {
                var count = 1;
                for (var i = 1; i <= diagonalsCount; i++)
                {
                    if (count >= diagonalsCount)
                    {
                        return i;
                    }

                    count += 2;
                }
            }
            else if (index == lenght - 1)
            {
                var count = 0;
                for (var i = 0; i <= diagonalsCount; i++)
                {
                    if (count >= diagonalsCount)
                    {
                        return i;
                    }

                    count += 2;
                }
            }

            return 0;
        }

        private int Equation(int n, int l, int x)
        {
            int y = n + (n - 1) * x - l;
            return y;
        }

        public string MathDecode(string text, int key)
        {
            char[] forDecodingPurposes = new char[text.Length];
            for (var i = 0; i < forDecodingPurposes.Length; i++)
            {
                forDecodingPurposes[i] = (char)(i+64);
            }

            char[] output = new char[text.Length];
            char[] order = MathEncode(new string(forDecodingPurposes), key).ToCharArray();
            for (int i = 0; i < order.Length; i++)
            {
                int pos = order[i]-64;
                output[pos] = text[i];
            }

            return new string(output);
        }

        #endregion
    }
}