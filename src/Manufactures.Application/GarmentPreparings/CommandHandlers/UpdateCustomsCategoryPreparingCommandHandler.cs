using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Data.EntityFrameworkCore.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentPreparings.CommandHandlers
{
    public class UpdateCustomsCategoryPreparingCommandHandler : ICommandHandler<UpdateCustomsCategoryPreparingCommand, int>
    {
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IStorage _storage;

        public UpdateCustomsCategoryPreparingCommandHandler(IStorage storage)
        {
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
            _storage = storage;
        }
        public async Task<int> Handle(UpdateCustomsCategoryPreparingCommand request, CancellationToken cancellationToken)
        {

            foreach(var item in request.Data)
            {
                var garmentPreparingItem = _garmentPreparingItemRepository.Query.Where(a => item.Ids.Contains(a.UENItemId)).Select(a => new Domain.GarmentPreparings.GarmentPreparingItem(a)).ToList();

                foreach (var preparing in garmentPreparingItem)
                {
                    preparing.SetCustomsCategory(item.Category);
                    preparing.SetModified();
                    await _garmentPreparingItemRepository.Update(preparing);
                }
            }
            _storage.Save();

            return request.Data.Count();
        }
    }
    
}
