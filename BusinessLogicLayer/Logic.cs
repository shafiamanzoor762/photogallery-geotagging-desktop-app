
using Business_Layer;
using DataAccessLayer2;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
namespace BusinessLogicLayer
{
    public class Logic
    {


        ////hshfsfafjksdhfas
        //DBManager mgr = new DBManager();
        private static readonly HttpClient client = new HttpClient();
        static Api mgr = new Api();

        public static string imageurl = Api.imagesUrl;
        public static string faces = Api.BaseUrl;

        public  async Task SaveDirectoryToBackend(string directoryPath)
        {
            try
            {
                // Flask server URL to the add-directory route
                string url = mgr.AddDirectory;  // Change if using a different port or server

                // Prepare the JSON data to send to the backend
                var data = new
                {
                    path = directoryPath
                };

                // Convert the data to JSON
                string json = JsonConvert.SerializeObject(data);

                // Create the content to send in the HTTP request
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the POST request to Flask backend
                var response = await client.PostAsync(url, content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Directory path saved successfully. {response.Content}");
                }
                else
                {
                    Console.WriteLine($"Error saving directory: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task<string> GetDirectoryPath()
        {
            // Replace with your actual Flask API URL
            string url = mgr.getDirectory;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode(); // Throw if not success

                    string responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Response from Flask API:");
                    Console.WriteLine(responseBody);
                    return responseBody;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Error calling Flask API:");
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }
    
    // Method to generate the hash of the image (SHA256)
    private string GenerateImageHash(string imagePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(imagePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
        }


        // Method to process images
        public async Task ProcessImagesAsync(string imagePath, string baseFolder)
        {
                string hash = GenerateImageHash(imagePath); // your logic to create hash


                // 🔥 Send image to backend
                await SendImageToBackend(imagePath, hash);
        }



        public async Task ProcessImagesWithMetadataAsync(string imagePath, string eventName, DateTime eventDate, string location)
        {
            string hash = GenerateImageHash(imagePath);

            var imageId = await SendImageToBackend(imagePath, hash);
            if (imageId.HasValue)
            {
                var imageDetails = await GetImageDetails((int)imageId);

                imageDetails.event_date = eventDate;
                if (imageDetails.location == null)
                {
                    imageDetails.location = new Location();
                }
                imageDetails.location.Name = location;

                // Make sure events array is not null
                if (imageDetails.events == null)
                {
                    imageDetails.events = new Event[] { new Event { Name = eventName } };
                }
                else
                {
                    var updatedEvents = new Event[imageDetails.events.Length + 1];

                    imageDetails.events.CopyTo(updatedEvents, 0);

                    updatedEvents[updatedEvents.Length - 1] = new Event { Name = eventName };

                    imageDetails.events = updatedEvents;
                }

                await EditImageDataAsync(imageDetails);
            }
            else
            {
                // Handle failure
                Console.WriteLine("Image upload failed");
            }

        }
        private async Task<int?> SendImageToBackend(string imagePath, string hash)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new MultipartFormDataContent();
                    byte[] imageBytes = File.ReadAllBytes(imagePath);

                    content.Add(new ByteArrayContent(imageBytes), "file", Path.GetFileName(imagePath));
                    content.Add(new StringContent(hash), "hash");
                    content.Add(new StringContent(imagePath), "path");  // send full image path here

                    var response = await client.PostAsync(mgr.AddImagePath, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Parse the JSON response
                        var jsonResponse = JObject.Parse(responseContent);

                        // Extract the ID from the response
                        int imageId = jsonResponse["message"].Value<int>();

                        Console.WriteLine($"✅ Image '{Path.GetFileName(imagePath)}' uploaded successfully. ID: {imageId}");
                        return imageId;
                    }
                    else
                    {
                        Console.WriteLine($"❌ Error uploading image '{Path.GetFileName(imagePath)}'.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }



        //Shafia's work starts
        public static async Task<EditImageModel> GetImageDetails(int imageId)
        {
            try
            {
                string apiUrl = $"{Api.GetImageCompleteDetailsPath}/{imageId}";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    EditImageModel imageDetails = JsonConvert.DeserializeObject<EditImageModel>(jsonData);
                    imageDetails.path = $"{Api.imagesUrl}{imageDetails.path}";
                    foreach (var img in imageDetails.persons)
                    {
                        img.path = $"{Api.BaseUrl}{img.path}";
                    }
                    return imageDetails;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return new EditImageModel();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new EditImageModel();
            }
        }

        public static async Task<string> EditImageDataAsync(EditImageModel imageData)
        {
            try
            {
                string apiUrl = Api.EditImageDataPath; // Flask API endpoint

                // Step 1: Get capture date (we'll use this to calculate age)
                DateTime? captureDate = imageData.capture_date; // Make sure this property exists in EditImageModel

                // Step 2: Calculate age for each person (if dob and capture date are present)
                var personList = imageData.persons != null
                    ? Array.ConvertAll(imageData.persons, p =>
                    {
                        int? age = null;
                        if (p.dob != null && captureDate.HasValue)
                        {
                            DateTime dob = (DateTime)p.dob;
                            DateTime cap = captureDate.Value;
                            age = cap.Year - dob.Year;
                            if (cap < dob.AddYears(age.Value)) age--; // Adjust if birthday hasn't occurred yet
                        }

                        return new
                        {
                            id = p.id,
                            path=p.path.Replace("http://127.0.0.1:5000/",""),
                            name = p.name,
                            gender = p.gender,
                            dob = p.dob,
                            age = age
                        };
                    })
                    : Array.Empty<object>();

                var requestData = new Dictionary<string, object>
                {
                    [imageData.id.ToString()] = new
                    {
                        persons_id = personList,

                        event_names = imageData.events != null
                            ? Array.ConvertAll(imageData.events, e => e.Name)
                            : Array.Empty<string>(),

                        event_date = imageData.event_date.HasValue
                            ? imageData.event_date.Value.ToString("yyyy-MM-ddTHH:mm:ss")
                            : null,

                        location = imageData.location != null
                            ? new object[] { imageData.location.Name, imageData.location.Latitude, imageData.location.Longitude }
                            : Array.Empty<object>()
                    }
                };

                string jsonData = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating image data: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }


        public static async Task<string> GetLocationFromLatLonAsync(double latitude, double longitude)
        {
            try
            {
                string url = Api.GetLocationFromLatLonPath;

                var requestData = new
                {
                    latitude = latitude,
                    longitude = longitude
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return jsonResponse;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        // Fetch all events
        public static async Task<List<Event>> FetchEventsAsync()
        {
            try
            {
                string apiUrl = Api.GetEventsPath;

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                List<Event> events = JsonConvert.DeserializeObject<List<Event>>(responseBody);

                return events ?? new List<Event>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching events: {ex.Message}");
                return new List<Event>(); // Return empty list if there's an error
            }
        }

        // Add a new event
        public static async Task<string> AddNewEventAsync(string name)
        {
            try
            {
                string apiUrl = Api.AddEventPath;

                // Create a new event
                Event newEvent = new Event
                {
                    Id = 0,  // This is auto-generated in backend
                    Name = name
                };

                // Convert event data to JSON
                string jsonData = JsonConvert.SerializeObject(newEvent);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send POST request
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                // Read response
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding event: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

		public static async Task<List<Person>> GetPersonListAsync(int personId)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string url = $"{Api.GetPersonList}{personId}";
					HttpResponseMessage response = await client.GetAsync(url);

					if (response.IsSuccessStatusCode)
					{
						string jsonString = await response.Content.ReadAsStringAsync();
		
						List<Person> people = JsonConvert.DeserializeObject<List<Person>>(jsonString);
						
                        foreach (var img in people)
						{
							img.path = $"{Api.BaseUrl}{img.path}";
						}
						return people;
					}
					else
					{
						Console.WriteLine("Failed to fetch data from server.");
						return new List<Person>();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error: {ex.Message}");
					return new List<Person>();
				}
			}
		}

        //
        public static async Task<string> SendImageAsync(string imagePath)
        {
            try
            {
                using (var client = new HttpClient())
                using (var form = new MultipartFormDataContent())
                using (var fileStream = File.OpenRead(imagePath))
                using (var streamContent = new StreamContent(fileStream))
                {
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    form.Add(streamContent, "file", Path.GetFileName(imagePath));

                    var response = await client.PostAsync(Api.GetPersonsGroupFromImage, form);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;

                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public static async Task<MemoryStream> LoadImageFromUrl(string imageUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] imageData = await client.GetByteArrayAsync(imageUrl);

                    // Return MemoryStream WITHOUT disposing it
                    return new MemoryStream(imageData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                return null;
            }
        }

        public static async Task<bool> DeleteImageMetadata(string imageId)
        {
            string url = $"{Api.DeleteImageMetadataPath}/{imageId}";
            Console.WriteLine($"Calling URL: {url}");

            try
            {
                HttpClient _httpClient = new HttpClient();
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

        //Shafia's work ends





        // Searches images via API based on criteria provided in ImageRequest.
        public static async Task<List<string>> SearchImagesAsync(ImageRequest requestData)
        {
            try
            {
                string apiUrl = $"{Api.BaseUrl}{Api.SearchingOnImage}";
                string jsonData = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    ImageResponse result = JsonConvert.DeserializeObject<ImageResponse>(jsonResponse);
                    return result.paths;
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchImagesAsync: {ex.Message}");
                return new List<string>();
            }
        }


        //Aimen's task starts here 
        public async Task<object> Group_by_date()
        {

            string apiUrl = mgr.GroupByDate; // API endpoint

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Throw exception if HTTP request fails

                string jsonData = await response.Content.ReadAsStringAsync();

                // Convert JSON response to DataTable
                var dataTable = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);

                // Log the deserialized dictionary for debugging
                foreach (var keyValue in dataTable)
                {
                    Console.WriteLine($"{keyValue.Key}: {keyValue.Value}");
                }

                return dataTable;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return null;
            }
        }
        public async Task<object> Group_by_Event()
        {

            string apiUrl = mgr.GroupByEvent; // API endpoint

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Throw exception if HTTP request fails

                string jsonData = await response.Content.ReadAsStringAsync();

                // Convert JSON response to DataTable
                var dataTable = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);



                return dataTable;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return null;
            }
        }

        public async Task<object> Group_by_Location()
        {

            string apiUrl = mgr.GroupByLocation; // API endpoint

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Throw exception if HTTP request fails

                string jsonData = await response.Content.ReadAsStringAsync();

                // Convert JSON response to DataTable
                var dataTable = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);



                return dataTable;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return null;
            }
        }
        public async Task<object> Group_by_person()
        {

            string apiUrl = mgr.GroupByperson; // API endpoint

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Throw exception if HTTP request fails

                string jsonData = await response.Content.ReadAsStringAsync();

                // Convert JSON response to DataTable
                var dataTable = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData);



                return dataTable;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return null;
            }
        }
        public async Task<object> LoadImages(string key, string value)
        {
            try
            {
                string apiUrl = mgr.Loadimages;
                // 🔹 Prepare the JSON body with just `name`
                var requestData = new Dictionary<string, string>
        {
            { key, value }
        };
                string jsonBody = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // 🔹 Make the POST request
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode(); // Throws exception if request fails

                // 🔹 Read the response
                string jsonData = await response.Content.ReadAsStringAsync();

                // 🔹 Deserialize the response  EditImageModel
                var apiResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);

                if (apiResponse != null && apiResponse.ContainsKey("images"))
                {
                    // Deserialize the images array into a list of EditImageModel
                    var images = JsonConvert.DeserializeObject<List<EditImageModel>>(apiResponse["images"].ToString());

                    return images;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching images: {ex.Message}");
                return null;
            }
        }
        public async Task<object> UnlabelledImages()
        {
            try
            {
                string apiUrl = mgr.unlabelled;
                using (HttpClient client = new HttpClient())
                {
                    // Make a GET request to the API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // If the request was successful, read and display the content
                        var images = await response.Content.ReadAsStringAsync();
                        return images;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        Console.WriteLine("No unedited images found.");
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling API: " + ex.Message);
            }
            return null;
        }


        public static async Task<List<Person>> GetAllPersons()
        {
            try
            {
                string apiUrl = mgr.GetAllpersons; // API Endpoint
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    List<Person> people = JsonConvert.DeserializeObject<List<Person>>(jsonData); // ✅ Correctly deserialize a list

                    if (people != null)
                    {
                        // Update paths for each person
                        foreach (var person in people)
                        {
                            person.path = $"{Api.BaseUrl}{person.path}";
                        }
                    }
                    return people;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return new List<Person>(); // Return an empty list on failure
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Person>(); // Return an empty list on exception
            }
        }

        public static async Task<string> SaveLinkDataAsync(int person1Id, int person2Id)
        {
            string url = mgr.Linkperson;
            try
            {
                var payload = new
                {
                    person1_id = person1Id,
                    person2_id = person2Id
                };

                // Serialize the object to JSON
                string jsonBody = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Send POST request with JSON body
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Ensure successful response
                response.EnsureSuccessStatusCode();

                // Read response content as string
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the API call
                Console.WriteLine($"Error saving link data: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public static async Task<string> SaveMergepeopleDataAsync(int person1Id, string person2_emb)
        {
            string url = mgr.linkmergefaces;
            try
            {
                var payload = new
                {
                    person1_id = person1Id,
                    person2 = person2_emb
                };

                // Serialize the object to JSON
                string jsonBody = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Send POST request with JSON body
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Ensure successful response
                response.EnsureSuccessStatusCode();

                // Read response content as string
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;

            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the API call
                Console.WriteLine($"Error saving link data: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public static async Task<string> SendPersonToBackend(Person person, string name)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var payload = new MergeRequest
                    {
                        person1 = person,
                        name = name
                    };

                    var json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    string apiUrl = mgr.faces;
                    var response = await client.PostAsync(apiUrl, content);

                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("✅ Person data sent successfully.");
                        return responseContent; // Return the response body as string
                    }
                    else
                    {
                        Console.WriteLine($"❌ Failed to send. Status: {response.StatusCode}");
                        Console.WriteLine(responseContent);
                        return $"Error: {response.StatusCode} - {responseContent}";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Exception: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
        }


        public static async Task<List<UndoImage>> get_undo_data()
        {

            string apiUrl = mgr.get_undo_data; // API endpoint

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Throw exception if HTTP request fails

                string jsonData = await response.Content.ReadAsStringAsync();

                // Convert JSON response to DataTable
                var listData = JsonConvert.DeserializeObject<List<UndoImage>>(jsonData);



                return listData;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return null;
            }
        }
        //public static async Task<EditImageModel> ShowUndoDetails(int imageId, int version)
        //{
        //    try
        //    {
        //        string apiUrl = $"{mgr.show_undo_details}/{imageId}/{version}";
        //        HttpResponseMessage response = await client.GetAsync(apiUrl);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string jsonData = await response.Content.ReadAsStringAsync();
        //            EditImageModel imageDetails = JsonConvert.DeserializeObject<EditImageModel>(jsonData);

        //            // Prepend base URL to image path
        //            //if (!string.IsNullOrEmpty(imageDetails.path))
        //            //{
        //            //    imageDetails.path = $"{Api.imagesUrl}{imageDetails.path.Replace("\\", "/")}";
        //            //}

        //            // Fix person image paths
        //            if (imageDetails.persons != null)
        //            {
        //                foreach (var person in imageDetails.persons)
        //                {
        //                    person.path = $"{Api.BaseUrl}{person.path}";
        //                }
        //            }

        //            return imageDetails;
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Error: {response.StatusCode}");
        //            return new EditImageModel();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception: {ex.Message}");
        //        return new EditImageModel();
        //    }
        //}

        public static async Task<EditImageModel> ShowUndoDetails(int imageId, int version)
        {
            try
            {
                string apiUrl = $"{mgr.show_undo_details}/{imageId}/{version}";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();

                    // Deserialize to Dictionary first
                    var dataDict = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(jsonData);

                    if (dataDict.TryGetValue(imageId.ToString(), out JObject obj))
                    {
                        EditImageModel image = new EditImageModel
                        {
                            id = imageId,
                            path = obj["path"]?.ToString(),
                            is_sync = obj["is_sync"]?.ToObject<bool>() ?? false,
                            capture_date = DateTime.Parse(obj["capture_date"]?.ToString() ?? DateTime.MinValue.ToString()),
                            event_date = DateTime.TryParse(obj["event_date"]?.ToString(), out var ed) ? ed : (DateTime?)null,
                            last_modified = DateTime.TryParse(obj["last_modified"]?.ToString(), out var lm) ? lm : (DateTime?)null,

                            // Map persons_id to persons
                            persons = obj["persons_id"]?.ToObject<Person[]>(),

                            // Map event_names to events
                            events = obj["event_names"]?.Select(ev => new Event { Name = ev.ToString() }).ToArray(),

                            // Map location array
                            location = new Location
                            {
                                Name = obj["location"]?[0]?.ToString(),

                            }
                        };

                        // Update person image paths
                        if (image.persons != null)
                        {
                            foreach (var person in image.persons)
                            {
                                person.path = $"{Api.BaseUrl}{person.path}";
                            }
                        }

                        return image;
                    }
                }

                return new EditImageModel(); // fallback
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ShowUndoDetails: {ex.Message}");
                return new EditImageModel();
            }
        }
        public static async Task<bool> UndoData(int imageId, int version)
        {
            try
            {
                string apiUrl = $"{mgr.undo_data}/{imageId}/{version}";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
        public static async Task<UndoImage> Bulk_undo(List<UndoImage> undoList)
        {
            try
            {
                string url = $"{mgr.bulk_undo}";

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(undoList),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync(url, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into an UndoImage object
                    UndoImage result = JsonConvert.DeserializeObject<UndoImage>(jsonResponse);
                    return result;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        //Aimen's task end here


        // Delete Image
        public async Task<bool> DeleteImage(string imageId)
        {
            bool result = await mgr.DeleteImage(imageId);
            return result;
        }

        // Delete Metadata


        // private readonly HttpClient _httpClient;
        private readonly string RemoveMetadataPath = "http://localhost:5000/remove_metadata";  // Flask API URL

        //public Logic()
        //{
        //    _httpClient = new HttpClient();
        //}

        // Function to send the image to the Flask API to remove metadata and save it locally
        public async Task<bool> DeleteMetadataAndSaveFile(string imagePath, string savePath)
        {
            // Ensure the file exists
            if (!File.Exists(imagePath))
            {
                Console.WriteLine("Image file not found.");
                return false;
            }

            try
            {
                // Read the image as a byte array
                byte[] imageBytes = File.ReadAllBytes(imagePath);

                // Create a MultipartFormDataContent object to send the image
                using (var content = new MultipartFormDataContent())
                {
                    var byteArrayContent = new ByteArrayContent(imageBytes);
                    byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpg"); // Assuming it's a JPG image

                    // Add the image content to the request
                    content.Add(byteArrayContent, "file", Path.GetFileName(imagePath)); // 'file' is the form key in the API

                    // Send the POST request to the API
                    var response = await client.PostAsync(RemoveMetadataPath, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        // If the response is not successful, read the error and log it
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {errorResponse}");
                        return false;
                    }

                    // Get the response content as byte array (image with no metadata)
                    byte[] resultImageBytes = await response.Content.ReadAsByteArrayAsync();

                    // Save the image to the specified path
                    File.WriteAllBytes(savePath, resultImageBytes);
                    Console.WriteLine("Metadata deleted and image saved successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the HTTP request
                Console.WriteLine($"Exception during API call: {ex.Message}");
                return false;
            }
        }

        // for Eddb

        public async Task<ImageDetailsModel> FetchImageDetails(int imageId)
        {

            string apiUrl = $"{Api.imagesUrl}{imageId}"; // Adjust if needed

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Parse JSON response
                        var imageDetails = JsonConvert.DeserializeObject<ImageDetailsModel>(responseBody);

                        return imageDetails; // Return the image details
                    }
                    else
                    {
                        //MessageBox.Show("Failed to fetch image details. Server returned: " + response.StatusCode,
                        //                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public  async Task<string> MoveImagesToPersonFolder(string sourceId, List<Person> selectedImages, string destinationPersonId)
        {
            string url = $"{mgr.moveImages}";  // E.g., "http://localhost:5000/move_images"

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Create the request object
                    var data = new
                    {
                        source_id = sourceId,
                        destination_id = destinationPersonId,
                        persons = selectedImages.Select(img => new {
                            id = img.id,
                            name = img.name
                        }).ToList()
                    };

                    // Serialize to JSON
                    string json = JsonConvert.SerializeObject(data);

                    // Prepare content
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Make POST request
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic result = JsonConvert.DeserializeObject(responseString);
                        return result.message != null ? result.message.ToString() : "Images moved successfully.";
                    }
                    else
                    {
                        dynamic error = JsonConvert.DeserializeObject(responseString);
                        return error.error != null ? error.error.ToString() : "Failed to move images.";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error occurred: " + ex.Message;
            }

        }
        /////////////rafia task /////// for folder

        public static async Task<GroupedImageResult> GetImagesGroupedByEventAsync(string personName)
        {
            var url = $"http://127.0.0.1:5000/api/images-by-person?name={Uri.EscapeDataString(personName)}";

            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                // Return empty object to prevent null reference issues
                return new GroupedImageResult
                {
                    events = new Dictionary<string, List<string>>(),
                    locations = new Dictionary<string, List<string>>(),
                    capture_dates = new Dictionary<string, List<string>>(),
                    event_dates = new Dictionary<string, List<string>>()
                };
            }

            string json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GroupedImageResult>(json);

            // Ensure null checks for safety
            if (result.events == null)
                result.events = new Dictionary<string, List<string>>();

            if (result.locations == null)
                result.locations = new Dictionary<string, List<string>>();

            if (result.capture_dates == null)
                result.capture_dates = new Dictionary<string, List<string>>();

            if (result.event_dates == null)
                result.event_dates = new Dictionary<string, List<string>>();


            return result;
        }

    }
}

