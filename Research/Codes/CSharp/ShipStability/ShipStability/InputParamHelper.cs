using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace ShipStability
{
    class InputParamHelper
    {
        #region Constructor

        public InputParamHelper()
        {
        }
        #endregion Constructor

        #region public method

        public ShipGeom ReaddShipgeomdata( )
        {
            ShipGeom shgeom = new ShipGeom();
            string path = @"D:\Ranadev\Research\Codes\Input\Hull Hydrostatics.dat";
            string[] lines = File.ReadAllLines(path);
            int num=Convert.ToInt32(lines[1]);
            for( int i=2;i<num+2;i++)
            {
                string[] substring = lines[i].Split(',');

                Point2D draftAp = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[2]));
                Point2D draftFp = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[3]));
                Point2D draftVol = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[4]));
                Point2D draftMass = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[5]));
                Point2D draftWetArea = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[6]));
                Point2D draftLcb = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[7]));
                Point2D draftTcb = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[8]));
                Point2D draftVcb = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[9]));
                Point2D draftWpa = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[10]));
                Point2D draftLcf = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[11]));
                Point2D draftIxx0 = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[12]));
                Point2D draftIxxg = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[13]));
                Point2D draftIyy0 = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[14]));
                Point2D draftBml = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[15]));
                Point2D draftBmt = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[16]));
                Point2D draftKml = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[17]));
                Point2D draftKmt = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[18]));
                Point2D draftTpc = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[19]));
                Point2D draftMct = new Point2D(Convert.ToDouble(substring[0]), Convert.ToDouble(substring[20]));


                shgeom.AdddraftAp(draftAp);
                shgeom.AdddraftFp(draftFp);
                shgeom.AdddraftVol(draftVol);
                shgeom.AdddraftMass(draftMass);
                shgeom.AdddraftWetarea(draftWetArea);
                shgeom.AdddraftLcb(draftLcb);
                shgeom.AdddraftTcb(draftTcb);
                shgeom.AdddraftVcb(draftVcb);
                shgeom.AdddraftWpa(draftWpa);
                shgeom.AdddraftLcf(draftLcf);
                shgeom.AdddraftIxx0(draftIxx0);
                shgeom.AdddraftIxxg(draftIxxg);
                shgeom.AdddraftIyy0(draftIyy0);
                shgeom.AdddraftBml(draftBml);
                shgeom.AdddraftBmt(draftBmt);
                shgeom.AdddraftKml(draftKml);
                shgeom.AdddraftKmt(draftKmt);
                shgeom.AdddraftTpc(draftTpc);
                shgeom.AdddraftMct(draftMct);
            }
            return shgeom;

        }
        public List<Section> ReadSectionData()
        {
            List<Section> sectiondata = new List<Section>();
            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//hullSection.dat");

            //string path = @"D:\Ranadev\Research\Codes\Input\Input30_05_2023.dat";
            //string[] lines = File.ReadAllLines(path);
            string st = sr.ReadLine();
            int num1 = int.Parse(st);
            st = sr.ReadLine();
            int num2 = int.Parse(st); ;
            st = sr.ReadLine();
            st = sr.ReadLine();
            //int length = lines.Length;
            int i = 0;
            int k = 0;
            

            while ( i<num1 )
            {
                Section sec = new Section();
                SectionCurve curve = new SectionCurve();
                //sec.Adddraft(0);
                //sec.AddArea(0);
                //sec.AddSecXMom(new Point2D(0,0));
                //curve.Addpoints(new Point2D(0, 0));
                for (int j = 0; j<num2; j++)
                {
                    st = sr.ReadLine();
                    string[] substring = st.Split(',');
                    double draft = double.Parse(substring[0]);
                    double yvalue = double.Parse(substring[1]);
                    double sarea = double.Parse(substring[2]);
                    double xMom = double.Parse(substring[2]);
                    Point2D pt = new Point2D(draft, yvalue);
                    Point2D xmPt = new Point2D(draft, xMom);
                    sec.Adddraft(draft);
                    sec.AddArea(sarea);
                    sec.AddSecXMom(xmPt);
                    curve.Addpoints(pt);



                }
                sec.SecCurve = curve;
                sec.SectionNo = k;
                k++;
                i++;
                sectiondata.Add(sec);
                st = sr.ReadLine();
                st = sr.ReadLine();
                st = sr.ReadLine();
                st = sr.ReadLine();
            }
            string path2 = @"D:\Ranadev\Research\Codes\Input\Xcoordinates.dat";
            string[] lines2 = File.ReadAllLines(path2);

            for (int m = 0; m< sectiondata.Count; m++)
            {
                sectiondata[m].Xlocation = Convert.ToDouble(lines2[m]);
            }
            return sectiondata;

        }
        //public CrossCurve ReadKNdata()
        //{
        //    CrossCurve Kndata = new CrossCurve();
        //    string path = @"D:\Ranadev\Research\Codes\Input\Res_KN.dat";
        //    string[] lines = File.ReadAllLines(path);
        //    int num1 = Convert.ToInt32(lines[0]) ;
        //    int i = 3;
        //    while (i < lines.Length)
        //    {
        //        int j = i;
        //        //list of point 2d to add all the points and then add them in the dictionary directly.
        //        List<Point2D> crosscurve=new List<Point2D>();
        //        List<Point2D> knVdraft = new List<Point2D>();
        //        List<Point2D> knVwpa = new List<Point2D>();
        //        List<Point2D> knVlcb = new List<Point2D>();
        //        List<Point2D> knVvcb = new List<Point2D>();
        //        List<Point2D> knVtcb = new List<Point2D>();
        //        List<Point2D> knVvol = new List<Point2D>();
        //        List<Point2D> knVwetarea = new List<Point2D>();
        //        double heel = new double();

        //        while (j < num1 + i)
        //        {   
        //            string[] substring = lines[i + j].Split(',');
        //            heel = Convert.ToDouble(substring[1]);
        //            Point2D crcurve1 = new Point2D(Convert.ToDouble(substring[2]), Convert.ToDouble(substring[8]));
        //            crosscurve.Add(crcurve1);
        //            Point2D kndraft1 = new Point2D(Convert.ToDouble(substring[2]), Convert.ToDouble(substring[0]));
        //            knVdraft.Add(kndraft1);
        //            Point2D knwpa1 = new Point2D(Convert.ToDouble(substring[2]), Convert.ToDouble(substring[3]));
        //            knVwpa.Add(knwpa1);
        //            Point2D knlcb1 = new Point2D(Convert.ToDouble(substring[2]), Convert.ToDouble(substring[4]));
        //            knVlcb.Add(knlcb1);
        //            Point2D kntcb1 = new Point2D(Convert.ToDouble(substring[2]), Convert.ToDouble(substring[5]));
        //            knVtcb.Add(kntcb1);
        //            Point2D knvcb1 = new Point2D(Convert.ToDouble(substring[2]), Convert.ToDouble(substring[6]));
        //            knVvcb.Add(knvcb1);
        //            Point2D knvol1 = new Point2D(Convert.ToDouble(substring[2]), Convert.ToDouble(substring[7]));
        //            knVvol.Add(knvol1); 
        //            Point2D knwetarea1 = new Point2D(Convert.ToDouble(substring[2]), Convert.ToDouble(substring[9]));
        //            knVwetarea.Add(knwetarea1);

        //        }

        //        // dictionaries
        //        Dictionary<double, List<Point2D>> crcurve = new Dictionary<double, List<Point2D>>();
        //        Dictionary<double, List<Point2D>> knDisp = new Dictionary<double, List<Point2D>>();
        //        Dictionary<double, List<Point2D>> knwpa = new Dictionary<double, List<Point2D>>();
        //        Dictionary<double, List<Point2D>> knlcb = new Dictionary<double, List<Point2D>>();
        //        Dictionary<double, List<Point2D>> kntcb = new Dictionary<double, List<Point2D>>();
        //        Dictionary<double, List<Point2D>> knvcb = new Dictionary<double, List<Point2D>>();
        //        Dictionary<double, List<Point2D>> knvol = new Dictionary<double, List<Point2D>>();
        //        Dictionary<double, List<Point2D>> knwetarea = new Dictionary<double, List<Point2D>>();

        //        //adding points to the dictionaries
        //        crcurve.Add(heel, crosscurve);
        //        knDisp.Add(heel, knVdraft);
        //        knwpa.Add(heel, knVwpa);
        //        knlcb.Add(heel, knVlcb);
        //        kntcb.Add(heel, knVtcb);
        //        knvcb.Add(heel, knVvcb);
        //        knvol.Add(heel, knVvol);
        //        knwetarea.Add(heel, knVwetarea);


        //        Kndata.AddCrossCurve(crcurve); 
        //        Kndata.AddKnVDisp(knDisp);
        //        Kndata.AddKnVWpa(knwpa);
        //        Kndata.AddKnVLcb(knlcb); 
        //        Kndata.AddKnVTcb(kntcb);
        //        Kndata.AddKnVVcb(knvcb);
        //        Kndata.AddKnVVol(knvol);
        //        Kndata.AddKnVWetArea(knwetarea);

        //        i = j;
        //    }

        //    return Kndata;
        //}

        public List<Section> ReadSectionCurve()
        {
            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//SectioLine.dat");
            double feetToM = 0.3048;
            List<Section> secCurve = new List<Section>();
            string st = sr.ReadLine();
            int n = int.Parse(st);
            for (int i = 0; i<n; i++)
            {
                Section sec = new Section();
                st = sr.ReadLine();
                string[] str = st.Split(',');
                sec.Xlocation = double.Parse(str[0]) * feetToM;
                int m = int.Parse(str[1]);
                sec.SecCurve.Xloc = sec.Xlocation;
                for (int j = 0; j<m-1;j++)
                {
                    st = sr.ReadLine();
                    str = st.Split(',');
                    Point2D pt = new Point2D ( feetToM*double.Parse(str[1]), 2*feetToM*double.Parse(str[0]));
                    sec.SecCurve.Addpoints(pt);

                }
                st = sr.ReadLine();
                secCurve.Add(sec);

            }


            return secCurve;
        }

        public List<Mapping> ReadFrameInput()
        {
            List<Mapping> mpList = new List<Mapping>();
            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//Frames.dat");
            string st = sr.ReadLine();
            while (st != "$")
            {
                string[] str = st.Split(",");
                Mapping map = new Mapping(int.Parse(str[0]), double.Parse(str[1]));
                mpList.Add(map);
                st = sr.ReadLine();
            }


            return mpList;
        }

        public List<Vector2D> ReadHullDistData()
        {
            List<Vector2D> hulldata = new List<Vector2D>();
            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//Hull_Dist.dat");

            string st = sr.ReadLine();
            while(st!="$")
            {
                string[] str = st.Split(',');
                Point2D location1 = new Point2D (double.Parse(str[0]), double.Parse(str[2]));
                Point2D location2 = new Point2D(double.Parse(str[1]), double.Parse(str[3]));
                Vector2D v = new Vector2D(location1, location2);
                hulldata.Add(v);
                    
                st = sr.ReadLine();
            }
            
             return hulldata;
        }

        public List<Mapping> ReadLoadingData()
        {
            List<Mapping> loadmap = new List<Mapping>();
            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//Loading.dat");
            string st = sr.ReadLine();
            while (st!="$")
            {
                Mapping map = new Mapping();
                map.TankName = st;
                st = sr.ReadLine();
                map.IndexValue = 4;
                map.Xvalues = double.Parse(st);
                st = sr.ReadLine();
                loadmap.Add(map);
            }


            return loadmap;
        }

        public List<Tank> GetTankGeom()
        {
            List<Tank> tankList = new List<Tank>();
            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//MSG tank data.dat");
            string st = sr.ReadLine();
            string[] str = st.Split(',');
            double feetToM = 0.3048;
            while(str[0]!="$")
            {
                Tank tank = new Tank();
                tank.TankCode = str[0];
                tank.TankName = str[1];
                st = sr.ReadLine();
                tank.Permiability = double.Parse(st);
                st = sr.ReadLine();
                tank.LiquidRho = (double.Parse(st))/(1000);
                st = sr.ReadLine();
                int row = int.Parse(st);
                for (int i = 0; i<row; i++)
                {
                    SectionCurve curve = new SectionCurve();
                    st = sr.ReadLine();
                    str = st.Split(',');
                    double xloc = double.Parse(str[0]);
                    curve.Xloc = (-1) * feetToM * xloc;
                    int col = int.Parse(str[1]);
                    for(int j = 0; j<col; j++)
                    {
                        st = sr.ReadLine();
                        str = st.Split(',');
                        Point2D pt = new Point2D((double.Parse(str[0])* feetToM), (double.Parse(str[1]))* feetToM);
                        curve.Addpoints(pt);
                    }

                    tank.AddCurvedata(curve);                    

                }

                st = sr.ReadLine();
                while(st == "")
                {
                    st = sr.ReadLine();
                }

                str = st.Split(',');

                tankList.Add(tank);
            }


            return tankList;
        }

        public void GetTankFSMValue ( ref List<Tank> tankList)
        {
            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//FSMData.dat");
            string st = sr.ReadLine();
            
            while (st != "$")
            {
                int k = 0;
                foreach(Tank tank in tankList)
                {
                    k++;
                    if(this.getmatchedstring(st,tank.TankName))
                    {
                        st = sr.ReadLine();
                        int ntot = int.Parse(st);
                        for( int i = 0; i<ntot; i++)
                        {
                            st = sr.ReadLine();
                            string[] str = st.Split(',');
                            double draft = double.Parse(str[0]);
                            double perfil = double.Parse(str[1]);
                            double fsmt = double.Parse(str[2]);
                            Point pt = new Point(draft, perfil, fsmt);
                            tank.AddDraftVFsmtListData(pt);
                        }
                        
                        break; 
                    }
                }

                st = sr.ReadLine();
                while (st == "")
                {
                    st = sr.ReadLine();
                }
            }


        }

        public List<Tank> GetPermiability()
        {
            List<Tank> tankList = new List<Tank>();
            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//permiability.dat");
            string st = sr.ReadLine();

            while (st!=null)
            {
                Tank tank = new Tank();
                string[] str = st.Split(',');
                tank.TankName = str[0];
                tank.Permiability = double.Parse(str[1]);
                tank.LiquidRho = double.Parse(str[2]);
                tankList.Add(tank);
                st = sr.ReadLine();

            }

            return tankList;
        }

        public List<FixedWeightModel> ReadFixedWeight(string filename)
        {
            List<FixedWeightModel> wtLoad = new List<FixedWeightModel>();
            string s = "D://Ranadev//Research//Codes//Input//" + filename;
            StreamReader sr = new StreamReader(s);
            string st = sr.ReadLine();
            while (st != "$")
            {
                FixedWeightModel model = new FixedWeightModel();
                model.ObjectName = st;
                st = sr.ReadLine();
                model.Weight = double.Parse(st);
                st = sr.ReadLine();
                string[] str = st.Split(',');
                Point pt = new Point(double.Parse(str[0]), double.Parse(str[1]), double.Parse(str[2]));
                model.CG = pt;
                wtLoad.Add(model);
                st = sr.ReadLine();
                
            }



            return wtLoad;
        }

        public List<string> ReadDamagedTank()
        {
            List<string> strList = new List<string>();

            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//damageTank.dat");
            string st = sr.ReadLine();
            while(st!= null)
            {
                strList.Add(st);
                st = sr.ReadLine();
            }

           


            return strList;
        }

        public Dictionary<string, List<string>> ReadSubstractedTankInfo()
        {
            Dictionary<string, List<string>> subtankList = new Dictionary<string, List<string>>();
            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//SubstractTanks.dat");
            string st = sr.ReadLine();
            while(st!="$")
            {
                string key = st;
                st = sr.ReadLine();
                int k = int.Parse(st);
                List<string> subTankLists = new List<string>();
                for(int i = 0; i<k;i++)
                {
                    st = sr.ReadLine();
                    subTankLists.Add(st);
                }

                subtankList.Add(key, subTankLists);
                st = sr.ReadLine();
            }



            return subtankList;
        }
        public List<FixedWeightModel> ReadLateralAreaInfo()
        {
            List<FixedWeightModel> superstructureData = new List<FixedWeightModel>();
            StreamReader sr = new StreamReader("D://Ranadev//Research//Codes//Input//windheel.dat");
            string st = sr.ReadLine();
            st = sr.ReadLine();
            while (st != "$")
            {
                string[] str = st.Split(',');

                FixedWeightModel data = new FixedWeightModel();
                data.ObjectName = str[0];
                data.Weight = double.Parse(str[1]);
                data.CG = new Point(double.Parse(str[2]), double.Parse(str[3]), double.Parse(str[4]));
                superstructureData.Add(data);
                st = sr.ReadLine();
            }

            return superstructureData;
        }
        

        


        public List<Tank> ReadShipTankData()
        {
            if(File.Exists("tank.txt"))
            {
                List<Tank> TankDataList = new List<Tank>();


                List<string> TankGeoLines = null;
                List<List<string>> TankData = new List<List<string>>();
                int linecount = 1;
                var fileLineCount = File.ReadLines("tank.txt").Count();
                foreach (var lines in File.ReadLines("tank.txt"))
                {
                    
                    if (lines.Equals("*PART*"))
                    {
                        if (TankGeoLines != null)
                            TankData.Add(TankGeoLines);
                        TankGeoLines = new List<string>();
                        TankGeoLines.Add(lines);
                        linecount++;
                    }
                    else if (TankGeoLines != null)
                    {
                        TankGeoLines.Add(lines);
                        if (linecount == fileLineCount)
                        {
                            TankData.Add(TankGeoLines);
                        }
                        else
                            linecount++;
                    }
                    else
                    {
                        //Invalid file
                        return null;
                    }
                    
                }



                if (TankData != null)
                {
                    foreach (var tankinfo in TankData)
                    {
                        TankDataList.Add(getTankGeo(tankinfo));
                    }
                    return TankDataList;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        

        #endregion public method

        private Tank getTankGeo(List<string> TankInfo)
        {

            Tank Tinfo = new Tank();
            int readerIndex = 0;
            Tinfo.TankName = TankInfo[readerIndex + 1];
            readerIndex = TankInfo.IndexOf("*DEFINITION*");
            Tinfo.TankCode = TankInfo[readerIndex + 1];
            readerIndex++;
            int xcordinatesCount = Int32.Parse(TankInfo[readerIndex + 1]);
            readerIndex++;
            List<SectionCurve> sectionCurves = new List<SectionCurve>();
            for (int x = 0; x < xcordinatesCount; x++)
            {
                SectionCurve sectionCurve = new SectionCurve();
                var temp = TankInfo[readerIndex + 1];
                sectionCurve.Xloc = (-0.3048)*double.Parse(temp.Split(',').First());
                int yzcordinatesCount = Int32.Parse(temp.Split(',').Last().Trim());
                readerIndex++;
                List<Point2D> point2Ds = new List<Point2D>();
                for(int y=0; y< yzcordinatesCount; y++)
                {
                    var yzTemp = TankInfo[readerIndex + 1];
                    Point2D point2D = new Point2D();
                    point2D.X = (0.3048)*double.Parse(yzTemp.Split(',').First());
                    point2D.Y = (0.3048)*double.Parse(yzTemp.Split(',')[1].Trim());
                    point2Ds.Add(point2D);
                    sectionCurve.Addpoints(point2D);
                    readerIndex++;
                }
                //sectionCurve.SectionPts = point2Ds;
                //sectionCurves.Add(sectionCurve);
                Tinfo.AddCurvedata(sectionCurve);
            }
            
            return Tinfo;
        }

        private bool getmatchedstring ( string s1, string s2)
        {
            int k = 0;
            byte[] ASCIIValues1 = Encoding.ASCII.GetBytes(s1.Trim());
            byte[] ASCIIValues2 = Encoding.ASCII.GetBytes(s2.Trim());

            for( int i = 0; i<ASCIIValues1.Length; i++)
            {
                k = ASCIIValues1[i] - ASCIIValues2[i];
                if(k!=0)
                {
                    return false;
                }
            }
 
            return true;
        }

    }
}
