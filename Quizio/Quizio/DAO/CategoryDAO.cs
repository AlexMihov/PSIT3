using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System.Collections.Generic;

namespace Quizio.DAO
{
    public class CategoryDAO : ICategoryDAO
    {
        public List<Category> loadCategories()
        {
            string getReq = REST.APIURL + "/categories";
            string json = REST.get(getReq);

            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(json);
            return categories;
        }

    }
}
