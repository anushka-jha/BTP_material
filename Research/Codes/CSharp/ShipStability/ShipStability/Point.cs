using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class Point
    {
        #region member variable
        private double _x;
        private double _y;
        private double _z;

        #endregion

        #region Constructor

        public Point()
        {
            this._x = new double();
            this._y = new double();
            this._z = new double();
        }

    
        public Point(double x, double y, double z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
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

        public double Z
        {
            get { return this._z; }
            set { this._z = value; }
        }

        #endregion

    }
}
