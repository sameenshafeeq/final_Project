using penalty__calculator__final__project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace penalty__calculator__final__project.DataLayer
{
    public class SQLDataHelper
    {
        private string connectionString;
        public SQLDataHelper()
        { connectionString = WebConfigurationManager.ConnectionStrings["DBCS"].ConnectionString; }
        [Route("GetCountriesdata")]
        public List<Country> GetCountryData()
        {
            List<Country> listOfStudents = new List<Country>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Select * from countries ", con);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Country str = new Country();
                    str.Country_ID = Convert.ToInt32(sdr["Country_ID"]); //These are table column names
                    str.Country_Name = sdr["Country_Name"].ToString();
                    str.Currency = sdr["Currency"].ToString();
                    str.Penalty_Amount = Convert.ToInt32(sdr["Penalty_Amount"]);
                    str.Tax = Convert.ToInt32(sdr["Tax"]);
                    listOfStudents.Add(str);
                }
                con.Close();
            }
            return listOfStudents;
        }
        [Route("GetBuisnessDays")]
        public int GetNumberOfWorkingDays(DateTime CheckinDate, DateTime CheckOutDate, int Country_ID)
        {
            CheckinDate = CheckinDate.Date;
            CheckOutDate = CheckOutDate.Date;
            if (CheckinDate > CheckOutDate)
                throw new ArgumentException("Incorrect last day " + CheckOutDate);
            TimeSpan span = CheckOutDate - CheckinDate;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            if (businessDays > fullWeekCount * 7)
            {
                int firstDayOfWeek = CheckinDate.DayOfWeek == DayOfWeek.Sunday
                     ? 7 : (int)CheckinDate.DayOfWeek;
                int lastDayOfWeek = CheckOutDate.DayOfWeek == DayOfWeek.Sunday
                    ? 7 : (int)CheckOutDate.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }
            businessDays -= fullWeekCount + fullWeekCount;
            List<Holidays> listOfHolidays = new List<Holidays>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Select * from holidays where CountryID = " + Country_ID, con);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Holidays str = new Holidays();
                    str.CountryID = Convert.ToInt32(sdr["Country_ID"]); //These are table column names
                    str.Holiday_Name = sdr["Holiday_Name"].ToString();
                    str.Holiday = (Convert.ToDateTime(sdr["Holiday"])).Date;
                    listOfHolidays.Add(str);
                }
                con.Close();
                for (int i=0;i< listOfHolidays.Count;i++)
                {

                    if (CheckinDate <= listOfHolidays[i].Holiday && listOfHolidays[i].Holiday <= CheckOutDate)
                        --businessDays;
                }
 
            }
            return businessDays;
        }
    }
}