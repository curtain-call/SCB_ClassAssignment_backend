using Microsoft.EntityFrameworkCore;
using resume.Service;
using resume.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database context
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30))));

// 添加你的服务
builder.Services.AddTransient<CompanyService>();
builder.Services.AddTransient<ApplicantService>();
builder.Services.AddTransient<ResumeService>();
builder.Services.AddTransient<JobService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(cfg =>
{
    cfg.AllowAnyOrigin(); //对应跨域请求的地址
    cfg.AllowAnyMethod(); //对应请求方法的Method
    cfg.AllowAnyHeader(); //对应请求方法的Headers
                          //cfg.AllowCredentials(); //对应请求的withCredentials 值
});

app.UseAuthorization();

app.MapControllers();

app.Run();
