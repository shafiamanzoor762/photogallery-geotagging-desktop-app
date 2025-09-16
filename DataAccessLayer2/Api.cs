using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer2
{
	public class Api
	{
		public static string BaseUrl = "http://127.0.0.1:5000/";
        public static string imagesUrl = "http://127.0.0.1:5000/images/";


        //Recognize Person
        public static string RecognizePersonPath = $"{Api.BaseUrl}recognize_person";
		public static string ExtractFacedPath = $"{Api.BaseUrl}extract_face";

		//Image
		public string AddImagePath = $"{Api.BaseUrl}add_image";
		public static string GetImageCompleteDetailsPath = $"{Api.BaseUrl}image_complete_details";
		public static string EditImageDataPath = $"{Api.BaseUrl}edit_image";
		public string DeleteImagePath = $"{Api.BaseUrl}images";
		public string GetImageListPath = "";
		public string GetImageListByLocationPath = "";
		public string GetImageListByDatePath = "";
		public string GetImageListByEventPath = "";
        public string AddDirectory = $"{Api.BaseUrl}add-directory";
        public string getDirectory = $"{Api.BaseUrl}get-directory";


        public static string SearchingOnImage = "searching_on_image"; // Will be appended to BaseUrl in BLL

        //person
        public string AddPersonPath = "";
		public string GetAllpersons = $"{Api.BaseUrl}get_all_person";
		public string UpdatePersonPath = "";
        public static string GetPersonList = $"{Api.BaseUrl}person/";
        //link person

        public string Linkperson = $"{Api.BaseUrl}create_link";
		//location
		public string AddLocationPath = "";
		public string GetLocationPath = "";
		public string UpdateLocationPath = "";
		public static string GetLocationFromLatLonPath = $"{Api.BaseUrl}get_loc_from_lat_lon";

		//event
		public static string AddEventPath = $"{Api.BaseUrl}addnewevent";
		public static string GetEventsPath = $"{Api.BaseUrl}fetch_events";
		public string UpdateEventPath = "";

        //group by date
        public string GroupByDate = $"{Api.BaseUrl}group_by_date";

        //group by events
        public string GroupByEvent = $"{Api.BaseUrl}groupbyevents";

        //group by location
        public string GroupByLocation = $"{Api.BaseUrl}group_by_location";

        //group by person
        public string GroupByperson = $"{Api.BaseUrl}group_by_person";

		//load all images with data
		public string Loadimages = $"{Api.BaseUrl}Load_images";

        //get unlabelled images
        public string unlabelled = $"{Api.BaseUrl}unedited_images";

        // move images 

        public string moveImages = $"{Api.BaseUrl}move_images";

        //get faces for merge on basis of same name
        public string faces = $"{Api.BaseUrl}get_merge_data";
        public string linkmergefaces = $"{Api.BaseUrl}/merge_persons";

        //undo images 
        public string get_undo_data = $"{Api.BaseUrl}get_undo_data";
        public string show_undo_details = $"{Api.BaseUrl}image_complete_details_undo";
        public string undo_data = $"{Api.BaseUrl}undo_data";
        public string bulk_undo = $"{Api.BaseUrl}bulk_undo";

        public string RemoveMetadataPath = $"{Api.BaseUrl}remove_metadata";  // Assuming POST method for metadata
        public static string DeleteImageMetadataPath = $"{Api.BaseUrl}delete_image_metadata";
        public static string GetPersonsGroupFromImage = $"{Api.BaseUrl}/get_persons_groups_from_image_desktop";

        private HttpClient _httpClient;
        public Api()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> DeleteImage(string imageId)
        {
            string url = $"{DeleteImagePath}/{imageId}";  // The correct URL to delete image by its ID
            Console.WriteLine($"Calling URL: {url}");

            try
            {
                var response = await _httpClient.DeleteAsync(url);  // Send DELETE request to the URL

                if (!response.IsSuccessStatusCode)
                {
                    // If the request fails, log the error response
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorResponse}");
                    return false;
                }

                // If successful, log the success response
                string successResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Success: {successResponse}");
                return true;
            }
            catch (Exception ex)
            {
                // Catch any exception and log the error
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

    }
}