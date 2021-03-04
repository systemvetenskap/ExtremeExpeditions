using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeExpeditions
{
    /// <summary>
    /// Mountain
    /// </summary>
    public class Peak
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int PeakId { get; set; }
        /// <summary>
        /// Name of peak
        /// </summary>
        public string PeakName { get; set; }
        /// <summary>
        /// Height in meters over sea level
        /// </summary>
        public int? Elevation { get; set; }
        /// <summary>
        /// Foreign key from table Range
        /// </summary>
        public int? RangeId { get; set; }

        public override string ToString()
        {
            return PeakName;
        }
    }
}
