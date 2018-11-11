using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace test
{
    public class testComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public testComponent()
          : base("name", "nickname",
              "Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.",
              "category", "subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Start", "S", "X coordinate of Start Point", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("End", "E", "X coordinate of End Point", GH_ParamAccess.item, 10.0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Sin Curve", "S", "Output Sin Curve", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double start = 0.0;
            double end = 10.0;
            int count = 10;

            if (!DA.GetData(0, ref start)) return;
            if (!DA.GetData(1, ref end)) return;

            if (start > end)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "E must be bigger than S");
                return;
            }
            
            Curve Sin = CreateSinCurve(start, end, count);

            DA.SetData(0, Sin);
        }

        private Curve CreateSinCurve(double start, double end, Int32 count)
        {
            Point3d startPt = new Point3d(start, 0, 0);
            Point3d endPt = new Point3d(end, 0, 0);
            
            Line line = new Line(startPt, endPt);

            Point3d[] pts;
            line.ToNurbsCurve().DivideByCount(count, true, out pts);

            for(int i=0; i<pts.Length; i++)
            {
                double z = Math.Sin(i / Convert.ToDouble(pts.Length - 1) * 2* Math.PI);
                Point3d pt = pts[i];
                Point point = new Point(pt);
                point.Translate(new Vector3d(0, 0, z));

                pts[i] = point.Location;
            }

            Curve Sin = Curve.CreateInterpolatedCurve(pts, 3);
            return Sin;
        }
        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return test.Properties.Resources.Icon1.ToBitmap();
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("460cb157-91ba-4435-9417-ac5fe2628245"); }
        }
    }
}
