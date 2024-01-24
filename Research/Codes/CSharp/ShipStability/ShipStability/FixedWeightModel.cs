using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipStability
{
    class FixedWeightModel:Object
    {
        #region member variables

        private string _objectname;
 

        #endregion member variables

        #region Constructor
        public FixedWeightModel()
        {
            
            
        }

        #endregion

        #region properties

        public string ObjectName
        {
            get
            {
                return this._objectname;
            }
            set
            {
                this._objectname = value;
            }
        }
        

        #endregion properties
    }
}
