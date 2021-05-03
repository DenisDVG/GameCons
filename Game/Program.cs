using System;

namespace Game
{
    // https://www.youtube.com/watch?v=tR30963rDig&ab_channel=%D0%94%D0%BC%D0%B8%D1%82%D1%80%D0%B8%D0%B9%D0%A1%D1%82%D0%BE%D0%BF%D0%BA%D0%B5%D0%B2%D0%B8%D1%87
    // 5:44
    class Program
    {
        private const int ScreenWidth = 100;
        private const int ScreenHeight = 50;

        private const int MapWidth = 32;
        private const int MapHeight = 32;

        private const double Fov = Math.PI / 3;
        private const double Depth = 16;

        private static double _playerX = 5;
        private static double _playerY = 5;
        private static double _playerA = 0;

        private static string _map = "";

        private static readonly char[] Screen = new char[ScreenWidth * ScreenHeight];


        static void Main(string[] args)
        {
            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
            Console.CursorVisible = false;

            _map += "################################";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "################################";
            while (true)
            {
                _playerA += 0.003;


                for (int x = 0; x < ScreenWidth; x++)
                {
                    double rayAngle = _playerA + Fov / 2 - x * Fov / ScreenWidth;

                    double rayX = Math.Sin(rayAngle);
                    double rayY = Math.Cos(rayAngle);

                    double distanceToWall = 0;
                    bool hitWall = false;

                    while (!hitWall && distanceToWall < Depth)
                    {
                        distanceToWall += 0.1;
                        int testX = (int)(_playerX + rayX * distanceToWall);
                        int testY = (int)(_playerY + rayY * distanceToWall);

                        if (testX < 0 || testX >= Depth + _playerX || 
                            testY < 0 || testY >= Depth + _playerY)
                        {
                            hitWall = true;
                            distanceToWall = Depth;
                        }
                        else
                        {
                            char testCell = _map[testY * MapWidth + testX];

                            if (testCell == '#')
                            {
                                hitWall = true;
                            }
                        }
                    }

                    int celling = (int)(ScreenHeight / 2d - ScreenHeight / distanceToWall);
                    int floor = ScreenHeight - celling;

                    for (int y = 0; y < ScreenHeight; y++)
                    {
                        if (y <= celling)
                        {
                            Screen[y * ScreenWidth + x] = ' ';
                        }
                        else if (y > celling && y <= floor)
                        {
                            Screen[y * ScreenWidth + x] = '#';
                        }
                        else
                        {
                            Screen[y * ScreenWidth + x] = '.';
                        }
                    }
                }

                Console.SetCursorPosition(left: 0, top:0);
                Console.Write(buffer:Screen);
            }
        }
    }
}
