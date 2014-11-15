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
    class CategoryDAO
    {


        public CategoryDAO() 
        {

        }


        public IEnumerable<Category> loadCategories()
        {
            string getReq = "quizio.micahjonas.com/api/category";
            IEnumerable<Category> categories = new List<Category>();
            string bla = REST.get(getReq);

            string json = @"{
                                id: 2,
                                name: 'Mathematik',
                                description: 'Lerne besser zu rechnen.',
                                questions: 
                                [{
                                  id = 1,
                                  title = 'Gleichungen',
                                  description = 'Etwas über Gleichungen.'
                                }
                                {
                                  id = 2,
                                  title = 'Satzaufgaben',
                                  description = 'Etwas über Satzaufgaben.'
                                }
                                {
                                  id = 3,
                                  title = 'Prosa',
                                  description = 'Etwas über Prosa.'
                                },
                                {
                                  id = 4,
                                  title = 'Relationale Algebra',
                                  description = 'Etwas über Relationale Algebra.'
                                  }]}";

            Category cat = JsonConvert.DeserializeObject<Category>(json);

            return categories;
        }

    }
}
