﻿// -----------------------------------------------------------------------
//  <copyright file="SignalRAutofacIocInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-10-08 19:22</last-date>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

using Autofac;
using Autofac.Integration.SignalR;

using Microsoft.AspNet.SignalR;

using OSharp.Core.Dependency;
using OSharp.Core.Initialize;
using OSharp.Web.SignalR.Initialize;


namespace OSharp.Autofac.SignalR
{
    /// <summary>
    /// SignalR-Autofac依赖注入初始化类
    /// </summary>
    public class SignalRAutofacIocInitializer : IocInitializerBase
    {
        /// <summary>
        /// 添加自定义服务映射
        /// </summary>
        /// <param name="services">服务信息集合</param>
        protected override void AddCustomTypes(IServiceCollection services)
        {
            services.AddInstance(this);
            services.AddSingleton<IIocResolver, SignalRIocResolver>();
        }

        /// <summary>
        /// 将服务构建成服务提供者<see cref="IServiceProvider"/>的实例
        /// </summary>
        /// <param name="services">服务映射信息集合</param>
        /// <param name="assemblies">要检索的程序集集合</param>
        /// <returns>服务提供者</returns>
        protected override IServiceProvider BuildServiceProvider(IServiceCollection services, Assembly[] assemblies)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterHubs().AsSelf().PropertiesAutowired();
            builder.Populate(services);
            IContainer container = builder.Build();
            GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);
            return container.Resolve<IServiceProvider>();
        }
    }
}