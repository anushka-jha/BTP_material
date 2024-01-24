using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ShipStability
{
    class WriteOutput
    {

        public WriteOutput()
        {
        }

        public void WritetankInfo(List<Tank> tankList)
        {
            string path = "D://Ranadev//Research//Codes//Input//TankInfo";
            foreach (Tank tank in tankList)
            {
                string s = tank.TankName;
                string path1 = path + "//" + s +".txt";
                StreamWriter sr = new StreamWriter(path1);
                List<SectionCurve> curveList = tank.TankCurvedata;
                foreach(SectionCurve curve in curveList)
                {
                    List<Point2D> secCurve = curve.SectionPts;
                    double xloc = curve.Xloc;
                    sr.WriteLine(xloc.ToString());
                    foreach(Point2D pt in secCurve)
                    {
                        string pts = pt.X.ToString() + "," + pt.Y.ToString();
                        sr.WriteLine(pts);
                    }
                    sr.WriteLine();
                }

                sr.Close();

            }


        }

        public void WritingSummary (List<Summary> sumList)
        {
            for (int i = 0; i<sumList.Count;i++)
            {
                Summary sum = sumList[i];
                Console.WriteLine(sum.ObjectName + ':' + sum.ObjectValue);
            }
        }

        public void WriteLongStData ( List<Point2D> data , string filename)
        {
            string path = "D://Ranadev//Research//Codes//Input//output";
            string st = path + "//" + filename;
            StreamWriter sw = new StreamWriter(st);
            foreach (Point2D pt in data)
            {
                string sr = pt.X.ToString() + ","+pt.Y.ToString();
                sw.WriteLine(sr);
            }

            sw.Close();
        }

        public void WriteLoadCurves(List<Point2D> data1, List<Point2D> data2,string filename)
        {
            string path = "D://Ranadev//Research//Codes//Input//output";
            string st = path + "//" + filename;
            StreamWriter sw = new StreamWriter(st);
            for(int i = 0; i<data1.Count;i++)
            {
                double x = data1[i].X;
                double load = data1[i].Y;
                double buoyancy = -data2[i].Y;

                string sr = x.ToString() + "," + load.ToString() + "," + buoyancy.ToString();
                sw.WriteLine(sr);

            }


            sw.Close();
        } 

        public void WriteLiquidWtSummary(List<LiquidWeightModel> model)
        {
            string path = "D://Ranadev//Research//Codes//Input//output/LiquidSummary.dat";
            StreamWriter wr = new StreamWriter(path);
            for (int i = 0; i < model.Count; i++)
            {
                wr.WriteLine(model[i].Tankname + ',' + model[i].Weight + ',' + model[i].CG.X.ToString() + ',' + model[i].CG.Y.ToString() + ',' + model[i].CG.Z.ToString());
            }

            wr.Close();

        }

        public void MaxTankVolume (List<LiquidWeightModel> model)
        {
            string path = "D://Ranadev//Research//Codes//Input//output/MaxTankVolune.dat";
            StreamWriter wr = new StreamWriter(path);
            for (int i = 0; i< model.Count;i++)
            {
                wr.WriteLine(model[i].Tankname + ','+model[i].Weight+',' + model[i].CG.X.ToString() +','+ model[i].CG.Y.ToString() + ',' + model[i].CG.Z.ToString());
            }

            wr.Close();           

        }

        public void WriteHydrostatData (Dictionary<double, List<double>> hydroDataTowrite)
        {
            string path = "D://Ranadev//Research//Codes//Input//output/hydrostatData.dat";
            StreamWriter wr = new StreamWriter(path);
            foreach(double key in hydroDataTowrite.Keys)
            {
                List<double> dlist = hydroDataTowrite[key];
                wr.WriteLine(key.ToString() + ',' + dlist[0].ToString() + ',' + dlist[1].ToString() + ',' + dlist[2].ToString() + ',' + dlist[3].ToString() + ',' + dlist[4].ToString());

            }

            wr.Close();
        }


        public void WriteLSCalculation ( List<LongitudinalCalculation> lsList)
        {
            string st = "Frame No  Frame Spacing  Shear F  Shear F %  Bending M   Bending M %";
            Console.WriteLine(st);
            foreach ( LongitudinalCalculation lc in lsList)
            {
                string sr = lc.FrameNo.ToString() + "  " + lc.FrameSpacing.ToString() + "  " + lc.ShearAtFrame.ToString() + "  " + lc.ShearPerAtFrame + "  " + lc.BendingAtFrame.ToString() + "  " + lc.BendingPerAtFrame.ToString();
                
                Console.WriteLine(sr);
            }
        }

        public void WriteWindGz (List<Point2D> gzCurve)
        {

            string path = "D://Ranadev//Research//Codes//Input//output/WindGZ.dat";
            StreamWriter wr = new StreamWriter(path);
            foreach (Point2D pt in gzCurve)
            {
                
                wr.WriteLine(pt.X.ToString() + ',' + pt.Y.ToString());

            }

            wr.Close();

        }

        public void WriteTankSoundings(List<Tank> tankList)
        {
            string path = "D://Ranadev//Research//Codes//Input//TankSoundings";


            foreach (Tank tank in tankList)
            {
                string s = tank.TankName;
                string path1 = path + "//" + s + ".txt";
                StreamWriter sr = new StreamWriter(path1);
                sr.WriteLine("draft" + "," + "vol" + "," + "lcg" + "," + "tcg" + "," + "vcg"+","+"FSM");
                List<Point2D> ptvol = tank.VolumeData;
                List<Point2D> ptLcg = tank.TankLcg;
                List<Point2D> ptTcg = tank.TankTcg;
                List<Point2D> ptVcg = tank.TankVcg;
                List<Point2D> ptFSMt = tank.Fsmt;

                for(int i = 0; i<ptvol.Count;i++)
                {
                    sr.WriteLine(ptvol[i].X.ToString() + "," + ptvol[i].Y.ToString() + "," + ptLcg[i].Y.ToString() + "," + ptTcg[i].Y.ToString() + "," + ptVcg[i].Y.ToString()+","+ ptFSMt[i].Y.ToString());
                }

                



                sr.Close();
            }

            
        }

        public void WriteSectionLines(List<Section> secLine)
        {
            string path = "D://Ranadev//Research//Codes//Input//output/SecLine.dat";
            string path1 = "D://Ranadev//Research//Codes//Input//output/SecLine.txt";
            StreamWriter wr = new StreamWriter(path);
            StreamWriter wr1 = new StreamWriter(path1);
            int count = 1;
            foreach (Section sec in secLine)
            {
                List<Point2D> pt2D = sec.SecCurve.SectionPts;
                wr.WriteLine(sec.Xlocation.ToString() + "," + pt2D.Count.ToString()+","+"secNo"+count.ToString());
                wr1.WriteLine(sec.Xlocation.ToString() + "," + pt2D.Count.ToString() + "," + "secNo" + count.ToString());
                string st = "";
                foreach (Point2D pt in pt2D)
                {
                    st = st + (pt.Y / 2).ToString() + "," + pt.X.ToString() + ";";
                    wr.WriteLine((pt.Y/2).ToString() + "," + pt.X.ToString());
                }
                count++;
                wr1.WriteLine(st);
                wr.WriteLine();
                wr1.WriteLine();
            }

            wr.Close();
            wr1.Close();
        }
        
    }
}
