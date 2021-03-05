using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace ExtremeExpeditions.Repositories
{
    public class ExpeditionRepository
    {
        private static readonly string connectionString = "Server=localhost;Port=5433;Database=expeditiondemodb;User ID=demouser; Password=Hemligt;";
        // Man måste ALLTID utgå från att en databas ger felmeddelanden!!
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
            return peak;
        }

        /// <summary>
        /// Gets a list of all peaks
        /// </summaryList of Peak</returns>
        public List<Peak> GetPeaks()
        {
            string stmt = "select * from peak";
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
                    PeakName = Convert.IsDBNull(reader["peakname"])? null: (string)reader["peakname"],
                    Elevation = Convert.IsDBNull(reader["elevation"]) ? null : (int?)reader["elevation"],
                    RangeId = Convert.IsDBNull(reader["rangeid"]) ? null : (int?)reader["rangeid"]
                };
                peaks.Add(peak);
            }
            return peaks;
        }


        #endregion

        #region Create
        /// <summary>
        /// Adds new peak in database
        /// </summary>
        /// <param name="peak"></param>
        /// <returns>Returns peak with newly generated id</returns>
        public Peak AddPeak(Peak peak)
        {
            string stmt = "insert into peak(peakname, elevation, rangeid) values(@peakname, @elevation, @rangeid) returning peakid";

            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);
                command.Parameters.AddWithValue("peakname", peak.PeakName);
                command.Parameters.AddWithValue("elevation", peak.Elevation ?? Convert.DBNull);
                command.Parameters.AddWithValue("rangeid", peak.RangeId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    peak.PeakId = (int)reader["peakid"];
                }

                return peak;
            }
            catch (PostgresException ex)
            {
                string errorCode = ex.SqlState;
                throw new Exception("Du måste ange i vilken fjällkedja som berget ingår i", ex);
            }
        }

       
        #endregion

        #region Update
        /// <summary>
        /// Updates a collection of peaks
        /// </summary>
        /// <param name="peaks">List of peaks</param>
        /// <returns>total number of affected rows</returns>
        public int UpdatePeaks(List<Peak> peaks)
        {
            string stmt = "update peak set rangeid = @rangeid, elevation=@elevation, peakname=@peakname where peakid = @peakid";
            int result = 0;
            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);
                foreach (var peak in peaks)
                {
                    command.Parameters.AddWithValue("peakname", peak.PeakName ?? Convert.DBNull);
                    command.Parameters.AddWithValue("elevation", peak.Elevation ?? Convert.DBNull);
                    command.Parameters.AddWithValue("rangeid", peak.RangeId); 
                    command.Parameters.AddWithValue("peakid", peak.PeakId);
                    result += command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
                return result;
            }
            catch (PostgresException ex)
            {
                string errorCode = ex.SqlState;
                throw new Exception("Du måste ange i vilken fjällkedja som berget ingår i", ex);
            }
        }

        /// <summary>
        /// Updates a collection of peaks, but wrapped in a transaction. This is the only way to ensure database consistency
        /// <seealso cref="UpdatePeaks(List{Peak})"/>
        /// </summary>
        /// <param name="peaks"></param>
        /// <returns>Total count of affected rows</returns>
        public int UpdatePeaksWithTransaction(List<Peak> peaks)
        {
            string stmt = "update peak set rangeid = @rangeid, elevation=@elevation, peakname=@peakname where peakid = @peakid";
            int result = 0;
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
               
                using var command = new NpgsqlCommand(stmt, conn);
                foreach (var peak in peaks)
                {
                    command.Parameters.AddWithValue("peakname", peak.PeakName ?? Convert.DBNull);
                    command.Parameters.AddWithValue("elevation", peak.Elevation ?? Convert.DBNull);
                    command.Parameters.AddWithValue("rangeid", peak.RangeId);
                    command.Parameters.AddWithValue("peakid", peak.PeakId);
                    result += command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
                transaction.Commit();
                return result;
            }
            catch (PostgresException ex)
            {
                transaction.Rollback();
                string errorCode = ex.SqlState;
                switch (errorCode)
                {
                    default:
                        break;
                }
                throw new Exception("Uppdateringen misslyckades...", ex);
            }
        }


        #endregion
    }
}
