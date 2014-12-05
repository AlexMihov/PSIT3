using Quizio.Models;
using System;
using System.Collections.Generic;

namespace Quizio.DAO
{
    public interface ICategoryDAO
    {
        List<Category> loadCategories();
    }
}
