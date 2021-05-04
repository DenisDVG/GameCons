using System;

namespace Game
{
    // https://www.youtube.com/watch?v=tR30963rDig&ab_channel=%D0%94%D0%BC%D0%B8%D1%82%D1%80%D0%B8%D0%B9%D0%A1%D1%82%D0%BE%D0%BF%D0%BA%D0%B5%D0%B2%D0%B8%D1%87
    // 18:59
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
            _map += "#.....................#........#";
            _map += "#.....................#........#";
            _map += "#.....................#........#";
            _map += "#.....................#........#";
            _map += "#.....................#........#";
            _map += "#.....................#........#";
            _map += "#.....................#........#";
            _map += "#......######.........#........#";
            _map += "#.....................#........#";
            _map += "#.....................#........#";
            _map += "#.....................#........#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#......................#########";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#........#.....................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "###########....................#";
            _map += "#..............................#";
            _map += "#.........#....................#";
            _map += "#.........#....................#";
            _map += "#.........#....................#";
            _map += "#.........#....................#";
            _map += "#.........#....................#";
            _map += "#.........#....................#";
            _map += "################################";

            DateTime dateTimeFrom = DateTime.Now;


            while (true)
            {
                DateTime dateTimeTo = DateTime.Now;
                double elapsedTime = (dateTimeTo - dateTimeFrom).TotalSeconds;
                dateTimeFrom = DateTime.Now;

                if (Console.KeyAvailable)
                {
                    ConsoleKey consoleKey = Console.ReadKey(intercept: true).Key;

                    switch (consoleKey)
                    {
                        case ConsoleKey.A:
                            _playerA += elapsedTime * 2;
                            break;
                        case ConsoleKey.D:
                            _playerA -= elapsedTime * 2;
                            break;
                        case ConsoleKey.W:
                        {
                            _playerX += Math.Sin(_playerA) * 10 * elapsedTime;
                            _playerY += Math.Cos(_playerA) * 10 * elapsedTime;

                            if (_map[(int) _playerY * MapWidth + (int) _playerX] == '#')
                            {
                                _playerX -= Math.Sin(_playerA) * 10 * elapsedTime;
                                _playerY -= Math.Cos(_playerA) * 10 * elapsedTime;
                            }
                            break;
                        }
                        case ConsoleKey.S:
                        {
                            _playerX -= Math.Sin(_playerA) * 10 * elapsedTime;
                            _playerY -= Math.Cos(_playerA) * 10 * elapsedTime;
                            if (_map[(int)_playerY * MapWidth + (int)_playerX] == '#')
                            {
                                _playerX += Math.Sin(_playerA) * 10 * elapsedTime;
                                _playerY += Math.Cos(_playerA) * 10 * elapsedTime;
                            }
                            break;
                        }

                    }
                }

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

                    char wallShade;

                    if (distanceToWall <= Depth / 4d)
                    {
                        wallShade = '\u2588';
                    }
                    else if (distanceToWall <= Depth / 3d)
                    {
                        wallShade = '\u2593';
                    }
                    else if (distanceToWall <= Depth / 2d)
                    {
                        wallShade = '\u2592';
                    }
                    else if (distanceToWall <= Depth)
                    {
                        wallShade = '\u2591';
                    }
                    else
                    {
                        wallShade = ' ';
                    }

                    for (int y = 0; y < ScreenHeight; y++)
                    {
                        if (y <= celling)
                        {
                            Screen[y * ScreenWidth + x] = ' ';
                        }
                        else if (y > celling && y <= floor)
                        {
                            Screen[y * ScreenWidth + x] = wallShade;
                        }
                        else
                        {
                            char floorShade;
                            double b = 1 - (y - ScreenHeight / 2d) / (ScreenHeight / 2d);

                            if (b < 0.25)
                            {
                                floorShade = '#';
                            }
                            else if (b < 0.5)
                            {
                                floorShade = 'x';
                            }
                            else if (b < 0.75)
                            {
                                floorShade = '-';
                            }
                            else if (b < 0.9)
                            {
                                floorShade = '.';
                            }
                            else
                            {
                                floorShade = ' ';
                            }

                            Screen[y * ScreenWidth + x] = floorShade;
                        }
                    }
                }

                Console.SetCursorPosition(left: 0, top:0);
                Console.Write(buffer:Screen);
            }
        }
    }
}
