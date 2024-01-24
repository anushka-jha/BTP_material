using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class Object
    {
        #region member
        private double _wt;
        private Point _cg;
        private double _fsmt;

        #endregion


        #region Properties

        public double Weight
        {
            get
            {
                return this._wt;
            }

            set { this._wt = value; }

        }

        public Point CG
        {
            get
            {
                return this._cg;
            }

            set { this._cg = value; }
        }

        public double Fsmt
        {
            get { return this._fsmt; }
            set { this._fsmt = value; }
        }

        #endregion
    }
}
