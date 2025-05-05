using CFAIProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class ChartAxisGroup
    {
        public string Name { get; set; } = String.Empty;

        public AxisGroupModes Mode { get; set; } = AxisGroupModes.Markers;

        public string Color { get; set; } = String.Empty;

        public List<ChartAxis> AxisList { get; set; } = new List<ChartAxis>();
        
        /// <summary>
        /// Sorts axis values in ascending order for specified axis and then re-order the other axes.
        /// 
        /// This is done typically for display purposes. If we're displaying a line chart then we want the x-axis values to
        /// be ordered so that the line travels from left to right rather than zig-zagging.
        /// </summary>
        /// <param name="axisIndex"></param>
        public void SortAxisValues(int axisIndex)
        {
            var chartAxis = AxisList[axisIndex];

            // Abort if no values to sort
            if (chartAxis.Values.Length < 2) return;

            for (int index1 = 0; index1 < chartAxis.Values.Length; index1++)
            {
                for (int index2 = index1; index2 < chartAxis.Values.Length; index2++)
                {
                    if (index1 != index2)
                    {
                        // Compare value #1 & #2
                        var isSwap = false;                        
                        if (chartAxis.Values[index1] is string)
                        {
                            if (String.Compare(chartAxis.Values[index1].ToString(), chartAxis.Values[index2].ToString()) < 0)
                            {
                                isSwap = true;
                            }
                        }
                        else if (Convert.ToDouble(chartAxis.Values[index1]) < Convert.ToDouble(chartAxis.Values[index2]))
                        {
                            isSwap = true;
                        }

                        if (isSwap)
                        {
                            // Swap values for this axis
                            chartAxis.SwapValues(index1, index2);

                            // Swap values for other axis
                            foreach (var otherChartAxis in AxisList.Where(a => a != chartAxis))
                            {
                                otherChartAxis.SwapValues(index1, index2);
                            }
                        }
                    }
                }
            }
        }
    }
}
