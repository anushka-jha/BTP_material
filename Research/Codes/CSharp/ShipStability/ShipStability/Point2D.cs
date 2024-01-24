using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class Point2D
    {
        #region member variable
        private double _x;
        private double _y;

        #endregion

        #region Constructor

        public Point2D()
        {
            this._x = new double();
            this._y = new double();

        }

        public Point2D(double x, double y)
        {
            this._x = x;
            this._y = y;

        }

        #endregion

        #region Properties

        public double X
        {
            get { return this._x; }
            set { this._x = value; }
        }

        public double Y
        {
            get { return this._y; }
            set { this._y = value; }
        }

        #endregion



    }
}
