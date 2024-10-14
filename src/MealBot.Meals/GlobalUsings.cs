global using MealBot.Meals.Common;
global using MealBot.Meals.DomainErrors;
global using MealBot.Meals.Interfaces;
global using MealBot.Meals.Models;
global using MealBot.Meals.Validators;
global using MealBot.Sdk.Contracts.Requests;
global using MealBot.Sdk.Contracts.Responses;
global using MealBot.Sdk.Enums;
global using MealBot.Sdk.ValueObjects;

global using MealBot.Common;

global using ErrorOr;

global using FluentValidation;
global using FluentValidation.Results;

global using MediatR;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.Extensions.DependencyInjection;
