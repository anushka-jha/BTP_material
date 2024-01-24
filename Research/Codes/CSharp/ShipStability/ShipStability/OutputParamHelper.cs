using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ShipStability
{
    class OutputParamHelper
    {

        #region Constructor
        public OutputParamHelper()
        {
        }

        #endregion Constructor

        #region public method

        public double GetShipHeelAngle(ShipGeom sGeom, List<Section> secList, double wt, double tcg, double rho)
        {
            List<SectionCurve> modifyseCurveList = this.getModifiedSectionCurve(secList);
            List<SectionCurve> refinedCurves = this.getDensePtCurve(modifyseCurveList);
            CrossCurve cHelper = new CrossCurve();
            PublicHelper helper = new PublicHelper();
            SectionCurve sechelp = new SectionCurve();
            WaterPlane wpHelper = new WaterPlane();
            bool unsatisfied = true;
            double draft = helper.InterpolateData(wt, sGeom.DraftMass, 1);
            double meandraft = draft;
            double maxdraft = secList[0].SecCurve.SectionPts[secList[0].SecCurve.SectionPts.Count - 1].X;
            double fp = draft;
            double ap = draft;
            double tcb = 0;
            double disp = helper.InterpolateData(draft, sGeom.DraftMass, 2);
            double tcf = tcb;
            //List<SectionCurve> bcurve = sechelp.GetUpdatedSectionCurve(new Point2D(0, ap), new Point2D(sGeom.LBP, fp), secList, maxdraft);
            double eps = 0.001;
            double eps1 = 0.0025;
            double heel = 0;

            if (Math.Abs((tcg - tcb) / tcg) < eps)
            {
                return 0;
            }

            while(unsatisfied)
            {
                

                if(unsatisfied)
                {

                    heel = this.updateheelangle(tcg, tcb, 0.0001, heel);
                    List<Point2D> zMomList = new List<Point2D>();
                    List<Point2D> tMomList = new List<Point2D>();
                    List<Point2D> area = cHelper.GetSecCrossCurveValue(refinedCurves, heel, draft, ref tMomList, ref zMomList);
                    double vol = helper.GetSecArea(area);
                    double tMom = helper.GetSecArea(tMomList);
                    double zMom = helper.GetSecArea(zMomList);
                    List<Point2D> waterline = wpHelper.GetHeeledWaterLine(refinedCurves, new Point2D(ap, fp));
                    tcf = wpHelper.GetTcf(waterline);
                    tcb = tMom / vol;
                    disp = vol * rho;
                    while (Math.Abs((wt - disp) / wt) > eps1)
                    {
                        
                        double sinkage = this.updateSinkage(0.01, wt, disp);
                        //double sinkage = this.updateSinkage(lcg - lcb, wt, disp);
                        ap = ap + sinkage;
                        fp = fp + sinkage;
                        //double theta = Math.Atan((fp - ap) / sGeom.Breadth);

                        draft = (ap + fp) / 2;
                        //List<Point2D> zMomList = new List<Point2D>();
                        //List<Point2D> tMomList = new List<Point2D>();
                        List<Point2D> yList = cHelper.GetSecCrossCurveValue(refinedCurves, heel, draft * Math.Cos(heel), ref tMomList, ref zMomList);

                        vol = helper.GetSecArea(yList);
                        tMom = helper.GetSecArea(tMomList);

                        //double zMom = helper.GetSecArea(zMomList);
                        tcb = tMom / vol;
                        disp = vol * rho;



                    }

                    if (Math.Abs((tcb - tcg) / tcb) < 0.003)
                    {
                        unsatisfied = false;

                    }


                }

                
            }

            return heel*(180/Math.PI);
        }
   
        public double SracpGetHeelAngleFromIteration ( ShipGeom sGeom, List<Section> secList, double wt, double tcg, double rho)

        {
            List<SectionCurve> modifyseCurveList = this.getModifiedSectionCurve(secList);
            List<SectionCurve> refinedCurves = this.getDensePtCurve(modifyseCurveList);
            CrossCurve cHelper = new CrossCurve();
            PublicHelper helper = new PublicHelper();
            SectionCurve sechelp = new SectionCurve();
            WaterPlane wpHelper = new WaterPlane();
            bool unsatisfied = true;
            double draft = helper.InterpolateData(wt, sGeom.DraftMass, 1);
            double meandraft = draft;
            double maxdraft = secList[0].SecCurve.SectionPts[secList[0].SecCurve.SectionPts.Count - 1].X;
            double fp = draft;
            double ap = draft;
            double tcb = 0;
            double disp = helper.InterpolateData(draft, sGeom.DraftMass, 2);
            double tcf = tcb;
            //List<SectionCurve> bcurve = sechelp.GetUpdatedSectionCurve(new Point2D(0, ap), new Point2D(sGeom.LBP, fp), secList, maxdraft);
            double eps = 0.001;
            double eps1 = 0.0010;
            double heel = 0;
            
            if (Math.Abs((tcg - tcb)/tcg) < eps)
            {
                return 0;
            }
            while (unsatisfied)
            {
                if (((Math.Abs((tcg - tcb))) < eps) && (Math.Abs((wt - disp) / wt) < eps1))
                {
                    unsatisfied = false;
                }
                while (Math.Abs((wt - disp) / wt) > eps1)
                {
                    heel = 0;
                    double sinkage = this.updateSinkage(0.001, wt, disp);
                    //double sinkage = this.updateSinkage(lcg - lcb, wt, disp);
                    ap = ap + sinkage;
                    fp = fp + sinkage;
                    double theta = Math.Atan((fp - ap) / sGeom.Breadth);

                    draft = (ap + fp) / 2;
                    List<Point2D> zMomList = new List<Point2D>();
                    List<Point2D> tMomList = new List<Point2D>();
                    List<Point2D> yList = cHelper.GetSecCrossCurveValue(refinedCurves, theta, draft*Math.Cos(theta), ref tMomList, ref zMomList);

                    double vol = helper.GetSecArea(yList);
                    double tMom = helper.GetSecArea(tMomList);

                    //double zMom = helper.GetSecArea(zMomList);
                    tcb = tMom / vol;
                    disp = vol * rho;

                }
                while (((Math.Abs((tcg - tcb))) > eps))
                {
                    heel = this.updateheelangle(tcg, tcb, 0.0001, heel);
                    double dz = (heel * tcf) / sGeom.Breadth;
                    ap = ap - dz;
                    fp = fp + (heel - dz);
                    draft = (ap + fp) / 2;
                    double theta = Math.Atan((fp - ap) / sGeom.Breadth);
                    List<Point2D> zMomList = new List<Point2D>();
                    List<Point2D> tMomList = new List<Point2D>();
                    List<Point2D> area = cHelper.GetSecCrossCurveValue(refinedCurves, theta, draft, ref tMomList, ref zMomList);
                    double vol = helper.GetSecArea(area);
                    double tMom = helper.GetSecArea(tMomList);
                    double zMom = helper.GetSecArea(zMomList);
                    List<Point2D> waterline = wpHelper.GetHeeledWaterLine(refinedCurves, new Point2D(ap, fp));
                    tcf = wpHelper.GetTcf(waterline);
                    tcb = tMom / vol;
                    disp = vol * rho;

                }
            } 


             double hangle = (180 / Math.PI) * Math.Atan((fp - ap) / (sGeom.Breadth));
            return hangle;
        }

        public double GetHeelangle(double wt, Point cg, CrossCurve curveData)
        {
            PublicHelper helper = new PublicHelper();
            CrossCurve cHelper = new CrossCurve();

            List<Point2D> gzCurve = this.GetGZcurve(cg.Z, 0, curveData, wt);
            List<Point2D> tcbCurve = this.getDataFromCrossCurve(curveData.DispVTcb, wt);
            double factor = 0;
            if(Math.Abs(cg.Y)>0.0000001)
            {
                 factor = cg.Y / Math.Abs(cg.Y);
            }
            
            
            double heelAngle = helper.InterpolateData(Math.Abs(cg.Y), gzCurve, 1);
            double heelAngle1 = helper.InterpolateData(Math.Abs(cg.Y), tcbCurve, 1);

            heelAngle = factor * heelAngle;


           


            return heelAngle;
        }

 
        public List<Point2D> GetWtDistribution(List<Point2D> steelWt, List<Point2D> solidWt, List<Tank> tankData, List<Mapping> mp)
        {
            List<Point2D> LoadCurve = new List<Point2D>();
            int k = steelWt.Count;
            double distance = steelWt[1].X - steelWt[0].X;
            for (int i = 0; i < k; i++)
            {
                double x = steelWt[i].X;
                double b1 = steelWt[i].Y;
                double b2 = getLiquidWt(x, tankData, mp);
                double b3 = getSolidWt(x, solidWt, distance);
                LoadCurve.Add(new Point2D(x, b1 + b2 + b3));
            }

            return LoadCurve;

        }

        public Object IntactWtDetails(List<FixedWeightModel> fixedWt, List<LiquidWeightModel> liquidWt)
        {

            PublicHelper helper = new PublicHelper();
            List<Object> objList = new List<Object>();
            objList.AddRange(fixedWt);
            objList.AddRange(liquidWt);

            Object obj = helper.GetCentroid(objList);

            return obj;
        }

        public double GetGm (Point pt, CrossCurve curveData, double disp)
        {
            PublicHelper helper = new PublicHelper();
            List<Point2D> gzCurve = this.GetGZcurve(pt.Z, 0, curveData, disp);
            double gm = helper.SolveLinearEquation(gzCurve[0], gzCurve[1], 180 / Math.PI);

            return gm;
        }

        public double GetGmfromSmalAngle(Point cg, double disp, ShipGeom shp, double rho)
        {
            PublicHelper helper = new PublicHelper();
            double bm = this.getBmt(shp, rho);
            double draft = helper.InterpolateData(disp, shp.DraftMass, 1);
            double vcb = helper.InterpolateData(draft, shp.DraftVcb, 2);

            double GM = vcb + bm - cg.Z;
            
            //double kb = helper.InterpolateData(disp, curveData.KnVvcb, 2);

                return GM;
        }

        public List<Point2D> GetGZcurve(double kg, double tcg, CrossCurve curveData, double disp)
        {
            StreamWriter sr = new StreamWriter("D://Ranadev//Research//Codes//Input//output//GZcurve.dat");
            List<Point2D> gzcurvePt = new List<Point2D>();
            List<Point2D> knCurvePt = this.getKNcurve(curveData, disp);
            double factor = (Math.PI / 180);

            for (int i = 0; i < knCurvePt.Count; i++)
            {
                double d = knCurvePt[i].Y - kg * Math.Sin(factor * knCurvePt[i].X) - tcg * Math.Cos(factor * knCurvePt[i].X);
                Point2D pt = new Point2D(knCurvePt[i].X, d);
                gzcurvePt.Add(pt);
                string s = pt.X.ToString() + "," + pt.Y.ToString();
                sr.WriteLine(s);
            }


            sr.Close();
            return gzcurvePt;

        }

        /// <summary>
        /// It returns the zero trim cross curve
        /// all the angular parameter should be given in degree
        /// maximum draft should be in metre
        /// </summary>
        /// <param name="secList"></param>
        /// <param name="thetaMin"></param>
        /// <param name="thetaMax"></param>
        /// <param name="thetaincrement"></param>
        /// <param name="maxDraft"></param>
        /// <param name="rho"></param>
        /// <returns></returns>

        public CrossCurve GetCrossCurveData(List<Section> secList, double thetaMin, double thetaMax, double thetaincrement, double maxDraft, double rho)
        {
            CrossCurve cHelper = new CrossCurve();
            PublicHelper helper = new PublicHelper();
            List<SectionCurve> modifyseCurveList = this.getModifiedSectionCurve(secList);
            List<SectionCurve> refinedCurves = this.getDensePtCurve(modifyseCurveList);
            CrossCurve curve = cHelper.GetKnCurve(refinedCurves, thetaMin, thetaMax, thetaincrement, maxDraft, rho);

            return curve;
        }
    
       
        public Point GetTrimValue(ShipGeom sGeom, List<Section> secList, double wt, double lcg, double rho)
        {
            Point pt = new Point();
            PublicHelper helper = new PublicHelper();
            SectionCurve sechelp = new SectionCurve();
            WaterPlane wphelper = new WaterPlane();
            bool unsatisfied = true;
            double draft = helper.InterpolateData(wt, sGeom.DraftMass, 1);
            double meandraft = draft;
            double maxdraft = secList[0].SecCurve.SectionPts[secList[0].SecCurve.SectionPts.Count - 1].X;
            double fp = draft;
            double ap = draft;
            double lcb = helper.InterpolateData(draft, sGeom.DraftLcb, 2);
            double disp = helper.InterpolateData(draft, sGeom.DraftMass,2);
            double lcf = helper.InterpolateData(draft, sGeom.DraftLcf, 2);
            List<SectionCurve> bcurve = sechelp.GetUpdatedSectionCurve(new Point2D(0, ap), new Point2D(sGeom.LBP, fp),secList,maxdraft);
            List<Point2D> wcurve = wphelper.GetTrimedWaterLine(secList, new Point2D(0, ap), new Point2D(sGeom.LBP, fp), maxdraft);

            double eps = 0;
            double eps1 = 0;
            if ((Math.Abs((lcg - lcb))) < 7)
            {
                 eps = 0.00050;
                 eps1 = 0.001;

            }
            else
            {

                eps = 0.005;
                eps1 = 0.001;

            }

            double trim = 0.0;
            int sig = this.getSignature(lcg,lcb);
            if ((Math.Abs((lcg - lcb) / lcg)) < eps)
            {
                pt.X = meandraft;
                pt.Y = meandraft;
                pt.Z = 0;
                sGeom.UpdateBuoyancyCurve(bcurve, rho);
                sGeom.UpdateWaterline(wcurve);
                return pt;
            }
            while (unsatisfied)
            {
                if ((Math.Abs((lcg - lcb) / lcg)) < eps)
                {
                    if (Math.Abs((wt - disp) / wt) < eps1)
                    {
                        unsatisfied = false;
                        sGeom.UpdateBuoyancyCurve(bcurve, rho);
                        sGeom.UpdateWaterline(wcurve);

                    }
                    else
                    {
                        double sinkage = this.updateSinkage(0.001, wt, disp);
                        //double sinkage = this.updateSinkage(lcg - lcb, wt, disp);
                        ap = ap + sinkage;
                        fp = fp + sinkage;

                        Point2D leftBotPt = new Point2D(0.0, ap);
                        Point2D rtTopPt = new Point2D(sGeom.LBP, fp);
                        List<SectionCurve> curveList = sechelp.GetUpdatedSectionCurve(leftBotPt, rtTopPt, secList, maxdraft);
                        double updateVol = helper.GetShipVol(curveList);
                        double xmoment = helper.Getmoment(curveList, 0);
                        lcb = xmoment / updateVol;
                        disp = updateVol * rho;
                    }

                }
                else
                {
                    sig = this.getSignature(lcg, lcb);
                    double modifiedtrim = this.getmodifiedtrim(lcg, lcb,sig);
                    trim = this.updateTrim(trim, modifiedtrim, lcg, lcb);
                    double dz = (trim * lcf) / sGeom.LBP;
                    ap = ap - dz;
                    fp = fp + (trim - dz);

                    Point2D leftBotPt = new Point2D(0.0, ap);
                    Point2D rtTopPt = new Point2D(sGeom.LBP, fp);
                    List<SectionCurve> curveList = sechelp.GetUpdatedSectionCurve(leftBotPt, rtTopPt, secList, maxdraft);
                    bcurve.RemoveRange(0, bcurve.Count);
                    bcurve.AddRange(curveList);
                    double updateVol = helper.GetShipVol(curveList);
                    double xmoment = helper.Getmoment(curveList, 0);
                    lcb = xmoment / updateVol;
                    List<Point2D> poinList = wphelper.GetTrimedWaterLine(secList, leftBotPt, rtTopPt, maxdraft);
                    wcurve.RemoveRange(0, wcurve.Count);
                    wcurve.AddRange(poinList);
                    double wparea = helper.GetSecArea(poinList);
                    double wpaxMom = wphelper.GetWpAreaMoment(poinList, 0);
                    lcf = wpaxMom / wparea;
                    disp = updateVol * rho;
                }
            }


            pt.X = ap;
            pt.Y = fp;
            pt.Z = (0.5)*(ap+fp);

            return pt;
        }

        public Dictionary<double,List<double>> GetHydrostatData(ShipGeom sGeom,List<Section> secList, double rho)
        {
            Dictionary<double, List<double>> hydrodata = new Dictionary<double, List<double>>();
            SectionCurve sechelp = new SectionCurve();
            PublicHelper helper = new PublicHelper();
            WaterPlane wphelper = new WaterPlane();
            double maxdraft = secList[0].SecCurve.SectionPts[secList[0].SecCurve.SectionPts.Count - 1].X;
            double z = 0.1;

            while (z<sGeom.Depth)
            {
                List<double> dlist = new List<double>();
                Point2D leftBotPt = new Point2D(0.0, z);
                Point2D rtTopPt = new Point2D(sGeom.LBP, z);
                List<SectionCurve> curveList = sechelp.GetUpdatedSectionCurve(leftBotPt, rtTopPt, secList, maxdraft);
                double updateVol = helper.GetShipVol(curveList);
                double xmoment = helper.Getmoment(curveList, 0);
                double lcb = xmoment / updateVol;
                List<Point2D> poinList = wphelper.GetTrimedWaterLine(secList, leftBotPt, rtTopPt, maxdraft);
                double wparea = helper.GetSecArea(poinList);
                double wpaxMom = wphelper.GetWpAreaMoment(poinList, 0);
                double lcf = wpaxMom / wparea;
                double vol = updateVol * rho;
                dlist.Add(vol);
                dlist.Add(vol * rho);
                dlist.Add(lcb);
                dlist.Add(wparea);
                dlist.Add(lcf);

                hydrodata.Add(z, dlist);
                z = z + 0.1;

            }
            
            


            return hydrodata;
        }

        public Point2D GetTrim(ShipGeom sGeom, List<Section> secList, double wt, double lcg, double rho)
        {
            Point2D pt = new Point2D();
            PublicHelper helper = new PublicHelper();
            SectionCurve sechelp = new SectionCurve();
            bool unsatisfied = true;
            double draft = helper.InterpolateData(wt, sGeom.DraftMass,1);
            double meandraft = draft;
            double fp = meandraft;
            double ap = meandraft;
            double maxdraft = secList[0].SecCurve.SectionPts[secList[0].SecCurve.SectionPts.Count - 1].X;
            double lcb = helper.InterpolateData(draft, sGeom.DraftLcb, 2);
            double disp = wt;
            double eps = 0.01;
            double eps1 = 0.0025;
            int param = 0;
            if ((Math.Abs((lcg - lcb) / lcg))< eps)
            {
                pt.X = meandraft;
                pt.Y = meandraft;
                return pt;
            }

            while (unsatisfied)
            {
                double moment = this.getMoment(lcb, lcg, wt);
                if((Math.Abs((lcg-lcb)/lcg))<eps)
                {
                    if((Math.Abs((wt-disp)/wt)<eps1)&&(param>0))
                    {
                        unsatisfied = false;

                    }
                    else
                    {
                       //double draft1 = helper.InterpolateData(disp, sGeom.DraftMass, 1);
                        double dz = meandraft - draft;
                        meandraft = draft;
                        double tpc = helper.InterpolateData(draft, sGeom.DraftTpc, 2);
                        double psink = (wt - disp) / (tpc*100); //do parallel sinkage
                        ap = ap + psink;
                        fp = fp + psink;
                        //disp = wt; //get update disp
                        //Point2D leftBotPt = new Point2D(secList[0].Xlocation, ap);
                        //Point2D rtTopPt = new Point2D(secList[secList.Count - 1].Xlocation, fp);
                        Point2D leftBotPt = new Point2D(0.0, ap);
                        Point2D rtTopPt = new Point2D(sGeom.LBP, fp);
                        //get update section line
                        List<SectionCurve> curveList = sechelp.GetUpdatedSectionCurve(leftBotPt, rtTopPt, secList, maxdraft);
                        //---------------------------------------------------
                        //get update volume
                        double updateVol = helper.GetShipVol(curveList);
                        double xmoment = helper.Getmoment(curveList, secList[0].Xlocation);
                        //double xmoment = helper.Getmoment(curveList, 0);
                        //get updated lcb
                        lcb = xmoment / updateVol;
                        //get update disp
                         disp = updateVol * rho;
                       //get update draft
                       draft = helper.InterpolateData(disp, sGeom.DraftMass, 1);
                        //draft = (ap + fp) / 2;
                        param++;


                    }
                }
                else
                {
                    double mct = helper.InterpolateData(draft, sGeom.DraftMct, 2);
                    double lcf = helper.InterpolateData(draft, sGeom.DraftLcf, 2);
                    double trim = (moment / mct)/100.0;
                    //trim = -3.71;
                    double dz = (trim * lcf) / sGeom.LBP;
                    ap = ap - dz;
                    fp = fp + (trim - dz);



                    //Point2D leftBotPt = new Point2D(secList[0].Xlocation, ap);
                    //Point2D rtTopPt = new Point2D(secList[secList.Count - 1].Xlocation, fp);
                    Point2D leftBotPt = new Point2D(0.0, ap);
                    Point2D rtTopPt = new Point2D(sGeom.LBP, fp);
                    //get update section line
                    List<SectionCurve> curveList = sechelp.GetUpdatedSectionCurve(leftBotPt, rtTopPt, secList, maxdraft);
                    //---------------------------------------------------
                    //get update volume
                    double updateVol = helper.GetShipVol(curveList);
                    double xmoment = helper.Getmoment(curveList, 0);
                    double xmoment1 = helper.Getmoment(secList, leftBotPt, rtTopPt, maxdraft);
                    ////get updated lcb
                    disp = updateVol * rho;
                   lcb =  xmoment / updateVol;
                   lcb = lcg;


                }


            }
            pt.X = ap;
            pt.Y = fp;
            return pt;
        }
        /// <summary>
        /// In this code, the interpolated data will be returned for font end 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="dens"></param>
        /// <param name="mp"></param>
        /// In Case of Density, input is tank t, updated density dens
        /// for mapping it is mp = (3,grid volume data )
        /// <returns></returns>
        public List<Mapping> GetTankData(Tank t, double dens, Mapping mp)
        {
            PublicHelper io = new PublicHelper();
            List<Mapping> tankReturnData = new List<Mapping>();
            switch (mp.IndexValue)
            {
                case 1:            // when density will change
                    double maxVol = t.MaxWeight / t.LiquidRho;
                    t.UpdateDensity(dens);
                    double draft1 = io.InterpolateData(mp.Xvalues, t.VolumeData, 1);


                    //Mapping pt2 = new Mapping(0, t.MaxWeight);  // Max weight
                    tankReturnData.Add(new Mapping(0, maxVol*dens));
                    t.UpdateMaxWt(maxVol * dens);

                    tankReturnData.Add(new Mapping(1, dens));
                    //weight
                    double xValues = (mp.Xvalues * dens);
                    tankReturnData.Add(new Mapping(2, xValues));
                    //Volume
                    xValues = (mp.Xvalues);
                    tankReturnData.Add(new Mapping(3, xValues));
                    // %vol    
                    xValues = (mp.Xvalues * dens / t.MaxWeight) * 100;
                    tankReturnData.Add(new Mapping(4, xValues));
                    //LCG

                    // sounding
                    tankReturnData.Add(new Mapping(6, draft1));

                    //LCG
                    xValues = io.InterpolateData(draft1, t.TankLcg, 2);
                    tankReturnData.Add(new Mapping(5, xValues));
                    // Vcg
                    xValues = io.InterpolateData(draft1, t.TankVcg, 2);
                    tankReturnData.Add(new Mapping(7, xValues));
                    // Tcg
                    xValues = io.InterpolateData(draft1, t.TankTcg, 2);
                    tankReturnData.Add(new Mapping(8, xValues));
                    // Actual Fsml
                    xValues = io.InterpolateData(draft1, t.Fsml, 2);
                    tankReturnData.Add(new Mapping(9, xValues));
                    // Actual Fsmt
                    xValues = io.InterpolateData(draft1, t.Fsmt, 2);
                    tankReturnData.Add(new Mapping(10, xValues));

                    break;



                case 2:                                          // When Weight is given
                    

                    double draft = io.InterpolateData(mp.Xvalues, t.MassData, 1);
                    


                    //Mapping pt2 = new Mapping(0, t.MaxWeight);  // Max weight
                    tankReturnData.Add(new Mapping(0, t.MaxWeight));

                    tankReturnData.Add(new Mapping(1, dens));
                    //weight
                    double xvalues = (mp.Xvalues);
                    tankReturnData.Add(new Mapping(2, xvalues));
                    //Volume
                    xvalues = (mp.Xvalues)/dens;
                    tankReturnData.Add(new Mapping(3, xvalues));
                    // %vol    
                    xvalues = (mp.Xvalues/t.MaxWeight)*100;
                    tankReturnData.Add(new Mapping(4, xvalues));

                    // soundings
                    tankReturnData.Add(new Mapping(6, draft));
                    //LCG
                    xvalues = io.InterpolateData(draft, t.TankLcg, 2);
                    tankReturnData.Add(new Mapping(5, xvalues));
                    // Vcg
                    xvalues = io.InterpolateData(draft, t.TankVcg, 2);
                    tankReturnData.Add(new Mapping(7, xvalues));
                    // Tcg
                    xvalues = io.InterpolateData(draft, t.TankTcg, 2);
                    tankReturnData.Add(new Mapping(8, xvalues));
                    // Actual Fsml
                    xvalues = io.InterpolateData(draft, t.Fsml, 2);
                    tankReturnData.Add(new Mapping(9, xvalues));
                    // Actual Fsmt
                    xvalues = io.InterpolateData(draft, t.Fsmt, 2);
                    tankReturnData.Add(new Mapping(10, xvalues));

                    break;

                case 3:                                                 //When vol is given
                    //PublicHelper io2 = new PublicHelper();

                     draft1 = io.InterpolateData(mp.Xvalues, t.VolumeData, 1);
                                  

                    //Mapping pt2 = new Mapping(0, t.MaxWeight);  // Max weight
                    tankReturnData.Add(new Mapping(0, t.MaxWeight));

                    tankReturnData.Add(new Mapping(1, dens));
                    //weight
                    xvalues = (mp.Xvalues * dens);
                    tankReturnData.Add(new Mapping(2, xvalues));
                    //Volume
                    xvalues = (mp.Xvalues);
                    tankReturnData.Add(new Mapping(3, xvalues));
                    // %vol    
                    xvalues = (mp.Xvalues*dens/t.MaxWeight)*100;
                    tankReturnData.Add(new Mapping(4, xvalues));
                    // sounding
                    tankReturnData.Add(new Mapping(6, draft1));
                    //LCG
                    xvalues = io.InterpolateData(draft1, t.TankLcg, 2);
                    tankReturnData.Add(new Mapping(5, xvalues));
                    // Vcg
                    xvalues = io.InterpolateData(draft1, t.TankVcg, 2);
                    tankReturnData.Add(new Mapping(7, xvalues));
                    // Tcg
                    xvalues = io.InterpolateData(draft1, t.TankTcg, 2);
                    tankReturnData.Add(new Mapping(8, xvalues));
                    // Actual Fsml
                    xvalues = io.InterpolateData(draft1, t.Fsml, 2);
                    tankReturnData.Add(new Mapping(9, xvalues));
                    // Actual Fsmt
                    xvalues = io.InterpolateData(draft1, t.Fsmt, 2);
                    tankReturnData.Add(new Mapping(10, xvalues));

                    break;
                 case 4:              //% of filling

                    PublicHelper io2 = new PublicHelper();

                    double draft2 = io2.InterpolateData(mp.Xvalues, t.PerOfFilling, 1);

                    //Mapping pt2 = new Mapping(0, t.MaxWeight);  // Max weight
                    tankReturnData.Add(new Mapping (0,t.MaxWeight));
                  
                    tankReturnData.Add(new Mapping (1,dens));
                      //weight
                     xvalues = (mp.Xvalues * t.MaxWeight)/100;
                    tankReturnData.Add(new Mapping (2,xvalues));
                     //Volume
                    xvalues = (mp.Xvalues * t.MaxWeight)/100 / dens;
                    tankReturnData.Add(new Mapping(3,xvalues));
                     // %vol    
                    xvalues = mp.Xvalues;
                    tankReturnData.Add(new Mapping(4,xvalues));
                    
                    // sounding 
                    tankReturnData.Add(new Mapping(6, draft2));

                    //LCG
                    xvalues = io2.InterpolateData(draft2, t.TankLcg, 2);
                    tankReturnData.Add(new Mapping(5,xvalues));
                         // Vcg
                    xvalues = io2.InterpolateData(draft2, t.TankVcg, 2);
                    tankReturnData.Add(new Mapping(7,xvalues));
                     // Tcg
                    xvalues = io2.InterpolateData(draft2, t.TankTcg, 2);
                    tankReturnData.Add(new Mapping (8,xvalues));
                     // Actual Fsml
                    xvalues = io2.InterpolateData(draft2, t.Fsml, 2);
                    tankReturnData.Add(new Mapping (9,xvalues));
                        // Actual Fsmt
                    xvalues = io2.InterpolateData(draft2, t.Fsmt, 2);
                    tankReturnData.Add(new Mapping(10,xvalues));
                    break;
                case 5:  //soundings

                                        //Mapping pt2 = new Mapping(0, t.MaxWeight);  // Max weight
                    tankReturnData.Add(new Mapping(0, t.MaxWeight));

                    tankReturnData.Add(new Mapping(1, dens));
                    //weight
                    xvalues = io.InterpolateData(mp.Xvalues, t.MassData,2);
                    tankReturnData.Add(new Mapping(2, xvalues));
                    //Volume
                    //xvalues = xvalues / dens;
                    tankReturnData.Add(new Mapping(3, xvalues/dens));
                    // %vol    
                    xvalues = (xvalues / t.MaxWeight) * 100;
                    tankReturnData.Add(new Mapping(4, xvalues));

                    // soundings
                    tankReturnData.Add(new Mapping(6, mp.Xvalues));
                    //LCG
                    xvalues = io.InterpolateData(mp.Xvalues, t.TankLcg, 2);
                    tankReturnData.Add(new Mapping(5, xvalues));
                    // Vcg
                    xvalues = io.InterpolateData(mp.Xvalues, t.TankVcg, 2);
                    tankReturnData.Add(new Mapping(7, xvalues));
                    // Tcg
                    xvalues = io.InterpolateData(mp.Xvalues, t.TankTcg, 2);
                    tankReturnData.Add(new Mapping(8, xvalues));
                    // Actual Fsml
                    xvalues = io.InterpolateData(mp.Xvalues, t.Fsml, 2);
                    tankReturnData.Add(new Mapping(9, xvalues));
                    // Actual Fsmt
                    xvalues = io.InterpolateData(mp.Xvalues, t.Fsmt, 2);
                    tankReturnData.Add(new Mapping(10, xvalues));

                    break;



                   
                                

            }
            return tankReturnData;
        }

        public List<Point2D> GetLoadCurve(List<Point2D> weight ,List<Point2D> Buoyancy)      
        {
            PublicHelper helper = new PublicHelper();
            List<Point2D> load = new List<Point2D>();
            double factor =  helper.GetSecAreaTrapz(weight)/ helper.GetSecAreaTrapz(Buoyancy);



            for(int i = 0; i < Buoyancy.Count; i++)
            {
                Point2D pt = new Point2D(weight[i].X, weight[i].Y- factor * Buoyancy[i].Y);
                load.Add(pt);
            }

            return load;
        }
        
        public List<Point2D> GetShearForce(List<Point2D> Load)
        {
            List<Point2D> shearForce = new List<Point2D>();
            //Point2D pt = new Point2D(Load[0].X,0);
            shearForce.Add(new Point2D(Load[0].X, 0));
            for(int i = 1; i < Load.Count; i++)
            {
                Point2D pt = new Point2D();
                pt.X = Load[i].X;
                pt.Y = shearForce[i - 1].Y + ((Load[i].X-Load[i-1].X)*(Load[i-1].Y+Load[i].Y)*0.5);
                shearForce.Add(pt);
            }
            
            return shearForce;
        }
        public List<Point2D> GetBendingMoment(List<Point2D> shearForce)
        {
            List<Point2D> bendingMoment = new List<Point2D>();
            Point2D pt1 = new Point2D(shearForce[0].X, 0);
            bendingMoment.Add(new Point2D (shearForce[0].X, 0)); ;
            for (int i = 1; i < shearForce.Count; i++)
            {
                Point2D pt = new Point2D();
                pt.X = shearForce[i].X;
                pt.Y = bendingMoment[i - 1].Y + ((shearForce[i].X - shearForce[i - 1].X) * (shearForce[i - 1].Y + shearForce[i].Y) * 0.5);
                if(pt.Y<0)
                {
                    pt.Y = 0;
                }
                bendingMoment.Add(pt);
            }
            return bendingMoment;
        }

        //public List<Summary> GetLoadingSummary ( Dictionary<double, List<Mapping>>)
        
        public List<Summary> GetSummary(List<LiquidWeightModel> LiquidWeights, List<FixedWeightModel> FixedWeights)
        {
            List<Summary> LoadSummary = new List<Summary>();
            for(int i = 0; i < FixedWeights.Count; i++)
            {
                LoadSummary.Add(new Summary(FixedWeights[i].ObjectName, FixedWeights[i].Weight));
            }
            for (int i = 0; i < LiquidWeights.Count; i++)
            {
                LoadSummary.Add(new Summary(LiquidWeights[i].Tankname, LiquidWeights[i].Weight));
            }
            LoadSummary.Add(new Summary("Deadweight", GetDeadWeight(LiquidWeights, FixedWeights)));
            LoadSummary.Add(new Summary("Deadweight LCG", GetDeadWeightLCG(LiquidWeights, FixedWeights)));
            LoadSummary.Add(new Summary("Deadweight VCG", GetDeadWeightVCG(LiquidWeights, FixedWeights)));
            LoadSummary.Add(new Summary("Deadweight TCG", GetDeadWeightTCG(LiquidWeights, FixedWeights)));
            LoadSummary.Add(new Summary("Diplacement", GetDisplacement(LiquidWeights, FixedWeights)));
            LoadSummary.Add(new Summary("Diplacement LCG", GetDisplacementLCG(LiquidWeights, FixedWeights)));
            LoadSummary.Add(new Summary("Diplacement VCG", GetDisplacementVCG(LiquidWeights, FixedWeights)));
            LoadSummary.Add(new Summary("Diplacement TCG", GetDisplacementTCG(LiquidWeights, FixedWeights)));
            return LoadSummary;
        }

        public double GetAreafromZeroTotheta (List<Point2D> ptList, double maxtheta)
        {
            double sum30 = 0;
            for (int i = 0; ptList[i].X <= maxtheta; i++)
            {
                sum30 += (0.5) * (ptList[i + 1].X - ptList[i].X) * (ptList[i + 1].Y + ptList[i].Y);
            }
            return sum30;
        }

        public double GetmaxGztilltheta(List<Point2D> ptList, double theta)
        {
            PublicHelper  phelp = new PublicHelper();
            List<double> updatedPtList = new List<double>();
            double eps = 0.001;
            foreach(Point2D pt in ptList)
            {
                if(pt.X<theta + eps)
                {
                    updatedPtList.Add(pt.Y);
                }
            }

            Point2D pt2D = phelp.GetMaxMinElementInArray(updatedPtList);

            return pt2D.Y;
        }

        public List<Summary> GetIntactCriteria(Object shipDisp, CrossCurve curve, ShipGeom ship, double rho, Point critPt, Point fldPt)
        {
            List<Point2D> GZ = this.GetGZcurve(shipDisp.CG.Z, shipDisp.CG.Y, curve, shipDisp.Weight);
            double heel = this.GetHeelangle(shipDisp.Weight, shipDisp.CG, curve);
            List<Summary> IntactCriteria = new List<Summary>();
            double factor = Math.PI / 180;
            double sum30 = (Math.PI/180.0)*this.GetAreafromZeroTotheta(GZ,30);
            double sum40 = (Math.PI / 180.0)*this.GetAreafromZeroTotheta(GZ, 40);
        
            double maxAngle= GZ[0].X, maxGz=GZ[0].Y;
            for(int i = 1; i < GZ.Count; i++)
            {
                if (GZ[i].Y > maxGz)
                {
                    maxGz = GZ[i].Y;
                    maxAngle = GZ[i].X;
                }
            
            }
            PublicHelper helper = new PublicHelper();
            double gm = this.GetGmfromSmalAngle(shipDisp.CG, shipDisp.Weight, ship, rho);

            IntactCriteria.Add(new Summary("Area from 0 deg. to 30 deg.", sum30));
            IntactCriteria.Add(new Summary("Area from 0 deg. to 40 deg. or FLD", sum40));
            IntactCriteria.Add(new Summary("Area from 30 deg. to 40 deg. or FLD", sum40-sum30));
            IntactCriteria.Add(new Summary("Angle at Max", maxAngle));
            IntactCriteria.Add(new Summary("GZ at Max", maxGz));
            IntactCriteria.Add(new Summary("GM", gm));
            IntactCriteria.Add(new Summary("Critical Point Height at EQU", critPt.Z - Math.Abs(critPt.Y) * Math.Cos(factor * Math.Abs(heel))));
            IntactCriteria.Add(new Summary("Fld Point Height at EQU", fldPt.Z - Math.Abs(fldPt.Y) * Math.Cos(factor * Math.Abs(heel))));
            return IntactCriteria;
        }
        
         public List<Summary> GetLongitudinalCalculation(List<Point2D> shearForce,List<Point2D> bendingMoment)
        {
            List<Summary> LongitudinalCalculation = new List<Summary>();
            
            //for(int i = 0; i < lines.Length; i++)
            //{
            //    PublicHelper helper = new PublicHelper();
            //    int fr = int.Parse(lines[i]);
            //    double dist = double.Parse(lines[i]) * 0.7;
            //    LongitudinalCalculation.Add(new LongitudinalCalculation(fr,dist ,helper.InterpolateData(dist,shearForce,2) , 0.0, helper.InterpolateData(dist, bendingMoment, 2), 0.0));
            //}

            return LongitudinalCalculation;
        }

        public List<Summary> GetIntactSummary()
        {
            List<Summary> IntactSummary = new List<Summary>();


            IntactSummary.Add(new Summary("Trim", 0.0));
            IntactSummary.Add(new Summary("Heel", 0.0));
            IntactSummary.Add(new Summary("Diaplacement", 0.0));
            IntactSummary.Add(new Summary("LCG", 0.0));
            IntactSummary.Add(new Summary("TCG", 0.0));
            IntactSummary.Add(new Summary("VCG", 0.0));
            IntactSummary.Add(new Summary("Draft Aft", 0.0));
            IntactSummary.Add(new Summary("Draft Fwd", 0.0));
            IntactSummary.Add(new Summary("GM (Fluid)", 0.0));
            IntactSummary.Add(new Summary("GM (Solid)", 0.0));
            IntactSummary.Add(new Summary("FS Correction", 0.0));
            IntactSummary.Add(new Summary("VCG Correction", 0.0));

            return IntactSummary;
        }
        public List<Summary> GetLongitudinalStrengthSummary(List<Point2D> shearForce,List<Point2D> bendingMoment)
        {
            List<Summary> LongitudinalSummary = new List<Summary>();
            double minShear = shearForce[0].Y, maxShear = shearForce[0].Y, minBenMoment = bendingMoment[10].Y, maxBenMoment = bendingMoment[0].Y;
            double minShearLoc = shearForce[0].X, maxShearLoc = shearForce[0].X, maxBenMomLoc = bendingMoment[0].X, minBenMomLoc = bendingMoment[0].X;
            for(int i = 1; i <shearForce.Count;i++)
            {
                if (shearForce[i].Y < minShear)
                {
                    minShear = shearForce[i].Y;
                    minShearLoc = shearForce[i].X;
                }
                if (shearForce[i].Y > maxShear)
                {
                    maxShear = shearForce[i].Y;
                    maxShearLoc = shearForce[i].X;
                }
            }
            for (int i = 10; i < bendingMoment.Count; i++)
            {
                if (bendingMoment[i].Y < minBenMoment)
                {
                    minBenMoment = bendingMoment[i].Y;
                    minBenMomLoc = bendingMoment[i].X;
                }
                if (bendingMoment[i].Y > maxBenMoment)
                {
                    maxBenMoment = bendingMoment[i].Y;
                    maxBenMomLoc = bendingMoment[i].X;
                }
            }
            LongitudinalSummary.Add(new Summary("Min shear", minShear));
            LongitudinalSummary.Add(new Summary("Min shear Loacation", minShearLoc));
            LongitudinalSummary.Add(new Summary("Max shear", maxShear));
            LongitudinalSummary.Add(new Summary("Max shear Location", maxShearLoc));
            LongitudinalSummary.Add(new Summary("Min Bending Moment", minBenMoment));
            LongitudinalSummary.Add(new Summary("Min Bending Moment Location", minBenMomLoc));
            LongitudinalSummary.Add(new Summary("Max Bending Moment", maxBenMoment));
            LongitudinalSummary.Add(new Summary("Max Bending Moment Location", maxBenMomLoc));
            return LongitudinalSummary;
        }

        public Point GetCriticalPoints (List<FixedWeightModel> models, ShipGeom shp, Point trim)
        {
            PublicHelper helper = new PublicHelper();
            List<double> dlist = new List<double>();
            Point2D aftPt = new Point2D(0, trim.X);
            Point2D fwdpt = new Point2D(shp.LBP, trim.Y);
            double trimangle = Math.Atan((trim.Y - trim.X) / shp.LBP);
            int indexmin = 0;
            int indexmax = 0;
            foreach ( FixedWeightModel mod in models)
            {
                double dz = helper.SolveLinearEquation(aftPt, fwdpt, mod.CG.X);
                dlist.Add(mod.CG.Z + dz);
            }

            Point2D minmax = helper.GetMaxMinElementInArrayWithPointer(dlist, out indexmin, out indexmax);

            return new Point(models[indexmin].CG.X, models[indexmin].CG.Y, minmax.X);
        }
        

        #endregion public method

        #region private Method

        #region GetHeelAngle

        public double updateheelangle (double tcg, double tcb, double increment, double angle)
        {
            double updateangle = 0;
            if((tcg - tcb)>0)
            {
                updateangle = angle + increment;
            }
            else
            {
                updateangle = angle - increment;
            }
            

            return updateangle;
        }



        #endregion 
        #region gettrimvalue
        private double updateTrim ( double trim, double zchange, double lcg, double lcb)
        {
            double update = 0.0;
            
           if((lcg - lcb)>0)
            {
                update = trim + zchange;
                
            }
           else
            {
                update = trim - zchange;
                
            }

            return update;
        }

        private int getSignature ( double lcg, double lcb)
        {
            if(lcg <lcb)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private double updateSinkage(double zchange, double wt, double disp)
        {
            double update = 0;
            if (wt > disp)
            {
                update = zchange;
            }
            else
            {
                update = -zchange;
            }

            return update;
        }

        private double getmodifiedtrim(double lcg, double lcb, int sig)
        {
            int currentsig = this.getSignature(lcg, lcb);
            {
                if(currentsig!=sig)
                {
                    return 0;
                }
            }
            double value = Math.Abs(lcg - lcb);
            double returnvalue = 0;
            if(value >= 5.0)
            {
                returnvalue = 0.001;
            }
            else if((value < 5.0)&&(value>=0.5))
            {
                returnvalue = 0.0001;
            }
            else if (value < 0.5)
            {
                returnvalue = 0.00001;
            }
            else if(value<0.05)
            {
                returnvalue = 0.000001;
            }


            return returnvalue;
        }



        #endregion gettrimvalue

        #region GetTrim
        private double getMoment(double lcb, double lcg, double wt)
        {
            double d = (lcg - lcb) * wt;
            return d;
        }
        #endregion GetTrim
  
        #region GetKnCurve
        private List<SectionCurve> getModifiedSectionCurve(List<Section> secList)
        {
            List<SectionCurve> curveList = new List<SectionCurve>();
            for (int i = 0; i < secList.Count; i++)
            {
                SectionCurve curve = secList[i].SecCurve;
                List<Point2D> dataPtList = curve.SectionPts;
                SectionCurve newCurve = this.modifyPtList(dataPtList);
                newCurve.Xloc = secList[i].Xlocation;

                curveList.Add(newCurve);


            }




            return curveList;
        }

        private SectionCurve modifyPtList(List<Point2D> ptList)
        {
            SectionCurve curve = new SectionCurve();

            foreach (Point2D pt in ptList)
            {
                Point2D newPt = new Point2D(pt.X, 0.5 * pt.Y);
                curve.Addpoints(newPt);
            }

            return curve;
        }

        private List<SectionCurve> getDensePtCurve(List<SectionCurve> curves)
        {
            PublicHelper helper = new PublicHelper();
            List<SectionCurve> curveList = new List<SectionCurve>();
            foreach (SectionCurve curve in curves)
            {
                SectionCurve modifyCurve = new SectionCurve();
                modifyCurve.Xloc = curve.Xloc;
                List<Point2D> ptList = helper.GetDensePointSet(curve);
                foreach (Point2D pt in ptList)
                {
                    modifyCurve.Addpoints(pt);
                }

                curveList.Add(modifyCurve);
            }


            return curveList;
        }

        #endregion GetknCurve
        #region GetGzcurve
        private List<Point2D> getKNcurve(CrossCurve curveData, double disp)
        {
            List<Point2D> knCurvedata = new List<Point2D>();

            List<Dictionary<double, List<Point2D>>> dataList = curveData.CrossCurves;

            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<double, List<Point2D>> curveDataset = dataList[i];
                foreach (double key in curveDataset.Keys)
                {
                    PublicHelper pubHelp = new PublicHelper();
                    List<Point2D> knDataset = curveDataset[key];
                    double kn = pubHelp.InterpolateData(disp, knDataset,2);
                    Point2D knpt = new Point2D(key, kn);
                    knCurvedata.Add(knpt);

                }

            }

            return knCurvedata;

        }
        private List<Point2D> getDataFromCrossCurve (List<Dictionary<double, List<Point2D>>> dataList, double disp)
        {
            List<Point2D> knCurvedata = new List<Point2D>();

            //List<Dictionary<double, List<Point2D>>> dataList = curve.DispVTcb;

            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<double, List<Point2D>> curveDataset = dataList[i];
                foreach (double key in curveDataset.Keys)
                {
                    PublicHelper pubHelp = new PublicHelper();
                    List<Point2D> knDataset = curveDataset[key];
                    double kn = pubHelp.InterpolateData(disp, knDataset, 2);
                    Point2D knpt = new Point2D(key, kn);
                    knCurvedata.Add(knpt);

                }

            }

            return knCurvedata;


        }
        #endregion GetGzCurve
        #region GetGMt

        private double getBmt ( ShipGeom shp, double rho)
        {
            PublicHelper helper = new PublicHelper();
            double vol = (helper.GetSecArea(shp.BuoyancyCurve))/rho;
            double ixx = this.getIxx(shp);

            return ixx / vol;
        }

        private double getIxx(ShipGeom shp)
        {
            PublicHelper helper = new PublicHelper();
            List<Point2D> pointList = this.ptListforIxxCalculation(shp.WaterLine);
            double ixx = (0.66) * helper.GetSecArea(pointList);            
            
            
            return ixx;
        }

        private List<Point2D> ptListforIxxCalculation(List<Point2D> waterline)
        {
            List<Point2D> ptList = new List<Point2D>();

            foreach ( Point2D pt in waterline)
            {
                Point2D curvePt = new Point2D(pt.X, Math.Pow((pt.Y/2), 3));
                ptList.Add(curvePt);
            }



            return ptList;

        }

        #endregion


        #region WriteOutputData
        private double getWt(Tank tank,double x, double xvalue)
        {
            List<SectionCurve> curveList = tank.TankCurvedata;
            //curveList.Reverse();
            double x1 = curveList[0].Xloc;
            double x2 = curveList[curveList.Count - 1].Xloc;
            if((x<x1)||(x>x2))
            {
                return 0;

            }
            else
            {
                return xvalue/Math.Abs(x2-x1);
            }
          
        }
        private double getLiquidWt(double x,List<Tank> tankData,List<Mapping> mp)
        {
            double sum = 0;
            for (int i = 0; i < mp.Count; i++)
            {
                
                string tname = mp[i].TankName;
                foreach(Tank tank in tankData)
                {
                    string s = tank.TankName;
                    double wt = tank.MaxWeight;
                    if (s == tname)
                    {
                        if(mp[i].IndexValue == 4)
                        {
                            double d = getWt(tank, x, wt*mp[i].Xvalues/100);
                            sum = sum + d;

                        }
                        
                        
                    }

                }
                
            }

            return sum;

        }
        private double SolidWt(double x,double wtLoc, double solidWt,double dist)
        {
            double x1 = x - dist/2;

            double x2 = x + dist/2;

            
            if ((wtLoc < x1) || (wtLoc > x2))
            {
                return 0;

            }
            else
            {
                return solidWt;
            }

        }
        private double getSolidWt(double x,List<Point2D> solidWt,double distance)
        {
            double sum = 0;
            foreach(Point2D pt in solidWt)
            {
                sum += SolidWt(x,pt.X, pt.Y, distance); 
            }
            return sum;
            
        }
        
        private double GetDeadWeight(List<LiquidWeightModel> LiquidWeights, List<FixedWeightModel> FixedWeights)
        {
            double DeadWeight = 0.0;
            for (int i = 0; i < LiquidWeights.Count; i++)
            {
                DeadWeight += LiquidWeights[i].Weight;
            }
            for (int i = 0; i < FixedWeights.Count; i++)
            {
                if (FixedWeights[i].ObjectName != "Lightship")
                {
                    DeadWeight += FixedWeights[i].Weight;
                }
            }
            return DeadWeight;
        }
        
        private double GetDeadWeightLCG(List<LiquidWeightModel> LiquidWeights, List<FixedWeightModel> FixedWeights)
        {
            double DeadWeight = 0.0, LcgCalc = 0;
            for (int i = 0; i < LiquidWeights.Count; i++)
            {
                DeadWeight += LiquidWeights[i].Weight;
                LcgCalc += LiquidWeights[i].Weight * LiquidWeights[i].CG.X;
            }
            for (int i = 0; i < FixedWeights.Count; i++)
            {
                if (FixedWeights[i].ObjectName != "Lightship")
                {
                    DeadWeight += FixedWeights[i].Weight;
                    LcgCalc += FixedWeights[i].Weight * FixedWeights[i].CG.X;
                }
            }
            return LcgCalc / DeadWeight;

        }
        private double GetDeadWeightVCG(List<LiquidWeightModel> LiquidWeights, List<FixedWeightModel> FixedWeights)
        {
            double DeadWeight = 0.0, VcgCalc = 0;
            for (int i = 0; i < LiquidWeights.Count; i++)
            {
                DeadWeight += LiquidWeights[i].Weight;
                VcgCalc += LiquidWeights[i].Weight * LiquidWeights[i].CG.Y;
            }
            for (int i = 0; i < FixedWeights.Count; i++)
            {
                if (FixedWeights[i].ObjectName != "Lightship")
                {
                    DeadWeight += FixedWeights[i].Weight;
                    VcgCalc += FixedWeights[i].Weight * FixedWeights[i].CG.Y;
                }
            }
            return VcgCalc / DeadWeight;

        }
        private double GetDeadWeightTCG(List<LiquidWeightModel> LiquidWeights, List<FixedWeightModel> FixedWeights)
        {
            double DeadWeight = 0.0, TcgCalc = 0;
            for (int i = 0; i < LiquidWeights.Count; i++)
            {
                DeadWeight += LiquidWeights[i].Weight;
                TcgCalc += LiquidWeights[i].Weight * LiquidWeights[i].CG.Z;
            }
            for (int i = 0; i < FixedWeights.Count; i++)
            {
                if (FixedWeights[i].ObjectName != "Lightship")
                {
                    DeadWeight += FixedWeights[i].Weight;
                    TcgCalc += FixedWeights[i].Weight * FixedWeights[i].CG.Z;
                }
            }
            return TcgCalc / DeadWeight;

        }
        public double GetDisplacement(List<LiquidWeightModel> LiquidWeights, List<FixedWeightModel> FixedWeights)
        {
            double displacement = GetDeadWeight(LiquidWeights, FixedWeights);
            for (int i = 0; i < FixedWeights.Count; i++)
            {
                if (FixedWeights[i].ObjectName == "Lightship")
                {
                    displacement += FixedWeights[i].Weight;
                }
            }
            return displacement;
        }
        public double GetDisplacementLCG(List<LiquidWeightModel> LiquidWeights, List<FixedWeightModel> FixedWeights)
        {
            double disp = 0.0, LcgCalc = 0;
            for (int i = 0; i < LiquidWeights.Count; i++)
            {
                disp += LiquidWeights[i].Weight;
                LcgCalc += LiquidWeights[i].Weight * LiquidWeights[i].CG.X;
            }
            for (int i = 0; i < FixedWeights.Count; i++)
            {
                disp += FixedWeights[i].Weight;
                LcgCalc += FixedWeights[i].Weight * FixedWeights[i].CG.X;
            }
            return LcgCalc / disp;
        }
        public double GetDisplacementVCG(List<LiquidWeightModel> LiquidWeights, List<FixedWeightModel> FixedWeights)
        {
            double disp = 0.0, VcgCalc = 0;
            for (int i = 0; i < LiquidWeights.Count; i++)
            {
                disp += LiquidWeights[i].Weight;
                VcgCalc += LiquidWeights[i].Weight * LiquidWeights[i].CG.Y;
            }
            for (int i = 0; i < FixedWeights.Count; i++)
            {
                disp += FixedWeights[i].Weight;
                VcgCalc += FixedWeights[i].Weight * FixedWeights[i].CG.Y;
            }
            return VcgCalc / disp;
        }
        public double GetDisplacementTCG(List<LiquidWeightModel> LiquidWeights, List<FixedWeightModel> FixedWeights)
        {
            double disp = 0.0, TcgCalc = 0;
            for (int i = 0; i < LiquidWeights.Count; i++)
            {
                disp += LiquidWeights[i].Weight;
                TcgCalc += LiquidWeights[i].Weight * LiquidWeights[i].CG.Z;
            }
            for (int i = 0; i < FixedWeights.Count; i++)
            {
                disp += FixedWeights[i].Weight;
                TcgCalc += FixedWeights[i].Weight * FixedWeights[i].CG.Z;
            }
            return TcgCalc / disp;
        }
        private int IndexValue(double pt,List<Point2D> steelWt)
        {
            int key = new int();
            key = 0;

            for (int i = 0; i < steelWt.Count; i++)
            {
                double dummy = pt - steelWt[i].X;

                if (dummy >= 0)
                {
                    key = i;
                }

            }

            return key;
        }
        #endregion


        #endregion private Method

    }
}
