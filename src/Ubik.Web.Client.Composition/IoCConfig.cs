﻿using Mehdime.Entity;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Ubik.Assets.Store.Core.Contracts;
using Ubik.Assets.Store.Core.Services;
using Ubik.Assets.Store.EF;
using Ubik.Assets.Store.EF.Contracts;
using Ubik.Assets.Store.EF.Repositories;
using Ubik.Assets.Store.EF.Services;
using Ubik.Cache.Runtime;
using Ubik.Domain.Core;
using Ubik.EF6;
using Ubik.Infra.Contracts;
using Ubik.Postal;
using Ubik.Postal.Contracts;
using Ubik.Postal.Services;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.Basis.Services;
using Ubik.Web.Basis.Services.Accessors;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Client.Backoffice;
using Ubik.Web.Client.Backoffice.Contracts;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.Components.AntiCorruption.Services;
using Ubik.Web.Components.AntiCorruption.ViewModels.Devices;
using Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies;
using Ubik.Web.Components.Contracts;
using Ubik.Web.EF;
using Ubik.Web.EF.Components;
using Ubik.Web.EF.Components.Contracts;
using Ubik.Web.EF.Membership;
using Ubik.Web.Membership;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.Membership.Managers;
using Ubik.Web.Membership.Services;
using Ubik.Web.Membership.ViewModels;
using Ubik.Web.SSO;

namespace Ubik.Web.Client.Composition
{
    public static class IoCConfigExtensions
    {
        private static readonly Assembly[] _asmbls;

        static IoCConfigExtensions()
        {
            foreach (var name in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                if (!AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName == name.FullName))
                {
                    if (name.Name.StartsWith("Ubik"))
                        LoadReferencedAssembly(Assembly.Load(name));
                }
            }
            _asmbls = AppDomain.CurrentDomain.GetAssemblies();
        }

        private static void LoadReferencedAssembly(Assembly assembly)
        {
            foreach (AssemblyName name in assembly.GetReferencedAssemblies())
            {
                if (!AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName == name.FullName))
                {
                    LoadReferencedAssembly(Assembly.Load(name));
                }
            }
        }

        public static void ConfigureUbikServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            WireUpDbContexts(services, configuration);
            WireUpInternals(services);
            WireUpAssetStore(services);
            WireUpSSSO(services);
            WireUpCms(services);
            WireUpEventHandlers(services);
            WireUpCommandHandlers(services);
            WireUpPostal(services, configuration);
        }

        private static void WireUpPostal(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<PostalSettings>(configuration.GetSection("Postal"));
            services.AddSingleton<IEmailService, SmtpImapEmailService>();
        }

        private static void WireUpAssetStore(IServiceCollection services)
        {
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IAssetVersionRepository, AssetVersionRepository>();
            services.AddScoped<IMimeRepository, MimeRepository>();
            services.AddScoped<IAssetStoreProjectionRepository, AssetStoreProjectionRepository>();
            services.AddScoped<IAssetProjectionRepository, AssetProjectionRepository>();

            services.AddSingleton<IConflictingNamesResolver, DefaultConflictingNamesResolver>();
            services.AddSingleton<IAssetDirectoryStrategy<int>, DefaultAssetDirectoryStrategy>();
            services.AddScoped<IStoreService<Guid>, StoreService>();
            services.AddScoped<IAssetService<int>, StoreService>();
        }

        private static void WireUpEventHandlers(IServiceCollection services)
        {
            var eventhandlers = from x in _asmbls.Where(asmbl => asmbl.FullName.StartsWith("Ubik.")).SelectMany(asmbl => asmbl.GetTypes())
                                where !x.IsAbstract && !x.IsInterface &&
                                x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IHandlesMessage<>))
                                select x;

            var events = from x in _asmbls.Where(asmbl => asmbl.FullName.StartsWith("Ubik.")).SelectMany(asmbl => asmbl.GetTypes())
                         where !x.IsAbstract && !x.IsInterface &&
                         typeof(Domain.Core.IEvent).IsAssignableFrom(x)
                         select x;

            foreach (var @event in events)
            {
                var t = typeof(IHandlesMessage<>).MakeGenericType(@event);
                var handlers = from x in eventhandlers where t.IsAssignableFrom(x) select x;
                foreach (var handler in handlers)
                {
                    var descriptor = new ServiceDescriptor(t, handler, ServiceLifetime.Transient);
                    services.Add(descriptor);
                }
            }
        }

        private static void WireUpCommandHandlers(IServiceCollection services)
        {
            var commandhandlers = from x in _asmbls.Where(asmbl => asmbl.FullName.StartsWith("Ubik.")).SelectMany(asmbl => asmbl.GetTypes())
                                  where !x.IsAbstract && !x.IsInterface &&
                                  x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IHandlesCommand<>))
                                  select x;

            var Commands = from x in _asmbls.Where(asmbl => asmbl.FullName.StartsWith("Ubik.")).SelectMany(asmbl => asmbl.GetTypes())
                           where !x.IsAbstract && !x.IsInterface &&
                           typeof(ICommand).IsAssignableFrom(x)
                           select x;

            foreach (var command in Commands)
            {
                var t = typeof(IHandlesCommand<>).MakeGenericType(command);
                var handlers = from x in commandhandlers where t.IsAssignableFrom(x) select x;
                foreach (var handler in handlers)
                {
                    var descriptor = new ServiceDescriptor(t, handler, ServiceLifetime.Transient);
                    services.Add(descriptor);
                }
            }
        }

        public static void UseUbik(this IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime, IConfigurationRoot configuration)
        {
            lifetime.ApplicationStopping.Register(() =>
            {
                var dispatcher = app.ApplicationServices.GetRequiredService<IDispatcherInstance>();
                dispatcher.Stop();
            });
        }

        private static void WireUpDbContexts(IServiceCollection services, IConfiguration configuration)
        {
            var cmsConnString = configuration["Data:cmsconnection:ConnectionString"];
            var authConnString = configuration["Data:authconnection:ConnectionString"];
            var assetsConnString = configuration["Data:assetsconnection:ConnectionString"];

            var connectionStrings = new Dictionary<Type, string>
            {
                {typeof (AuthDbContext), authConnString},
                {typeof (ElmahDbContext), cmsConnString},
                {typeof (ComponentsDbContext), cmsConnString},
                {typeof (AssetsStoreDbContext), assetsConnString}
            };

            var serviceDescriptor = new ServiceDescriptor(typeof(IDbContextFactory), new DbContextFactory(connectionStrings));
            services.Add(serviceDescriptor);

            services.AddSingleton<IDbContextScopeFactory, DbContextScopeFactory>();
            services.AddSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
        }

        private static void WireUpInternals(IServiceCollection services)
        {
            services.AddSingleton<IResident, Resident>();
            var backofficeMenu = XmlBackOfficeMenuProvider.FromInternalConfig();
            services.AddInstance(typeof(IBackOfficeMenuProvider), backofficeMenu);
            services.AddInstance(typeof(IResidentAdministration), new ResidentAdministration(backofficeMenu));

            services.AddSingleton<ICacheProvider, MemoryDefaultCacheProvider>();
            services.AddSingleton<IModuleDescovery, ModuleDescovery>();

            //hack
            var hack = new System.BuildingBlocks.Partials.Views.PageBody();
            var moduleDescriptors = _asmbls
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IModuleDescriptor)) && !t.IsAbstract);
            foreach (var moduleDescriptor in moduleDescriptors)
            {
                services.AddSingleton(typeof(IModuleDescriptor), moduleDescriptor);
            }

            services.AddScoped<ICRUDRespoditory<PersistedExceptionLog>, PersistedExceptionLogRepository>();
            services.AddScoped<IErrorLogManager, ErrorLogManager>();
        }

        private static void WireUpCms(IServiceCollection services)
        {
            services.AddScoped<IPersistedTextualRepository, PersistedTextualRepository>();
            services.AddScoped<IPersistedDeviceRepository, PersistedDeviceRepository>();
            services.AddScoped<IPersistedSectionRepository, PersistedSectionRepository>();
            services.AddScoped<IPersistedTaxonomyDivisionRepository, PersistedTaxonomyDivisionRepository>();
            services.AddScoped<IPersistedTaxonomyElementRepository, PersistedTaxonomyElementRepository>();

            services.AddScoped<IDeviceAdministrationService<int>, DeviceAdministrationService>();
            services.AddScoped<IDeviceAdministrationViewModelService, DeviceAdministrationService>();
            services.AddScoped<ITaxonomiesViewModelService, TaxonomiesViewModelService>();

            services.AddScoped<IViewModelCommand<DeviceSaveModel>, DeviceViewModelCommand>();
            services.AddScoped<IViewModelCommand<SectionSaveModel>, SectionViewModelCommand>();
            services.AddScoped<IViewModelCommand<SlotSaveModel>, SlotViewModelCommand>();
            services.AddScoped<IViewModelCommand<DivisionSaveModel>, DivisionViewModelCommand>();
            services.AddScoped<IViewModelCommand<ElementSaveModel>, ElementViewModelCommand>();

            services.AddSingleton<ISlugifier, SystemSlugService>();
            services.AddSingleton<ISlugWordReplacer, SystemSlugWordRplacer>();
            services.AddSingleton<ISlugCharOmmiter, SystemSlugCharReplacer>();
            services.AddSingleton<IInternationalCharToAsciiProvider, GreekToAsciiProvider>();

            services.AddScoped<IContentAccessor, ContentAccessor>();
            services.AddScoped<IDeviceAccesor, DeviceAccessor>();
            services.AddScoped<IServerFeedbackAccessor, ServerFeedackAccessor>();
        }

        private static void WireUpSSSO(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleClaimRepository, RoleClaimRepository>();

            services.AddScoped<IUserAdminstrationService, UserAdminstrationService>();
            services.AddScoped<IUserAdminstrationViewModelService, UserAdminstrationService>();

            services.AddScoped<IViewModelCommand<RoleSaveModel>, RoleViewModelCommand>();
            services.AddScoped<IViewModelCommand<NewUserSaveModel>, NewUserViewModelCommand>();
            services.AddScoped<IViewModelCommand<UserSaveModel>, UserViewModelCommand>();

            var authProviders = _asmbls
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IResourceAuthProvider)) && !t.IsAbstract);
            foreach (var authProvider in authProviders)
            {
                services.AddSingleton(typeof(IResourceAuthProvider), authProvider);
            }

            services.AddSingleton<IResidentSecurity, ResidentSecurity>();

            services.AddUbikIdentity<UbikUser, UbikRole>(
            options =>
            {
                options.Cookies.ApplicationCookieAuthenticationScheme = UserAdministrationAuth.AuthCookieName;
                options.Cookies.ApplicationCookie.AuthenticationScheme = IdentityCookieOptions.ApplicationCookieAuthenticationType = UserAdministrationAuth.AuthCookieName;

                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = true;
                options.Password.RequireNonLetterOrDigit = true;
            })
            .AddUbikIdentityStores()
            .AddDefaultTokenProviders();

            services.AddScoped(typeof(UserManager<UbikUser>), typeof(UbikUserManager<UbikUser>));
            services.AddScoped(typeof(RoleManager<UbikRole>), typeof(UbikRoleManager<UbikRole>));
            services.AddScoped(typeof(SignInManager<UbikUser>), typeof(UbikSignInManager<UbikUser, UbikRole>));
        }

        public static void ConfigureUbikBus(this IServiceCollection services)
        {
            //BusConfiguration busConfiguration = new BusConfiguration();
            //ConventionsBuilder conventions = busConfiguration.Conventions();
            //conventions.DefiningCommandsAs(t => typeof(Domain.Core.ICommand).IsAssignableFrom(t) && !t.IsAbstract);
            //conventions.DefiningEventsAs(t => typeof(Domain.Core.IEvent).IsAssignableFrom(t) && !t.IsAbstract);
            //conventions.DefiningMessagesAs(t => typeof(Domain.Core.IMessage).IsAssignableFrom(t) && !t.IsAbstract);

            //busConfiguration.EndpointName("Ubik.Mvc.Endpoint");
            //busConfiguration.UseSerialization<JsonSerializer>();
            //busConfiguration.UsePersistence<InMemoryPersistence>();
            //busConfiguration.AssembliesToScan(_asmbls);
            //busConfiguration.CustomConfigurationSource(new ConfigurationSource());

            ////var builder = new ContainerBuilder();
            ////builder.Populate(services);

            ////var containerConfig = new NServiceBusLocator(builder);
            ////busConfiguration.UseContainer(containerConfig);

            //// var container = builder.Build();

            //busConfiguration.EnableInstallers();

            //IBus endpoint = Bus.Create(busConfiguration).Start();

            //IBus endpoint = null;
            //var dispatcher = new DefaultDispatcher(endpoint);
            //services.AddInstance(typeof(IDispatcherInstance), dispatcher);
            //services.AddInstance(typeof(IEventBus), dispatcher);

            //return container.Resolve<IServiceProvider>();

            services.AddSingleton<IDispatcherInstance, PoorMansDispatcher>();
            services.AddSingleton<IEventBus, PoorMansDispatcher>();
        }

        //public class ConfigurationSource : IConfigurationSource
        //{
        //    public T GetConfiguration<T>() where T : class, new()
        //    {
        //        if (typeof(T) == typeof(MessageForwardingInCaseOfFaultConfig))
        //        {
        //            MessageForwardingInCaseOfFaultConfig errorConfig = new MessageForwardingInCaseOfFaultConfig
        //            {
        //                ErrorQueue = "error"
        //            };

        //            return errorConfig as T;
        //        }

        //        if (typeof(T) == typeof(UnicastBusConfig))
        //        {
        //            var mappings = new MessageEndpointMappingCollection();
        //            var asmbls = _asmbls.Where(x => x.FullName.StartsWith("Ubik"));
        //            foreach (var asmbl in asmbls)
        //            {
        //                mappings.Add(new MessageEndpointMapping() { AssemblyName = asmbl.FullName, Endpoint = "Ubik.Mvc.Endpoint" });
        //            }

        //            UnicastBusConfig unicastrConfig = new UnicastBusConfig()
        //            {
        //                MessageEndpointMappings = mappings
        //            };

        //            return unicastrConfig as T;
        //        }

        //        return null;
        //    }
        //}

        //public class NServiceBusContainer : ContainerDefinition
        //{
        //    Autofac.IContainer _autofac;
        //    public NServiceBusContainer(Autofac.IContainer autofac) { _autofac = autofac; }
        //    public override NServiceBus.ObjectBuilder.Common.IContainer CreateContainer(ReadOnlySettings settings)
        //    {
        //        //Create a class that implements 'IContainer'
        //        return new NServiceBusLocator(_autofac);
        //    }
        //}
        //public class NServiceBusLocator : NServiceBus.ObjectBuilder.Common.IContainer
        //{
        //    private readonly Autofac.ContainerBuilder _autofac;
        //    public NServiceBusLocator(Autofac.ContainerBuilder container) { _autofac = container; }

        //    public object Build(Type typeToBuild)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IEnumerable<object> BuildAll(Type typeToBuild)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public NServiceBus.ObjectBuilder.Common.IContainer BuildChildContainer()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Configure(Type component, DependencyLifecycle dependencyLifecycle)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Configure<T>(Func<T> component, DependencyLifecycle dependencyLifecycle)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void ConfigureProperty(Type component, string property, object value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public bool HasComponent(Type componentType)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void RegisterSingleton(Type lookupType, object instance)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Release(object instance)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    #region IDisposable Support
        //    private bool disposedValue = false; // To detect redundant calls

        //    protected virtual void Dispose(bool disposing)
        //    {
        //        if (!disposedValue)
        //        {
        //            if (disposing)
        //            {
        //               // _autofac.Dispose();
        //            }

        //            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        //            // TODO: set large fields to null.

        //            disposedValue = true;
        //        }
        //    }

        //    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        //    // ~NServiceBusLocator() {
        //    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //    //   Dispose(false);
        //    // }

        //    // This code added to correctly implement the disposable pattern.
        //    public void Dispose()
        //    {
        //        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //        Dispose(true);
        //        // TODO: uncomment the following line if the finalizer is overridden above.
        //        // GC.SuppressFinalize(this);
        //    }
        //    #endregion
        //}
    }
}