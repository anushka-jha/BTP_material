using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class Section
    {
        #region member variables
        private int _sectionNo;
        private double _xlocation;
        private List<double> _secdraftList;
        private SectionCurve _secCurve;
        private List<double> _secArea;
        private List<double> _secTmom;
        private List<double> _secVmom;
        private List<Point2D> _secXmom;
        private double _vcb;
        private double _secCurveLength;
        private List<double> _secVcb;

        #endregion

        #region Constructor

        public Section()
        {
            this._sectionNo = new int();
            this._xlocation = new double();
            this._secdraftList = new List<double>();
            this._secCurve = new SectionCurve();
            this._secArea = new List<double>();
            this._secTmom = new List<double>();
            this._secVmom = new List<double>();
            this._secCurveLength = new double();
            this._vcb = new double();
            this._secVcb = new List<double>();
            this._secXmom = new List<Point2D>();

    }


        #endregion

        #region properties

        public int SectionNo
        {
            get { return this._sectionNo; }
            set { this._sectionNo = value; }
        }

        public double Xlocation
        {
            get { return this._xlocation; }
            set { this._xlocation = value; }
        }

        public SectionCurve SecCurve
        {
            get { return this._secCurve; }
            set { this._secCurve = value; }
        }

        public List<double> SecTMoment
        {
            get { return this._secTmom; }
            
        }

        public List<double> SecVMoment
        {
            get { return this._secVmom; }

        }

        public List<Point2D> SecXMoment
        {
            get { return this._secXmom; }

        }

        public List<double> SecArea
        {
            get { return this._secArea; }

        }

        public List<double> SecDraftList
        {
            get { return this._secdraftList; }

        }

        public double SecCurveLength
        {
            get { return this._secCurveLength; }
            set { this._secCurveLength = value; }
        }

        public double VCb
        {
            get { return this._vcb; }
            set { this._vcb = value; }
        }

        public List<double> SecVcb
        {
            get { return this._secVcb; }
        }

        #endregion

        #region public Method

        public void AddArea(double sArea)
        {
            this._secArea.Add(sArea);     
           
        }

        public void Adddraft(double draft)
        {
            this._secdraftList.Add(draft);

        }

        public void AddSecVCb(double secVcb)
        {
            this._secVcb.Add(secVcb);
        }

        public void AddSecTMom ( double tMom)
        {
            this._secTmom.Add(tMom);
        }

        public void AddSecVMom ( double vMom)
        {
            this._secVmom.Add(vMom);
        }

        public void AddSecXMom(Point2D xMom)
        {
            this._secXmom.Add(xMom);
        }

        public void UpdateSectionCurve (Section sec)
        {
            SectionCurve curve = new SectionCurve();
            curve.Xloc = sec.SecCurve.Xloc;
            List<Point2D> pt2D = sec.SecCurve.SectionPts;
            double zlim = pt2D[0].X;
            double z = 0;
            double dz = 0.05;
            while (z<zlim)
            {
                curve.Addpoints(new Point2D(z, 0));
                z = z + dz;
            }

            foreach (Point2D pt in pt2D)
            {
                curve.Addpoints(pt);
            }

            this._secCurve = curve;

        }


        #endregion


    }


}
