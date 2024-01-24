using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace ShipStability
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            double shpLbp = 110; // 126.8;
            //double rho = 1.025;
            WriteOutput wrout = new WriteOutput();
            ShipGeom shgeom = new ShipGeom();
            InputParamHelper iohelper = new InputParamHelper();
            OutputParamHelper outHelper = new OutputParamHelper();
            PublicHelper pHelper = new PublicHelper();
            Tank thelp = new Tank();
            Wind wHelper = new Wind();
            Summary sHelp = new Summary();
            #region Loading Data
            shgeom = iohelper.ReaddShipgeomdata();
            shgeom.LBP = shpLbp;
            shgeom.Depth = 9.0;
            shgeom.FLD = 13;
            shgeom.DownFLDangle = 65;
            shgeom.Breadth = 17.60;
            shgeom.DesignDraft = 6.6;
            shgeom.Rho = 1.025;
            List<Section> sectionList = iohelper.ReadSectionData();
            List<Section> dummySeclist = iohelper.ReadSectionCurve();
            Dictionary<string, List<string>> subtankList = iohelper.ReadSubstractedTankInfo();
            foreach (Section sec in dummySeclist)
            {
                sec.UpdateSectionCurve(sec);
            }

            wrout.WriteSectionLines(dummySeclist);

           
            //List<Tank> tankGeoInfoList =  iohelper.ReadShipTankData();
            List<Tank> tankGeoInfoList = iohelper.GetTankGeom();
            iohelper.GetTankFSMValue(ref tankGeoInfoList);
            //tankGeoInfoList.RemoveAt(0);

            //List<Tank> tankbasicInfoList = iohelper.GetPermiability();
            System.IO.File.WriteAllText("result.json", Newtonsoft.Json.JsonConvert.SerializeObject(tankGeoInfoList));
            wrout.WritetankInfo(tankGeoInfoList);

            List<Vector2D> hulldist = iohelper.ReadHullDistData();
            shgeom.GetHullWtDistribution(dummySeclist, hulldist, 1.0);

            List<string> damageTankNameList = iohelper.ReadDamagedTank();
            string filename = "solidWt.dat";
            List<FixedWeightModel> solidWtList = iohelper.ReadFixedWeight(filename);
            filename = "DamageWt.dat";
            List<FixedWeightModel> creitModel = iohelper.ReadFixedWeight(filename);
            filename = "FloodPt.dat";
            List<FixedWeightModel> floodmodel = iohelper.ReadFixedWeight(filename);

            List<Mapping> mList = iohelper.ReadLoadingData();
            List<Mapping> fmList = iohelper.ReadFrameInput();

            List<FixedWeightModel> supStructList = iohelper.ReadLateralAreaInfo();

            // end of reading files
            // Testing
            List<LiquidWeightModel> liqModelList = new List<LiquidWeightModel>();
            #region main part of code
            //foreach (Tank tank in tankGeoInfoList)
            //{
            //    //if(tank.TankName== "COTNO.3.P")
            //    //{
            //    //    tank.GetTankFsm(tank, shgeom);
            //    //}
            //    LiquidWeightModel tankmodel = new LiquidWeightModel();
            //    foreach (Tank dummytank in tankbasicInfoList)
            //    {
            //        if (dummytank.TankName == tank.TankName)
            //        {
            //            tank.Permiability = dummytank.Permiability;
            //            tank.LiquidRho = dummytank.LiquidRho;
            //            tank.GetTankSoundings(tank);
            //            tank.GetSecTankAreaSounding(tank);
            //            tank.GetTankFsm(tank, shgeom);
            //            break;
            //        }
            //    }

            //    tankmodel.Tankname = tank.TankName;
            //    //tankmodel.Weight = tank.MaxWeight /tank.LiquidRho;
            //    //Point pt = new Point(tank.TankLcg[tank.TankLcg.Count - 1].Y, tank.TankTcg[tank.TankTcg.Count - 1].Y, tank.TankVcg[tank.TankVcg.Count - 1].Y);
            //    //tankmodel.CG = pt;

            //    liqModelList.Add(tankmodel);

            //}
            //wrout.MaxTankVolume(liqModelList);
            #endregion

            foreach( Tank tank in tankGeoInfoList)
            {

                tank.GetTankSoundings(tank);
                tank.GetSecTankAreaSounding(tank);

            }

            wrout.WriteTankSoundings(tankGeoInfoList);

            thelp.UpdateTankSounding(tankGeoInfoList, subtankList);



            wrout.WriteTankSoundings(tankGeoInfoList);
            Tank thelper = new Tank();
            LiquidWeightModel lhelper = new LiquidWeightModel();
            Damage dhelper = new Damage();
            List<LiquidWeightModel> liquidWeight = lhelper.GetLiquidWt(mList, tankGeoInfoList);
            List<Tank> dmageTankList = thelper.GetDamageTankDetails(damageTankNameList, tankGeoInfoList);
            List<Object> objlst = new List<Object>();
            objlst.AddRange(solidWtList);
            objlst.AddRange(liquidWeight);
            Object hulldetails = pHelper.GetCentroid(objlst);
            Console.WriteLine("Total Loading");
            Console.WriteLine(hulldetails.Weight.ToString() + ',' + hulldetails.CG.X.ToString() + ',' + hulldetails.CG.Y.ToString() + ',' + hulldetails.CG.Z.ToString());


            CrossCurve curve = new CrossCurve();


            {
                Console.WriteLine("Generate Crosscurve Data for GZ calculation");
                Console.WriteLine("Give minimum heeling angle in degree ");
                string a1 = Console.ReadLine();
                Console.WriteLine("Give maximum heeling angle in degree ");
                string a2 = Console.ReadLine();

                Console.WriteLine("Give the increment of angle in degree ");
                string a3 = Console.ReadLine();

                Console.WriteLine("Give maximum draft mark in metre ");
                string a4 = Console.ReadLine();

                curve = outHelper.GetCrossCurveData(sectionList, double.Parse(a1), double.Parse(a2), double.Parse(a3), double.Parse(a4), shgeom.Rho);

            }
            #endregion Loading Data



            sHelp.getHydrostaticData(shgeom, dummySeclist, shgeom.Rho);
            #region For Trim

            bool execution = true;
            Console.WriteLine("press n/N to skip trim calculation");
            string str = Console.ReadLine();
            if ((str == "n") || (str == "N"))
            {
                execution = false;
            }


            while (execution)
            {
                //Point trimingvalue = outHelper.GetTrimValue(shgeom, sectionList, hulldetails.Weight, hulldetails.CG.X, rho);
                //double theta = (180 / Math.PI) * Math.Atan((trimingvalue.Y - trimingvalue.X) / shgeom.LBP);
                //Console.WriteLine();
                //Console.WriteLine("theta in degree, trim, draft at Ap, Draft at Fp");
                //string output = theta.ToString() + "," + (trimingvalue.Y - trimingvalue.X).ToString() + "," + trimingvalue.X.ToString() + "," + trimingvalue.Y.ToString();
                //Console.WriteLine(output);
                //Console.WriteLine();
                //List<Point2D> windGz = new List<Point2D>();
                //List<Summary> windInfo = wHelper.GetWindCriteria(25, supStructList, shgeom, sectionList, solidWtList, liquidWeight, curve, rho, -35, ref windGz);
                ////double heelAngle = outHelper.GetHeelAngle(shgeom, sectionList, double.Parse(sr[0]), double.Parse(sr[2]), rho);
                ////Console.WriteLine("Heel =  " + heelAngle.ToString()+"  degree");
                List<Summary> conditionalloadSummary = sHelp.GetEquilibriumConditionDetails(shgeom, curve, solidWtList, liquidWeight, dummySeclist, tankGeoInfoList, shgeom.Rho);
                Console.WriteLine("Equilibrium Details");
                Console.WriteLine();
                wrout.WritingSummary(conditionalloadSummary);
                Console.WriteLine();
                Console.WriteLine("Intact Criteria");
                Point critPoint = sHelp.GetCriticalPoint(creitModel, shgeom, new Point(conditionalloadSummary[6].ObjectValue, conditionalloadSummary[8].ObjectValue, conditionalloadSummary[7].ObjectValue));
                Point fldPoint = sHelp.GetCriticalPoint(floodmodel, shgeom, new Point(conditionalloadSummary[6].ObjectValue, conditionalloadSummary[8].ObjectValue, conditionalloadSummary[7].ObjectValue));
                List<Summary> intactCriteriaSummary = sHelp.GetIntactCriteriaValue(solidWtList, liquidWeight, curve, shgeom, shgeom.Rho,critPoint,fldPoint);
                wrout.WritingSummary(intactCriteriaSummary);
                Console.WriteLine();
                Console.WriteLine("WindCriteria");
                List<Point2D> windGz = new List<Point2D>();
                Wind criteria = new Wind(1,1,1,0.09,50);
                List<Summary> windCriteriaInfo = sHelp.GetWindCriteriaInfo(40, supStructList, shgeom, dummySeclist, solidWtList, liquidWeight, curve, shgeom.Rho, criteria, ref windGz);
                wrout.WritingSummary(windCriteriaInfo);
                wrout.WriteWindGz(windGz);

                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("press n/N to stop");
                string st = Console.ReadLine();
                if((st=="n")||(st == "N"))
                {
                    execution = false;
                }

                





            }

            execution = true;
            #endregion For trim

    
            #region for Longitudinal Strength
            execution = true;
            Console.WriteLine("press n/N to skip Longitudinal Load calculation");
            str = Console.ReadLine();
            if ((str == "n") || (str == "N"))
            {
                execution = false;
            }

            while (execution)
            {
                //LongitudinalCalculation lhelp = new LongitudinalCalculation();
                //List<Point2D> solidWt = new List<Point2D>();
                //solidWt.Add(new Point2D(38.080, 1000 * 30.75));
                List<Point2D> shearForce = new List<Point2D>();
                List<Point2D> bendingMoment = new List<Point2D>();
                List<Summary> longitudinalSummary = sHelp.GetLongitudinalStrengthSummaryInfo(shgeom, solidWtList, tankGeoInfoList, mList, dummySeclist, shgeom.Rho, ref shearForce, ref bendingMoment);
                Console.WriteLine("Longitudinal Summary");
                wrout.WritingSummary(longitudinalSummary);
                Console.WriteLine();
                Console.WriteLine("Longitudinal Calculation");
                List<LongitudinalCalculation> lcList = sHelp.GetLSCalculationInfo(longitudinalSummary, fmList, shearForce, bendingMoment);
                wrout.WriteLSCalculation(lcList);
                




                execution = false;
            }





            #endregion

            #region DamageStability
            execution = true;
            Console.WriteLine("press n/N to skip Damage Stability");
            str = Console.ReadLine();
            if ((str == "n") || (str == "N"))
            {
                execution = false;
            }

            while (execution)
            {
                List<Point2D> damageGZcurve = new List<Point2D>();
                List<Summary> damageCriteria = new List<Summary>();

                List<Summary> summaryList = dhelper.GetDamagedHydrostatic(shgeom, solidWtList, liquidWeight, dummySeclist, dmageTankList, curve, shgeom.Rho, ref damageCriteria, ref damageGZcurve);
                Console.WriteLine();
                Console.WriteLine("Damage Stability");
                wrout.WritingSummary(summaryList);
                Console.WriteLine();
                Console.WriteLine("Damage Criteria");
                wrout.WritingSummary(damageCriteria);

                execution = false;
            }



            #endregion





        }
    }

    //class FixedWeightModel
    //{
    //    public string _solidTankName;
    //    public double _solidWt;
    //    public Point _cGsolid;

    //}

    //class LiquidWeightModel
    //{
    //    public string _tankName;
    //    public double _wt;
    //    public Point _Cg;
    //}

    //class RequestModel
    //{

    //    public List<FixedWeightModel> _fixedWt;
    //    public List<LiquidWeightModel> _liquidWt;
        
    //}
}
