using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using RecipesAppApp.Classes;
using RecipesAppApp.Models;

namespace RecipesAppApp.Services
{
    public class RecipesAppWebAPIProxy
    {
        #region without tunnel
        /*
        //Define the serevr IP address! (should be realIP address if you are using a device that is not running on the same machine as the server)
        private static string serverIP = "localhost";
        private HttpClient client;
        private string baseUrl;
        public static string BaseAddress = (DeviceInfo.Platform == DevicePlatform.Android &&
            DeviceInfo.DeviceType == DeviceType.Virtual) ? "http://10.0.2.2:5110/api/" : $"http://{serverIP}:5110/api/";
        private static string ImageBaseAddress = (DeviceInfo.Platform == DevicePlatform.Android &&
            DeviceInfo.DeviceType == DeviceType.Virtual) ? "http://10.0.2.2:5110" : $"http://{serverIP}:5110";
        */
        #endregion

        #region with tunnel
        //Define the serevr IP address! (should be realIP address if you are using a device that is not running on the same machine as the server)
        private static string serverIP = "32zgfnxw-5281.euw.devtunnels.ms";
        private HttpClient client;
        private string baseUrl;
        public static string BaseAddress = "https://32zgfnxw-5281.euw.devtunnels.ms/api/";
        private static string ImageBaseAddress = "https://32zgfnxw-5281.euw.devtunnels.ms/";
        #endregion

        public RecipesAppWebAPIProxy()
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            this.client = new HttpClient(handler);
            this.baseUrl = BaseAddress;
        }

        public string GetImagesBaseAddress()
        {
            return RecipesAppWebAPIProxy.ImageBaseAddress;
        }

        public string GetDefaultProfilePhotoUrl()
        {
            return $"{RecipesAppWebAPIProxy.ImageBaseAddress}/profileImages/default.png";
        }

        //Login - if email and password are correct User object is returned. otherwise a null will be returned
        public async Task<User?> LoginAsync(LoginInfo userInfo)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}login";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(userInfo);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    User? result = JsonSerializer.Deserialize<User>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //This methos call the Register web API on the server and return the AppUser object with the given ID
        //or null if the call fails
        public async Task<RegisterInfo?> Register(RegisterInfo registerInfo)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}register";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(registerInfo);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    RegisterInfo? result = JsonSerializer.Deserialize<RegisterInfo>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Image
        //This method call the UploadProfileImage web API on the server and return the AppUser object with the given URL
        //of the profile image or null if the call fails
        //when registering a user it is better first to call the register command and right after that call this function
        public async Task<User?> UploadProfileImage(string imagePath)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}uploadprofileimage";
            try
            {
                //Create the form data
                MultipartFormDataContent form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                form.Add(fileContent, "file", imagePath);
                //Call the server API
                HttpResponseMessage response = await client.PostAsync(url, form);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    User? result = JsonSerializer.Deserialize<User>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string?> UploadRecipeImage(Recipe recipe)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}uploadRecipeImage?RecipeName={recipe.RecipesName}&MadeBy={recipe.MadeBy}";
            try
            {
                string imagePath = recipe.RecipeImage;
                //Create the form data
                MultipartFormDataContent form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                form.Add(fileContent, "file", imagePath);
                //Call the server API
                HttpResponseMessage response = await client.PostAsync(url, form);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string? result = resContent;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string?> UploadUserImage(User user)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}uploadUserImage?Id={user.Id}";
            try
            {
                string imagePath = user.UserImage;
                //Create the form data
                MultipartFormDataContent form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                form.Add(fileContent, "file", imagePath);
                //Call the server API
                HttpResponseMessage response = await client.PostAsync(url, form);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string? result = resContent;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string?> UploadIngredientImage(Ingredient ingredient)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}uploadUserImage?IngredientName={ingredient.IngredientName}";
            try
            {
                string imagePath = ingredient.IngredientName;
                //Create the form data
                MultipartFormDataContent form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                form.Add(fileContent, "file", imagePath);
                //Call the server API
                HttpResponseMessage response = await client.PostAsync(url, form);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string? result = resContent;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        public async Task<List<Recipe>> GetAllRecipes()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUrl}getRecipes");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    List<Recipe> r = JsonSerializer.Deserialize<List<Recipe>>(content, options);
                    if (r == null)
                        return null;
                    else return r;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUrl}getUsers");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    List<User> u = JsonSerializer.Deserialize<List<User>>(content, options);
                    if (u == null)
                        return null;
                    else return u;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<List<Level>> GetLevelsByRecipe(int recipeId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getLevelsByRecipe";
            try
            {
                string json = JsonSerializer.Serialize(recipeId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string Responsecontent = await response.Content.ReadAsStringAsync();
                    List<Level> l = JsonSerializer.Deserialize<List<Level>>(Responsecontent, options);
                    if (l == null)
                        return null;
                    else return l;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<List<Ingredient>> GetIngredientsByRecipe(int recipeId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getIngredientsByRecipe";
            try
            {
                string json = JsonSerializer.Serialize(recipeId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true

                    };
                    string Responsecontent = await response.Content.ReadAsStringAsync();
                    List<Ingredient> i = JsonSerializer.Deserialize<List<Ingredient>>(Responsecontent, options);
                    if (i == null)
                        return null;
                    else return i;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<List<IngredientRecipe>> GetIngredientRecipesByRecipe(int recipeId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getIngredientRecipesByRecipe";
            try
            {
                string json = JsonSerializer.Serialize(recipeId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)

                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true

                    };
                    string Responsecontent = await response.Content.ReadAsStringAsync();
                    List<IngredientRecipe> i = JsonSerializer.Deserialize<List<IngredientRecipe>>(Responsecontent, options);
                    if (i == null)
                        return null;
                    else return i;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<int> GetRecipesAmountByUser(int userId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getRecipesAmountByuser";
            try
            {
                string json = JsonSerializer.Serialize(userId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string Responsecontent = await response.Content.ReadAsStringAsync();
                    int i = JsonSerializer.Deserialize<int>(Responsecontent, options);
                    if (i == null)

                        return 0;
                    else return i;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
        public async Task<int> GetCommentsAmountByUser(int userId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getCommentsAmountByuser";
            try
            {
                string json = JsonSerializer.Serialize(userId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string Responsecontent = await response.Content.ReadAsStringAsync();
                    int i = JsonSerializer.Deserialize<int>(Responsecontent, options);
                    if (i == null)

                        return 0;
                    else return i;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
        public async Task<int> GetRatingsAmountByUser(int userId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getRatingsAmountByuser";
            try
            {
                string json = JsonSerializer.Serialize(userId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string Responsecontent = await response.Content.ReadAsStringAsync();
                    int i = JsonSerializer.Deserialize<int>(Responsecontent, options);
                    if (i == null)

                        return 0;
                    else return i;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
        public async Task<List<User>> GetUsersbyStorage(int userId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getUsersbyStorage";
            try
            {
                string json = JsonSerializer.Serialize(userId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string Responsecontent = await response.Content.ReadAsStringAsync();
                    List<User> i = JsonSerializer.Deserialize<List<User>>(Responsecontent, options);
                    if (i == null)
                        return null;
                    else return i;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<Storage> GetStoragesbyUser(int StorageId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getStorageByUser";
            try
            {
                string json = JsonSerializer.Serialize(StorageId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string Responsecontent = await response.Content.ReadAsStringAsync();
                    Storage i = JsonSerializer.Deserialize<Storage>(Responsecontent, options);
                    if (i == null)
                        return null;
                    else return i;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<bool> ChangeName(User user)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}changeName";
            try
            {
                string json = JsonSerializer.Serialize(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                     return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> ChangeMail(User user)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}changeMail";
            try
            {
                string json = JsonSerializer.Serialize(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> ChangeStorageName(Storage newStorage)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}changeStorageName";
            try
            {
                string json = JsonSerializer.Serialize(newStorage);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> RemoveMember(User user)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}removeStorageMember";
            try
            {
                string json = JsonSerializer.Serialize(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

    }
}
