using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipStability
{
    class Wind
    {
        #region meember
        private double _x1;
        private double _x2;
        private double _k;
        private double _s;
        private double _maxheel;
        #endregion
               
        #region Constructor

        public Wind (double X1, double X2, double k, double s, double maxheel)
        {
            this._x1 = X1;
            this._x2 = X2;
            this._k = k;
            this._s = s;
            this._maxheel = maxheel;
        }

        public Wind()
        {
            this._x1 = new double();
            this._x2 = new double();
            this._k = new double();
            this._s = new double();
            this._maxheel = new double();

        }

        #endregion

        #region Properties
        public double X1
        {
            get { return this._x1; }
            set { this._x1 = value;}
        }

        public double X2
        {
            get { return this._x2; }
            set { this._x2 = value; }
        }
        public double s
        {
            get { return this._s; }
            set { this._s = value; }
        }
        public double k
        {
            get { return this._k; }
            set { this._k = value; }
        }

        public double Maxheel
        {
            get { return this._maxheel; }
            set { this._maxheel = value; }
        }

        #endregion

        #region Public Method

        public List<Summary> GetWindCriteria (double velocity, List<FixedWeightModel> superstructureList, ShipGeom ship, List<Section> secList, List<FixedWeightModel> fixwWt, List<LiquidWeightModel> liqWt, CrossCurve curve, double rho,  Wind rollCriteria, ref List<Point2D> windGz)
        {
            OutputParamHelper outHelper = new OutputParamHelper();
            PublicHelper pHelper = new PublicHelper();
            windGz = new List<Point2D>();
            List<Summary> windSummary = new List<Summary>();
            Object obj = outHelper.IntactWtDetails(fixwWt, liqWt);
            Point initialTrim = outHelper.GetTrimValue(ship, secList, obj.Weight, obj.CG.X, rho);
            double height = ship.Depth-initialTrim.Z;
            double halfBreadth = ship.Breadth/2;
            double downHeelingAngle = this.calculateDownheelingangle(height, halfBreadth);
            double gm = outHelper.GetGmfromSmalAngle(obj.CG, obj.Weight, ship, rho);
            //List<FixedWeightModel> latAreaInfo = this.areaCalculation(secList, initialTrim, ship.LBP, ship.Depth,initialTrim.Z);
            List<FixedWeightModel> latAreaInfo = this.areaCalculation(initialTrim,ship.LBP,ship.Depth);
            Object objc = this.getsuperStructureData(superstructureList, latAreaInfo[0]);
            double heelingArm = calculateHeelingarm(objc.Weight, objc.CG.Z - latAreaInfo[1].CG.Z, obj.Weight);
            // double force = this.calculateForce(objc.Weight, velocity);
            double force = this.calculateForceIMO(objc.Weight, objc.CG.Z - latAreaInfo[1].CG.Z); 
            double heelAngle = findHeelangle(force, obj.Weight, gm);
            Point dummypt = obj.CG;
            Point windPt = new Point(dummypt.X, heelingArm, dummypt.Z);
            //double heelangle = outHelper.GetHeelangle(obj.Weight, windPt, curve);
            //------------------------------------------------------------------------//
            Dictionary<int, List<Point2D>> latAreainfoList = new Dictionary<int, List<Point2D>>();
            double minTheta = this.getMintheta(rollCriteria,initialTrim.Z, obj.CG.Z, ship.Depth);
            windGz = this.getWgz(obj, curve, minTheta-heelAngle, ship.DownFLDangle,rollCriteria.Maxheel, heelingArm, ref latAreainfoList);
            List<double> areainfList = this.areaRatio(latAreainfoList);
            Summary sum1 = new Summary("Residual Ratio from Roll to ABS 50 deg. or FLD", (areainfList[1] / areainfList[0]));
            windSummary.Add(sum1);
            Summary sum2 = new Summary("Angle Ratio of EQU to Dl", heelAngle / downHeelingAngle);
            windSummary.Add(sum2);
            Summary sum3 = new Summary("Angle at EQU", heelAngle);
            windSummary.Add(sum3);


            return windSummary;
        }



        #endregion

        #region private method
        //private double getWgm( Point cg, double disp, ShipGeom ship, double rho)
        //{
        //    OutputParamHelper outHelper = new OutputParamHelper();
        //    double gm = outHelper.GetGmfromSmalAngle(cg, disp, ship, rho);

        //    return gm;
        //}

        private double getMintheta(Wind criteria, double meandraft, double vcg, double depth)
        {
            double r = 0.73 + (vcg - meandraft) / depth;
            double rolltheta = 109 * criteria.k * criteria.X1 * Math.Sqrt(r * criteria.s);

            return rolltheta;
        }

        private List<double> areaRatio(Dictionary<int,List<Point2D>> areaInfo)
        {
            PublicHelper pHelper = new PublicHelper();
            List<double> areaList = new List<double>();
            foreach(int key in areaInfo.Keys)
            {
                List<Point2D> ptList = areaInfo[key];
                double area = pHelper.GetSecArea(ptList);
                areaList.Add(Math.Abs(area));

            }

            return areaList;
        }

        private List<Point2D>  getWgz(Object shp,  CrossCurve curve, double minAngle, double downfloodingAngle, double maxheel, double wharm, ref Dictionary<int, List<Point2D>> areaInfo)
        {
            
            double d = downfloodingAngle - maxheel;
            double dheel = 0;
            if(d<0)
            {
                dheel = downfloodingAngle;
            }
            else
            {
                dheel = maxheel;
            }
            areaInfo = new Dictionary<int, List<Point2D>>();
            OutputParamHelper outHelper = new OutputParamHelper();
            List<Point2D> gz = outHelper.GetGZcurve(shp.CG.Z, 0, curve, shp.Weight);
            List<Point2D> windGz = new List<Point2D>();
            int i = 0;
            while (Math.Abs(minAngle) > gz[i].X)
            {
                i++;
            }

            while (i > 0)
            {

                windGz.Add(new Point2D(-gz[i].X, -gz[i].Y));
                i--;
            }
            while (i < gz.Count() && gz[i].X < dheel)
            {
                windGz.Add(new Point2D(gz[i].X, gz[i].Y));
                i++;
            }

            areaInfo = this.getsplitedCurve(windGz, this.getsplitPoint(windGz, wharm));


            return windGz;
        }

        private Point2D getsplitPoint(List<Point2D> windGz, double wharm)
        {
            PublicHelper pHelper = new PublicHelper();
            double theta = pHelper.InterpolateData(1.5*wharm, windGz, 2);
            return new Point2D(theta, 1.5 * wharm);
        }

        private Dictionary<int, List<Point2D>> getsplitedCurve(List<Point2D> windGz,Point2D splitPt)
        {
            Dictionary<int, List<Point2D>> areaDictionary = new Dictionary<int, List<Point2D>>();
            List<Point2D> leftsidepts = new List<Point2D>();
            List<Point2D> rightsidepts = new List<Point2D>();
            PublicHelper pHelper = new PublicHelper();
            int k = 1;
            double eps = 0.01;
            for( int i = 0;i<windGz.Count;i++)
            {
                if(windGz[i].X < splitPt.X + eps)
                {
                    leftsidepts.Add(windGz[i]);
                    k++;
                }

            }
            leftsidepts.Add(splitPt);
            rightsidepts.Add(splitPt);
            for (int i = k;i<windGz.Count;i++)
            {
                rightsidepts.Add(windGz[i]);
            }

            areaDictionary.Add(0, leftsidepts);
            areaDictionary.Add(1, rightsidepts);

            return areaDictionary;
        }

        private double calculateHeelingarm(double area, double z, double disp)
        {
            return (585 * area * z) / (1000* 9.81*disp);
        }
        private double calculateDownheelingangle(double height,double breadth)
        {
            return (Math.Atan(height / breadth)*180)/Math.PI;
        }
        private double calculateForce(double area,double velocity)
        {
            return 0.5 * 1.225 * area * velocity*velocity;
        }

        private double calculateForceIMO(double latArea, double Z)
        {
            return (585.0 * latArea * Z)/1000;
        }
        private double findHeelangle(double force, double disp, double gm)
        {
            return (Math.Asin((force) / (9.81*disp * gm))*180)/Math.PI;
        }
        private Object  getsuperStructureData(List<FixedWeightModel> superstructureList, FixedWeightModel aboveWater)
        {
            PublicHelper pHelper = new PublicHelper();
            List<Object> supStList = new List<Object>();
            supStList.AddRange(superstructureList);
            supStList.Add(aboveWater);
            Object obj = pHelper.GetCentroid(supStList); 
           
            return obj;
        }
        
        private List<FixedWeightModel> areaCalculation(List<Section> secList, Point apFpinfo, double lbp, double depth, double meandraft)
        {
            List<FixedWeightModel> result = new List<FixedWeightModel>();
            PublicHelper phelper = new PublicHelper();
            InputParamHelper iHelper = new InputParamHelper();
            List<Point2D> waterLineup = new List<Point2D>();
            List<Point2D> waterLinedown = new List<Point2D>();
            List<Point2D> apftPtlist = this.getDividingLine(apFpinfo, lbp);
            for (int i = 0; i < secList.Count; i++)
            {
                if(secList[i].Xlocation>=apftPtlist[0].X && secList[i].Xlocation <= apftPtlist[1].X)
                {
                    waterLineup.Add(new Point2D (secList[i].Xlocation, depth - phelper.SolveLinearEquation(apftPtlist[0], apftPtlist[1], secList[i].Xlocation)));
                    waterLinedown.Add(new Point2D(secList[i].Xlocation, phelper.SolveLinearEquation(apftPtlist[0], apftPtlist[1], secList[i].Xlocation)));
                }
                else
                {
                    waterLineup.Add(new Point2D(secList[i].Xlocation, 0));
                    waterLinedown.Add(new Point2D(secList[i].Xlocation, 0));
                }
            }

            Point2D areaup = phelper.GetCgfromPoint2DVcg(waterLineup);
            Point2D areadown = phelper.GetCgfromPoint2DVcg(waterLinedown);

            FixedWeightModel aboveWaterdata = new FixedWeightModel();
            aboveWaterdata.ObjectName = "Above Water";
            aboveWaterdata.Weight = areaup.X;
            aboveWaterdata.CG = new Point(0,0,areaup.Y+meandraft);
            FixedWeightModel belowWaterdata = new FixedWeightModel();
            belowWaterdata.ObjectName = "Below Water";
            belowWaterdata.Weight = areadown.X;
            belowWaterdata.CG = new Point(0,0,areadown.Y);
            
            result.Add(aboveWaterdata);
            result.Add(belowWaterdata);
            
            return result;
        }

        private List<FixedWeightModel> areaCalculation(Point apFpinfo, double lbp, double depth)
        {
            List<FixedWeightModel> result = new List<FixedWeightModel>();
            PublicHelper phelper = new PublicHelper();
            Dictionary<int, List<Point2D>> windAreaInfoList = this.getLateralAreaInfo(apFpinfo, lbp, depth);
            foreach(int key in windAreaInfoList.Keys)
            {
                FixedWeightModel fixArea = new FixedWeightModel();
                List<Point2D> ptList = windAreaInfoList[key];
                double area = phelper.GetAreaofPolygon(ptList);
                Point2D pt = phelper.GetCentriodofPolygon(area, ptList);
                Point cG = new Point(pt.X, 0, pt.Y);
                fixArea.ObjectName = "UpperArea";
                fixArea.Weight = area;
                fixArea.CG = cG;
                result.Add(fixArea);

            }



       

            return result;
        }

        private List<Point2D> getDividingLine (Point Pt, double lbp)
        {
            List<Point2D> pt2D = new List<Point2D>();
            Point2D p1 = new Point2D(0, Pt.X);
            pt2D.Add(p1);
            Point2D p2 = new Point2D(lbp, Pt.Y);
            pt2D.Add(p2);

            return pt2D;
        }

        private Dictionary<int,List<Point2D>> getLateralAreaInfo(Point apfpInfo, double lbp, double depth)
        {
            Dictionary<int, List<Point2D>> infoList = new Dictionary<int, List<Point2D>>();
            List<Point2D> upperptList = new List<Point2D>();
            List<Point2D> belowptList = new List<Point2D>();
            Point2D p1 = new Point2D(0, apfpInfo.X);
            upperptList.Add(p1);
            belowptList.Add(p1);
            Point2D p2 = new Point2D(lbp, apfpInfo.Y);
            upperptList.Add(p2);
            belowptList.Add(p2);
            Point2D p3 = new Point2D(lbp, depth);
            upperptList.Add(p3);
            Point2D p4 = new Point2D(lbp, 0);
            belowptList.Add(p4);
            Point2D p5 = new Point2D(0, depth);
            upperptList.Add(p5);
            Point2D p6 = new Point2D(0, 0);
            belowptList.Add(p6);
            infoList.Add(0, upperptList);
            infoList.Add(1, belowptList);

            return infoList;
        }
        #endregion 
    }
}
