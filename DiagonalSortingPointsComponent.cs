using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.InteropServices;

using Rhino.DocObjects;
using Rhino.Collections;
using GH_IO;
using GH_IO.Serialization;


//██████╗ ██╗ █████╗  ██████╗  ██████╗ ███╗   ██╗ █████╗ ██╗     
//██╔══██╗██║██╔══██╗██╔════╝ ██╔═══██╗████╗  ██║██╔══██╗██║     
//██║  ██║██║███████║██║  ███╗██║   ██║██╔██╗ ██║███████║██║     
//██║  ██║██║██╔══██║██║   ██║██║   ██║██║╚██╗██║██╔══██║██║     
//██████╔╝██║██║  ██║╚██████╔╝╚██████╔╝██║ ╚████║██║  ██║███████╗
//╚═════╝ ╚═╝╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝╚═╝  ╚═╝╚══════╝

//███████╗ ██████╗ ██████╗ ████████╗██╗███╗   ██╗ ██████╗        
//██╔════╝██╔═══██╗██╔══██╗╚══██╔══╝██║████╗  ██║██╔════╝        
//███████╗██║   ██║██████╔╝   ██║   ██║██╔██╗ ██║██║  ███╗       
//╚════██║██║   ██║██╔══██╗   ██║   ██║██║╚██╗██║██║   ██║       
//███████║╚██████╔╝██║  ██║   ██║   ██║██║ ╚████║╚██████╔╝       
//╚══════╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝   ╚═╝╚═╝  ╚═══╝ ╚═════╝        

//██████╗  ██████╗ ██╗███╗   ██╗████████╗███████╗                
//██╔══██╗██╔═══██╗██║████╗  ██║╚══██╔══╝██╔════╝                
//██████╔╝██║   ██║██║██╔██╗ ██║   ██║   ███████╗                
//██╔═══╝ ██║   ██║██║██║╚██╗██║   ██║   ╚════██║                
//██║     ╚██████╔╝██║██║ ╚████║   ██║   ███████║  Parastorm lab 
//╚═╝      ╚═════╝ ╚═╝╚═╝  ╚═══╝   ╚═╝   ╚══════╝                


namespace DiagonalSortingPoints
{
    public class DiagonalSortingPointsComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        /// 

        // Component Written By @maan.arch - PARASTORM lab.
        // Sometimes I Need A Component That Do Something Like Sort a List Of Points.
        // And So, I Done This Code To Help Me & You (: . Try And Let Me Know !!
        public DiagonalSortingPointsComponent()
          : base("DiagonalSortingPoints", "DSP",
            "Diagonal Sorting List of Points",
            "Sets", "List")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("ListOfPoints", "L", "List of Point3d To Sort Them Diagonally Like O'clock Wise",
                GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "points", "Sorted Points", GH_ParamAccess.list);
            pManager.AddNumberParameter("Keys", "keys", "Sorted Keys", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Index", "index", "Indices of Points", GH_ParamAccess.list);
            pManager.AddPointParameter("Average", "Average", "An Average Point", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> pointsList = new List<Point3d>();
            List<Point3d> ListP = new List<Point3d>();


            if (!DA.GetDataList(0, ListP)) return;

            //Algorithm

            if (ListP.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "There Are No Data Yet > Please Input a List Of Points");
                return;
            }
            else
            {
                Point3d avg = new Point3d();

                // Get An Average Point Between A List Of Points (Start)------->
                for (int i = 0; i < ListP.Count; i++)
                {
                    if (ListP.Count == 1 || ListP.Count == 0) 
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "you're not able to get a result from less than 2 points");
                        return;

                    }
                    else 
                    {
                         avg += new Point3d((ListP[i].X / ListP.Count), (ListP[i].Y / ListP.Count), (ListP[i].Z / ListP.Count));

                    }

                }

                // Get An Average Point Between A List Of Points (End)--------->

                // compute victors btween 2points & dot-product values and get sorted angles!--------start

                List<Vector3d> Vectors = new List<Vector3d>();
                List<double> angles = new List<double>();

                for (int i = 0; i < ListP.Count; i++)
                {
                    Vector3d vecs = ListP[i] - avg;
                    Vectors.Add(vecs);
                    double Dx = Vector3d.Multiply(Vectors[i], Plane.WorldXY.XAxis);
                    double Dy = Vector3d.Multiply(Vectors[i], Plane.WorldXY.YAxis);
                    // compute victors btween 2points & dot-product values and get sorted angles!--------end

                    // Compute Angles (keys) (Start)------------------------------------>

                    double a = Math.Atan2(Dx, Dy);
                    double angle = a * (180 / Math.PI);
                    // Compute Angles (keys) (End)-------------------------------------->

                    angles.Add(angle);

                }

                // Compute a Dot Product Values of 2Vectors (End)------------------>

                // Sorting DataKeys (Start)---------------------------------------->

                double[] k = angles.ToArray();
                angles.Sort();

                // Sorting DataKeys (End)------------------------------------------>

                // Sorting Data (Start)-------------------------------------------->
                RhinoList<double> sortPointsX = new RhinoList<double>();
                RhinoList<double> sortPointsY = new RhinoList<double>();
                RhinoList<double> sortPointsZ = new RhinoList<double>();

                double[] X0 = new double[ListP.Count];
                double[] Y0 = new double[ListP.Count];
                double[] Z0 = new double[ListP.Count];


                for (int i = 0; i < ListP.Count; i++)
                {

                    Point3d pxyz = ListP[i];
                    X0[i] = pxyz.X;
                    sortPointsX.Add(pxyz.X);
                    Y0[i] = pxyz.Y;
                    sortPointsY.Add(pxyz.Y);
                    Z0[i] = pxyz.Z;
                    sortPointsZ.Add(pxyz.Z);

                }

                sortPointsX.Sort(k);
                sortPointsY.Sort(k);
                sortPointsZ.Sort(k);

                List<int> Pointsindices = new List<int>();

                for (int i = 0; i < ListP.Count; i++)
                {
                    Point3d PL = new Point3d(sortPointsX[i], sortPointsY[i], sortPointsZ[i]);

                    Pointsindices.Add(i);
                    pointsList.Add(PL);

                }

                // Sorting Data (End)---------------------------------------------->

                //output
                DA.SetDataList(0, pointsList);
                DA.SetDataList(1, k);
                DA.SetDataList(2, Pointsindices);
                DA.SetData(3, avg);


            }

        }
        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.diagonalSortingPoints_icon;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("639AC073-00C3-434E-8440-0E73C571E392");
    }


}