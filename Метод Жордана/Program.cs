class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите количество строк матрицы:");
        //Ввод числа строк и столбцов матрицы с консоли (ниже 3 строки)
        //int rows = int.Parse(Console.ReadLine());

        //Console.WriteLine("Введите количество столбцов матрицы:");
        //int columns = int.Parse(Console.ReadLine());

        int rows = 3;
        int columns = 6;

        JordanElimination jordanElimination = new JordanElimination(rows, columns);

        // double[,] matrix = new double[rows, columns];

        /*Console.WriteLine("Введите матрицу построчно (элементы разделяйте пробелами):");
            for (int i = 0; i < rows; i++)
            {
                string[] rowValues = Console.ReadLine().Split(' ');
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = double.Parse(rowValues[j]);
                }
            }*/ 

        // double[,] matrix = new double[3, 5]
        // {
        //     { 4, 1, 2, 1 , 0 },
        //     { 6, 1, 1, 0, 1 },
        //     { 10, 1, -1, -2, 3 }
        // };
        double[,] matrix = new double[3, 6]
           {
               { 5, 1, 1, 0, 0, 1},
               { 8, -1, 2, 0, -1, 0},
               { 2, 2, 1, 1, 0, 0}
           };

        // double[,] matrix = new double[,]
        // {
        //     { 1, 1, 2, 3},
        //     { 1, 1, 1, 1},
        //     { 2, 1, 0, -1}
        // };

        //double[,] matrix = new double[,]
        //{
            //{ 1, 1, 1, -2, 3},
            //{ 2, 2, -1, -1, 3},
        //};


        jordanElimination.SetMatrix(matrix);

        while (true)
        {
            Console.WriteLine("Текущая матрица:");
            jordanElimination.PrintMatrix();

            Console.WriteLine("Введите номер строки разрешающего элемента:");
            int razRow = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите номер столбца разрешающего элемента:");
            int razCol = int.Parse(Console.ReadLine());

            jordanElimination.PerformElimination(razRow, razCol);

            Console.WriteLine("Матрица после исключения:");
            jordanElimination.PrintMatrix();

            Console.WriteLine("Хотите продолжить? (Enter - да / Ctrl + C - нет / Esc - вывести результат):");
            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.Escape)
            {
                jordanElimination.PrintResult();
                break;
            }
        }

        Console.WriteLine(" Программа завершена. Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}