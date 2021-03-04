using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace ExtremeExpeditions.Repositories
{
    public class ExpeditionRepository
    {
        private static readonly string connectionString = "Server=localhost;Port=5433;Database=expeditiondemodb;User ID=demouser; Password=Hemligt;";

        #region Read
        /// <summary>
        /// Gets a peak from database
        /// </summary>
        /// <param name="id">Unique primary key</param>
        /// <returns>Peak</returns>
        public Peak GetPeak(int id)
        {
            string stmt = "select * from peaks where peakid = @id";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("id", id);

            using var reader = command.ExecuteReader();
            Peak peak = null;
            while (reader.Read())
            {
                peak = new Peak
                {
                    PeakId = (int)reader["peakid"],
                    PeakName = (string)reader["peakname"],
                    Elevation=(int)reader["elevation"],
                    RangeId=Convert.IsDBNull(reader["rangeid"]) ? null : (int?)reader["rangeid"]
                };
            }
            //return peak;

            return new Peak
            {
                PeakId = 123,
                PeakName = "Erik"
            };
        }

        /// <summary>
        /// Gets a list of all peaks
        /// </summaryList of Peak</returns>
        public List<Peak> GetPeaks()
        {
            string stmt = "select * from peaks";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            using var reader = command.ExecuteReader();
            Peak peak = null;
            var peaks = new List<Peak>();
            while (reader.Read())
            {
                peak = new Peak
                {
                    PeakId = (int)reader["peakid"],
                    PeakName = (string)reader["peakname"],
                    Elevation = (int)reader["elevation"],
                    RangeId = Convert.IsDBNull(reader["rangeid"]) ? null : (int?)reader["rangeid"]
                };
                peaks.Add(peak);
            }
            return peaks;
        }


        #endregion
    }
}
