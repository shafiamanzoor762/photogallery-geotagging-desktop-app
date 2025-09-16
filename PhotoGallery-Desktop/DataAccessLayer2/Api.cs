using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer2
{
	public class Api
	{
		public static string BaseUrl = "http://127.0.0.1:5000/";

		//Recognize Person
		public string RecognizePersonPath = $"{Api.BaseUrl}recognize_person";
		public string ExtractFacedPath = $"{Api.BaseUrl}extract_face";

		//Image
		public string AddImagePath = $"{Api.BaseUrl}add_image";
		public string GetImagePath = $"{Api.BaseUrl}edit_image";
		public string UpdateImagePath = "";
		public string DeleteImagePath = "";
		public string GetImageListPath = "";
		public string GetImageListByLocationPath = "";
		public string GetImageListByDatePath = "";
		public string GetImageListByEventPath = "";
		//person
		public string AddPersonPath = "";
		public string GetPersonPath = "";
		public string UpdatePersonPath = "";

		//location
		public string AddLocationPath = "";
		public string GetLocationPath = "";
		public string UpdateLocationPath = "";

		//event
		public string AddEventPath = "";
		public string GetEventPath = "";
		public string UpdateEventPath = "";

		//group by date
		public string GroupByDate = $"{Api.BaseUrl}group_by_date";
        //Sync
    }
}
