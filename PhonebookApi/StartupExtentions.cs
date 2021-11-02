using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using PhonebookApi.Authorization;
using PhonebookApi.Interfaces;
using PhonebookApi.Mappers;
using PhonebookApi.Services;
using PhonebookBusiness.Interfaces;
using PhonebookData.Interfaces;
using PhonebookData.Mappers;
using PhonebookData.Repositories;

namespace PhonebookApi
{
    public static class StartupExtentions {

        public static IServiceCollection ConfigureDependencyInjection( this IServiceCollection services) {

            //Authentication Services
            services.AddScoped<IAuthentication, AuthenticationService>();
            services.AddScoped<IAuthenticatedUser, AuthenticatedUserService>();

            //Authorization Services
            services.AddScoped<IAuthorizationHandler, ContactOwnerAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ContactUserAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, AccountOwnerAuthorizationHandler>();

            //Repositories
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();

            //MAPPERS
            services.AddTransient<IContactBusinessToDataModelMapper, ContactBusinessToDataModelMapper>();
            services.AddTransient<IContactDataToBusinessModelMapper, ContactDataToBusinessModelMapper>();
            services.AddTransient<IUserBusinessToDataModelMapper, UserBusinessToDataModelMapper>();
            services.AddTransient<IUserDataToBusinessModelMapper, UserDataToBusinessModelMapper>();
            services.AddTransient<
                IPhonebookBusinessToDataContactListItemMapper, ContactListItemBusinessToDataMapper>();
            services.AddTransient<
                IContactListItemDataToBusinessModelMapper, ContactListItemDataToBusinessModelMapper>();

            services.AddTransient<IContactRequestMapper, ContactRequestMapper>();
            services.AddTransient<IContactResponseMapper, ContactResponseMapper>();
            services.AddTransient<IMessageResponseMapper, MessageResponseMapper>();
            services.AddTransient<IContactInDetailResponseMapper, ContactInDetailResponseMapper>();
            services.AddTransient<IApplicationUserResponseMapper, ApplicationUserResponseMapper>();
            services.AddTransient<IApplicationUserInDetailResponseMapper, ApplicationUserInDetailResponseMapper>();
            services.AddTransient<IAuthenticationResponseMapper, AuthenticationResponseMapper>();
            services.AddTransient<IApplicationUserRequestMapper, ApplicationUserRequestMapper>();

            return services;
        }
    }

}