using System;
using System.Collections.Generic;
using System.Linq;

namespace JordanConsole
{
    class Program
    {
        static List<List<double>> JordanExclusionStep(List<List<double>> A, int k, int s)
        {
            int m = A.Count;
            int n = A[0].Count;

            double Aks = A[k][s];
            if (Math.Abs(Aks) < 1e-12)
                throw new InvalidOperationException("A[k][s] равен нулю");

            var Anew = A.Select(row => row.ToList()).ToList();

            Anew[k][s] = 1.0 / Aks;

            for (int j = 0; j < n; j++)
            {
                if (j == s) continue;
                Anew[k][j] = A[k][j] / Aks;
            }

            for (int i = 0; i < m; i++)
            {
                if (i == k) continue;
                Anew[i][s] = -(A[i][s] / Aks);
            }

            for (int i = 0; i < m; i++)
            {
                if (i == k) continue;
                double ais = A[i][s];
                for (int j = 0; j < n; j++)
                {
                    if (j == s) continue;
                    double aij = A[i][j];
                    double akj = A[k][j];
                    Anew[i][j] = (aij * Aks - ais * akj) / Aks;
                }
            }

            return Anew;
        }

        static void PrintMatrix(List<List<double>> A, List<string> rowLabels = null, List<string> colLabels = null)
        {
            if (colLabels != null)
            {
                Console.Write("       ");
                foreach (var label in colLabels)
                    Console.Write($"{label,8}");
                Console.WriteLine();
            }

            for (int i = 0; i < A.Count; i++)
            {
                string label = rowLabels != null && i < rowLabels.Count ? rowLabels[i] : "";
                Console.Write($"{label,-6}");
                foreach (var val in A[i])
                    Console.Write($"{val,8:0.00}");
                Console.WriteLine();
            }
        }

        static List<double> ReadRowOfDoubles(string prompt, int n)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine().Trim();
                string[] parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != n)
                {
                    Console.WriteLine($"Ожидано {n} чисел. Попробуйте снова.");
                    continue;
                }
                try
                {
                    return parts.Select(p => double.Parse(p)).ToList();
                }
                catch
                {
                    Console.WriteLine("Ошибка ввода. Введите числа через пробел.");
                }
            }
        }

        static List<double> JoinRowOfDoubles(int n, string row1)
        {
            string s = row1.Trim();
            string[] parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                return parts.Select(p => double.Parse(p)).ToList();
            }
            catch
            {
                Console.WriteLine("Ошибка");
                return new List<double>();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество строк и столбцов матрицы:");
            Console.Write("Число строк m: ");
            int m = int.Parse(Console.ReadLine().Trim());
            Console.Write("Число столбцов n: ");
            int n = int.Parse(Console.ReadLine().Trim());

            Console.WriteLine("Введите значения матрицы построчно:");
            var A = new List<List<double>>();
            for (int i = 0; i < m; i++)
            {
                var row = ReadRowOfDoubles($"Строка {i + 1}: ", n);
                A.Add(row);
            }

            var aRow = new List<string>();
            for (int j = n - 1; j < m + n - 2; j++)
                aRow.Add($"x{j + 1}");
            aRow.Add("f");
            aRow.Add("g");

            var aRow1 = new List<string>(aRow);

            var aStolbec = new List<string> { "1" };
            for (int j = 1; j < n; j++)
                aStolbec.Add($"x{j}");

            string row1 = "";
            for (int i = 0; i < n; i++)
            {
                double currentSum = 0;
                for (int j = 0; j < m - 1; j++)
                    currentSum += A[j][i];
                double minusCurrentSum = -1 * currentSum;
                row1 += " " + minusCurrentSum.ToString();
            }

            var currentRow = JoinRowOfDoubles(n, row1);
            A.Add(currentRow);

            for (int i = 0; i < m; i++)
            {
                if (A[i][0] < 0)
                {
                    for (int j = 0; j < n; j++)
                        A[i][j] = -A[i][j];
                }
            }

            m = m + 1;
            Console.WriteLine("Измененная матрица:");
            PrintMatrix(A, rowLabels: aRow, colLabels: aStolbec);

            double e = 0.00001;
            bool t = true;
            int x = 0;

            while (t)
            {
                int sIndex = -1;
                double valueOfMin = 10;
                for (int s = 1; s < n; s++)
                {
                    if (A[m - 1][s] < valueOfMin)
                    {
                        valueOfMin = A[m - 1][s];
                        sIndex = s;
                    }
                }

                int countOfPositiv = 0;
                for (int i = 0; i < m - 1; i++)
                {
                    if (A[i][sIndex] > 0.01)
                        countOfPositiv++;
                }

                if (countOfPositiv < 1)
                {
                    Console.WriteLine("f стремиться к бесконечности");
                    t = false;
                    break;
                }

                double minDrob = 100;
                int kIndex = -1;
                int count = 0;
                for (int i = 0; i < m - 2; i++)
                {
                    if (A[i][sIndex] != 0 && A[i][sIndex] > 0.01)
                    {
                        double elem = Math.Abs(A[i][0] / A[i][sIndex]);
                        if (elem < minDrob)
                        {
                            kIndex = i;
                            minDrob = elem;
                            count++;
                        }
                    }
                }

                if (count == 0)
                {
                    valueOfMin = 10;
                    for (int s = 1; s < n; s++)
                    {
                        if (A[m - 1][s] < valueOfMin && s != sIndex)
                        {
                            valueOfMin = A[m - 1][s];
                            sIndex = s;
                        }
                    }

                    minDrob = 100;
                    kIndex = -1;
                    for (int i = 0; i < m - 1; i++)
                    {
                        if (A[i][sIndex] != 0 || Math.Abs(A[i][sIndex]) > e)
                        {
                            double elem = A[i][0] / A[i][sIndex];
                            if (elem < minDrob)
                            {
                                kIndex = i;
                                minDrob = elem;
                            }
                        }
                    }
                }

                string element = aRow[kIndex];
                aRow[kIndex] = aStolbec[sIndex];
                aStolbec[sIndex] = element;

                A = JordanExclusionStep(A, kIndex, sIndex);
                //Console.WriteLine("Матрица после шага жардановых исключений:");
                PrintMatrix(A, rowLabels: aRow, colLabels: aStolbec);

                int countOfNonOtric = 0;
                for (int i = 1; i < n; i++)
                {
                    if (A[m - 1][i] > -e)
                        countOfNonOtric++;
                }

                if (countOfNonOtric == n - 1)
                {
                    if (aRow.SequenceEqual(aRow1))
                    {
                        Console.WriteLine("Оптимальный план:");
                        var exit = new double[m - 2];
                        for (int j = 0; j < m - 2; j++)
                        {
                            string elementt = aStolbec[j];
                            double element1 = double.Parse(elementt.Substring(1));
                            exit[(int)element1 - 1] = A[j][0];
                        }
                        string row_itog = string.Join(", ", exit);
                        Console.WriteLine("X^" + x + " = ( " + row_itog + " )");
                        Console.WriteLine("F = " + A[m - 2][0] + " - " + A[m - 1][0] + "M");
                        t = false;
                    }
                    else
                    {
                        Console.WriteLine("Оптимальный план:");
                        for (int j = 0; j < m - 2; j++)
                        {
                            string elementt = aRow[j];
                            Console.WriteLine(elementt + " = " + Math.Round(A[j][0]));
                        }
                        Console.WriteLine("F = " + Math.Round(A[m - 2][0]));
                        t = false;
                    }
                }
                x++;
            }
        }
    }
}