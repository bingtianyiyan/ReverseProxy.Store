﻿using FluentValidation;
using ReverseProxy.Store.EFCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace EFCoreSample.Validator
{
    public class ClusterValidator : AbstractValidator<Cluster>
    {
        public ClusterValidator()
        {
            RuleFor(x => x.HealthCheck.Passive.ReactivationPeriod)
                .Must(period =>
                {
                    if (!string.IsNullOrWhiteSpace(period))
                    {
                        return TimeSpan.TryParse(period, CultureInfo.InvariantCulture, out TimeSpan result);
                    }
                    return true;
                })
                .WithMessage("Cluster.HealthCheck.Passive.ReactivationPeriod Must Format 00:00:00");
                ;
            RuleFor(x => x.HealthCheck.Active.Interval)
                .Must(interval =>
                {
                    if (!string.IsNullOrWhiteSpace(interval))
                    {
                        return TimeSpan.TryParse(interval, CultureInfo.InvariantCulture, out TimeSpan result);
                    }
                    return true;
                })
                .WithMessage("Cluster.HealthCheck.Active.Interval Must Format 00:00:00");
                ;
            RuleFor(x => x.HealthCheck.Active.Timeout)
                .Must(timeout =>
                {
                    if (!string.IsNullOrWhiteSpace(timeout))
                    {
                        return TimeSpan.TryParse(timeout, CultureInfo.InvariantCulture, out TimeSpan result);
                    }
                    return true;
                })
                .WithMessage("Cluster.HealthCheck.Active.Timeout Must Format 00:00:00");
                ;
            RuleFor(x => x.HttpClient.SslProtocols)
                .Must(sslProtocols =>
                {
                    if (!string.IsNullOrWhiteSpace(sslProtocols))
                    {
                        var sslProtocolArr = sslProtocols.Split(",");
                        if(sslProtocolArr.Length > 0)
                        {
                            foreach (var sslProtocol in sslProtocolArr)
                            {
                                var isRight = Enum.TryParse<SslProtocols>(sslProtocol, ignoreCase: true, out SslProtocols result);
                                if (!isRight)
                                    return false;
                            }
                        }
                    }
                    return true;
                })
                .WithMessage("Cluster.HttpClient.SslProtocols Must in None|Ssl2|Ssl3|Default|Tls|Tls11|Tls12|Tls13");
                ;
            RuleFor(x => x.HttpClient.ActivityContextHeaders)
                .Must(activityContextHeaders =>
                {
                    if (!string.IsNullOrWhiteSpace(activityContextHeaders))
                    {
                        return Enum.TryParse<Microsoft.ReverseProxy.Abstractions.ActivityContextHeaders>(activityContextHeaders, ignoreCase: true, out Microsoft.ReverseProxy.Abstractions.ActivityContextHeaders result);
                    }
                    return true;
                })
                .WithMessage("Cluster.HttpClient.ActivityContextHeaders Must in None|Baggage|CorrelationContext|BaggageAndCorrelationContext");
                ;
            RuleFor(x => x.HttpRequest.Timeout)
                .Must(timeout =>
                {
                    if (!string.IsNullOrWhiteSpace(timeout))
                    {
                        return TimeSpan.TryParse(timeout, CultureInfo.InvariantCulture, out TimeSpan result);
                    }
                    return true;
                })
                .WithMessage("Cluster.HttpRequest.Timeout Must Format 00:00:00");
                ;
            RuleFor(x => x.HttpRequest.Version)
                .Must(value =>
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        return Version.TryParse(value + (value.Contains('.') ? "" : ".0"), out Version? result);
                    }
                    return true;
                })
                .WithMessage("Cluster.HttpRequest.Version Format error");
                ;
            RuleFor(x => x.HttpRequest.VersionPolicy)
                .Must(versionPolicy =>
                {
                    if (!string.IsNullOrWhiteSpace(versionPolicy))
                    {
                        return Enum.TryParse<HttpVersionPolicy>(versionPolicy, ignoreCase: true, out HttpVersionPolicy result);
                    }
                    return true;
                })
                .WithMessage("Cluster.HttpRequest.VersionPolicy Must in RequestVersionOrLower|RequestVersionOrHigher|RequestVersionExact");
                ;
        }
    }
}
