using EmployeeLeaveManagementAPI.Data;
using EmployeeLeaveManagementAPI.Repositories;
using EmployeeLeaveManagementAPI.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using EmployeeLeaveManagementAPI.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();


builder.Services.AddScoped<IValidator<CreateEmployeeDto>, CreateEmployeeValidator>();
builder.Services.AddScoped<IValidator<UpdateEmployeeDto>, UpdateEmployeeValidator>();
builder.Services.AddScoped<IValidator<SubmitLeaveDto>, SubmitLeaveValidator>();
builder.Services.AddScoped<IValidator<LeaveActionDto>, LeaveActionValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();