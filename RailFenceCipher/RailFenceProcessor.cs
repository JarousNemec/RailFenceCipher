using System;
using System.Drawing;

namespace RailFenceCipher
{
    public class RailFenceProcessor
    {
        private char[,] _railFence;
        private Point _cursor;
        private Direction _direction;

        public RailFenceProcessor()
        {
        }

        public string Encode(string toEncode, int key)
        {
            if (key <= 1 || key >= toEncode.Length)
                return "Wrong key";
            
            Initialize(toEncode, key);
            
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

        private void PrintArray()
        {
            for (int y = 0; y < _railFence.GetLength(0); y++)
            {
                for (int x = 0; x < _railFence.GetLength(1); x++)
                {
                    Console.Write(_railFence[y, x]);
                }
                Console.WriteLine();
                for (int x = 0; x < _railFence.GetLength(1); x++)
                {
                    Console.Write('-');
                }
                Console.WriteLine();
            }
        }

        private void Initialize(string toEncode, int key)
        {
            _railFence = new char[key, toEncode.Length];
            for (int i = 0; i < _railFence.GetLength(0); i++)
            {
                for (int j = 0; j < _railFence.GetLength(1); j++)
                {
                    _railFence[i, j] = Char.MinValue;
                }

                Console.WriteLine();
            }
            _cursor = new Point(0, 0);
            _direction = Direction.RightDown;
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

        public string Decode(string toDecode, int key)
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
    }
}