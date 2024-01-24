using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class WaterPlane
    {
        #region member variable
        private int _nwaterLine;
        private double _zlocation;
        private SectionCurve _waterline;
        private double _awp;
        private double _lcf;
        private double _lengthWline;

        #endregion

        #region Constructor

        public WaterPlane()
        {
            this._lcf = new double();
            this._nwaterLine = new int();
            this._waterline = new SectionCurve();
            this._zlocation = new double();
            this._awp = new double();
            this._lengthWline = new double();

        }

        #endregion

        #region property

        public int NwaterLine
        {
            get { return this._nwaterLine; }
            set { this._nwaterLine = value; }
        }

        public double Lcf
        {
            get { return this._lcf; }
            set { this._lcf = value; }
        }

        public double Awp
        {
            get { return this._awp; }
            set { this._awp = value; }
        }

        public double Zlocation
        {
            get { return this._zlocation; }
            set { this._zlocation = value; }
        }

        public double WlineLength
        {
            get { return this._lengthWline; }
            set { this._lengthWline = value; }
        }

        public SectionCurve Waterline
        {
            get { return this._waterline; }
            set { this._waterline = value; }
        }



        #endregion

        #region public Method
        public List<Point2D> GetTrimedWaterLine (List<Section> secList, Point2D stlpt, Point2D endrtPt, double maxdraft)
        {
            SectionCurve curveHelper = new SectionCurve();
            List<Point2D> waterLine = new List<Point2D>();
            for (int i = 0; i < secList.Count; i++)
            {
                SectionCurve curve = secList[i].SecCurve;
                List<Point2D> ptList = curve.SectionPts;
                double xLoc = secList[i].Xlocation;
                double yvalue = curveHelper.GetSectionalYvalue(xLoc, stlpt, endrtPt, ptList, maxdraft);
                Point2D endPt = new Point2D(xLoc, yvalue);
                waterLine.Add(endPt);

            }



            return waterLine;
        }

        public double GetWpAreaMoment (List<Point2D> ptList, double refxPt)
        {
            PublicHelper helper = new PublicHelper();
           
            List<Point2D> xmomPtList = new List<Point2D>();
            for ( int i = 0; i < ptList.Count; i++)
            {
                Point2D pt = new Point2D(ptList[i].X, (ptList[i].X - refxPt) * ptList[i].Y);
                xmomPtList.Add(pt);
            }

            double value = helper.GetSecArea(xmomPtList);

            return value;
        }

        public double GetTcf ( List<Point2D> ptList)
        {
            PublicHelper helper = new PublicHelper();
            List<Point2D> yptList = new List<Point2D>();
            foreach ( Point2D pt in ptList)
            {
                double value = pt.Y * (pt.Y / 2);
                yptList.Add(new Point2D (pt.X, value));

            }

            double area = helper.GetSecArea(ptList);
            double mom = helper.GetSecArea(yptList);

            double tcf = mom / area;

            return tcf;
        }

        public List<Point2D> GetHeeledWaterLine(List<SectionCurve> refineList, Point2D pt)
        {
            PublicHelper helper = new PublicHelper();
            List<Point2D> waterLinePt = new List<Point2D>();

            foreach(SectionCurve curve in refineList)
            {
                double xloc = curve.Xloc;
                List<Point2D> ptList = curve.SectionPts;
                double py = helper.InterpolateData(pt.X, ptList, 2);
                double sy = helper.InterpolateData(pt.Y, ptList, 2);
                double x2 = (pt.X - pt.Y) * (pt.X - pt.Y);
                double y2 = (py - sy) * (py - sy);

                double dist = Math.Sqrt(x2 + y2);

                waterLinePt.Add(new Point2D(xloc, dist));
            }
           

            return waterLinePt;
        }

        #endregion public method


    }
}
