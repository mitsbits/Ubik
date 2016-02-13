using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Web.Basis;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.Components.AntiCorruption.ViewModels.Devices;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.Domain;
using Ubik.Web.EF.Components;

namespace Ubik.Web.Components.AntiCorruption.Services
{
    public class DeviceAdministrationService : IDeviceAdministrationService<int>, IDeviceAdministrationViewModelService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly ICRUDRespoditory<PersistedDevice> _persistedDeviceRepo;
        private readonly ICRUDRespoditory<PersistedSection> _persistedSectionRepo;

        private readonly IViewModelBuilder<PersistedDevice, DeviceViewModel> _deviceBuilder;
        private readonly IViewModelCommand<DeviceSaveModel> _deviceCommand;

        private readonly IViewModelBuilder<PersistedSection, SectionViewModel> _sectionBuilder;
        private readonly IViewModelCommand<SectionSaveModel> _sectionCommand;

        private readonly IViewModelBuilder<PersistedSlot, SlotViewModel> _slotBuilder;

        private readonly IResident _resident;

        public DeviceAdministrationService(IDbContextScopeFactory dbContextScopeFactory, ICRUDRespoditory<PersistedDevice> persistedDeviceRepo, IViewModelCommand<DeviceSaveModel> deviceCommand, IViewModelCommand<SectionSaveModel> sectionCommand, ICRUDRespoditory<PersistedSection> persistedSectionRepo, IResident resident)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _persistedDeviceRepo = persistedDeviceRepo;
            _deviceCommand = deviceCommand;
            _sectionCommand = sectionCommand;
            _persistedSectionRepo = persistedSectionRepo;
            _resident = resident;

            _deviceBuilder = new DeviceViewModelBuilder(_resident);
            _sectionBuilder = new SectionViewModelBuilder();
            _slotBuilder = new SlotViewModelBuilder(_resident);
        }

        #region IDeviceAdministrationService

        public async Task<PagedResult<Device<int>>> All(int pageNumber, int pageSize)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var result = await _persistedDeviceRepo.FindAsync(x => true,
                       new[] { new OrderByInfo<PersistedDevice>() { Ascending = true, Property = x => x.FriendlyName } },
                       pageNumber, pageSize);

                var output = new PagedResult<Device<int>>(
                    new List<Device<int>>(result.Data.Select(Mapper.MapToDomain)), result.PageNumber, result.PageSize,
                    result.TotalRecords);

                return output;
            }
        }

        public async Task<Device<int>> Get(int id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var entity = await _persistedDeviceRepo.GetAsync(x => x.Id == id, x => x.Sections);
                return Mapper.MapToDomain(entity);
            }
        }

        public async Task<IServerResponse> DeleteSection(int id)
        {
            ServerResponse response;
            try
            {
                using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
                {
                    await _persistedSectionRepo.DeleteAsync(x => x.Id == id);
                    await db.SaveChangesAsync();
                    response = new ServerResponse(ServerResponseStatus.SUCCESS, "Persisted Section deleted", "");
                }
            }
            catch (Exception ex)
            {
                response = new ServerResponse(ex);
            }
            return response;
        }

        #endregion IDeviceAdministrationService

        #region IDeviceAdministrationViewModelService

        public async Task<DeviceViewModel> DeviceModel(int id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var data = await _persistedDeviceRepo.GetAsync(x => x.Id == id) ?? new PersistedDevice();
                var model = _deviceBuilder.CreateFrom(data);
                _deviceBuilder.Rebuild(model);
                model.Sections.Clear();
                foreach (var persistedSection in data.Sections)
                {
                    var section = _sectionBuilder.CreateFrom(persistedSection);
                    _sectionBuilder.Rebuild(section);
                    section.Slots.Clear();
                    foreach (var persistedSlot in persistedSection.Slots)
                    {
                        var slot = _slotBuilder.CreateFrom(persistedSlot);
                        _slotBuilder.Rebuild(slot);
                        section.Slots.Add(slot);
                    }
                    model.Sections.Add(section);
                }
                return model;
            }
        }

        public async Task<IEnumerable<DeviceViewModel>> DeviceModels()
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var data = await _persistedDeviceRepo.GetQuery().Include(x => x.Sections).ToListAsync();
                var bucket = new List<DeviceViewModel>();
                foreach (var model in data.Select(persistedDevice => _deviceBuilder.CreateFrom(persistedDevice)))
                {
                    _deviceBuilder.Rebuild(model);
                    bucket.Add(model);
                }
                return bucket;
            }
        }

        public async Task Execute(DeviceSaveModel model)
        {
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                await _deviceCommand.Execute(model);
                await db.SaveChangesAsync();
            }
        }

        public async Task Execute(SectionSaveModel model)
        {
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                await _sectionCommand.Execute(model);
                await db.SaveChangesAsync();
            }
        }

        #endregion IDeviceAdministrationViewModelService
    }
}