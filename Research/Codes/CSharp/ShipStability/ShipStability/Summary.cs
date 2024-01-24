using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class Summary
    {
        #region member
        string _objectname;
        double _objectvalue;
        #endregion

        #region Constructor
        public Summary()
        {
            this._objectvalue = new double();
        }

        public Summary ( string str, double objValue)
        {
            this._objectname = str;
            this._objectvalue = objValue;
        }

        #endregion

        #region Properties

        public string ObjectName
        { 
            get { return this._objectname; }
            set { this._objectname = value; }

            
        }

        public double ObjectValue
        {
            get { return this._objectvalue; }
            set { this._objectvalue = value; }
        }

        #endregion


        #region public method

        public Point GetCriticalPoint(List<FixedWeightModel> models, ShipGeom shp, Point trim)
        {
            OutputParamHelper helper = new OutputParamHelper();
            return (helper.GetCriticalPoints(models, shp, trim));
        }

        public List<Mapping> GettTableforIntact(List<Point2D> gZcurve, Point critPt, Point fldPt)
        {
            IntactStability ishelper = new IntactStability();
            return (ishelper.GettTable(gZcurve, critPt, fldPt));
        }
        public double GetMaxFSMValue (Tank tank)
        {
            PublicHelper phelper = new PublicHelper();
            List<Point2D> pt2D = tank.Fsmt;
            List<double> fsmList = this.convertPtarreyToDoubleArray(pt2D);
            Point2D pt = phelper.GetMaxMinElementInArray(fsmList);

            return pt.Y;
        }

        public List<Point2D> GetGzCurveInfo(List<FixedWeightModel> fixedWt, List<LiquidWeightModel> liqWt, CrossCurve curve)
        {
            OutputParamHelper outHelper = new OutputParamHelper();
            Object shipDisp = outHelper.IntactWtDetails(fixedWt, liqWt);
            List<Point2D> gzCurve = outHelper.GetGZcurve(shipDisp.CG.Z, 0, curve, shipDisp.Weight);
            return gzCurve;
        }

        public List<Summary> GetLoadingSummary ( Dictionary<string, List<LiquidWeightModel>>liquidWeight, List<FixedWeightModel> fixedWt)
        {
            OutputParamHelper helper = new OutputParamHelper();
            List<LiquidWeightModel> liqidWtList = this.getCategoryWiseLiquidWt(liquidWeight);

            List<Summary> loadingSummary = helper.GetSummary(liqidWtList, fixedWt);

            return loadingSummary;
        }

        public List<Summary> GetIntactCriteriaValue (List<FixedWeightModel> fixedWt, List<LiquidWeightModel> liqWt, CrossCurve curve, ShipGeom ship, double rho, Point critPt, Point fldPt )
        {
            OutputParamHelper outhelper = new OutputParamHelper();
            Object shipDisp = outhelper.IntactWtDetails(fixedWt, liqWt);
            OutputParamHelper helper = new OutputParamHelper();
            return (helper.GetIntactCriteria(shipDisp, curve, ship, rho, critPt,fldPt));
        }

        public List<Summary> GetEquilibriumConditionDetails(ShipGeom ship, CrossCurve curveData, List<FixedWeightModel> fixedWt, List<LiquidWeightModel> liquidWt, List<Section> secList, List<Tank> tanks, double rho)
        {
            IntactStability iHelper = new IntactStability();
            return (iHelper.GetIntactSummaryInfo(ship, curveData, fixedWt, liquidWt, secList, tanks, rho));
            
        }

        public List<Mapping> GetGridvalue (Tank tank, double rho, Mapping map)
        {
            OutputParamHelper helper = new OutputParamHelper();
            List<Mapping> mappingList = helper.GetTankData(tank, rho, map);
            return mappingList;

        }
        /// <summary>
        /// In this List<FixedWeightModel> SolidWT
        /// Remove the Lightshp weight from the list.
        /// </summary>
        /// <param name="shp"></param>
        /// <param name="solidWt"></param>
        /// <param name="tanks"></param>
        /// <param name="mList"></param>
        /// <returns></returns>
        public List<Summary> GetLongitudinalStrengthSummaryInfo (ShipGeom shp, List<FixedWeightModel> solidWt, List<Tank> tanks, List<Mapping> mList, List<Section> secList, double rho, ref List<Point2D> shearforce, ref List<Point2D> bendingMoment)
        {
            WriteOutput wrout = new WriteOutput();
            List<Summary> summaryList = new List<Summary>();
            List<Point2D> wtList = this.convertObjectType(solidWt);
            LongitudinalCalculation lHelper = new LongitudinalCalculation();
            OutputParamHelper outHelper = new OutputParamHelper();
            shearforce = new List<Point2D>();
            bendingMoment = lHelper.GetBendingMomentInfo(shp, wtList, tanks, mList, secList,rho, ref shearforce);
            summaryList = outHelper.GetLongitudinalStrengthSummary(shearforce, bendingMoment);
            wrout.WriteLongStData(shp.HullWtDistribution, "hull_Dist.dat");
            wrout.WriteLongStData(shearforce, "SheareForce.dat");
            wrout.WriteLongStData(bendingMoment, "BendingMoment.dat");
            wrout.WriteLoadCurves(shp.WtCurve, shp.BuoyancyCurve, "wtVbuoyancy.dat");
            

            return summaryList;
        }

        public List<LongitudinalCalculation> GetLSCalculationInfo( List<Summary> lsSummary, List<Mapping> frameInfo, List<Point2D> shearF, List<Point2D> bendingMom)
        {
            LongitudinalCalculation lhelp = new LongitudinalCalculation();
            List<LongitudinalCalculation> lsCalInfoList = new List<LongitudinalCalculation>();
            Point2D maxShearBendingInfo = this.getmaxBendingShearInfo(lsSummary);
            foreach(Mapping map in frameInfo)
            {
                LongitudinalCalculation lc = lhelp.GetLongitudinaldata(maxShearBendingInfo, map, shearF, bendingMom);
                
                lc.FrameNo = map.IndexValue;
                lc.FrameSpacing = map.Xvalues;
                lsCalInfoList.Add(lc);
            }

            return lsCalInfoList;
        }
        public Dictionary<string, List<Summary>> GetDamagedHydrostaticFull(ShipGeom ship, List<FixedWeightModel> fixedWt, List<LiquidWeightModel> liquidWt, List<Section> secList, Dictionary<string, List<string>> damageTankName, CrossCurve curve, double rho, ref Dictionary<string, List<Summary>> damageCritaria, List<Tank> tankList, ref Dictionary <string , List<Point2D>> damageGzCurve)
        {
            Damage damHelper = new Damage();
            Dictionary<string, List<Summary>> damageConditionalDetails = new Dictionary<string, List<Summary>>();
            damageCritaria = new Dictionary<string, List<Summary>>();

            foreach (string key in damageTankName.Keys)
            {
                List<Point2D> damGz = new List<Point2D>();
                List<Tank> damageTanks = this.getdamageTanks(damageTankName[key], tankList);
                List<Summary> summaryListCriteria = new List<Summary>();
                List<Summary> conditionaldamage = damHelper.GetDamagedHydrostatic(ship, fixedWt, liquidWt, secList, damageTanks, curve, rho, ref summaryListCriteria, ref damGz);
                damageCritaria.Add(key, summaryListCriteria);
                damageConditionalDetails.Add(key, conditionaldamage);
                damageGzCurve.Add(key, damGz);
            }

            return damageConditionalDetails;

        }

        public List<Summary> GetWindCriteriaInfo(double velocity, List<FixedWeightModel> superstructureList, ShipGeom ship, List<Section> secList, List<FixedWeightModel> fixwWt, List<LiquidWeightModel> liqWt, CrossCurve curve, double rho, Wind rollCriteria, ref List<Point2D> windGz)
        {
            Wind whelper = new Wind();
            windGz = new List<Point2D>();
            List<Summary> winfInfoList = whelper.GetWindCriteria(velocity, superstructureList, ship, secList, fixwWt, liqWt, curve, rho, rollCriteria, ref windGz);
                
            return winfInfoList;
        }

        /// <summary>
        /// This code returns few hydrostatic data for checking
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        /// 
        public void getHydrostaticData(ShipGeom sGeom, List<Section> secList, double rho)
        {
            WriteOutput wrhelper = new WriteOutput();
            OutputParamHelper outhelper = new OutputParamHelper();
            Dictionary<double, List<double>> hydroStatdata = outhelper.GetHydrostatData(sGeom, secList, rho);
            wrhelper.WriteHydrostatData(hydroStatdata);

        }

        #endregion

        #region private

        private List<Point2D> convertObjectType (List<FixedWeightModel> objList)
        {
            List<Point2D> wtlist = new List<Point2D>();
            foreach(FixedWeightModel model in objList)
            {
                if(model.ObjectName != "Lightship")
                {
                    Point2D pt = new Point2D(model.CG.X, model.Weight);
                    wtlist.Add(pt);
                }

            }

            return wtlist;
        }

        private List<LiquidWeightModel> getCategoryWiseLiquidWt (Dictionary<string, List<LiquidWeightModel>> liquidWeight)
        {
            
            PublicHelper helper = new PublicHelper();
            List<LiquidWeightModel> modelList = new List<LiquidWeightModel>();
            foreach (string key in liquidWeight.Keys)
            {
                List<Object> objList = new List<Object>();
                LiquidWeightModel model = new LiquidWeightModel();
                model.Tankname = key;
                List<LiquidWeightModel> mapList = liquidWeight[key];
                objList.AddRange(mapList);
                Object obj = helper.GetCentroid(objList);
                model.CG = obj.CG;
                model.Weight = obj.Weight;
                modelList.Add(model);
                
                
            }

            return modelList;

        }

        private List<Tank> getdamageTanks ( List<String> tankname, List<Tank> tankList)
        {
            List<Tank> damageTanks = new List<Tank>();
            foreach ( string str in tankname)
            {
                foreach (Tank tank in tankList)
                {
                    if(str == tank.TankName)
                    {
                        damageTanks.Add(tank);
                        break;
                    }
                }
            }

            return damageTanks;
        }

        private Point2D getmaxBendingShearInfo(List<Summary> longsumList)
        {
            double p1 = 0;
            double p2 = 0;
            foreach(Summary sum in longsumList)
            {
                if(sum.ObjectName == "Max shear")
                {
                    p1 = sum.ObjectValue;
                }
                else if(sum.ObjectName == "Max Bending Moment")
                {
                    p2 = sum.ObjectValue;
                }
            }

            Point2D pt = new Point2D(p1, p2);

            return pt;
        }

        private List<double> convertPtarreyToDoubleArray( List<Point2D> ptList)
        {
            List<double> dList = new List<double>();
            foreach( Point2D pt in ptList)
            {
                dList.Add(pt.Y);
            }

            return dList;
        }

 

        #endregion






    }
}
