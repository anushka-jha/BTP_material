using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class ShipGeom
    {
        #region member variable
        private double _lbp = new double();
        private double _loa = new double();
        private double _breadth = new double();
        private double _designdraft = new double();
        private double _depth = new double();
        private double _fld = new double();
        private double _downfloodingangle = new double();
        private List<Point2D> _draftAp = new List<Point2D>();
        private List<Point2D> _draftFp = new List<Point2D>();
        private List<Point2D> _draftVol = new List<Point2D>();
        private List<Point2D> _draftMass = new List<Point2D>();
        private List<Point2D> _draftWetarea = new List<Point2D>();
        private List<Point2D> _draftLcb = new List<Point2D>();
        private List<Point2D> _draftTcb = new List<Point2D>();
        private List<Point2D> _draftVcb = new List<Point2D>();
        private List<Point2D> _draftWpa = new List<Point2D>();
        private List<Point2D> _draftLcf = new List<Point2D>();
        private List<Point2D> _draftIxx0 = new List<Point2D>();
        private List<Point2D> _draftIxxg = new List<Point2D>();
        private List<Point2D> _draftIyy0 = new List<Point2D>();
        private List<Point2D> _draftBml = new List<Point2D>();
        private List<Point2D> _draftBmt = new List<Point2D>();
        private List<Point2D> _draftKml = new List<Point2D>();
        private List<Point2D> _draftKmt = new List<Point2D>();
        private List<Point2D> _draftTpc = new List<Point2D>();
        private List<Point2D> _draftMct = new List<Point2D>();
        private List<Point2D> _wtCurve = new List<Point2D>();
        private List<Point2D> _buoyancyCurve = new List<Point2D>();
        private List<Point2D> _hulldistribution = new List<Point2D>();
        private List<Point2D> _waterLine = new List<Point2D>();
        private double _rho;
        #endregion member variable

        #region Constructor

        public ShipGeom()
        {
            this._draftAp = new List<Point2D>();
            this._draftFp = new List<Point2D>();
            this._draftVol = new List<Point2D>();
            this._draftMass = new List<Point2D>();
            this._draftWetarea = new List<Point2D>();
            this._draftLcb = new List<Point2D>();
            this._draftTcb = new List<Point2D>();
            this._draftVcb = new List<Point2D>();
            this._draftWpa = new List<Point2D>();
            this._draftLcf = new List<Point2D>();
            this._draftIxx0 = new List<Point2D>();
            this._draftIxxg = new List<Point2D>();
            this._draftIyy0 = new List<Point2D>();
            this._draftBml = new List<Point2D>();
            this._draftBmt = new List<Point2D>();
            this._draftKml = new List<Point2D>();
            this._draftKmt = new List<Point2D>();
            this._draftTpc = new List<Point2D>();
            this._draftMct = new List<Point2D>();
            this._wtCurve = new List<Point2D>();
            this._buoyancyCurve = new List<Point2D>();
            this._hulldistribution = new List<Point2D>();
            this._waterLine = new List<Point2D>();
            this._lbp = new double();
            this._loa = new double();
            this._breadth = new double();
            this._designdraft = new double();
            this._depth = new double();
            this._fld = new double();
            this._downfloodingangle = new double();
            this._rho = new double();
    }

        public ShipGeom( double Lbp)
        {
            this._draftAp = new List<Point2D>();
            this._draftFp = new List<Point2D>();
            this._draftVol = new List<Point2D>();
            this._draftMass = new List<Point2D>();
            this._draftWetarea = new List<Point2D>();
            this._draftLcb = new List<Point2D>();
            this._draftTcb = new List<Point2D>();
            this._draftVcb = new List<Point2D>();
            this._draftWpa = new List<Point2D>();
            this._draftLcf = new List<Point2D>();
            this._draftIxx0 = new List<Point2D>();
            this._draftIxxg = new List<Point2D>();
            this._draftIyy0 = new List<Point2D>();
            this._draftBml = new List<Point2D>();
            this._draftBmt = new List<Point2D>();
            this._draftKml = new List<Point2D>();
            this._draftKmt = new List<Point2D>();
            this._draftTpc = new List<Point2D>();
            this._draftMct = new List<Point2D>();
            this._wtCurve = new List<Point2D>();
            this._buoyancyCurve = new List<Point2D>();
            this._hulldistribution = new List<Point2D>();
            this._waterLine = new List<Point2D>();
            this._lbp = Lbp;
            this._loa = new double();
            this._breadth = new double();
            this._designdraft = new double();
            this._depth = new double();
            this._fld = new double();
            this._downfloodingangle = new double();
            this._rho = new double();

        }

        #endregion Constructor

        #region Properties

        public double Rho
        {
            get { return this._rho; }
            set { this._rho = value; }
        }

        public double LBP
        {
            get { return this._lbp; }
            set { this._lbp = value; }
        }

        public double Loa
        {
            get { return this._loa; }
            set { this._loa = value; }
        }

        public double Breadth
        {
            get { return this._breadth; }
            set { this._breadth= value; }
        }

        public double DesignDraft
        {
            get { return this._designdraft; }
            set { this._designdraft = value; }
        }

        public double Depth
        {
            get { return this._depth; }
            set { this._depth = value; }
        }

        public double FLD
        {
            get { return this._fld; }
            set { this._fld = value; }
        }

        public double DownFLDangle
        {
            get { return this._downfloodingangle; }
            set { this._downfloodingangle = value; }
        }
        public List<Point2D> DraftAp
        {
            get
            {
                return this._draftAp;
            }
        }
        public List<Point2D> DrafFp
        {
            get
            {
                return this._draftFp;
            }
        }
        public List<Point2D> DraftVol
        {
            get
            {
                return this._draftVol;
            }
        }

        public List<Point2D> DraftMass
        {
            get
            {
                return this._draftMass;
            }
        }
        public List<Point2D> DraftWetarea
        {
            get
            {
                return this._draftWetarea;
            }
        }

        public List<Point2D> DraftLcb
        {
            get
            {
                return this._draftLcb;
            }
        }
        public List<Point2D> DraftTcb
        {
            get
            {
                return this._draftTcb;
            }
        }
        public List<Point2D> DraftVcb
        {
            get
            {
                return this._draftVcb;
            }
        }
        public List<Point2D> DraftWpa
        {
            get
            {
                return this._draftWpa;
            }
        }
        public List<Point2D> DraftLcf
        {
            get
            {
                return this._draftLcf;
            }
        }
        public List<Point2D> DraftIxx0
        {
            get
            {
                return this._draftIxx0;
            }
        }
        public List<Point2D> DraftIxxg
        {
            get
            {
                return this._draftIxxg;
            }
        }
        public List<Point2D> DraftIyy0
        {
            get
            {
                return this._draftIyy0;
            }
        }
        public List<Point2D> DraftBml
        {
            get
            {
                return this._draftBml;
            }
        }
        public List<Point2D> DraftBmt
        {
            get
            {
                return this._draftBmt;
            }
        }
        public List<Point2D> DraftKml
        {
            get
            {
                return this._draftKml;
            }
        }
        public List<Point2D> DraftKmt
        {
            get
            {
                return this._draftKmt;
            }
        }
        public List<Point2D> DraftTpc
        {
            get
            {
                return this._draftTpc;
            }
        }
        public List<Point2D> DraftMct
        {
            get
            {
                return this._draftMct;
            }
        }

        public List<Point2D> WtCurve
        {
            get
            {
                return this._wtCurve;
            }
        }

        public List<Point2D> BuoyancyCurve
        {
            get
            {
                return this._buoyancyCurve;
            }
        }

        public List<Point2D> HullWtDistribution
        {
            get
            {
                return this._hulldistribution;
            }
        }

        public List<Point2D> WaterLine
        {
            get
            {
                return this._waterLine;
            }
        }

        #endregion Properties

        #region public method

        public void AdddraftAp ( Point2D pt)

        {
            this._draftAp.Add(pt);
        }
        public void AdddraftFp(Point2D pt)

        {
            this._draftFp.Add(pt);
        }
        public void AdddraftVol(Point2D pt)

        {
            this._draftVol.Add(pt);
        }
        public void AdddraftMass(Point2D pt)

        {
            this._draftMass.Add(pt);
        }
        public void AdddraftWetarea(Point2D pt)

        {
            this._draftWetarea.Add(pt);
        }
        public void AdddraftLcb(Point2D pt)

        {
            this._draftLcb.Add(pt);
        }
        public void AdddraftTcb(Point2D pt)

        {
            this._draftTcb.Add(pt);
        }
        public void AdddraftVcb(Point2D pt)

        {
            this._draftVcb.Add(pt);
        }
        public void AdddraftWpa(Point2D pt)

        {
            this._draftWpa.Add(pt);
        }
        public void AdddraftLcf(Point2D pt)

        {
            this._draftLcf.Add(pt);
        }
        public void AdddraftIxx0(Point2D pt)

        {
            this._draftIxx0.Add(pt);
        }
        public void AdddraftIxxg(Point2D pt)

        {
            this._draftIxxg.Add(pt);
        }
        public void AdddraftIyy0(Point2D pt)

        {
            this._draftIyy0.Add(pt);
        }
        public void AdddraftBml(Point2D pt)

        {
            this._draftBml.Add(pt);
        }
        public void AdddraftBmt(Point2D pt)

        {
            this._draftBmt.Add(pt);
        }
        public void AdddraftKml(Point2D pt)

        {
            this._draftKml.Add(pt);
        }
        public void AdddraftKmt(Point2D pt)

        {
            this._draftKmt.Add(pt);
        }
        public void AdddraftTpc(Point2D pt)

        {
            this._draftTpc.Add(pt);
        }
        public void AdddraftMct(Point2D pt)

        {
            this._draftMct.Add(pt);
        }

        public void AdddWtCurvePt(Point2D pt)

        {
            this._wtCurve.Add(pt);
        }

        public void AdddBuoyancyCurvePt(Point2D pt)

        {
            this._buoyancyCurve.Add(pt);
        }

        public void AddWaterlinePt(Point2D pt)

        {
            this._waterLine.Add(pt);
        }



        public void UpdateBuoyancyCurve(List<SectionCurve> curveList, double rho)
        {
            List<Point2D> bptList = new List<Point2D>();
            foreach ( SectionCurve curve in curveList)
            {
                PublicHelper helper = new PublicHelper();
                double xloc = curve.Xloc;
                List<Point2D> ptList = curve.SectionPts;
                double area = helper.GetSecArea(ptList);
                Point2D pt = new Point2D(xloc, area * rho);
                bptList.Add(pt);
            }

            this._buoyancyCurve.RemoveRange(0, _buoyancyCurve.Count);
            this._buoyancyCurve.AddRange(bptList);
        }

        public void UpdateWaterline(List<Point2D> updatedCurve)
        {
            this._waterLine.RemoveRange(0, _waterLine.Count);
            this._waterLine.AddRange(updatedCurve);
        }

        

        public void UpdateLoadDistribution(List<Point2D> steelWt, List<Point2D> solidWt, List<Tank> tankData, List<Mapping> mp)
        {
            OutputParamHelper helper = new OutputParamHelper();
            List<Point2D> loadList = helper.GetWtDistribution(steelWt, solidWt,  tankData, mp);
            this._wtCurve.RemoveRange(0, WtCurve.Count);
            this._wtCurve.AddRange(loadList);

        }

        public void GetHullWtDistribution(List<Section> secList, List<Vector2D> distGraph, double conversion)
        {
            PublicHelper helper = new PublicHelper();
            foreach (Section sec in secList)
            {
                double x = sec.Xlocation;
                double y = 0;
                foreach (Vector2D v in distGraph)
                {
                    
                    if((v.StPt.X<=x)&&(v.EndPt.X>x))
                    {
                        y = y +  conversion * helper.SolveLinearEquation(v.StPt, v.EndPt, x);
                        
                        
                    }
                }
                this._hulldistribution.Add(new Point2D(x, 0.9737*y));
                //double load = helper.InterpolateData(x, distGraph, 2);
                //this._hulldistribution.Add(new Point2D(x, load*conversion));
            }
        }


        #endregion public method


    }
}
