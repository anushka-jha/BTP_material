using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ShipStability
{
    class CrossCurve
    {
        #region member variable

        private List<Dictionary<double, List<Point2D>>> _crosscurves;        
        private List<Dictionary<double, List<Point2D>>> _dispVlcb;
        private List<Dictionary<double, List<Point2D>>> _dispVtcb;
        private List<Dictionary<double, List<Point2D>>> _dispVvcb;
      

        #endregion

        #region Constructor

        #endregion Constructor

        public CrossCurve()
        {
            this._crosscurves = new List<Dictionary<double, List<Point2D>>>();
            this._dispVlcb = new List<Dictionary<double, List<Point2D>>>();
            this._dispVtcb = new List<Dictionary<double, List<Point2D>>>();
            this._dispVvcb = new List<Dictionary<double, List<Point2D>>>();

        }

        #region Properties

        public List<Dictionary<double, List<Point2D>>> CrossCurves

        {
            get { return this._crosscurves; }
        }

        public List<Dictionary<double, List<Point2D>>> DispVLcb

        {
            get { return this._dispVlcb; }
        }

        public List<Dictionary<double, List<Point2D>>> DispVTcb

        {
            get { return this._dispVtcb; }
        }

        public List<Dictionary<double, List<Point2D>>> DispVVcb

        {
            get { return this._dispVvcb; }
        }


        //public void GetCrossCurveData
        //{

        //}


        #endregion Properties

        #region Public Method
        public void AddCrossCurve(Dictionary<double, List<Point2D>> crCurve) 
        {
            //Dictionary<double, Dictionary<double, double>> dic = new Dictionary<double, Dictionary<double, double>>(value, curve);
            this._crosscurves.Add(crCurve);
        }

        public void AddLcbPoint(Dictionary<double, List<Point2D>> crCurve)
        {
            //Dictionary<double, Dictionary<double, double>> dic = new Dictionary<double, Dictionary<double, double>>(value, curve);
            this._dispVlcb.Add(crCurve);
        }

        public void AddTcbPoint(Dictionary<double, List<Point2D>> crCurve)
        {
            //Dictionary<double, Dictionary<double, double>> dic = new Dictionary<double, Dictionary<double, double>>(value, curve);
            this._dispVtcb.Add(crCurve);
        }

        public void AddVcbPoint(Dictionary<double, List<Point2D>> crCurve)
        {
            //Dictionary<double, Dictionary<double, double>> dic = new Dictionary<double, Dictionary<double, double>>(value, curve);
            this._dispVvcb.Add(crCurve);
        }




        #endregion Public Method

        #region public function for kn
        /// <summary>
        /// In this program Kn curve is calculated for zero trim
        /// All the input angle should be in degree, maximum 75 degree
        /// Theta increment is recomended for 5 degree
        /// </summary>
        /// <param name="secCurveList"></param>
        /// <param name="thetamin"></param>
        /// minimum theta in degree
        /// <param name="thetamax"></param>
        /// maximum theta in degree
        /// <param name="thetaIncrement"></param>
        /// <param name="zMax"></param>
        /// <param name="rho"></param>
        /// <returns></returns>
        public CrossCurve GetKnCurve(List<SectionCurve> secCurveList, double thetamin, double thetamax, double thetaIncrement, double zMax, double rho)
        {
            StreamWriter sr = new StreamWriter("D://Ranadev//Research//Codes//Input//output//CrossCurve.dat");
            sr.WriteLine("disp  " + "kn  " + "Tcb   " + "VcB");
            CrossCurve knCurve = new CrossCurve();
            
            double theta = thetamin;
            double thetaRadian = 0;
            double factor = Math.PI / 180;
            while ( theta < thetamax + thetaIncrement/2)
            {
                Console.WriteLine("Writing the data for theta = " + theta.ToString());
                Dictionary<double, List<Point2D>> knCurvefortheta = new Dictionary<double, List<Point2D>>();
                Dictionary<double, List<Point2D>> tcbCurveforTheta = new Dictionary<double, List<Point2D>>();
                Dictionary<double, List<Point2D>> vcbCurveforTheta = new Dictionary<double, List<Point2D>>();
                thetaRadian = factor * theta;
                List<Point2D> tcbList = new List<Point2D>();
                List<Point2D> vcbList = new List<Point2D>();
                List<Point2D> knList = this.getKnforFixedtheta(secCurveList, thetaRadian, zMax, rho, sr, ref tcbList, ref vcbList);
                knCurvefortheta.Add(theta, knList);
                tcbCurveforTheta.Add(theta, tcbList);
                vcbCurveforTheta.Add(theta, tcbList);
                knCurve.AddCrossCurve(knCurvefortheta);
                knCurve.AddTcbPoint(vcbCurveforTheta);
                knCurve.AddVcbPoint(vcbCurveforTheta);
                theta = theta + thetaIncrement;
            }

            sr.Close();

            return knCurve;

            
        }

        #endregion public function for kn

        public List<Point2D> GetSecCrossCurveValue(List<SectionCurve> secCurveList, double theta, double dz, ref List<Point2D> tMomList, ref List<Point2D> zMomList, ref List<Point2D>yList)
        {
            List<Point2D> crossCurveValueList = new List<Point2D>();
            tMomList = new List<Point2D>();
            zMomList = new List<Point2D>();
            yList = new List<Point2D>();


            for (int i = 0; i < secCurveList.Count; i++)
            {
                List<Point2D> ptList = secCurveList[i].SectionPts;
                double xLoc = secCurveList[i].Xloc;
                double ymax = 0;
                Point pt = this.getValuesforfixZ(ptList, theta, dz, out ymax);
                Point2D pt1 = new Point2D(xLoc, pt.X);
                Point2D pt2 = new Point2D(xLoc, pt.Y);
                Point2D pt3 = new Point2D(xLoc, pt.Z);
                Point2D pt4 = new Point2D(xLoc, ymax);
                crossCurveValueList.Add(pt1);
                tMomList.Add(pt2);
                zMomList.Add(pt3);
                yList.Add(pt4);

            }



            return crossCurveValueList;

        }

        public List<Point2D> GetSecCrossCurveValue(List<SectionCurve> secCurveList, double theta, double dz, ref List<Point2D> tMomList, ref List<Point2D> zMomList)
        {
            List<Point2D> crossCurveValueList = new List<Point2D>();
            tMomList = new List<Point2D>();
            zMomList = new List<Point2D>();


            for (int i = 0; i < secCurveList.Count; i++)
            {
                List<Point2D> ptList = secCurveList[i].SectionPts;
                double xLoc = secCurveList[i].Xloc;
                Point pt = this.getValuesforfixZ(ptList, theta, dz);
                Point2D pt1 = new Point2D(xLoc, pt.X);
                Point2D pt2 = new Point2D(xLoc, pt.Y);
                Point2D pt3 = new Point2D(xLoc, pt.Z);
                crossCurveValueList.Add(pt1);
                tMomList.Add(pt2);
                zMomList.Add(pt3);


            }



            return crossCurveValueList;

        }

        #region private method



        private double getYcrosscurvevalues(Point2D pt, double m, double c)
        {
            //pt contains data in the format (z,y) and equation is of the form z = m*y + c;
            PublicHelper io = new PublicHelper();
            double yDash = io.InverseSolveLinearEquation(m, c, pt.X);
            double epi = 0.0001;
            if (Math.Abs(yDash) < pt.Y)
                return (pt.Y - yDash);
            if (Math.Abs(yDash - pt.Y) < epi && yDash < 0)
                return 2 * pt.Y;
            else if (Math.Abs(yDash - pt.Y) < epi && yDash > 0)
                return 0;
            else if (Math.Abs(yDash) >= pt.Y && yDash < 0)
                return 2 * pt.Y;
            else if (Math.Abs(yDash) >= pt.Y && yDash > 0)
                return 0;
            return 0;
        }

        private List<Point2D> getKnforFixedtheta(List<SectionCurve> secCurveList, double theta, double maxZ, double rho, StreamWriter sr, ref List<Point2D> tcbList, ref List<Point2D> vcbList)
        {
            tcbList = new List<Point2D>();
            vcbList = new List<Point2D>();
            List<Point2D> getKnfortheta = new List<Point2D>();
            double dz = -2;
            double zincrement = 0.2;
            while (dz < maxZ)
            {
                Point2D pt2d = new Point2D();
                Point2D pt = this.getKnforfixedthetaAnddisp(secCurveList, theta, dz, rho, out pt2d);
                getKnfortheta.Add(pt);
                tcbList.Add(new Point2D(pt.X, pt2d.X));
                vcbList.Add(new Point2D(pt.X, pt2d.Y));
                
                dz = dz + zincrement;
                this.writeData(theta, pt, pt2d, sr);
            }



            return getKnfortheta;
        }

        private void writeData ( double theta, Point2D pt, Point2D pt1, StreamWriter sr)
        {

            double d1 = (180 / Math.PI) * theta;
            double d2 = pt.X;
            double d3 = pt.Y;
            double d4 = pt1.X;
            double d5 = pt1.Y;
            if(d2>0)
            {
                string st = d1.ToString() + ',' + d2.ToString() + "," + d3.ToString();
                sr.WriteLine(st + ',' + d4.ToString() + ',' + d5.ToString());

            }


        }

        private Point2D getKnforfixedthetaAnddisp (List<SectionCurve> secCurveList, double theta, double dz, double rho, out Point2D cgValue)
        {
            PublicHelper helper = new PublicHelper();
            Point2D pt = new Point2D();
            cgValue = new Point2D();
            List<Point2D> tMomList = new List<Point2D>();
            List<Point2D> zMomList = new List<Point2D>();
            List<Point2D> secAreaList = this.GetSecCrossCurveValue(secCurveList, theta, dz, ref tMomList, ref zMomList);
            double vol = helper.GetSecArea(secAreaList);
            double tMom = helper.GetSecArea(tMomList);
            double zMom = helper.GetSecArea(zMomList);
            double y = 0;
            double z = 0;
            if(Math.Abs(vol - 0.0)>0.001)
            {
                y = tMom / vol;
                z = zMom / vol;
            }
            
            
            double kn = y * Math.Cos(theta) + z * Math.Sin(theta);
            double disp = vol * rho;

            pt.X = disp;
            pt.Y = kn;

            cgValue.X = y;
            cgValue.Y = z;



            return pt;
        }

        private Point getValuesforfixZ(List<Point2D> draftVsY, double theta, double dz, out double ymax)
        {
            PublicHelper helper = new PublicHelper();
            Point pt = new Point();
            double slope = Math.Tan(theta);
            double c = dz / Math.Cos(theta);
            List<Point2D> tMomList = new List<Point2D>();
            List<Point2D> zMomList = new List<Point2D>();
            List<Point2D> yList = this.getYcrosscurvelist(draftVsY, slope, c, ref tMomList, ref zMomList);
            double area = helper.GetSecArea(yList);
            double tMom = helper.GetSecArea(tMomList);
            double zMom = helper.GetSecArea(zMomList);

            pt.X = area;
            pt.Y = tMom;
            pt.Z = zMom;
            ymax = yList[yList.Count - 1].Y;


            return pt;
        }

        private Point getValuesforfixZ (List<Point2D> draftVsY, double theta, double dz)
        {
            PublicHelper helper = new PublicHelper();
            Point pt = new Point();
            double slope = Math.Tan(theta);
            double c = dz / Math.Cos(theta);
            List<Point2D> tMomList = new List<Point2D>();
            List<Point2D> zMomList = new List<Point2D>();
            List<Point2D> yList = this.getYcrosscurvelist(draftVsY, slope, c, ref tMomList, ref zMomList);
            double area = helper.GetSecArea(yList);
            double tMom = helper.GetSecArea(tMomList);
            double zMom = helper.GetSecArea(zMomList);

            pt.X = area;
            pt.Y = tMom;
            pt.Z = zMom;


            return pt;
        }
        private List<Point2D> getYcrosscurvelist(List<Point2D> draftVsY,double m, double c, ref List<Point2D> tMomList, ref List<Point2D> zMomList)
        {
            tMomList = new List<Point2D>();
            zMomList = new List<Point2D>();
            List<Point2D> yList = new List<Point2D>();
            for(int i = 0; i < draftVsY.Count; i++)
            {
                double y = getYcrosscurvevalues(draftVsY[i], m, c);
                Point2D pt = new Point2D(draftVsY[i].X, y);
                Point2D pt1 = new Point2D(draftVsY[i].X, (draftVsY[i].Y - y/2)*y);
                Point2D pt2 = new Point2D(draftVsY[i].X, draftVsY[i].X * y);
                yList.Add(pt);
                tMomList.Add(pt1);
                zMomList.Add(pt2);

            }
            return yList;
        }

        #endregion private method



    }

}
