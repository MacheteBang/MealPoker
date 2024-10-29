global using System.Diagnostics;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;

global using MealBot.Api;
global using MealBot.Api.Identity;
global using MealBot.Api.Identity.Contracts.Responses.Google;
global using MealBot.Api.Identity.Enums;
global using MealBot.Api.Identity.Models;
global using MealBot.Api.Identity.Options;
global using MealBot.Api.Identity.Services;
global using MealBot.Api.Common;
global using MealBot.Api.Common.Exceptions;
global using MealBot.Api.Common.Extensions;
global using MealBot.Api.Common.Http;
global using MealBot.Api.Database;
global using MealBot.Api.Families;
global using MealBot.Api.Families.Models;
global using MealBot.Api.Meals;
global using MealBot.Api.Meals.Models;
global using MealBot.Api.Meals.Repositories;
global using MealBot.Api.Meals.Validators;
global using MealBot.Api.Users;
global using MealBot.Api.Users.Models;
global using MealBot.Api.Users.Services;
global using MealBot.Sdk.Contracts.Requests;
global using MealBot.Sdk.Contracts.Responses;
global using MealBot.Sdk.Enums;
global using MealBot.Sdk.ValueObjects;

global using ErrorOr;

global using Serilog;
global using Serilog.Events;

global using FluentValidation;
global using FluentValidation.Results;

global using MediatR;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Infrastructure;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;