using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipStability
{
    class Matrix
    {
        #region Member Variables
        private int _row;
        private int _column;
        private double[,] _elements;
        #endregion

        #region Constructor
        public Matrix(int row, int column, double[,] elements)
        {
            this._row = row;
            this._column = column;
            this._elements = new double[_row, _column];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    this._elements[i, j] = elements[i, j];

                }
            }
        }

        public Matrix(double[,] elements)
        {
            this._row = elements.GetLength(0);
            this._column = elements.GetLength(1);
            this._elements = new double[_row, _column];
            for (int i = 0; i < elements.GetLength(0); i++)
            {

                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    this._elements[i, j] = elements[i, j];

                }

            }
        }
        public Matrix()
        {
            this._row = new int();
            this._column = new int();
            this._elements = new double[_row, _column];
        }

        #endregion

        #region Properties
        public int Row
        {
            get
            {
                return this._row;
            }

        }

        public int Column
        {
            get
            {
                return this._column;
            }

        }

        public double[,] Element
        {
            get
            {
                return this._elements;
            }


        }
        #endregion

        #region Public method
        public Matrix Matmul(Matrix m1, Matrix m2)
        {
            int rowForM1 = m1._row;
            int rowForM2 = m2._row;
            int colForM1 = m1._column;
            int colForM2 = m2._column;

            try
            {
                if (colForM1 == rowForM2)
                {
                    int rowFormatmul = rowForM1;
                    int colFormatmul = colForM2;
                    double[,] element = new double[rowFormatmul, colFormatmul];
                    for (int i = 0; i < rowFormatmul; i++)
                    {
                        for (int j = 0; j < colFormatmul; j++)
                        {
                            double sum = 0;
                            for (int k = 0; k < colForM1; k++)
                            {
                                sum = sum + m1._elements[i, k] * m2._elements[k, j];

                            }
                            element[i, j] = sum;

                        }

                    }
                    Matrix mat = new Matrix(element);
                    return mat;

                }

                else
                {
                    throw new MatrixException("ERROR........... Matrix Multiplication is not possible as comumn index in first matrix is not equal to row index of second matrix");
                }

            }

            catch
            {
                throw new MatrixException("ERROR........... Matrix Multiplication is not possible ");
            }
        }

        /// <summary>
        /// TransPose
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Matrix TransPose(Matrix m)
        {
            int rowindex = m._column;
            int colindex = m._row;
            double[,] element = new double[rowindex, colindex];

            for (int i = 0; i < m._column; i++)
            {
                for (int j = 0; j < m._row; j++)
                {
                    element[i, j] = m._elements[j, i];

                }

            }
            Matrix transpose = new Matrix(element);
            return transpose;
        }
        /// <summary>
        /// Inverse
        /// </summary>
        /// <param name="inveratbleMatrix"></param>
        /// <returns></returns>
        public Matrix Inverse(Matrix matToInverse)
        {

            Matrix inveratbleMatrix = new Matrix(matToInverse.Element);
            try
            {
                if (inveratbleMatrix.Row == inveratbleMatrix.Column)
                {
                    int row = inveratbleMatrix._row;
                    int col = inveratbleMatrix._column;
                    double[,] identity = new double[row, col];
                    #region  forming Identity Matrix
                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < col; j++)
                        {
                            if (i == j)
                            {
                                identity[i, j] = 1.0F;
                            }
                            else
                            {
                                identity[i, j] = 0.0F;
                            }

                        }

                    }

                    Matrix identityMatrix = new Matrix(identity);

                    #endregion

                    #region Operating on identity Matrix

                    for (int i = 0; i < row; i++)
                    {

                        #region Checking for  non zero Heading element
                        if ((Math.Abs(inveratbleMatrix.Element[i, i])) < 0.0000001)
                        {
                            int firstNonzeroRow = 0;
                            inveratbleMatrix.SwapWithFirstNonzeroHeadingRow(inveratbleMatrix, i, ref firstNonzeroRow);
                            identityMatrix.SwapBetweenTwoRows(identityMatrix, i, firstNonzeroRow);
                        }
                        #endregion

                        #region Making Heading element one

                        double factorToDivide = inveratbleMatrix.Element[i, i];
                        for (int j = 0; j < col; j++)
                        {
                            inveratbleMatrix.Element[i, j] = inveratbleMatrix.Element[i, j] / factorToDivide;
                            identityMatrix.Element[i, j] = identityMatrix.Element[i, j] / factorToDivide;

                        }
                        #endregion

                        #region Making All element zero below given row

                        for (int j = 0; j < inveratbleMatrix.Row; j++)
                        {
                            if (j != i)
                            {
                                double factor = inveratbleMatrix.Element[j, i];
                                for (int k = 0; k < inveratbleMatrix.Column; k++)
                                {
                                    inveratbleMatrix.Element[j, k] = inveratbleMatrix.Element[j, k] - factor * inveratbleMatrix.Element[i, k];
                                    identityMatrix.Element[j, k] = identityMatrix.Element[j, k] - factor * identityMatrix.Element[i, k];

                                }

                            }

                        }



                        #endregion


                    }



                    #endregion

                    return identityMatrix;

                }

                else
                {
                    throw new MatrixException("Inverse is not possible");
                    //return null;
                }

            }
            catch
            {
                throw new MatrixException("Inverse is not possible");
            }
        }
        #endregion

        #region Private Function Swaping With first nonzero row and with given row
        private void SwapWithFirstNonzeroHeadingRow(Matrix m, int rowindex, ref int FirstNonZeroRow)
        {
            int row = m.Row;
            int col = m._column;
            FirstNonZeroRow = -1;

            if (rowindex == row - 1)
            {
                throw new MatrixException("Matrix inversion is not possible as matrix is singular");
            }
            else
            {
                for (int i = rowindex + 1; i < m.Row; i++)
                {
                    if ((Math.Abs(m.Element[i, rowindex])) > 0.0000001)
                    {
                        for (int j = 0; j < m.Column; j++)
                        {
                            double temp = m.Element[rowindex, j];
                            m.Element[rowindex, j] = m.Element[i, j];
                            m.Element[i, j] = temp;

                        }
                        FirstNonZeroRow = i;
                        break;

                    }
                }

            }

            if (FirstNonZeroRow == -1)
            {
                throw new MatrixException("Inverse is not possible as det=0");
            }


        }
        private void SwapBetweenTwoRows(Matrix m, int rowindex, int changewithRow)
        {
            for (int j = 0; j < m.Column; j++)
            {
                double temp = m.Element[rowindex, j];
                m.Element[rowindex, j] = m.Element[changewithRow, j];
                m.Element[changewithRow, j] = temp;

            }
        }
        #endregion

        #region Private Class MatrixException
        private class MatrixException : Exception
        {
            public MatrixException()
                : base()
            { }

            public MatrixException(string Message)
                : base(Message)
            { }

        }
        #endregion        
    }
}
