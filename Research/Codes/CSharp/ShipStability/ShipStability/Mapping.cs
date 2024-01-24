using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class Mapping
    {
        #region Member Variable
        private int _indexvalue;
        private double _dvalue;
        private string _tankname;
        #endregion Member Variable

        #region Constructor

        public Mapping()
        {
            this._indexvalue = new int();
            this._dvalue = new double();
            
        }

        public Mapping ( int inDexValue , double values)
        {
            this._indexvalue = inDexValue;
            this._dvalue = values;
            
        }

        #endregion Constructor

        #region Properties

        public int IndexValue
        {
            get
            {
                return this._indexvalue;
            }

            set
            {
                this._indexvalue = value;
            }
        }

        public double Xvalues
        {
            get
            {
                return this._dvalue;
            }

            set
            {
                this._dvalue = value;
            }
        }

        public string TankName
        { 
            get
            {
                return this._tankname;
            }
            set
            {
                this._tankname = value;
            }


        
        }

        #endregion Properties



    }
}
