using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Utilities
{
    public class CategoryDAO
    {


        public CategoryDAO() 
        {

        }


        public virtual List<Category> loadCategories()
        {
            string getReq = REST.APIURL + "/categories";
            string json = REST.get(getReq);

            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(json);
            return categories;
        }

    }
}
