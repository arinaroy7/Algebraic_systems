using System;
using System.Collections.Generic;

public class JordanElimination
{
    public int Rows { get; }
    public int Columns { get; }
    public double[,] Matrix { get; private set; }
    public List<string> LeftVariables { get; private set; }
    public List<string> TopVariables { get; private set; }

    public JordanElimination(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Matrix = new double[rows, columns];
        LeftVariables = new List<string>();
        TopVariables = new List<string>();

        for (int i = 0; i < rows; i++)
        {
            LeftVariables.Add("0=");
        }

        for (int j = 0; j < columns; j++)
        {
            if (j == 0)
                TopVariables.Add("1");
            else
                TopVariables.Add($"-x{j}");
        }
    }

    public void SetMatrix(double[,] matrix)
    {
        Matrix = matrix;
    }

    public void PrintMatrix()
    {
        const int cellWidth = 8; // ширина каждой ячейки (можно менять)

        // Заголовки
        Console.Write("|".PadRight(cellWidth));
        for (int j = 0; j < TopVariables.Count; j++)
        {
            if (TopVariables[j] != null)
                Console.Write($"{TopVariables[j], cellWidth}|");
        }
        Console.WriteLine();

        // Разделительная линия
        Console.WriteLine(new string('-', (Columns + 1) * (cellWidth + 1)));

        // Строки матрицы
        for (int i = 0; i < Rows; i++)
        {
            bool isZeroRow = true;
            for (int j = 0; j < Columns; j++)
            {
                if (TopVariables[j] != null && Matrix[i, j] != 0)
                {
                    isZeroRow = false;
                    break;
                }
            }

            if (isZeroRow && LeftVariables[i] == "0=")
                continue;

            Console.Write($"|{LeftVariables[i],cellWidth-1}|");
            for (int j = 0; j < Columns; j++)
            {
                if (TopVariables[j] != null)
                    Console.Write($"{Matrix[i, j],cellWidth:0.00}|");
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', (Columns + 1) * (cellWidth + 1)));
        }
    }


    public void PerformElimination(int razRow, int razCol)
    {
        double razrElem = Matrix[razRow, razCol];
        double[,] element = new double[Rows, Columns];

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (i == razRow && j == razCol)
                {
                    element[i, j] = 1 / razrElem;
                }
                else if (i == razRow)
                {
                    element[i, j] = -Matrix[razRow, j] / razrElem;
                }
                else if (j == razCol)
                {
                    element[i, j] = Matrix[i, razCol] / razrElem;
                }
                else
                {
                    element[i, j] = (Matrix[i, j] * razrElem - Matrix[i, razCol] * Matrix[razRow, j]) / razrElem;
                }
            }
        }

        // Обновляем текущую матрицу
        Matrix = element;

        // Перемещаем разрешающий элемент налево и убираем минус
        string variable = TopVariables[razCol].TrimStart('-');
        LeftVariables[razRow] = variable;
        TopVariables[razCol] = null;

        // Домножаем всю строку на -1
        for (int j = 0; j < Columns; j++)
        {
            Matrix[razRow, j] *= -1;
        }
    }

    public void PrintResult()
    {
        Console.WriteLine("Результат:");

        if (LeftVariables.Contains("0="))
        {
            for (int i = 0; i < Rows; i++)
            {
                if(LeftVariables[i] == "0=")
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if(TopVariables[j] != null && Matrix[i, j] != 0)
                        {
                            Console.WriteLine("Система несовместна");
                            return;
                        }
                    }
                }
            }
        }


        char variableName = 'a';
        for (int j = 1; j < TopVariables.Count; j++)
        {
            if (TopVariables[j] != null)
            {
                Console.WriteLine($"{TopVariables[j].TrimStart('-')} = {variableName}");
                variableName++;
            }
        }

        for (int i = 0; i < Rows; i++)
        {
            if (LeftVariables[i] != "0=")
            {
                Console.Write($"{LeftVariables[i]} = {Matrix[i, 0]}");
                variableName = 'a';
                for (int j = 1; j < Columns; j++)
                {
                    if (TopVariables[j] != null)
                    {
                        Console.Write($" + {Matrix[i, j]}{variableName}");
                        variableName++;
                    }
                }
                Console.WriteLine();
            }
        }
    }
}