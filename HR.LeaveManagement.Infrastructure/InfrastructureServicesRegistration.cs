﻿using HR.LeaveManagement.Application.Contracts.Infastructure;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.LeaveManagement.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>(); // transient, any time being called a new instance is created
            return services;
        }
    }
}
