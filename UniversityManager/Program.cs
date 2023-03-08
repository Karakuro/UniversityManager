using UniversityManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//builder.Services.AddCors(options => options.AddPolicy("CORS", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

//builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

builder.Services.AddControllers();
builder.Services.AddScoped<ExamResultDataService>();
builder.Services.AddScoped<ExamDataService>();
builder.Services.AddScoped<StudentDataService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseFileServer();
app.UseHttpsRedirection();
//app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
