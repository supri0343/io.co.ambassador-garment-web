using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Manufactures.Domain.GarmentPreparings.Commands
{
    public class UpdateCustomsCategoryPreparingCommand : ICommand<int>
    {
        public UpdateCustomsCategoryPreparingCommand(List<CategoryUpdateModel> data)
        {
            Data = data;
        }


        public List<CategoryUpdateModel> Data { get; set; }

        public class CategoryUpdateModel
        {
            public List<long> Ids { get; set; } // Matches the previous Dictionary key
            public string Category { get; set; } // Matches the previous Dictionary value
        }
    }
}
