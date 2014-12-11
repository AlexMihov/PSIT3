using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System.Collections.Generic;

namespace Quizio.DAO
{   
    /// <summary>
    /// A class which is used to get the Categories from the Database and fill them into a Model
    /// </summary>
    public class CategoryDAO : ICategoryDAO
    {
        /// <summary>
        /// Gets a list of the currently available categories from the server
        /// </summary>
        /// <returns><c>List</c> with all the <c>Categories</c> which are available.</returns>
        public List<Category> loadCategories()
        {
            string getReq = REST.APIURL + "/categories";
            string json = REST.get(getReq);

            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(json);
            return categories;
        }

    }
}
