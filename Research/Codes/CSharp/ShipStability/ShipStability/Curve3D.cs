using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipStability
{
    class Curve3D
    {
        #region member variables
        List<Point> _curvePt;
        #endregion member variables

        #region Constructor
        public Curve3D()
        {
            this._curvePt = new List<Point>();
        }
        #endregion Constructor

        #region Properties
        public List<Point> SectionPts
        {
            get { return this._curvePt; }
        }
        #endregion Properties

        #region public methods
        public void Addpoints(Point pt2D)
        {
            this._curvePt.Add(pt2D);
        }
        #endregion public methods
    }
}
