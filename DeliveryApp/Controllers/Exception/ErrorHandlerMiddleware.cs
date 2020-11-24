using DeliveryApp.Service;
using DeliveryApp.Service.Exception;
using DeliveryApp.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeliveryApp.Controller
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch(error)
                {
                    case BussinessException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ObjectNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                
                var json = new
                {
                    status = response.StatusCode,
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    message = "An error occurred whilst processing your request",
                    detailed = error?.Message
                };

                var result = JsonSerializer.Serialize(json);
                await response.WriteAsync(result);
            }
        }
    }
}