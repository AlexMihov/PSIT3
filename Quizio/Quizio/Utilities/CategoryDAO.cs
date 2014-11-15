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


        public List<Category> loadCategories()
        {
            //string getReq = "quizio.micahjonas.com/api/category";
            //List<Category> categories = new List<Category>();
            //string bla = REST.get(getReq);
            /*
            string json = @"{
                                id: 2,
                                name: 'Mathematik',
                                description: 'Lerne besser zu rechnen.',
                                quizies: 
                                [{
                                  id: 1,
                                  title: 'Gleichungen',
                                  description: 'Etwas über Gleichungen.'
                                },
                                {
                                  id: 2,
                                  title: 'Satzaufgaben',
                                  description: 'Etwas über Satzaufgaben.'
                                },
                                {
                                  id: 3,
                                  title: 'Prosa',
                                  description: 'Etwas über Prosa.'
                                },
                                {
                                  id: 4,
                                  title: 'Relationale Algebra',
                                  description: 'Etwas über Relationale Algebra.'
                                  }]}";*/

            string json = @"[{
id: 1,
name: 'Englisch',
description: 'Alle Quizzes die mit Englisch zu tun haben.',
quizies: 
[{
  id: 1,
  title: 'Thesaurus',
  description: 'Something about Thesaurus.'
},
{
  id: 2,
  title: 'Vokabulary',
  description: 'Something about Vokabulary.'
},
{
  id: 3,
  title: 'Grammar',
  description: 'Something about Grammar.'
},
{
  id: 4,
  title: 'True or false',
  description: 'Something about True or false.'
  }]},
{
id: 2,
name: 'Mathematik',
description: 'Lerne besser zu rechnen.',
quizies: 
[{
  id: 1,
  title: 'Gleichungen',
  description: 'Etwas über Gleichungen.'
},
{
  id: 2,
  title: 'Satzaufgaben',
  description: 'Etwas über Satzaufgaben.'
},
{
  id: 3,
  title: 'Prosa',
  description: 'Etwas über Prosa.'
},
{
  id: 4,
  title: 'Relationale Algebra',
  description: 'Etwas über Relationale Algebra.'
  }]}]";

            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(json);

            //categories.Add(cat);

            return categories;
        }

    }
}
