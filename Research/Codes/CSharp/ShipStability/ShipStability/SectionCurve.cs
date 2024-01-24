using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class SectionCurve
    {
        #region member variables
        private double _xloc ;
        private List<Point2D> _point2Dlist;

        #endregion

        #region Constructor
        public SectionCurve()
        {
           this._point2Dlist = new List<Point2D>();
            this._xloc = new double();
        }

        #endregion

        #region Properties

        public double Xloc
        {
            get { return this._xloc; }
            set { this._xloc = value; }
        }
       

        public List<Point2D> SectionPts
        {
            get { return this._point2Dlist; }
        }

        #endregion

        #region public methods

        public void Addpoints(Point2D pt2D)
        {
            this._point2Dlist.Add(pt2D);
        }

              

        public double GetSecCurveLength(SectionCurve curve)
        {
            return (getCurveLength(curve.SectionPts));
        }

        public List<SectionCurve> GetUpdatedSectionCurve(Point2D lbPt, Point2D rtPt, List<Section> secList, double maxDraft)
        {
            List<SectionCurve> curveList = new List<SectionCurve>();
            PublicHelper helper = new PublicHelper();

            for (int i = 0; i<secList.Count; i++)
            {
                SectionCurve curve = secList[i].SecCurve;
                List<Point2D> ptList = curve.SectionPts;
                double xLoc = secList[i].Xlocation;
                double draft = helper.SolveLinearEquation(lbPt, rtPt, xLoc);
                double zvalue = this.getUpdatedDrat(maxDraft, draft);
                double yvalue = this.GetSectionalYvalue(xLoc, lbPt, rtPt, ptList, maxDraft);               
                Point2D endPt = new Point2D(zvalue, yvalue);
                SectionCurve updatedCurve = this.GetModifiedCurve(ptList, endPt);
                updatedCurve.Xloc = xLoc;
                curveList.Add(updatedCurve);

            }


            return curveList;
        }

        public double GetSectionalYvalue ( double xloc, Point2D lbPt, Point2D rtPt, List<Point2D> sectionLines, double maxdraft)
        {
            PublicHelper helper = new PublicHelper();

            double draft = helper.SolveLinearEquation(lbPt, rtPt, xloc);
            double zvalue = this.getUpdatedDrat(maxdraft, draft);
            double yvalue = helper.InterpolateData(zvalue, sectionLines, 2);

            return yvalue;
        }

        public double getUpdatedDrat(double maxDraft, double draft)
        {

            PublicHelper helper = new PublicHelper();
            double d = draft;

            if (draft > maxDraft)
            {
                d = maxDraft;
            }
            else if (draft < 0)
            {
                d = 0;
            }



            return d;

        }

        public SectionCurve GetModifiedCurve (List<Point2D> ptList, Point2D endPt)
        {
            SectionCurve curve = new SectionCurve();
            
            double z = ptList[0].X;
            int k = 0;
            while ((endPt.X > z))
            {
                curve.Addpoints(ptList[k]);
                k++;
                z = ptList[k].X;
            }

            curve.Addpoints(endPt);
            
            
            return curve;
        }

        



        #endregion

        #region private method
        private double getCurveLength(List<Point2D> pt2Dlist)
        {
            double sum = 0;

            for (int i = 0; i < pt2Dlist.Count - 1; i++)
            {
                sum = sum + this.getDistance(pt2Dlist[i + 1], pt2Dlist[i]);
            }


            return sum;
        }

        private double getDistance(Point2D pt1, Point2D pt2)
        {

            double dist = Math.Sqrt((pt1.X - pt2.X) * (pt1.X - pt2.X) + (pt1.Y - pt2.Y) * (pt1.Y - pt2.Y));
            return dist;
        }

        #region GetModifiedCurve

       

        //int getIndex (List<Point2D> pt)

        #endregion GetModifiedCurve

        #endregion

    }
}
