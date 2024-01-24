using System.Collections.Generic;

namespace ShipStability
{
    class Tank
    {
        #region member variable
        private List<SectionCurve> _tankCurvedata;
        private double _liquidrho;
        private List<Point2D> _voldata;
        private List<Point2D> _massdata;
        private List<Point2D> _percentageOffilling;
        private List<Point2D> _tankLcg;
        private List<Point2D> _tankTcg;
        private List<Point2D> _tankVcg;
        private List<Point2D> _fsmt;
        private List<Point2D> _fsml;
        private List<SectionCurve> _sectankDraftVarea;
        private List<Point> _drftvFsmtList;
        private double _maxWeight;
        private double _permiability;
        string _tankName;
        string _tankCode;



        #endregion member variable

        #region Consturctor
        public Tank()
        {
            this._tankCurvedata = new List<SectionCurve>();
            this._liquidrho = new double();
            this._voldata = new List<Point2D>();
            this._massdata = new List<Point2D>();
            this._percentageOffilling = new List<Point2D>();
            this._tankLcg = new List<Point2D>();
            this._tankTcg = new List<Point2D>();
            this._tankVcg = new List<Point2D>();
            this._fsmt = new List<Point2D>();
            this._fsml = new List<Point2D>();
            this._maxWeight = new double();
            this._permiability = new double();
            this._sectankDraftVarea = new List<SectionCurve>();
            this._drftvFsmtList = new List<Point>();



    }
        #endregion Constructor

        #region properties

        //public string TankCode
        //{
        //    get { return this._tankCode; }
        //    set { this._tankCode = value; }
        //}

        //public string Tankname
        //{
        //    get { return this._tankName; }
        //    set { this._tankName = value;}
        //}
        public double LiquidRho
        {
            get { return this._liquidrho; }
            set { this._liquidrho = value; }
        }

        public List<SectionCurve> TankCurvedata

        {
            get
            {
                return this._tankCurvedata;
            }
            //set
            //{
            //    this._tankCurvedata = value;
            //}
        }
        public List<Point2D> VolumeData
        {
            get
            {
                return this._voldata;
            }
        }
        public List<Point2D> MassData
        {
            get
            {
                return this._massdata;
            }
        }
        public List<Point2D> PerOfFilling
        {
            get
            {
                return this._percentageOffilling;
            }
        }
        public List<Point2D> TankLcg
        {
            get
            {
                return this._tankLcg;
            }
        }
        public List<Point2D> TankTcg
        {
            get
            {
                return this._tankTcg;
            }
        }
        public List<Point2D> TankVcg
        {
            get
            {
                return this._tankVcg;
            }
        }
        public List<Point2D> Fsmt
        {
            get
            {
                return this._fsmt;
            }
        }
        public List<Point2D> Fsml
        {
            get
            {
                return this._fsml;
            }
        }

        public double MaxWeight
        {
            get
            {
                return this._maxWeight;
            }
            set
            {
                this._maxWeight = value;
            }
        }

        public List<SectionCurve> SecTankDraftVArea
        {
            get
            {
                return this._sectankDraftVarea;
            }
        }

        public string TankName 
        { get { return this._tankName; }
            set { this._tankName = value; }
            
        }

        public string TankCode
        {
            get { return this._tankCode; }
            set { this._tankCode = value; }
        }

        public double Permiability
        {
            get { return this._permiability; }
            set { this._permiability = value; }
        }

        public List<Point> DraftvsFsmtList
        {
            get { return this._drftvFsmtList; }
        }
        #endregion properties


        #region public method
        public void AddCurvedata(SectionCurve curve)
        {

            this._tankCurvedata.Add(curve);
        }
      

        public void AddVolData(Point2D pt)
        {

            this._voldata.Add(pt);
        }
        public void AddMassData(Point2D pt)
        {

            this._massdata.Add(pt);
        }
        public void AddPerOfFillData(Point2D pt)
        {

            this._percentageOffilling.Add(pt);
        }

        public void AddTankLcg(Point2D pt)
        {

            this._tankLcg.Add(pt);
        }
        public void AddTankVcg(Point2D pt)
        {

            this._tankVcg.Add(pt);
        }
        public void AddTankTcg(Point2D pt)
        {

            this._tankTcg.Add(pt);
        }
        public void AddFsmt(Point2D pt)
        {

            this._fsmt.Add(pt);
        }
        public void AddFsml(Point2D pt)
        {

            this._fsml.Add(pt);
        }

        public void AddDraftVFsmtListData (Point pt)
        {
            this._drftvFsmtList.Add(pt);
        }

        #region Tank Related

        #endregion Tank Related
        
        public void GetSecTankAreaSounding(Tank tank)
        {
            List<SectionCurve> curveList = tank._tankCurvedata;
            Vector2D vHelper = new Vector2D();
            Point2D maxmin = this.getMaxmin(tank);
            
            List<SectionCurve> modifiedCurve = new List<SectionCurve>();

            foreach (SectionCurve curve in curveList)
            {
                double z0 = maxmin.X;
                double zm = maxmin.Y;
                double dz = 0.1;
                double z = z0 + dz;
                SectionCurve modCurve = new SectionCurve();
                modCurve.Xloc = curve.Xloc;
                while (z < zm + dz)
                {
                    double area = vHelper.GetTankAreaforFixZ(curve, z);
                    modCurve.Addpoints(new Point2D(z, area));
                    z = z + dz;
                }

                modifiedCurve.Add(modCurve);
            }


            this._sectankDraftVarea.RemoveRange(0, _sectankDraftVarea.Count);
            this._sectankDraftVarea.AddRange(modifiedCurve);

        }
        
        public void GetTankSoundings ( Tank tank)
        {
            //this._liquidrho = rho;
            if (this.getstring(tank.TankName) == "S")
            {
                this._tankCurvedata.Reverse();
                List<SectionCurve> curveList = tank.TankCurvedata;
                
                //curveList.Reverse();
                this.fillsoundingForStank(curveList, tank);
                
                //Point2D pt2D = vHelper.GetTankSecPropertyforFixZ(curveList, 8, ref ptList);
                //double vol = vHelper.GetMaxTankVolume(curveList, out cgPt);

            }
            else if(this.getstring(tank.TankName) == "P")
            {
                this.modifyTankdataForPtank(tank);
                List<SectionCurve> curveList = tank.TankCurvedata;

               // curveList.Reverse();
                this.fillsoundingForPtank(curveList, tank);

            }
            else
            {
                this.modifyTankDataforCtank(tank);
                List<SectionCurve> curveList = tank.TankCurvedata;

               // curveList.Reverse();
                this.fillsoundingForCtank(curveList, tank);

            }
            
        }

        public void UpdateTankSounding(List<Tank> tankGeoInfoList, Dictionary<string, List<string>> updatedTankdata)
        {

            foreach(string key in updatedTankdata.Keys)
            {
                foreach(Tank tank in tankGeoInfoList)
                {
                    if (tank.TankName == key)
                    {
                        List<string> subTankName = updatedTankdata[key];
                        List<Tank> subTankList = this.getTankfromTankList(subTankName, tankGeoInfoList);
                        tank.getupdatedTankdata(tank, subTankList);
                        break;
                    }

                }
            }


        }

        public void GetTankFsm(Tank tank, ShipGeom shp)
        {
            Vector2D vHelper = new Vector2D();
            PublicHelper pHelper = new PublicHelper();
            List<Point2D> ptList = tank.VolumeData;
            this._fsml.RemoveRange(0, Fsml.Count);
            this._fsmt.RemoveRange(0, Fsmt.Count);

            for ( int i = 0; i<ptList.Count; i++)
            {
                double z = ptList[i].X;
                double ixx = vHelper.GetTankixxforfixedZ(z, tank);
                ////double bmt = pHelper.InterpolateData(z, ship.DraftBmt,2);
                double disp = pHelper.InterpolateData(z, shp.DraftMass, 2);
                double fsmt = (ixx* this._liquidrho) / disp;
                this.AddFsml(new Point2D(z, 0));
                this.AddFsmt(new Point2D(z, fsmt));

            }
        }

        public void UpdateDensity( double rho)
        {
            this._liquidrho = rho;
        }

        public void UpdateMaxWt (double maxWt)
        {
            this._maxWeight = maxWt;
        }

        public List<Tank> GetDamageTankDetails(List<string> tankNameList, List<Tank> tankList)
        {
            List<Tank> dtankList = new List<Tank>();
            
            foreach ( string str in tankNameList)
            {
                foreach(Tank tank in tankList)
                {
                    if( str == tank.TankName)
                    {
                        dtankList.Add(tank);
                        break; 
                    }
                }
    
            }


            return dtankList;
        }

        

        #endregion public method

        #region private method

        private void fillsoundingForStank (List<SectionCurve> curveList, Tank tank)
        {
            Vector2D vHelper = new Vector2D();
            Point2D maxmin = this.getMaxmin(tank);
            double z0 = maxmin.X;
            double zm = maxmin.Y;
            double dz = 0.1;
            double z = z0 + dz;
            Point pmaxCg = new Point();
            double vol = this._permiability*vHelper.GetMaxTankVolume(curveList, out pmaxCg);


            while (z<zm)
            {
                List<Point2D> cgList = new List<Point2D>();
                
                Point2D pt2D = vHelper.GetTankSecPropertyforFixZ(curveList, z, ref cgList);
                pt2D.Y = pt2D.Y * this._permiability;
                double mass = pt2D.Y * this._liquidrho;
                double perfil = ((pt2D.Y) / vol) * 100;
                this._voldata.Add(pt2D);
                this._massdata.Add(new Point2D(pt2D.X, mass));
                this._percentageOffilling.Add(new Point2D(pt2D.X,perfil));
                this._tankLcg.Add(cgList[0]);
                this._tankTcg.Add(cgList[1]);
                this._tankVcg.Add(cgList[2]);
                this._fsml.Add(new Point2D(cgList[0].X, 0));
                double d = this.getfsmtvalue(z, tank.DraftvsFsmtList);
                this._fsmt.Add(new Point2D(cgList[0].X, d));
                z = z + dz;
            }

            this._voldata.Add(new Point2D(zm, vol));
            this._massdata.Add(new Point2D(zm,vol*this._liquidrho));
            this._tankLcg.Add(new Point2D(zm, pmaxCg.X));
            this._tankTcg.Add(new Point2D(zm, pmaxCg.Y));
            this._tankVcg.Add(new Point2D(zm, pmaxCg.Z));
            this._fsml.Add(new Point2D(zm, 0));
            this._fsmt.Add(new Point2D(zm, 0));
            this._maxWeight = vol * this._liquidrho;
            this._fsml.Add(new Point2D(zm, 0));
            this._fsmt.Add(new Point2D(zm, 0));



        }

        private void fillsoundingForPtank(List<SectionCurve> curveList, Tank tank)
        {
            Vector2D vHelper = new Vector2D();
            Point2D maxmin = this.getMaxmin(tank);
            double z0 = maxmin.X;
            double zm = maxmin.Y;
            double dz = 0.1;
            double z = z0 + dz;
            Point pmaxCg = new Point();
            double vol = this._permiability * vHelper.GetMaxTankVolume(curveList, out pmaxCg);
            pmaxCg.Y = -pmaxCg.Y;

            while (z < zm)
            {
                List<Point2D> cgList = new List<Point2D>();

                Point2D pt2D = vHelper.GetTankSecPropertyforFixZ(curveList, z, ref cgList);
                pt2D.Y = pt2D.Y * this._permiability;
                double mass = pt2D.Y * this._liquidrho;
                double perfil = ((pt2D.Y) / vol) * 100;
                this._voldata.Add(pt2D);
                this._massdata.Add(new Point2D(pt2D.X, mass));
                this._percentageOffilling.Add(new Point2D(pt2D.X, perfil));
                this._tankLcg.Add(cgList[0]);
                this._tankTcg.Add(new Point2D (cgList[1].X,-cgList[1].Y));
                this._tankVcg.Add(cgList[2]);
                this._fsml.Add(new Point2D(cgList[0].X, 0));
                double d = this.getfsmtvalue(z, tank.DraftvsFsmtList);
                this._fsmt.Add(new Point2D(cgList[0].X, d));
                z = z + dz;
            }

            this._voldata.Add(new Point2D(zm, vol));
            this._massdata.Add(new Point2D(zm, vol * this._liquidrho));
            this._tankLcg.Add(new Point2D(zm, pmaxCg.X));
            this._tankTcg.Add(new Point2D(zm, pmaxCg.Y));
            this._tankVcg.Add(new Point2D(zm, pmaxCg.Z));
            this._maxWeight = vol * this._liquidrho;
            this._fsml.Add(new Point2D(zm, 0));
            this._fsmt.Add(new Point2D(zm, 0));



        }

        private void fillsoundingForCtank(List<SectionCurve> curveList, Tank tank)
        {

            Vector2D vHelper = new Vector2D();
            Point2D maxmin = this.getMaxmin(tank);
            List<LiquidWeightModel> liqModel = new List<LiquidWeightModel>();
            double z0 = maxmin.X;
            double zm = maxmin.Y;
            double dz = 0.1;
            double z = z0 + dz;
            Point pmaxCg = new Point();
            double vol =this._permiability*vHelper.GetMaxTankVolume(curveList, out pmaxCg);
            while (z < zm)
            {
                List<Point2D> cgList = new List<Point2D>();
                
                Point2D pt2D = vHelper.GetTankSecPropertyforFixZ(curveList, z, ref cgList);
                pt2D.Y = pt2D.Y * this._permiability;
                double mass = pt2D.Y * this._liquidrho;
                double perfil = ((pt2D.Y) / vol) * 100;
                this._voldata.Add(pt2D);
                this._massdata.Add(new Point2D(pt2D.X, mass));
                this._percentageOffilling.Add(new Point2D(pt2D.X, perfil));
                this._tankLcg.Add(cgList[0]);
                this._tankTcg.Add(new Point2D(cgList[1].X,0));
                this._tankVcg.Add(cgList[2]);
                this._fsml.Add(new Point2D(cgList[0].X, 0));
                double d = this.getfsmtvalue(z, tank.DraftvsFsmtList);
                this._fsmt.Add(new Point2D(cgList[0].X, d));
                z = z + dz;
            }

            this._voldata.Add(new Point2D(zm, vol));
            this._massdata.Add(new Point2D(zm, vol * this._liquidrho));
            this._tankLcg.Add(new Point2D(zm, pmaxCg.X));
            this._tankTcg.Add(new Point2D(zm, 0));
            this._tankVcg.Add(new Point2D(zm, pmaxCg.Z));
            this._maxWeight = vol * this._liquidrho;
            this._fsml.Add(new Point2D(zm, 0));
            this._fsmt.Add(new Point2D(zm, 0));




        }

        private void modifyTankDataforCtank (Tank tank)
        {
            List<SectionCurve> curveList = new List<SectionCurve>();
            foreach (SectionCurve curve in tank.TankCurvedata)
            {
                SectionCurve secCurve = new SectionCurve();
                secCurve.Xloc = curve.Xloc;
                foreach (Point2D pt2D in curve.SectionPts)
                {
                    secCurve.Addpoints(new Point2D ( pt2D.X,pt2D.Y));
                }

                int m = curve.SectionPts.Count;

                while (m > 0)
                {
                    Point2D pt = new Point2D();
                    pt.X = -curve.SectionPts[m - 1].X;
                    pt.Y = curve.SectionPts[m - 1].Y;
                    secCurve.Addpoints(pt);
                    m = m - 1;
                }

                curveList.Add(secCurve);
            }
            this._tankCurvedata.RemoveRange(0, TankCurvedata.Count);
            this._tankCurvedata.AddRange(curveList);
            this._tankCurvedata.Reverse();


        }

        private void modifyTankdataForPtank (Tank tank)
        {
            List<SectionCurve> curveList = new List<SectionCurve>();
            foreach (SectionCurve curve in TankCurvedata)
            {
                SectionCurve modyfycurve = new SectionCurve();
                modyfycurve.Xloc = curve.Xloc;
                foreach(Point2D pt2D in curve.SectionPts)
                {
                    modyfycurve.Addpoints(new Point2D(-pt2D.X, pt2D.Y));
                }
                //this._tankCurvedata.Remove(curve);
                curveList.Add(modyfycurve);
            }

            this._tankCurvedata.RemoveRange(0, TankCurvedata.Count);
            this._tankCurvedata.AddRange(curveList);
            this._tankCurvedata.Reverse();

        }

        private string getstring (string s)
        {
            string[] st = s.Split('.');

            string str = st[st.Length - 1];

            return str;
        }

        private Point2D getMaxmin (Tank tank)
        {
            Point2D pt = new Point2D();
            List<double> zList = new List<double>();
            foreach (SectionCurve curve in tank.TankCurvedata)
            {
                foreach(Point2D pt2D in curve.SectionPts)
                {
                    zList.Add(pt2D.Y);
                }
            }

            pt = this.maximumMinimumElementArray(zList);
            return pt;
        }

        private Point2D maximumMinimumElementArray(List<double> ptList)
        {
            
            double[] array = ptList.ToArray();
            //int[] array = { 10, 30, 40, 100, 170, 50, 20, 60 };
            double max = array[0];
            double min = array[0];
            for (int i = 0; i <= array.Length - 1; i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                }
                if (array[i] < min)
                {
                    min = array[i];
                }
            }

            Point2D pt = new Point2D(min, max);

            return pt;
           
        }

        private double getfsmtvalue ( double z, List<Point> ptList)
        {
            PublicHelper outhelp = new PublicHelper();

            
            if(ptList.Count == 0)
            {
                return 0;
            }

            List<Point2D> pt2dList = this.get2DfsmData(ptList);
            double d = outhelp.InterpolateData(z, pt2dList, 2);
            return d;
        }

        private List<Point2D> get2DfsmData( List<Point> ptList)
        {
            List<Point2D> pt2dList = new List<Point2D>();
            foreach (Point pt in ptList)
            {
                Point2D pt2D = new Point2D(pt.X, pt.Z);
                pt2dList.Add(pt2D);
            }

            return pt2dList;
        }

        #region for update tank soundings

        private void getupdatedTankdata (Tank tank, List<Tank> subtankList)
        {
            List<Object> objList = new List<Object>();
            PublicHelper pHelper = new PublicHelper();
            Point2D minmax = this.getMaxmin(tank);
            double zmax = minmax.Y;
            double zin = 0.1;
            double z0 = minmax.X + zin;
            List<Point2D> volData = new List<Point2D>();
            List<Point2D> lcgdata = new List<Point2D>();
            List<Point2D> tcgdata = new List<Point2D>();
            List<Point2D> vcgdata = new List<Point2D>();
            while (z0 < zmax + zin)
            {
                Object obj2 = this.getsubtankVol(subtankList, z0);
                double vol = pHelper.InterpolateData(z0, tank.VolumeData, 2);
                double x = pHelper.InterpolateData(z0, tank.TankLcg, 2);
                double y = pHelper.InterpolateData(z0, tank.TankTcg, 2);
                double z = pHelper.InterpolateData(z0, tank.TankVcg, 2);

                double correctedVol = vol - obj2.Weight;
                double correctedx = (vol * x - obj2.Weight * obj2.CG.X)/ correctedVol;
                double correctedy = (vol * y - obj2.Weight * obj2.CG.Y) / correctedVol;
                double correctedz = (vol * z - obj2.Weight * obj2.CG.Z) / correctedVol;

                volData.Add(new Point2D (z0,correctedVol));
                lcgdata.Add(new Point2D(z0, correctedx));
                tcgdata.Add(new Point2D(z0, correctedy));
                vcgdata.Add(new Point2D(z0, correctedz));

                z0 = z0 + zin;


            }


            this._voldata.RemoveRange(0, VolumeData.Count);
            this._voldata.AddRange(volData);
            this._tankLcg.RemoveRange(0, TankLcg.Count);
            this._tankLcg.AddRange(lcgdata);
            this._tankTcg.RemoveRange(0, TankTcg.Count);
            this._tankTcg.AddRange(tcgdata);
            this._tankVcg.RemoveRange(0, TankVcg.Count);
            this._tankVcg.AddRange(vcgdata);

        }


        private Object getsubtankVol (List<Tank> subtanks , double draftLine)
        {
            PublicHelper pHelper = new PublicHelper();
            double vol = 0;
            List<Object> objList = new List<Object>();
            foreach(Tank tank in subtanks)
            {
                List<Point2D> volData = tank.VolumeData;
                List<Point2D> lcgdata = tank.TankLcg;
                List<Point2D> tcgdata = tank.TankTcg;
                List<Point2D> vcgdata = tank.TankVcg;
                double v = pHelper.InterpolateData(draftLine, volData, 2);
                double x = pHelper.InterpolateData(draftLine, lcgdata, 2);
                double y = pHelper.InterpolateData(draftLine, tcgdata, 2);
                double z = pHelper.InterpolateData(draftLine, vcgdata, 2);
                Object obj = new Object();
                obj.Weight = v;
                obj.CG = new Point(x, y, z);
                objList.Add(obj);
                vol = vol + v;

            }

            Object objFinal = pHelper.GetCentroid(objList);

            
            return objFinal;
        }

        private List<Tank> getTankfromTankList ( List<string> tankName, List<Tank> tankList)
        {
            List<Tank> subTankInfo = new List<Tank>();

            foreach (string tname in tankName)
            {
                foreach(Tank tank in tankList)
                {
                    if(tname == tank.TankName)
                    {
                        subTankInfo.Add(tank);
                        break;
                    }
                }
            }


            return subTankInfo;
        }
        #endregion update tank soundings


        #endregion private method


    }
}
