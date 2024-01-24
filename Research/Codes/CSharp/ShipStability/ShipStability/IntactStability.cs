using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipStability
{
    class IntactStability
    {
        #region public
        public List<Summary> GetIntactSummaryInfo(ShipGeom ship, CrossCurve curveData, List<FixedWeightModel> fixedWt, List<LiquidWeightModel> liquidWt, List<Section> secList, List<Tank> tankList, double rho)
        {
            List<Summary> IntactSummary = new List<Summary>();
            OutputParamHelper outputhelper = new OutputParamHelper();
            //double factor = Math.PI / 180;
            Object shpObj = outputhelper.IntactWtDetails(fixedWt, liquidWt);
            Point trimDraft = outputhelper.GetTrimValue(ship, secList,shpObj.Weight, shpObj.CG.X, rho);
            double trim = trimDraft.Y - trimDraft.X;
            double draftMid = (trimDraft.X + trimDraft.Y) / 2;


            //heel
            double heel = outputhelper.GetHeelangle(shpObj.Weight, shpObj.CG, curveData);

            //GM
            double gm = outputhelper.GetGmfromSmalAngle(shpObj.CG, shpObj.Weight, ship,rho);
            double fsCorrection = this.getfsmcorrection(liquidWt,draftMid,tankList);

            IntactSummary.Add(new Summary("Trim", trim));
            IntactSummary.Add(new Summary("Heel", heel));
            IntactSummary.Add(new Summary("Displacement", shpObj.Weight));
            IntactSummary.Add(new Summary("LCG", shpObj.CG.X));
            IntactSummary.Add(new Summary("TCG", shpObj.CG.Y));
            IntactSummary.Add(new Summary("VCG", shpObj.CG.Z));
            IntactSummary.Add(new Summary("Draft AFT", trimDraft.X));
            IntactSummary.Add(new Summary("Draft MS", draftMid));
            IntactSummary.Add(new Summary("Draft FWD", trimDraft.Y));
            IntactSummary.Add(new Summary("GM(Fluid)", gm - fsCorrection));
            IntactSummary.Add(new Summary("GM(Solid)",gm));
            IntactSummary.Add(new Summary("FS Correction", fsCorrection));
            IntactSummary.Add(new Summary("VCG Correction", shpObj.CG.Z + fsCorrection));
            
            return IntactSummary;
        }

        public List<Mapping> GettTable (List<Point2D> gZcurve, Point critPt, Point fldPt)
        {
            double factor = Math.PI / 180;
            List<Mapping> mapList = new List<Mapping>();
            foreach (Point2D pt in gZcurve)
            {
                Mapping mp = new Mapping(1, pt.X);
                mapList.Add(mp);
                mp = new Mapping(2, pt.Y);
                mapList.Add(mp);
                mp = new Mapping(3, fldPt.Z - Math.Abs(fldPt.Y) * Math.Sin(Math.Abs(pt.X)*factor));
                mapList.Add(mp);
                mp = new Mapping(4, fldPt.Z - Math.Abs(fldPt.Y) * Math.Sin(Math.Abs(pt.X)*factor));
                mapList.Add(mp);
            }

            return mapList;
        }
        #endregion public

        #region private

        private double getfsmcorrection (List<LiquidWeightModel> liqWtList, double meandraft,List<Tank> tankList)
        {
            PublicHelper pHelper = new PublicHelper();
          
            double fsm = 0;
            double wt = 0;

            foreach (LiquidWeightModel liqWt in liqWtList)
            {
                wt = wt + liqWt.Weight;
                fsm = fsm + liqWt.Fsmt; 

             }


            return fsm/wt;

        }


        #endregion private
    }
}
