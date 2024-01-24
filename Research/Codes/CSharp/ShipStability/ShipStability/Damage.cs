using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class Damage
    {
        #region Constructor

        public Damage()
        {

        }

        #endregion Constructor

        #region Public Method



        public List<Summary> GetDamagedHydrostatic(ShipGeom ship, List<FixedWeightModel> fixedWt, List<LiquidWeightModel> liquidWt, List<Section> secList, List<Tank> tankList, CrossCurve curve, double rho, ref List<Summary> criteria , ref List<Point2D> damageGZ)
        {
            
            criteria = new List<Summary>();
            OutputParamHelper outhelper = new OutputParamHelper();
            List<Summary> summaryList = new List<Summary>();
            Object obj = outhelper.IntactWtDetails(fixedWt, liquidWt);
            Point  initialTrim = this.trimValue(ship, secList, obj.Weight, obj.CG.X, rho);
            Vector2D v = new Vector2D(new Point2D(0, initialTrim.X), new Point2D(ship.LBP, initialTrim.Y));
            bool sati = false;
            double eps = 0.05;
            double eps1 = 0.0000001;
            while(!sati)
            {

                obj = this.damagedWt(fixedWt, liquidWt, tankList, v);
                Point modifyTrim = this.trimValue(ship, secList, obj.Weight, obj.CG.X, rho);
                if(Math.Abs((initialTrim.Z - modifyTrim.Z) / (initialTrim.Z+eps1)) <eps)
                {
                    sati = true;
                }
                else
                {
                    initialTrim.X = modifyTrim.X;
                    initialTrim.Y = modifyTrim.Y;
                    initialTrim.Z = modifyTrim.Z;
                    v.StPt.X = 0;
                    v.StPt.Y = modifyTrim.X;
                    v.EndPt.X = ship.LBP;
                    v.EndPt.Y = modifyTrim.Y;
                }
                
            }
            List<Point2D> ptList = outhelper.GetGZcurve(obj.CG.Z, 0, curve, obj.Weight);
            damageGZ.AddRange(ptList);
            double gm = outhelper.GetGmfromSmalAngle(obj.CG, obj.Weight, ship, rho);
            double heel = outhelper.GetHeelangle(obj.Weight,obj.CG, curve);
            double fbh = ship.Depth - (0.5) * (initialTrim.X + initialTrim.Y);
            double draftms = (0.5) * (initialTrim.X + initialTrim.Y);
            double maxGztilltheta = outhelper.GetmaxGztilltheta(ptList, 20);
            Summary sum = new Summary("Trim", initialTrim.Y - initialTrim.X);
            Summary sum2 = new Summary("Displacement", obj.Weight);
            Summary sum1 = new Summary("Heel", heel);
            Summary sum3 = new Summary("LCG", obj.CG.X);
            Summary sum4 = new Summary("TCG", obj.CG.Y);
            Summary sum5 = new Summary("VCG", obj.CG.Z);
            Summary sum6 = new Summary("Draft AFT", initialTrim.X);
            Summary sum7 = new Summary("Draft MS", draftms);
            Summary sum8 = new Summary("Draft FWD", initialTrim.Y);
            Summary sum9 = new Summary("GM(Solid)", gm);
            Summary sum10 = new Summary("GM(Fluid)", gm);
            Summary sum11 = new Summary("Freeboard at Equilibrium",fbh );
            
                       
            summaryList.Add(sum);
            summaryList.Add(sum1);
            summaryList.Add(sum2);
            summaryList.Add(sum3);
            summaryList.Add(sum4);
            summaryList.Add(sum5);
            summaryList.Add(sum6);
            summaryList.Add(sum7);
            summaryList.Add(sum8);
            summaryList.Add(sum9);
            summaryList.Add(sum10);
            summaryList.Add(sum11);



            criteria.Add(new Summary("Freeboard Height at EQU", fbh));
            criteria.Add(new Summary("Angle at EQU", heel));
            criteria.Add(this.getdownFLDinfo("Angle from EQU to RA0 or FLD", ptList[ptList.Count - 1].X,heel, ship.DownFLDangle));
            criteria.Add(new Summary("Area from EQU to 20 deg", (Math.PI/180.0)*outhelper.GetAreafromZeroTotheta(ptList, 20)));
            criteria.Add(new Summary("Flood Point Height at EQU", ship.FLD - draftms));
            criteria.Add(new Summary("Maximum GZ from EQU to 20 deg", maxGztilltheta));


            return summaryList;
        }


        




        #endregion Public Method

        #region private Method

        #region GetDamagedHydrostatic

        private Summary getdownFLDinfo( string text, double rA0, double heel, double dFLDangle)
        {
            if(dFLDangle> rA0)
            {
                return new Summary(text, rA0 - heel);
            }
            else
            {
                return new Summary(text, dFLDangle - heel);
            }
        }

        

        private Point trimValue(ShipGeom sGeom, List<Section> secList, double wt, double lcg, double rho)
        {
            OutputParamHelper outhelper = new OutputParamHelper();
            Point pt = outhelper.GetTrimValue(sGeom, secList, wt, lcg, rho);
            pt.Z = (pt.Y - pt.X);


            return pt ;    
        }

       

        private Object damagedWt(List<FixedWeightModel> fixedWt, List<LiquidWeightModel> liquidWt, List<Tank> damagedTank, Vector2D draftLine)
        {
            PublicHelper helper = new PublicHelper();
            Object obj = new Object();
            List<Object> objList = new List<Object>();
            List<Object> updatedLiwWtList = this.addedwtList(liquidWt, damagedTank, draftLine);
            objList.AddRange(updatedLiwWtList);
            objList.AddRange(fixedWt);
            obj = helper.GetCentroid(objList);


            return obj;
        }

        private List<Object> addedwtList(List<LiquidWeightModel> liquidWt, List<Tank> damagedTank, Vector2D draftLine)
        {
            List<Object> objList = new List<Object>();
       
            
            foreach(Tank tank in damagedTank)
            {
                                
                {
                    LiquidWeightModel modifyModel = this.modifytankValue(tank, draftLine);
                    objList.Add(modifyModel);
                }
            }

            foreach (LiquidWeightModel model in liquidWt)
            {
                string tankName = model.Tankname;
                int count = 0;
                foreach ( Tank tank in damagedTank)
                {
                    string str = tank.TankName;
                    {
                        if(tankName == str)
                        {
                            count++;
                        }
                    }
                }

                if(count == 0)
                {
                    objList.Add(model);
                }
            }

                
        
  
            return objList;
        }

        private LiquidWeightModel modifytankValue (Tank tank, Vector2D draftLine)
        {
            PublicHelper helper = new PublicHelper();
            LiquidWeightModel model = new LiquidWeightModel();
            List<SectionCurve> curveList = tank.SecTankDraftVArea;
            List<Point2D> areaList = new List<Point2D>();
            foreach(SectionCurve curve in curveList)
            {
                double x = curve.Xloc;
                List<Point2D> draftVarea = curve.SectionPts;
                double z = helper.SolveLinearEquation(draftLine.StPt, draftLine.EndPt, x);
                if (z < draftVarea[0].X)
                {
                    areaList.Add(new Point2D(x, 0));
                }
                else
                {
                    double area = helper.InterpolateData(z, draftVarea, 2);
                    Point2D pt = new Point2D(x, area);
                    areaList.Add(pt);

                }
                    
              

            }

            double vol = helper.GetSecArea(areaList);
            double draft = helper.InterpolateData(vol, tank.VolumeData, 1);
            double lcg = helper.InterpolateData(draft, tank.TankLcg, 2);
            double tcg = helper.InterpolateData(draft, tank.TankTcg, 2);
            double vcg = helper.InterpolateData(draft, tank.TankVcg, 2);

            model.Tankname = tank.TankName;
            model.Density = tank.LiquidRho;
            model.Weight = vol * tank.LiquidRho;
            model.CG = new Point(lcg, tcg, vcg);
  
            return model;
        }
        

        #endregion

        #endregion


    }
}
