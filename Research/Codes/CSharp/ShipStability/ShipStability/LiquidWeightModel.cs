using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShipStability
{
    class LiquidWeightModel:Object
    {
        #region member variables
        private string _tankName;
        private double _density;

        #endregion member variables

        #region Constructor
        public LiquidWeightModel()
        {
            this._density = new double();
        }
        #endregion

        #region properties
        public string Tankname
        {
            get
            {
                return this._tankName;
            }

            set { this._tankName = value; }
        }

        public double Density
        {
            get { return this._density; }
            set { this._density = value; }
        }
        

        #endregion properties

        #region private

        #endregion private

        #region public

        public List<LiquidWeightModel> GetLiquidWt ( List<Mapping> mappingList , List<Tank> tankList)
        {
            
            PublicHelper helper = new PublicHelper();
            WriteOutput wrhelper = new WriteOutput();
            List<LiquidWeightModel> liqidWtList = new List<LiquidWeightModel>();
            foreach ( Mapping map in mappingList)
            {
                foreach (Tank tank in tankList)
                {
                    if(map.TankName == tank.TankName)
                    {
                        LiquidWeightModel model = new LiquidWeightModel();
                        model.Tankname = tank.TankName;
                        model.Weight = (tank.MaxWeight * map.Xvalues) / 100;
                        double draft = helper.InterpolateData(model.Weight, tank.MassData, 1);
                        double lcg = helper.InterpolateData(draft, tank.TankLcg, 2);
                        double tcg = helper.InterpolateData(draft, tank.TankTcg, 2);
                        double vcg = helper.InterpolateData(draft, tank.TankVcg, 2);
                        double fsmt = helper.InterpolateData(draft, tank.Fsmt, 2);
                        model.CG = new Point(lcg, tcg, vcg);
                        
                        model.Density = tank.LiquidRho;
                        model.Fsmt = fsmt;
                        liqidWtList.Add(model);
                        
                        break;
                    }
                }
            }

            wrhelper.WriteLiquidWtSummary(liqidWtList);
            return liqidWtList;
        }

   

        #endregion public


        #region private
        

        #endregion
    }
}
