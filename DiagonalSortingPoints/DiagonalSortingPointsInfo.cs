using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace DiagonalSortingPoints
{
    public class DiagonalSortingPointsInfo : GH_AssemblyInfo
    {
        public override string Name => "DiagonalSortingPoints";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "Sometimes I Need A Component That Do Something Like Sort a List Of Points";

        public override Guid Id => new Guid("9710694E-DB6A-4235-B0CB-F2DB167B90DF");

        //Return a string identifying you or your company.
        public override string AuthorName => "Maan Abdulkareem Akber";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "maanfordesign@gmail.com";
    }
}