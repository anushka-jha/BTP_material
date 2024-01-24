using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipStability
{
    class LongitudinalCalculation
    {
        #region member

        private int _frameno;
        private double _framespacing;
        private double _shearforceval;
        private double _bendingmomentval;
        private double _shearforceper;
        private double _bendingmomper;

        #endregion


        #region constructor

        public LongitudinalCalculation()
        {
            this._frameno = new int();
            this._framespacing = new double();
            this._shearforceval = new double();
            this._bendingmomentval = new double();
            this._shearforceper = new double();
            this._bendingmomper = new double();
        }


        #endregion

        #region Properties

        public int FrameNo
        {
            get { return this._frameno; }
            set { this._frameno = value; }
        }

        public double FrameSpacing
        {
            get { return this._framespacing; }
            set { this._framespacing = value; }
        }

        public double ShearAtFrame
        {
            get { return this._shearforceval; }
            set { this._shearforceval = value; }
        }

        public double BendingAtFrame
        {
            get { return this._bendingmomentval; }
            set { this._bendingmomentval = value; }
        }

        public double ShearPerAtFrame
        {
            get { return this._shearforceper; }
            set { this._shearforceper = value; }
        }

        public double BendingPerAtFrame
        {
            get { return this._bendingmomper; }
            set { this._bendingmomper = value; }
        }

        #endregion






        #region Public Method
        public List<Point2D> GetBendingMomentInfo (ShipGeom shp, List<Point2D> solidWt, List<Tank> tankGeoInfoList, List<Mapping> mList, List<Section> secList, double rho, ref List<Point2D> shearforce)
        {
            WriteOutput wrout = new WriteOutput();
            PublicHelper pHelper = new PublicHelper();
            shearforce = new List<Point2D>();
            OutputParamHelper outHelper = new OutputParamHelper();
            shp.UpdateLoadDistribution(shp.HullWtDistribution, solidWt, tankGeoInfoList, mList);
            Point2D pt = pHelper.GetCgfromPoint2DList(shp.WtCurve);
            Point pt3D = outHelper.GetTrimValue(shp, secList, pt.X, pt.Y, rho);
            List<Point2D> loadList = outHelper.GetLoadCurve(shp.WtCurve, shp.BuoyancyCurve);
            wrout.WriteLongStData(loadList, "LoadDist.dat");
            shearforce = outHelper.GetShearForce(loadList);
            List<Point2D> bendingMoment = outHelper.GetBendingMoment(shearforce);



            //List<Point2D> loadlist = outHelper.GetLoadCurve(shp.WtCurve, shp.BuoyancyCurve);
            //shearforce = outHelper.GetShearForce(loadlist);
            //List<Point2D> bendingMoment = outHelper.GetBendingMoment(shearforce);

            return bendingMoment;

        }

        public LongitudinalCalculation GetLongitudinaldata(Point2D maxInfo, Mapping map, List<Point2D> shearF, List<Point2D> bendingM)
        {
            LongitudinalCalculation ldata = new LongitudinalCalculation();
            Point2D dataforShearF = this.getDataforForce(maxInfo.X, shearF, map);
            Point2D dataforbendingM = this.getDataforForce(maxInfo.Y, bendingM, map);
            ldata.ShearAtFrame = dataforShearF.X;
            ldata.ShearPerAtFrame = dataforShearF.Y;
            ldata.BendingAtFrame = dataforbendingM.X;
            ldata.BendingPerAtFrame = dataforbendingM.Y;

            return ldata;
        }

        #endregion

        #region private method

        private Point2D getDataforForce(double maxdata, List<Point2D> forceData, Mapping map)
        {
            Vector2D vHelper = new Vector2D();
            PublicHelper helper = new PublicHelper();
            Point2D pt = new Point2D();
            List<Vector2D> vList = vHelper.GetVectorfromPointList(forceData);
            foreach (Vector2D v in vList)
            {
                if ((v.StPt.X <= map.Xvalues) && (v.EndPt.X > map.Xvalues))
                {
                    pt.X =  helper.SolveLinearEquation(v.StPt, v.EndPt,map.Xvalues);
                    break;
                }
            }

            pt.Y = (pt.X / maxdata) * 100;



            return pt;
        }

        #endregion


    }

}
