using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Grasshopper;
using Rhino.Geometry;
using Grasshopper.Kernel;

namespace DiagonalSortingPoints.Utility
{
    public class Tabs_Properties : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            var server = Grasshopper.Instances.ComponentServer;

            server.AddCategoryShortName("SortPoints", "SP");
            server.AddCategorySymbolName("SortPoints", 'S');
            server.AddCategoryIcon("SortPoints", Properties.Resources.DiagonalSortingPoint_icon);

            return GH_LoadingInstruction.Proceed;
        }

    }
}
