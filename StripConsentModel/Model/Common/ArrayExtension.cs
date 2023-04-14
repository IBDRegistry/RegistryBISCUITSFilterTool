using System;
using System.Linq;

namespace StripV3Consent.Model
{
    public static class ArrayExtension
    {
        public static bool IsEmpty(this string[] Array)
        {
			if (Array.All(element => element == "")) {
                return true; 
            } else
			{
                return false;
            }	
        }

        public static T[,] Transpose<T>(this T[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            var result = new T[columns, rows];

            for (var c = 0; c < columns; c++)
            {
                for (var r = 0; r < rows; r++)
                {
                    result[c, r] = matrix[r, c];
                }
            }
            return result;
        }

        /// <summary>
        /// Jagged transpose, unsafe because jagged but saves on refactoring
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T[][] Transpose<T>(this T[][] source)
        {
            var rows = source.Length;
            var columns = source[0].Length;

            var result = source[0].Select(x => new T[rows]).ToArray();

            for (var c = 0; c < columns; c++)
            {
                for (var r = 0; r < rows; r++)
                {
                    result[c][r] = source[r][c];
                }
            }
            return result;
        }

        public static T[,] JaggedTo2D<T>(this T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }
    }

    
}
